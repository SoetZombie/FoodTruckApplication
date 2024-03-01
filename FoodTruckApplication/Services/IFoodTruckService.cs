using FoodTruckApplication.Models;

namespace FoodTruckApplication.Services;

public interface IFoodTruckService
{
    IEnumerable<FoodTruck> FindClosestFoodTrucks(double latitude, double longitude, string preferredFood, int amountOfResults);

} 