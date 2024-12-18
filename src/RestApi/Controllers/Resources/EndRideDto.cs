using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Controllers.Resources;

public class EndRideRDto
{
	[Required, JsonRequired] public Guid VehicleId { get; set; }
	[Required, Range(0, 100)] public int Battery { get; set; }
	[Required, EmailAddress] public string Email { get; set; } = null!;
	[Required, Range(-180, 180)] public double Longitude { get; set; }
	[Required, Range(-90, 90)] public double Latitude { get; set; }
}
