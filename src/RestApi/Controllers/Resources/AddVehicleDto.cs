using System.ComponentModel.DataAnnotations;

namespace Controllers.Resources;

public class AddVehicleDto
{
	[Required, Range(0, 100)] public int? Battery { get; set; }
	[Required] public bool InUse { get; set; } = false;
	[Required] public string VehicleType { get; set; } = "";
	[Required, Range(-180, 180)] public double Longitude { get; set; }
	[Required, Range(-90, 90)] public double Latitude { get; set; }
}
