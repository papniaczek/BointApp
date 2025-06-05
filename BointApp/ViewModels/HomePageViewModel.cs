using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        NumberOfBikes = dataStore.Bikes != null ? dataStore.Bikes.Count.ToString() : "0";
        NumberOfCityBikes = dataStore.GetAllCityBikes() != null ? dataStore.GetAllCityBikes().Count().ToString() : "0";
        NumberOfMountainBikes = dataStore.GetAllMountainBikes() != null ? dataStore.GetAllMountainBikes().Count().ToString() : "0";
        NumberOfElectricBikes = dataStore.GetAllElectricBikes() != null ? dataStore.GetAllElectricBikes().Count().ToString() : "0";
        
        NumberOfStations = dataStore.Stations != null ? dataStore.Stations.Count.ToString() : "0";
        NumberOfBlockedStations = dataStore.GetBlockedStations() != null ? dataStore.GetBlockedStations().Count().ToString() : "0";
        NumberOfUnblockedStations = dataStore.GetUnblockedStations() != null ? dataStore.GetUnblockedStations().Count().ToString() : "0";
    }
}