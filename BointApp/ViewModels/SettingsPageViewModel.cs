using System.Collections.Generic;
using System.Linq;
using BointApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BointApp.ViewModels;

public partial class SettingsPageViewModel : ViewModelBase
{
    // Lista wszystkich użytkowników dostępnych w systemie
    public List<User> Users { get; }

    // Właściwość przechowująca aktualnie wybranego użytkownika.
    // Dzięki CommunityToolkit.Mvvm, zmiana tej właściwości automatycznie
    // wywoła metodę OnCurrentUserChanged.
    [ObservableProperty]
    private User? _currentUser;

    public SettingsPageViewModel()
    {
        Users = App.Context.DataStore.Users.ToList();
        // Na starcie ustawiamy aktualnego użytkownika na tego, który jest w AppContext
        _currentUser = App.Context.CurrentUser; 
    }

    // Ta metoda jest automatycznie wywoływana po każdej zmianie właściwości CurrentUser
    partial void OnCurrentUserChanged(User? value)
    {
        // Jeśli wybrano jakiegoś użytkownika, aktualizujemy go w głównym kontekście aplikacji
        if (value != null)
        {
            App.Context.CurrentUser = value;
        }
    }
}