using Core;

namespace Persistence;

public class UnitOfWork(ILogger<UnitOfWork> log, MovrContext context) : IUnitOfWork
{
	public async Task<int> CompleteAsync()
	{
		return await context.SaveChangesAsync();
	}
}
