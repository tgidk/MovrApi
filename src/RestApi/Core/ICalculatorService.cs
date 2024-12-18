namespace Core;

public interface ICalculatorService
{
	double Distance(double latitude1, double longitude1, double latitude2, double longitude2);
	double DurationMinutes(DateTime startTime, DateTime endTime);
	double DurationHours(DateTime startTime, DateTime endTime);
	double Velocity(double distance, DateTime startTime, DateTime endTime);
}