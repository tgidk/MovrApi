using Calculations;
using Core;
using Moq;
using Services;

namespace CalculatorTests;

[TestFixture]
public class CalculatorServiceTests
{
   [Test]
   public void Test_Distance_Between_2_Points()
   {
      double latitude1 = 40.416775;
      double longitude1 = -3.703790;
      double latitude2 = 46.947975;
      double longitude2 = 7.447447;
      var haversineDistance = new HaversineDistance();

      var distance = haversineDistance.CalculateDistance(latitude1, longitude1, latitude2, longitude2);

      Assert.That(distance, Is.EqualTo(1151.975d).Within(0.005d));
   }  

   [Test]
   public void Test_No_Distance_Between_2_Points()
   {
      double latitude1 = 40;
      double longitude1 = -3;
      double latitude2 = 40;
      double longitude2 = -3;
      var haversineDistance = new HaversineDistance();

      var distance = haversineDistance.CalculateDistance(latitude1, longitude1, latitude2, longitude2);

      Assert.That(distance, Is.EqualTo(0));
   }

    [Test]
   public void Test_Velocity_When_Start_Equals_EndTime()
   {
      var haversineDistance = new Mock<IHaversineDistance>();
      var calculator = new CalculatorService(haversineDistance.Object);
      DateTime startTime = new DateTime(2024,12,12,12,12,12);
      DateTime endTime  = new DateTime(2024,12,12,12,12,12);

      var speed = calculator.Velocity(distance: 0, startTime, endTime);

      Assert.That(speed, Is.EqualTo(0));
   }
}
