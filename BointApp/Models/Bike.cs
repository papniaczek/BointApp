using System;

namespace BointApp.Models;

public abstract class Bike
{
    public Guid BikeID { get; set; } = Guid.NewGuid();
    public string Brand { get; set; }
    public bool IsAvailable { get; set; } = true;

    protected Bike(string brand)
    {
        Brand = brand;
    }
    
    public abstract decimal CostPerHour { get; }

    public virtual void MarkAsRented()
    {
        IsAvailable = false;
    }

    public virtual void MarkAsReturned()
    {
        IsAvailable = true;
    }
}