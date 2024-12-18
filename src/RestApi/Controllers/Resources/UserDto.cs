using System.ComponentModel.DataAnnotations;

namespace Controllers.Resources;

public class UserDto
{
	[Required, EmailAddress] public string Email { get; set; } = null!;
	[Required] public string FirstName { get; set; } = null!;
	[Required] public string LastName { get; set; } = null!;
	//[Required, MinLength(1)] 
	public string[] PhoneNumbers { get; set; } = null!;
}
