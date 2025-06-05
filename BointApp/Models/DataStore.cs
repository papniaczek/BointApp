using System.Collections.Generic;
using System.Linq;

namespace BointApp.Models;

public class DataStore
{
    private readonly List<Bike> _bikes = new();
    private readonly List<Station> _stations = new();
    private readonly List<User> _users = new();
    private readonly List<Rental> _rentals = new();

    public IReadOnlyList<Bike> Bikes => _bikes.AsReadOnly();
    public IReadOnlyList<Station> Stations => _stations.AsReadOnly();
    public IReadOnlyList<User> Users => _users.AsReadOnly();
    public IReadOnlyList<Rental> Rentals => _rentals.AsReadOnly();
    
    public void AddBike(Bike bike) => _bikes.Add(bike);
    public void AddStation(Station station) => _stations.Add(station);
    public void AddUser(User user) => _users.Add(user);
    
    public void RemoveBike(Bike bike) => _bikes.Remove(bike);
    public void RemoveStation(Station station) => _stations.Remove(station);
    public void RemoveUser(User user) => _users.Remove(user);
    
    // Filters for searching etc.
    // Bikes
    public IEnumerable<Bike> GetAvailableBikes() => _bikes.FindAll(b => b.IsAvailable);
    public IEnumerable<CityBike> GetAllCityBikes() => _bikes.OfType<CityBike>().ToList();
    public IEnumerable<MountainBike> GetAllMountainBikes() => _bikes.OfType<MountainBike>().ToList();
    public IEnumerable<ElectricBike> GetAllElectricBikes() => _bikes.OfType<ElectricBike>().ToList();
    
    // Stations
    public IEnumerable<Station> GetAvailableStations() => _stations.FindAll(s => s.Blocked == false);
    public IEnumerable<Station> GetBlockedStations() => _stations.FindAll(s => s.Blocked);
    public IEnumerable<Station> GetUnblockedStations() => _stations.FindAll(s => s.Blocked == false);
    
    // Rentals
    public IEnumerable<Rental> GetLast10ActiveRentals() => _rentals
        .Where(r => r.EndTime == null)
        .OrderByDescending(r => r.StartTime)
        .Take(10);
}