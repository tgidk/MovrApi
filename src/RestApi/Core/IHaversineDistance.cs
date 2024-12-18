namespace Core;

public interface IHaversineDistance
{
	double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2);
}