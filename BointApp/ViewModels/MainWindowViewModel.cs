using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BointApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isPaneOpen = true;
    // Przywracamy inicjalizację 'CurrentPage' bezpośrednio w deklaracji
    [ObservableProperty] private ViewModelBase _currentPage = new HomePageViewModel();
    [ObservableProperty] private ListItemTemplate? _selectedListItem;
    
    public void GoToUserPage()
    {
        CurrentPage = new UserPageViewModel();
        SelectedListItem = null;
    }
    
    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;
        var instance = Activator.CreateInstance(value.ModelType);
        if (instance is null) return;
        CurrentPage = (ViewModelBase)instance;
    }

    public ObservableCollection<ListItemTemplate> Items { get; } = new()
    {
        new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular"),
        new ListItemTemplate(typeof(BikesPageViewModel), "VehicleBicycleRegular"),
        new ListItemTemplate(typeof(StationsPageViewModel), "MapRegular"),
        new ListItemTemplate(typeof(SettingsPageViewModel), "SettingsRegular")
    };

    [RelayCommand] private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
}

public class ListItemTemplate
{
    public ListItemTemplate(Type modelType, string iconKey)
    {
        ModelType = modelType;
        Label = modelType.Name.Replace("PageViewModel", "");

        // Zostawiamy bezpieczną wersję ładowania ikon
        if (Application.Current != null && Application.Current.TryFindResource(iconKey, out var res) && res is StreamGeometry geometry)
        {
            ListItemIcon = geometry;
        }
        else
        {
            ListItemIcon = new StreamGeometry();
        }
    }
    
    public string Label { get; }
    public Type ModelType { get; }
    public StreamGeometry ListItemIcon { get; }
}