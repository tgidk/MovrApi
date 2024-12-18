using Core;

namespace Services;

public class PaginationService(ILogger<PaginationService> log, IConfiguration config) : IPaginationService
{
	public int MaxPageSize(int pageSize)
	{
		int maxSize = config.GetValue<int>("MaxPageSize");
		return Math.Min(pageSize, maxSize);
	}

	public int Offset(int page, int pageSize)
	{
		return (page - 1) * pageSize;
	}
}
