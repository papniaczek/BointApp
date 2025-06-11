using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BointApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BointApp.ViewModels;

public partial class BikesPageViewModel : ViewModelBase
{
    private readonly DataStore _dataStore;

    [ObservableProperty] 
    private bool _isAdmin;
    
    [ObservableProperty] 
    private bool _isAddPanelVisible = false;

    public ObservableCollection<Bike> Bikes { get; } = new(App.Context.DataStore.BikesView.ToList());

    [ObservableProperty] private string _newBikeBrand = "";
    [ObservableProperty] private int _selectedBikeTypeIndex = 0;
    [ObservableProperty] private bool _newCityBikeHasBasket = false;
    [ObservableProperty] private int _newMountainBikeWheelSize = 26;
    [ObservableProperty] private int _newElectricBikeBattery = 100;

    public BikesPageViewModel()
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
    private void ShowAddPanel() => IsAddPanelVisible = true;
    
    [RelayCommand]
    private void HideAddPanel() => IsAddPanelVisible = false;

    [RelayCommand]
    private void AddBike()
    {
        if (string.IsNullOrWhiteSpace(NewBikeBrand)) return;

        Bike newBike;
        switch (SelectedBikeTypeIndex)
        {
            case 0: newBike = new CityBike(NewBikeBrand, NewCityBikeHasBasket); break;
            case 1: newBike = new MountainBike(NewBikeBrand, NewMountainBikeWheelSize); break;
            case 2: newBike = new ElectricBike(NewBikeBrand, NewElectricBikeBattery); break;
            default: return;
        }
        
        _dataStore.AddBike(newBike);
        
        var firstAvailableStation = _dataStore.StationsView.FirstOrDefault(s => !s.Blocked);
        if (firstAvailableStation != null)
        {
            firstAvailableStation.TryAddBike(newBike);
        }
        
        Bikes.Add(newBike);
        NewBikeBrand = "";
        NewCityBikeHasBasket = false;
        NewMountainBikeWheelSize = 26;
        NewElectricBikeBattery = 100;
        IsAddPanelVisible = false;
    }

    [RelayCommand]
    private void RemoveBike(Bike bike)
    {
        if (bike is null) return;
        _dataStore.RemoveBike(bike);
        Bikes.Remove(bike);
    }
}