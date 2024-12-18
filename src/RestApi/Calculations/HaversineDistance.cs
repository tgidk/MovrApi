using Core;

namespace Calculations;

public class HaversineDistance : IHaversineDistance
{
	private const double EarthRadiusKm = 6371.0;

	public double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
	{
		double radiansLat = ToRadians(latitude2 - latitude1);
		double radiansLon = ToRadians(longitude2 - longitude1);

		double a = Math.Sin(radiansLat / 2) * Math.Sin(radiansLat / 2) +
						  Math.Cos(ToRadians(latitude1)) * Math.Cos(ToRadians(latitude2)) *
						  Math.Sin(radiansLon / 2) * Math.Sin(radiansLon / 2);

		double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

		return EarthRadiusKm * c;
	}

	private static double ToRadians(double angleInDegrees)
	{
		return angleInDegrees * (Math.PI / 180.0);
	}
}
