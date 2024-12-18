using System.ComponentModel.DataAnnotations;

namespace Controllers.Resources;

public class PagingDto
{
	[Required] public int? Limit { get; set; }
	[Required] public int? Page { get; set; }
}
