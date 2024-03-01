using FoodTruckApplication.Helpers;

namespace FoodTruckTest;

public class GeoUtilsTests
{
    [Theory]
    [InlineData(52.5200, 13.4050, 48.8566, 2.3522, 878000, 5000)] // Berlin to Paris ~878 km
    [InlineData(40.7128, -74.0060, 34.0522, -118.2437, 3940000, 50000)] // NYC to Los Angeles ~3940 km
    public void CalculateDistance_ReturnsApproximateDistanceBetweenTwoPoints(
        double lat1, double lon1, double lat2, double lon2, double expectedDistance, double tolerance)
    {
        // Act
        double distance = DistanceCalculator.CalculateDistance(lat1, lon1, lat2, lon2);

        // Assert
        Assert.InRange(distance, expectedDistance - tolerance, expectedDistance + tolerance);
    }
}