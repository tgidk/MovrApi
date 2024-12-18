using Core.Models;

namespace Extensions;

public static class SearchEntensions
{
	public static LocationHistory GetFirstByRide(this ICollection<LocationHistory> locationHistory, Ride ride)
	{
		return locationHistory.First(l => l.Ts.Date == ride.StartTs.Date && l.Ts.Hour == ride.StartTs.Hour
				&& l.Ts.Minute == ride.StartTs.Minute && l.Ts.Second == ride.StartTs.Second);
	}
}
