using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Controllers.Resources;
using Core;
using Core.Models;
using Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MovrApi.Controllers;

[ApiController]
[Route("/api/rides")]
public class RideController(IRideRepo rideRepo, IUserRepo userRepo, IVehicleRepo vehicleRepo,
	 ILogger<RideController> log, IUnitOfWork unitOfWork, IMapper mapper, ICalculatorService calculator) : ControllerBase
{
	/// <summary>
	/// Starts a ride on this vehicle for this user
	/// </summary>
	/// <param name="email"></param>
	/// <param name="vehicleId"></param>
	/// <returns></returns>
	//[HttpGet(template: "start")]
	[HttpPost(template: "start")]
	public async Task<IActionResult> Start([Required] string email, [Required] Guid vehicleId)
	{
		try
		{
			_ = await userRepo.GetFirst(email);
			var vehicle = await vehicleRepo.GetFirst(vehicleId, addRelated: true); // limit related
			if (vehicle.InUse)
				return StatusCode(StatusCodes.Status409Conflict, $"Vehicle {vehicleId} is currently in use");

			vehicle.InUse = true;
			var lastLocation = vehicle.LocationHistory.Last();
			LocationHistory newLoc = new() {
				Latitude = lastLocation.Latitude,
				Longitude = lastLocation.Longitude,
				Vehicle = vehicle,
				Ts = DateTime.UtcNow,
				Id = Guid.NewGuid()
			};
			Ride newRide = new() { Id = Guid.NewGuid(), Vehicle = vehicle, UserEmail = email, StartTs = DateTime.UtcNow };
			vehicleRepo.AddVehicleLocation(newLoc);
			rideRepo.Add(newRide);
			await unitOfWork.CompleteAsync();
			return Ok($"Ride started with vehicle {vehicleId}");
		} catch (Exception ex)
		{
			log.LogError(ex, "Error performing transaction");
			return NotFound($"Could not start ride on vehicle {vehicleId}. Either the vehicle is actively being ridden or it has been deleted from the database");
		}
	}

	/// <summary>
	/// Gets a list of all rides for the given user
	/// </summary>
	/// <param name="email"></param>
	/// <param name="addRelated"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> ByUser([Required] string email, [Required] bool addRelated = true)
	{
		try
		{
			var rides = await rideRepo.ByUser(email, addRelated);
			var rideDtos = mapper.Map<Ride[], RideDto[]>(rides);
			return rides.Length > 0 ? Ok(rideDtos) : NotFound();
		} catch (Exception ex)
		{
			log.LogError(ex, "Error performing transaction");
			return NotFound($"Could not get rides");
		}
	}

	/// <summary>
	/// Gets the active ride for this vehicle/user combination
	/// </summary>
	/// <param name="email"></param>
	/// <param name="vehicleId"></param>
	/// <returns></returns>
	[HttpGet(template: "active")]
	public async Task<IActionResult> Active([Required] string email, [Required] Guid vehicleId)
	{
		try
		{
			var ride = await rideRepo.Active(email, vehicleId);
			if (ride == null)
				return StatusCode(StatusCodes.Status404NotFound, "No active rides found");

			var rideDto = mapper.Map<Ride, RideDto>(ride);
			return Ok(rideDto);
		} catch (Exception ex)
		{
			log.LogError(ex, "Error performing transaction");
			return NotFound($"Could not get active ride");
		}
	}

	/// <summary>
	/// Ends this specific ride (also calculates time, distance, and speed travelled)
	/// </summary>
	/// <param name="endRideDto"></param>
	/// <returns></returns>
	[HttpPost(template: "end")]
	public async Task<IActionResult> End([FromBody] EndRideRDto endRideDto)
	{
		try
		{
			var vehicle = await vehicleRepo.GetFirst(endRideDto.VehicleId, addRelated: true);
			if (!vehicle.InUse)
				return NotFound("Requested vehicle is not in use");

			var currentRide = await rideRepo.GetFirst(endRideDto.Email, endRideDto.VehicleId, endTs: null);
			var startLocation = vehicle.LocationHistory.GetFirstByRide(currentRide);

			vehicle.InUse = false;
			vehicle.Battery = endRideDto.Battery;
			DateTime now = DateTime.UtcNow;
			LocationHistory newLoc = new() {
				Latitude = endRideDto.Latitude,
				Longitude = endRideDto.Longitude,
				Vehicle = vehicle,
				Ts = now,
				Id = Guid.NewGuid()
			};
			vehicleRepo.AddVehicleLocation(newLoc);
			currentRide.EndTs = now;
			await unitOfWork.CompleteAsync();

			double distance = calculator.Distance(startLocation.Latitude, startLocation.Longitude, endRideDto.Latitude, endRideDto.Longitude);
			double duration = calculator.DurationMinutes(currentRide.StartTs, now);
			double speed = calculator.Velocity(distance, currentRide.StartTs, now);

			return Ok(new
			{
				messages = new List<string> {
					 $"You have completed your ride on vehicle {endRideDto.VehicleId}.",
					 $"You traveled {distance} km in {duration} minutes, for an average velocity of {speed} km/h"
				}
			});
		} catch (Exception ex)
		{
			log.LogError(ex, "Error performing transaction");
			return BadRequest($"Unable to end ride on vehicle {endRideDto.VehicleId}");
		}
	}
}
