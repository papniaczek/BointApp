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
    
    [ObservableProperty]
    private bool _isAddPanelVisible = false;

    public ObservableCollection<Station> Stations { get; } = new(App.Context.DataStore.Stations.ToList());

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
    private void ShowAddPanel()
    {
        IsAddPanelVisible = true;
    }
    
    [RelayCommand]
    private void HideAddPanel()
    {
        IsAddPanelVisible = false;
    }

    [RelayCommand]
    private void AddStation()
    {
        if (string.IsNullOrWhiteSpace(NewStationLocation) || NewStationCapacity <= 0) return;

        var newStation = new Station(NewStationLocation, NewStationCapacity);
        
        _dataStore.AddStation(newStation);
        Stations.Add(newStation);

        NewStationLocation = "";
        NewStationCapacity = 10;
        
        IsAddPanelVisible = false;
    }

    [RelayCommand]
    private void RemoveStation(Station station)
    {
        if (station is null) return;
        
        if (station.AvailableBikes.Any())
        {
            // nothing rn
        }

        _dataStore.RemoveStation(station);
        Stations.Remove(station);
    }
    
    [RelayCommand]
    private void RentBike(Bike bike)
    {
        if (bike is null || App.Context.CurrentUser is null) return;

        var station = Stations.FirstOrDefault(s => s.AvailableBikes.Contains(bike));
        if (station is null) return;

        try
        {
            App.Context.RentalService.StartRental(App.Context.CurrentUser, bike, station);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"{ex.Message}");
        }
    }
}