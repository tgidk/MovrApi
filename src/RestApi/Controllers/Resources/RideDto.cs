using System.ComponentModel.DataAnnotations;

namespace Controllers.Resources;

public class RideDto
{
	[Required] public Guid VehicleId { get; set; }
	[Required] public VehicleDto Vehicle { get; set; } = null!;
	[Required, EmailAddress] public string UserEmail { get; set; } = null!;
	[Required] public DateTime StartTs { get; set; }
	public DateTime? EndTs { get; set; }
}
