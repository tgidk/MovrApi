using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class User
{
	[Key, Column("email")]
	public string Email { get; set; } = null!;
	[Column("first_name")]
	public string FirstName { get; set; } = null!;
	[Column("last_name")]
	public string LastName { get; set; } = null!;
	[Column("phone_numbers")]
	public string[] PhoneNumbers { get; set; } = null!;

	override public string ToString()
	{
		return $"email: {Email} first_name: {FirstName} last_name: {LastName} phone_numbers: {PhoneNumbers}";
	}
}
