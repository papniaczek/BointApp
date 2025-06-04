using System;

namespace BointApp.Models;

public class MountainBike : Bike
{
    public int WheelSizeInInches { get; set; }

    public MountainBike(string brand, int wheelSizeInInches = 26) : base(brand)
    {
        WheelSizeInInches = wheelSizeInInches;
    }

    public override decimal CostPerHour => 20.0m;
}