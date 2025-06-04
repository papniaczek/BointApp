using System;

namespace BointApp.Models;

public class CityBike : Bike
{
    public bool HasBasket { get; set; }

    public CityBike(string brand, bool hasBasket = false) : base(brand)
    {
        HasBasket = hasBasket;
    }

    public override decimal CostPerHour => 15.0m;
}