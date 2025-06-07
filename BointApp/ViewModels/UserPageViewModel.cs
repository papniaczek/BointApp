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
    
    // Kolekcje do przechowywania wypożyczeń
    public ObservableCollection<Rental> ActiveRentals { get; } = new();
    public ObservableCollection<Rental> CompletedRentals { get; } = new();
    
    // Lista stacji, na które można zwrócić rower
    public ObservableCollection<Station> AvailableStations { get; }

    // Wybrana stacja do zwrotu
    [ObservableProperty]
    private Station? _selectedReturnStation;

    public UserPageViewModel()
    {
        _currentUser = App.Context.CurrentUser!;
        _rentalService = App.Context.RentalService;
        
        // Filtrujemy stacje, na które można dokonać zwrotu (te, które nie są pełne)
        AvailableStations = new ObservableCollection<Station>(App.Context.DataStore.GetUnblockedStations());
        SelectedReturnStation = AvailableStations.FirstOrDefault();

        LoadRentals();
    }

    private void LoadRentals()
    {
        // Czyszczenie list przed załadowaniem
        ActiveRentals.Clear();
        CompletedRentals.Clear();
        
        // Ładujemy aktywne wypożyczenia dla bieżącego użytkownika
        var active = _rentalService.ActiveRentals.Where(r => r.User.UserID == _currentUser.UserID);
        foreach(var rental in active)
        {
            ActiveRentals.Add(rental);
        }
        
        // Ładujemy historię wypożyczeń dla bieżącego użytkownika
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
            // Po udanym zwrocie, odświeżamy obie listy
            LoadRentals();
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Błąd zwrotu roweru: {ex.Message}");
        }
    }
}