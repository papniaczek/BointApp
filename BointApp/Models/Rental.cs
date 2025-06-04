using System;

namespace BointApp.Models;

public class Rental
{
    public Guid RentalID { get; set; }
    public User User { get; set; }
    public Bike Bike { get; set; }
    public Station StartStation { get; set; }
    public Station? EndStation { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan? Duration => EndTime.HasValue ? EndTime.Value - StartTime : null;
    public decimal? Cost => Duration.HasValue ? CalculateCost(Duration.Value, Bike.CostPerHour) :  null;

    public Rental(User user, Bike bike, Station startStation)
    {
        User = user;
        Bike = bike;
        StartStation = startStation;
        StartTime = DateTime.Now;
    }

    public void EndRental(Station endStation)
    {
        if (EndTime.HasValue) throw new ArgumentException("Rental is already ended");
        EndStation = endStation;
        EndTime = DateTime.Now;
    }

    private decimal CalculateCost(TimeSpan duration, decimal costPerHour)
    {
        var totalHours = Math.Ceiling(duration.TotalHours);
        return (decimal)totalHours * costPerHour;
    }
}