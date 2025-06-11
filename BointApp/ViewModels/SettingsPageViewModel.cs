using CommunityToolkit.Mvvm.Input;

namespace BointApp.ViewModels;

public partial class SettingsPageViewModel : ViewModelBase
{
    [RelayCommand]
    private void Logout()
    {
        // Wywołujemy statyczną metodę w klasie App, która zajmie się procesem wylogowania
        App.RequestLogout();
    }
}