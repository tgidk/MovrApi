using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Controllers.Resources;
using Core;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace MovrApi.Controllers;

[ApiController]
[Route("/api/vehicles")]
public class VehicleController(IVehicleRepo vehicleRepo, ILogger<VehicleController> log, IUnitOfWork unitOfWork,
	IMapper mapper, IPaginationService pagination) : ControllerBase
{
	/// <summary>
	/// Adds a vehicle
	/// </summary>
	/// <param name="addVehicle"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> Add([FromBody] AddVehicleDto addVehicle)
	{
		try
		{
			var vehicle = mapper.Map<AddVehicleDto, Vehicle>(addVehicle);
			vehicle.Id = Guid.NewGuid();
			vehicleRepo.Add(vehicle);
			LocationHistory newLoc = new() {
				Latitude = addVehicle.Latitude,
				Longitude = addVehicle.Longitude,
				Vehicle = vehicle,
				Ts = DateTime.UtcNow,
				Id = Guid.NewGuid()
			};
			vehicleRepo.AddVehicleLocation(newLoc);
			_ = await unitOfWork.CompleteAsync();
			return Ok(new { vehicle_id = vehicle.Id });
		} catch (Exception ex)
		{
			log.LogError(ex, "Error performing transaction");
			return BadRequest("Cannot create vehicle");
		}
	}

	/// <summary>
	/// Gets a list of all vehicles (limited by passed value)
	/// </summary>
	/// <param name="pageSize"></param>
	/// <param name="page"></param>
	/// <returns></returns>
	[HttpGet(template: "all")]
	public async Task<IActionResult> Get([FromQuery, Required, Range(10, 100)] int pageSize, [FromQuery, Required, Range(1, int.MaxValue)] int page)
	{
		try
		{
			int size = pagination.MaxPageSize(pageSize);
			int offset = pagination.Offset(page, size);
			var vehicles = await vehicleRepo.GetVehicles(size, offset);
			var vehicleDtos = mapper.Map<Vehicle[], VehicleDto[]>(vehicles);
			foreach (var vehicle in vehicles)
			{
				log.LogInformation("Vehicle {Vehicle}", vehicleDtos);
			}
			return Ok(vehicleDtos);
		} catch (Exception ex)
		{
			log.LogError(ex, "Error performing transaction");
			return NotFound("No vehicles found");
		}
	}

	/// <summary>
	/// Gets a specific vehicle with its location history
	/// </summary>
	/// <param name="id"></param>
	/// <param name="addRelated"></param>
	/// <returns></returns>
	[HttpGet("{id}")]
	public async Task<IActionResult> GetVehicle([Required] Guid id, [Required] bool addRelated)
	{
		try
		{
			var vehicle = await vehicleRepo.GetFirst(id, addRelated);
			var vehicleDto = mapper.Map<Vehicle, VehicleDto>(vehicle);
			return Ok(vehicleDto);
		} catch (Exception ex)
		{
			log.LogError(ex, "Error performing transaction");
			return NotFound("Vehicle not found");
		}
	}

	/// <summary>
	/// Removes a specific vehicle
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		try
		{
			var vehicle = await vehicleRepo.GetFirst(id, addRelated: false);
			if (!vehicle.InUse)
			{
				vehicleRepo.Remove(vehicle);
				await unitOfWork.CompleteAsync();
				return Ok($"Deleted vehicle with id {id} from database");
			} else
			{
				return StatusCode(StatusCodes.Status409Conflict, $"Vehicle {id} is currently in use");
			}
		} catch (Exception ex)
		{
			log.LogError(ex, "Error performing transaction");
			return NotFound("No vehicle to delete");
		}
	}
}
