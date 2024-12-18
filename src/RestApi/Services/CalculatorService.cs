using Core;

namespace Services;

public class CalculatorService(IHaversineDistance haversineDistance) : ICalculatorService
{
	public double Distance(double latitude1, double longitude1, double latitude2, double longitude2)
	{
		return haversineDistance.CalculateDistance(latitude1, longitude1, latitude2, longitude2);
	}

	public double DurationMinutes(DateTime startTime, DateTime endTime)
	{
		return (endTime - startTime).TotalMinutes;
	}

	public double DurationHours(DateTime startTime, DateTime endTime)
	{
		return (endTime - startTime).TotalHours;
	}

	public double Velocity(double distance, DateTime startTime, DateTime endTime)
	{
		double duration = DurationHours(startTime, endTime);
		if (startTime == endTime) return 0;
		return distance / duration;
	}
}