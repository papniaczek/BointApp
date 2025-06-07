using System.Linq;
using BointApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BointApp.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    [ObservableProperty] private string _numberOfBikes;
    [ObservableProperty] private string _numberOfCityBikes;
    [ObservableProperty] private string _numberOfElectricBikes;
    [ObservableProperty] private string _numberOfMountainBikes;
    
    [ObservableProperty] private string _numberOfStations;
    [ObservableProperty] private string _numberOfBlockedStations;
    [ObservableProperty] private string _numberOfUnblockedStations;

    public HomePageViewModel()
    {
        var dataStore = App.Context.DataStore;
        
        NumberOfBikes = dataStore.Bikes.Count.ToString();
        NumberOfCityBikes = dataStore.GetAllCityBikes().Count().ToString();
        NumberOfMountainBikes = dataStore.GetAllMountainBikes().Count().ToString();
        NumberOfElectricBikes = dataStore.GetAllElectricBikes().Count().ToString();
        
        NumberOfStations = dataStore.Stations.Count.ToString();
        NumberOfBlockedStations = dataStore.GetBlockedStations().Count().ToString();
        NumberOfUnblockedStations = dataStore.GetUnblockedStations().Count().ToString();
    }
}