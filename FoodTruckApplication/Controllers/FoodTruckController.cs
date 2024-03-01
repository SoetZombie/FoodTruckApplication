using FoodTruckApplication.Services;
using FoodTruckApplication.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace FoodTruckApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class FoodTruckController : ControllerBase
{
    private readonly IFoodTruckService _foodTruckService;

    public FoodTruckController(IFoodTruckService foodTruckService)
    {
        _foodTruckService = foodTruckService;
    }

    [HttpGet]
    public IActionResult GetClosestFoodTrucks(double latitude, double longitude, string? preferredFood, int amountOfResults = 5)
    {
        var trucks = _foodTruckService.FindClosestFoodTrucks(latitude, longitude, preferredFood, amountOfResults);
        return Ok(trucks);
    }
}