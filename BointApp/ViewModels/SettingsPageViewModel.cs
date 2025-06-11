using CommunityToolkit.Mvvm.Input;

namespace BointApp.ViewModels;

public partial class SettingsPageViewModel : ViewModelBase
{
    [RelayCommand]
    private void Logout()
    {
        App.RequestLogout();
    }
}