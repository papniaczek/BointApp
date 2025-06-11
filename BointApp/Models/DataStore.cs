using System;
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

    public DataStore()
    {
        SeedInitialData();
    }

    private void SeedInitialData()
    {
        // Creating bikes
        var bike1 = new CityBike("Kross Trans 1.0", true);
        var bike2 = new CityBike("Romet Gazela 2", false);
        var bike3 = new MountainBike("KTM Ultra Fun", 29);
        var bike4 = new MountainBike("Scott Aspect 970", 27);
        var bike5 = new ElectricBike("Haibike AllMtn 2", 95);
        var bike6 = new ElectricBike("Ecobike S-Cross M", 70);

        AddBike(bike1);
        AddBike(bike2);
        AddBike(bike3);
        AddBike(bike4);
        AddBike(bike5);
        AddBike(bike6);
        
        // Creating stations
        var station1 = new Station("Dworzec Centralny", 10);
        var station2 = new Station("Metro Politechnika", 8);
        var station3 = new Station("Stare Miasto", 12);

        // Adding bikes to stations
        station1.TryAddBike(bike1);
        station1.TryAddBike(bike3);
        station1.TryAddBike(bike5);
        
        station2.TryAddBike(bike2);
        station2.TryAddBike(bike4);
        
        station3.TryAddBike(bike6);

        AddStation(station1);
        AddStation(station2);
        AddStation(station3);

        // Creating users for testing
        var user2 = new User("Szymon", "Niemyjski", "szymon.niemyjski@gmail.com", "user123", UserRole.User); // Normal user
        var user1 = new User("Kacper", "Strzesniewski", "kacper.strzesniewski@gmail.com", "admin123", UserRole.Admin); // Admin
        
        AddUser(user1);
        AddUser(user2);
    }
    
    public User? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
    
    public bool IsEmailTaken(string email)
    {
        return _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
    
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