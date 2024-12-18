using Core;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class UserRepo(ILogger<UserRepo> log, MovrContext context) : IUserRepo
{
	public void Add(User user)
	{
		context.Users.Add(user);
	}

	public async Task<User> GetFirst(string email)
	{
		return await context.Users.FirstAsync(u => u.Email == email);
	}

	public async Task<User?> GetUser(string email)
	{
		return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
	}

	public void Remove(User user)
	{
		context.Users.Remove(user);
	}
}
