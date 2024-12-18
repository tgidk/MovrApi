using Core.Models;

namespace Core;

public interface IVehicleRepo
{
	void Add(Vehicle vehicle);
	void AddVehicleLocation(LocationHistory location);
	Task<Vehicle> GetFirst(Guid id, bool addRelated = true);
	Task<Vehicle[]> GetVehicles(int pageSize, int offset);
	void Remove(Vehicle vehicle);
}
