using Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Xunit;
using System.Collections.Generic;
using System.Linq;

public class FoodTruckServiceTests
{
    private readonly Mock<IMemoryCache> _memoryCacheMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly FoodTruckService _foodTruckService;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;
    private readonly string _fakeCsvPath = "./data/food_trucks.csv";

    public FoodTruckServiceTests()
    {
        // Mock IConfiguration to return the fake CSV path
        _configurationMock.Setup(c => c.GetValue<string>("FoodTrucksCsvPath")).Returns(_fakeCsvPath);

        // Setup MemoryCache mock
        var cache = new MemoryCache(new MemoryCacheOptions());
        _memoryCacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns((object key) =>
        {
            return cache.CreateEntry(key);
        });

        // Initialize the service with mocks
        _foodTruckService = new FoodTruckService(_memoryCacheMock.Object, _configurationMock.Object);

        // Example data to load into cache
        var foodTrucks = new List<FoodTruck>
        {
            new FoodTruck("Truck 1", new List<string> { "Tacos", "Burritos" }, 37.780000, -122.410000),
            new FoodTruck("Truck 2", new List<string> { "Pizza" }, 37.781000, -122.411000),
            // Add more trucks as needed for testing
        };

        // Pre-load the cache with food truck data
        _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out foodTrucks)).Returns(true);
    }

    [Fact]
    public void FindClosestFoodTrucks_ReturnsCorrectTrucksBasedOnFoodPreference()
    {
        // Arrange
        var latitude = 37.780000;
        var longitude = -122.410000;
        var preferredFood = "Pizza";
        var amountOfResults = 1;

        // Act
        var result = _foodTruckService.FindClosestFoodTrucks(latitude, longitude, preferredFood, amountOfResults).ToList();

        // Assert
        Assert.Single(result); // Expecting only one result
        Assert.Contains(result, t => t.FoodItems.Contains(preferredFood)); // The result should contain the preferred food
        Assert.Equal("Truck 2", result.First().Applicant); // Ensure the correct truck is returned
    }
}