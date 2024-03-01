using System.Collections.ObjectModel;

namespace FoodTruckApplication.Models;

public class FoodTruck
{
    public string Applicant { get; }
    public string FoodItems { get; }
    public double Latitude { get; }
    public double Longitude { get; }

    public FoodTruck(string applicant, string foodItems, double latitude, double longitude)
    {
        Applicant = applicant;
        FoodItems = foodItems;
        Latitude = latitude;
        Longitude = longitude;
    }
}