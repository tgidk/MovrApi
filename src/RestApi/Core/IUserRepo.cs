using Core.Models;

namespace Core;

public interface IUserRepo
{
	void Add(User user);
	Task<User> GetFirst(string email);
	Task<User?> GetUser(string email);
	void Remove(User user);
}