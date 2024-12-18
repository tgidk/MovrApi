using Core;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class RideRepo(ILogger<RideRepo> log, MovrContext context) : IRideRepo
{
	public void Add(Ride ride)
	{
		context.Rides.Add(ride);
	}

	public async Task<Ride> GetFirst(string email, Guid vehicleId, DateTime? endTs)
	{
		return await context.Rides.FirstAsync(r => r.UserEmail == email && r.VehicleId == vehicleId && r.EndTs == endTs);
	}

	public async Task<Ride[]> ByUser(string email, bool addRelated = false)
	{
		if (!addRelated)
			return await context.Rides.Where(r => r.UserEmail == email).ToArrayAsync();

		return await context.Rides.Include(r => r.User)
				.Include(r => r.Vehicle)
				.Where(r => r.UserEmail == email)
				.ToArrayAsync();
	}

	public async Task<Ride?> Active(string email, Guid vehicleId)
	{
		return await context.Rides
				.Include(r => r.Vehicle).ThenInclude(v => v.LocationHistory.OrderByDescending(v => v.Ts).Take(1))
				.Where(r => r.UserEmail == email && r.VehicleId == vehicleId && r.Vehicle.InUse && r.EndTs == null)
				.FirstOrDefaultAsync();
	}
}
