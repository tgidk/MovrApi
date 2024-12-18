using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Controllers.Resources;
using Core;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace MovrApi.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController(IUserRepo userRepo, ILogger<UserController> log, IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
	/// <summary>
	/// Registers a new user (add a User entity) 
	/// </summary>
	/// <param name="userDto"></param>
	/// <returns></returns>
	[HttpPost(template: "register")]
	public async Task<IActionResult> Register([FromBody] UserDto userDto)
	{
		if (userDto.PhoneNumbers.Length == 0 || userDto.PhoneNumbers.Any(n => string.IsNullOrWhiteSpace(n)))
			return BadRequest("No phone number found");

		try
		{
			if (await userRepo.GetUser(userDto.Email) != null)
			{
				log.LogError("User not created: user already exists");
				return StatusCode(StatusCodes.Status409Conflict, "User not created: user already exists");
			}

			var user = mapper.Map<UserDto, User>(userDto);
			userRepo.Add(user);
			_ = await unitOfWork.CompleteAsync();
			return Ok("User successfully created");
		} catch (Exception ex)
		{
			log.LogError(ex, "Error performing transaction");
			return BadRequest($"Unable to register user");
		}
	}

	/// <summary>
	/// Gets a user
	/// </summary>
	/// <param name="email"></param>
	/// <returns></returns>
	[HttpGet(template: "profile")]
	public async Task<IActionResult> Profile([Required] string email)
	{
		try
		{
			var user = await userRepo.GetFirst(email);
			var userDto = mapper.Map<User, UserDto>(user);
			return Ok(userDto);
		} catch (Exception ex)
		{
			log.LogError(ex, "Error performing transaction");
			return NotFound("No user found");
		}
	}

	/// <summary>
	/// Logs in user
	/// </summary>
	/// <param name="email"></param>
	/// <returns></returns>
	[HttpGet(template: "login")]
	public async Task<IActionResult> Login([Required] string email)
	{
		try
		{
			_ = await userRepo.GetFirst(email);
			return Ok(new { is_authorized = true });
		} catch (Exception ex)
		{
			log.LogError(ex, "Error performing transaction");
			return Unauthorized("Unable to log in user");
		}
	}

	/// <summary>
	/// Deletes a user
	/// </summary>
	/// <param name="email"></param>
	/// <returns></returns>
	[HttpDelete("{email}")]
	public async Task<IActionResult> Delete([Required] string email)
	{
		try
		{
			var user = await userRepo.GetFirst(email);
			userRepo.Remove(user);
			await unitOfWork.CompleteAsync();
			return Ok("You have successfully deleted your account");
		} catch (Exception ex)
		{
			log.LogError(ex, "Error performing transaction");
			return NotFound("No user to delete");
		}
	}
}