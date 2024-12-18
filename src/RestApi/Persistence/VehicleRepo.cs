using Core;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class VehicleRepo(ILogger<VehicleRepo> log, MovrContext context) : IVehicleRepo
{
	public void Add(Vehicle vehicle)
	{
		context.Add(vehicle);
	}

	public void AddVehicleLocation(LocationHistory location)
	{
		context.LocationHistory.Add(location);
	}

	public async Task<Vehicle> GetFirst(Guid id, bool addRelated = true)
	{
		if (!addRelated)
			return await context.Vehicles.FirstAsync(v => v.Id == id);

		return await context.Vehicles.Include(v => v.LocationHistory.OrderByDescending(l => l.Ts).Take(10))
			 .FirstAsync(v => v.Id == id);
	}

	public async Task<Vehicle[]> GetVehicles(int pageSize, int offset)
	{
		return await context.Vehicles.Include(v => v.LocationHistory.OrderBy(l => l.Ts))
			 .Skip(offset)
			 .Take(pageSize)
			 .ToArrayAsync();
	}

	public void Remove(Vehicle vehicle)
	{
		context.Vehicles.Remove(vehicle);
	}
}
