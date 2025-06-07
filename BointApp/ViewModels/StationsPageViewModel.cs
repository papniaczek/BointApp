using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BointApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppContext = BointApp.Models.AppContext;

namespace BointApp.ViewModels;

public partial class StationsPageViewModel : ViewModelBase
{
    private readonly DataStore _dataStore;

    [ObservableProperty]
    private bool _isAdmin;
    
    [ObservableProperty]
    private Station? _selectedStation;

    // Kolekcja stacji, inicjalizowana od razu w deklaracji
    public ObservableCollection<Station> Stations { get; } = new(App.Context.DataStore.Stations.ToList());

    // Właściwości dla formularza dodawania nowej stacji
    [ObservableProperty]
    private string _newStationLocation = "";
    [ObservableProperty]
    private int _newStationCapacity = 10;

    public StationsPageViewModel()
    {
        _dataStore = App.Context.DataStore;
        UpdateUserRole();
        App.Context.PropertyChanged += OnAppContextPropertyChanged;
    }

    private void UpdateUserRole()
    {
        IsAdmin = App.Context.CurrentUser?.Role == UserRole.Admin;
    }

    private void OnAppContextPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppContext.CurrentUser))
        {
            UpdateUserRole();
        }
    }

    [RelayCommand]
    private void AddStation()
    {
        if (string.IsNullOrWhiteSpace(NewStationLocation) || NewStationCapacity <= 0) return;

        var newStation = new Station(NewStationLocation, NewStationCapacity);
        
        _dataStore.AddStation(newStation);
        Stations.Add(newStation);

        // Wyczyszczenie formularza
        NewStationLocation = "";
        NewStationCapacity = 10;
    }

    [RelayCommand]
    private void RemoveStation(Station station)
    {
        if (station is null) return;
        
        // Można dodać logikę zapobiegającą usunięciu stacji, jeśli są na niej rowery
        if (station.AvailableBikes.Any())
        {
            // Na razie pomijamy, ale to dobre miejsce na walidację
        }

        _dataStore.RemoveStation(station);
        Stations.Remove(station);
    }
    
    [RelayCommand]
    private void RentBike(Bike bike)
    {
        if (bike is null || App.Context.CurrentUser is null) return;

        // Znajdź stację, na której jest ten rower
        var station = Stations.FirstOrDefault(s => s.AvailableBikes.Contains(bike));
        if (station is null) return;

        try
        {
            // Użyj centralnego serwisu do rozpoczęcia wypożyczenia
            App.Context.RentalService.StartRental(App.Context.CurrentUser, bike, station);
            // Dzięki ObservableCollection, UI odświeży się automatycznie!
        }
        catch (Exception ex)
        {
            // Tutaj można by wyświetlić okienko z błędem, np. o niskim stanie baterii
            // na razie błąd pojawi się tylko w konsoli debugowej
            System.Diagnostics.Debug.WriteLine($"Błąd wypożyczenia: {ex.Message}");
        }
    }
}