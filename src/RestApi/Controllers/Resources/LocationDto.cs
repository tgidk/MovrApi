using System.ComponentModel.DataAnnotations;

namespace Controllers.Resources;

public class LocationDto
{
	[Required] public DateTime Ts { get; set; }
	[Required, Range(-180, 180)] public double Longitude { get; set; }
	[Required, Range(-90, 90)] public double Latitude { get; set; }
	[Required] public Guid VehicleId { get; set; }
}
