using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class Ride
{
	[Key, Column("id")]
	public Guid Id { get; set; }
	[Required, Column("vehicle_id")]
	public Guid VehicleId { get; set; }
	public Vehicle Vehicle { get; set; } = null!;
	[Required, Column("user_email")]
	public string UserEmail { get; set; } = null!;
	public User User { get; set; } = null!;
	[Column("start_ts")]
	public DateTime StartTs { get; set; }
	[Column("end_ts")]
	public DateTime? EndTs { get; set; }

	override public string ToString()
	{
		return $"user_email: {UserEmail} start_ts: {StartTs} end_ts: {EndTs} ";
	}
}
