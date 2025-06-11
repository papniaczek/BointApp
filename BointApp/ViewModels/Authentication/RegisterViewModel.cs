using System;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BointApp.ViewModels.Authentication;

public partial class RegisterViewModel : ViewModelBase
{
    public Action ShowLoginView { get; set; }

    [ObservableProperty] private string _name = "";
    [ObservableProperty] private string _surname = "";
    [ObservableProperty] private string _email = "";
    [ObservableProperty] private string _password = "";
    [ObservableProperty] private string _confirmPassword = "";
    [ObservableProperty] private string? _errorMessage;
    [ObservableProperty] private bool _isSuccess = false;

    [RelayCommand]
    private void Register()
    {
        ErrorMessage = null;
        IsSuccess = false;

        if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            ErrorMessage = "E-mail incorrect.";
            return;
        }
        
        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Both passwords must match";
            return;
        }
        
        if (App.Context.DataStore.IsEmailTaken(Email))
        {
            ErrorMessage = "This e-mail is already taken.";
            return;
        }

        try
        {
            var newUser = new Models.User(Name, Surname, Email, Password);
            App.Context.DataStore.AddUser(newUser);
            IsSuccess = true;
        }
        catch (Exception e)
        {
            ErrorMessage = $"{e.Message}";
        }
    }
    
    [RelayCommand]
    private void GoToLogin()
    {
        ShowLoginView?.Invoke();
    }
}