using Core.Models;

namespace Core;

public interface IRideRepo
{
	void Add(Ride ride);
	Task<Ride> GetFirst(string email, Guid vehicleId, DateTime? endTs);
	Task<Ride[]> ByUser(string email, bool addRelated = false);
	Task<Ride?> Active(string email, Guid vehicleId);
}