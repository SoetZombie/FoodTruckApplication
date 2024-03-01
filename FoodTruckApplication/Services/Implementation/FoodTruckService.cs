using System.Globalization;
using Ardalis.GuardClauses;
using CsvHelper;
using CsvHelper.Configuration;
using FoodTruckApplication.Helpers;
using FoodTruckApplication.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FoodTruckApplication.Services.Implementation;

public class FoodTruckService : IFoodTruckService
{
    private readonly IMemoryCache _memoryCache;
    private readonly string? _csvFilePath;

    public FoodTruckService(IMemoryCache memoryCache, IConfiguration configuration)
    {
        _memoryCache = memoryCache;
        _csvFilePath = configuration.GetValue<string>("FoodTrucksCsvPath") ??
                       throw new ArgumentNullException(nameof(_csvFilePath), "Failed to load food truck data");
    }

    private IReadOnlyList<FoodTruck>? LoadFoodTrucks()
    {
        return _memoryCache.GetOrCreate("FoodTrucks", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(99);
            using var reader = new StreamReader(_csvFilePath!);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };
            using var csv = new CsvReader(reader, csvConfig);
            var foodTrucks = csv.GetRecords<FoodTruck>().ToList();
            return foodTrucks.ToArray();
        });
    }

    public IEnumerable<FoodTruck> FindClosestFoodTrucks(double latitude, double longitude, string preferredFood,
        int amountOfResults)
    {
        var foodTrucks = LoadFoodTrucks();

        Guard.Against.NullOrEmpty(foodTrucks);

        if (!string.IsNullOrWhiteSpace(preferredFood))
        {
            return foodTrucks.Where(truck => truck.FoodItems.Contains(preferredFood)).Select(truck => new
                {
                    Truck = truck,
                    Distance = DistanceCalculator.CalculateDistance(latitude, longitude, truck.Latitude,
                        truck.Longitude)
                })
                .OrderBy(x => x.Distance)
                .Take(amountOfResults)
                .Select(x => x.Truck);
        }

        return foodTrucks
            .Select(truck => new
            {
                Truck = truck,
                Distance = DistanceCalculator.CalculateDistance(latitude, longitude, truck.Latitude, truck.Longitude)
            })
            .OrderBy(x => x.Distance)
            .Take(amountOfResults)
            .Select(x => x.Truck);
    }
}