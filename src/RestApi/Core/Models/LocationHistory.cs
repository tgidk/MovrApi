using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Models;

public class LocationHistory
{
	[Key, Column("id")]
	public Guid Id { get; set; }
	[Column("ts")]
	public DateTime Ts { get; set; }
	[Column("longitude")]
	public double Longitude { get; set; }
	[Column("latitude")]
	public double Latitude { get; set; }
	//foreign key for vehicle
	[Required, Column("vehicle_id")]
	public Guid VehicleId { get; set; }
	[JsonIgnore]
	public Vehicle? Vehicle { get; set; }

	override public string ToString()
	{
		return $"id: {Id} timestamp: {Ts} longitude: {Longitude} latitude: {Latitude}";
	}
}
