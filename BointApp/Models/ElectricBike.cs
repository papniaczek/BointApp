using System;

namespace BointApp.Models;

public class ElectricBike : Bike
{
    public int BatteryLevel { get; set; }

    public ElectricBike(string brand, int batteryLevel = 100) : base(brand)
    {
        BatteryLevel = batteryLevel;
    }

    public override decimal CostPerHour => 30.0m;

    public override void MarkAsRented()
    {
        if (BatteryLevel < 20) throw new Exception("Rental is impossible - Battery level is too low");
        base.MarkAsRented();
    }
}