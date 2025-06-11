using System.Collections.ObjectModel;
using System.Linq;
using BointApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BointApp.ViewModels;

public partial class UserPageViewModel : ViewModelBase
{
    private readonly User _currentUser;
    private readonly RentalService _rentalService;
    
    public ObservableCollection<Rental> ActiveRentals { get; } = new();
    public ObservableCollection<Rental> CompletedRentals { get; } = new();
    public ObservableCollection<Station> AvailableStations { get; }

    [ObservableProperty]
    private Station? _selectedReturnStation;

    public UserPageViewModel()
    {
        _currentUser = App.Context.CurrentUser!;
        _rentalService = App.Context.RentalService;
        
        AvailableStations = new ObservableCollection<Station>(App.Context.DataStore.GetUnblockedStations());
        SelectedReturnStation = AvailableStations.FirstOrDefault();

        LoadRentals();
    }

    private void LoadRentals()
    {
        ActiveRentals.Clear();
        CompletedRentals.Clear();
        
        var active = _rentalService.ActiveRentals.Where(r => r.User.UserID == _currentUser.UserID);
        foreach(var rental in active)
        {
            ActiveRentals.Add(rental);
        }
        
        var completed = _rentalService.CompletedRentals.Where(r => r.User.UserID == _currentUser.UserID);
        foreach(var rental in completed)
        {
            CompletedRentals.Add(rental);
        }
    }

    [RelayCommand]
    private void ReturnBike(Rental rentalToReturn)
    {
        if (rentalToReturn is null || SelectedReturnStation is null) return;

        try
        {
            _rentalService.EndRental(rentalToReturn, SelectedReturnStation);
            LoadRentals();
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"{ex.Message}");
        }
    }
}