namespace Core;

public interface IPaginationService
{
	int MaxPageSize(int pageSize);
	int Offset(int page, int pageSize);
}
