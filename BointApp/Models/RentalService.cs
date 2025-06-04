using System;
using System.Collections.Generic;
using System.Linq;

namespace BointApp.Models;

public class RentalService
{
    private readonly List<Rental> _activeRentals = new();
    private readonly List<Rental> _completedRentals = new();
    
    public IReadOnlyList<Rental> ActiveRentals => _activeRentals.AsReadOnly();
    public IReadOnlyList<Rental> CompletedRentals => _completedRentals.AsReadOnly();

    public Rental StartRental(User user, Bike bike, Station startStation)
    {
        if (user == null || bike == null || startStation == null)
            throw new ArgumentNullException("Cannot start rentals without a bike or station");
        if (!startStation.AvailableBikes.Contains(bike))
            throw new InvalidOperationException("Chosen bike is not available");
        if (IsBikeCurrentlyRented(bike))
            throw new InvalidOperationException("This bike has already been rented");
        
        startStation.AvailableBikes.Remove(bike);
        bike.MarkAsRented();
        
        var rental = new Rental(user, bike, startStation);
        _activeRentals.Add(rental);
        user.RentalHistory.Add(rental);
        
        return rental;
    }
    
    public void EndRental(Rental rental, Station endStation)
    {
        if (rental == null || endStation == null)
            throw new ArgumentNullException("Brakuje danych do zakończenia wypożyczenia.");

        if (! _activeRentals.Contains(rental))
            throw new InvalidOperationException("To wypożyczenie nie jest aktywne.");

        if (endStation.Blocked || endStation.AvailableSlots <= 0)
            throw new InvalidOperationException("Stacja jest pełna i zablokowana.");

        rental.EndRental(endStation);

        rental.Bike.MarkAsReturned();
        endStation.AvailableBikes.Add(rental.Bike);

        _activeRentals.Remove(rental);
        _completedRentals.Add(rental);
    }

    public bool IsBikeCurrentlyRented(Bike bike)
    {
        return _activeRentals.Any(r => r.Bike.BikeID == bike.BikeID);
    }

    public List<Rental> GetRentalsByUser(User user)
    {
        return _completedRentals
            .Where(r => r.User.UserID == user.UserID)
            .OrderByDescending(r => r.StartTime)
            .ToList();
    }

    public void ForceEndRental(Rental rental)
    {
        if (rental.EndTime.HasValue) return;
        
        rental.EndTime = DateTime.Now;
        rental.Bike.MarkAsReturned();
        _activeRentals.Remove(rental);
        _completedRentals.Add(rental);
    }
}