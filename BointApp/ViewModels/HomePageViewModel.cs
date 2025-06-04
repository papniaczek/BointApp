using System;
using System.Collections.Generic;
using System.Diagnostics;
using BointApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BointApp.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    [ObservableProperty] private string _numberOfBikes;
    [ObservableProperty] private string _numberOfCityBikes;

    public HomePageViewModel()
    {
        var dataStore = App.Context.DataStore;
        NumberOfBikes = dataStore.Bikes.Count.ToString();
    }
}