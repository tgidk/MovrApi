using System.ComponentModel.DataAnnotations;

namespace Controllers.Resources;

public class VehicleDto
{
	[Required] public Guid Id { get; set; }
	[Required, Range(0, 100)] public int Battery { get; set; }
	[Required] public bool InUse { get; set; }
	[Required] public string VehicleType { get; set; } = "";
	//[Required, MinLength(1)] 
	public LocationDto[] LocationHistory { get; set; } = null!;
}
