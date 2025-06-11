using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BointApp.ViewModels.Authentication;

public partial class LoginViewModel : ViewModelBase
{
    public Action<Models.User> OnLoginSuccess { get; set; }
    public Action ShowRegisterView { get; set; }

    [ObservableProperty] private string _email = "";
    [ObservableProperty] private string _password = "";
    [ObservableProperty] private string? _errorMessage;

    [RelayCommand]
    private void Login()
    {
        ErrorMessage = null;
        var user = App.Context.DataStore.GetUserByEmail(Email);

        if (user != null && user.VerifyPassword(Password))
        {
            OnLoginSuccess?.Invoke(user);
        }
        else
        {
            ErrorMessage = "Invalid e-mail or password";
        }
    }

    [RelayCommand]
    private void GoToRegister()
    {
        ShowRegisterView?.Invoke();
    }
}