using System;
using System.Collections.ObjectModel; // Zmiana z List
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel; // Dodajemy, aby móc powiadamiać o zmianach

namespace BointApp.Models;

// Klasa dziedziczy teraz z ObservableObject, aby powiadamiać UI o zmianach
public partial class Station : ObservableObject
{
    public Guid StationID { get; set; }
    public string Location { get; set; }
    
    // Zmieniamy Capacity, aby powiadamiało o zmianach
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Blocked))]
    [NotifyPropertyChangedFor(nameof(AvailableSlots))]
    private int _capacity;
    
    public bool Blocked => AvailableBikes.Count >= Capacity;
    public int AvailableSlots => Capacity - AvailableBikes.Count;
    
    // ZMIANA: Używamy ObservableCollection zamiast List<Bike>
    public ObservableCollection<Bike> AvailableBikes { get; private set; } = new();

    public Station(string location, int capacity)
    {
        if (capacity <= 0) throw new Exception("Capacity must be > 0");
        Location = location;
        Capacity = capacity;
        
        // Musimy nasłuchiwać na zmiany w kolekcji, aby zaktualizować właściwości 'Blocked' i 'AvailableSlots'
        AvailableBikes.CollectionChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(Blocked));
            OnPropertyChanged(nameof(AvailableSlots));
        };
    }

    public bool TryAddBike(Bike bike)
    {
        if (Blocked) return false;
        AvailableBikes.Add(bike);
        bike.MarkAsReturned();
        return true;
    }

    public bool TryRemoveBike(Bike bike)
    {
        if (!AvailableBikes.Contains(bike)) return false;
        AvailableBikes.Remove(bike);
        bike.MarkAsRented();
        return true;
    }
}