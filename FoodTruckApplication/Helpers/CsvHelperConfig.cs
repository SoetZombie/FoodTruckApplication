using FoodTruckApplication.Models;

namespace FoodTruckApplication.Helpers;

using CsvHelper.Configuration;

public class FoodTruckMap : ClassMap<FoodTruck>
{
    public FoodTruckMap()
    {
        Map(m => m.Latitude).Name("Latitude");
        Map(m => m.Longitude).Name("Longitude");
        Map(m => m.Applicant).Name("Applicant");
        Map(m => m.FoodItems).Name("FoodItems");
    }
}