using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class Vehicle
{
	[Key, Column("id")]
	public Guid Id { get; set; }
	[Column("battery")]
	public int Battery { get; set; }
	[Column("in_use")]
	public bool InUse { get; set; }
	[Column("vehicle_type")]
	public string VehicleType { get; set; } = "";
	public ICollection<LocationHistory> LocationHistory { get; set; } = null!;

	override public string ToString()
	{
		return $"id: {Id} battery: {Battery} in_use: {InUse} vehicle_type: {VehicleType}";
	}
}
