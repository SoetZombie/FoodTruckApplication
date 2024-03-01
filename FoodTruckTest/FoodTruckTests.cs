using Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using FoodTruckApplication.Models;
using FoodTruckApplication.Services.Implementation;

public class FoodTruckServiceTests
{
    private readonly Mock<IMemoryCache> _memoryCacheMock = new();
    private readonly FoodTruckService _foodTruckService;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;
    private readonly string _fakeCsvPath = "./data/food_trucks.csv";

    public FoodTruckServiceTests()
    {
        var configForSmsApi = new Dictionary<string, string>
        {
            {"FoodTrucksCsvPath", "./data/Mobile_Food_Facility_Permit.csv"},
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configForSmsApi)
            .Build();
        
        // Setup MemoryCache mock
        var cache = new MemoryCache(new MemoryCacheOptions());

        // Initialize the service with mocks
        _foodTruckService = new FoodTruckService(cache, configuration);

        // Example data to load into cache
        var foodTrucks = new List<FoodTruck>
        {
            new FoodTruck("Truck 1", "Tacos Burritos" , 37.780000, -122.410000),
            new FoodTruck("Truck 2",  "Pizza", 37.781000, -122.411000),
            // Add more trucks as needed for testing
        };
        cache.GetOrCreate("FoodTrucks", _ => foodTrucks.ToArray());
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