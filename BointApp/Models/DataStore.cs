using System;
using System.Collections.Generic;
using System.Linq;
using BointApp.Services;

namespace BointApp.Models;

public class DataStore
{
    public List<Bike> Bikes { get; private set; } = new();
    public List<Station> Stations { get; private set; } = new();
    public List<User> Users { get; private set; } = new();
    public List<Rental> _rentals = new();

    public IReadOnlyList<Bike> BikesView => Bikes.AsReadOnly();
    public IReadOnlyList<Station> StationsView => Stations.AsReadOnly();
    public IReadOnlyList<User> UsersView => Users.AsReadOnly();
    public IReadOnlyList<Rental> Rentals => _rentals.AsReadOnly();
    
    private readonly JsonDataService _dataService = new("datastore.json");

    public DataStore()
    {
        LoadOrSeedData();
    }
    
    private void LoadOrSeedData()
    {
        var loadedData = _dataService.LoadData();
        if (loadedData != null)
        {
            // Jeśli dane zostały wczytane z pliku
            Bikes = loadedData.Bikes;
            Users = loadedData.Users;
            Stations = loadedData.Stations;

            // Ważne: Musimy ponownie powiązać rowery ze stacjami, bo referencje po deserializacji
            // mogą wymagać odświeżenia.
            // W tym przypadku Newtonsoft.Json z PreserveReferencesHandling powinien sobie z tym poradzić,
            // więc dodatkowy kod nie jest konieczny.
        }
        else
        {
            // Jeśli plik nie istnieje (pierwsze uruchomienie), tworzymy dane i zapisujemy je
            SeedInitialData();
            SaveChanges();
        }
    }
    
    public void SaveChanges()
    {
        var dataToSave = new DataContainer
        {
            Bikes = this.Bikes,
            Stations = this.Stations,
            Users = this.Users
        };
        _dataService.SaveData(dataToSave);
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
        var user1 = new User("Kacper", "Strzesniewski", "a@a.a", "aaa", UserRole.Admin); // Admin
        
        AddUser(user1);
        AddUser(user2);
    }
    
    public User? GetUserByEmail(string email)
    {
        return Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
    
    public bool IsEmailTaken(string email)
    {
        return Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
    
    public void AddBike(Bike bike) => Bikes.Add(bike);
    public void AddStation(Station station) => Stations.Add(station);
    public void AddUser(User user) => Users.Add(user);
    
    public void RemoveBike(Bike bike) => Bikes.Remove(bike);
    public void RemoveStation(Station station) => Stations.Remove(station);
    public void RemoveUser(User user) => Users.Remove(user);
    
    // Filters for searching etc.
    // Bikes
    public IEnumerable<Bike> GetAvailableBikes() => Bikes.FindAll(b => b.IsAvailable);
    public IEnumerable<CityBike> GetAllCityBikes() => Bikes.OfType<CityBike>().ToList();
    public IEnumerable<MountainBike> GetAllMountainBikes() => Bikes.OfType<MountainBike>().ToList();
    public IEnumerable<ElectricBike> GetAllElectricBikes() => Bikes.OfType<ElectricBike>().ToList();
    
    // Stations
    public IEnumerable<Station> GetAvailableStations() => Stations.FindAll(s => s.Blocked == false);
    public IEnumerable<Station> GetBlockedStations() => Stations.FindAll(s => s.Blocked);
    public IEnumerable<Station> GetUnblockedStations() => Stations.FindAll(s => s.Blocked == false);
    
    // Rentals
    public IEnumerable<Rental> GetLast10ActiveRentals() => _rentals
        .Where(r => r.EndTime == null)
        .OrderByDescending(r => r.StartTime)
        .Take(10);
}