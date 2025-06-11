using System;
using BointApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BointApp.ViewModels.Authentication;

public partial class AuthViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentViewModel;
    
    private readonly Lazy<LoginViewModel> _loginViewModel;
    private readonly Lazy<RegisterViewModel> _registerViewModel;

    public Action<User> CloseAction { get; }

    public AuthViewModel(Action<User> closeAction)
    {
        CloseAction = closeAction;

        _loginViewModel = new Lazy<LoginViewModel>(() => new LoginViewModel
        {
            OnLoginSuccess = OnLoginSuccess,
            ShowRegisterView = ShowRegisterView
        });

        _registerViewModel = new Lazy<RegisterViewModel>(() => new RegisterViewModel
        {
            ShowLoginView = ShowLoginView
        });
        
        _currentViewModel = _loginViewModel.Value;
    }

    private void OnLoginSuccess(User user)
    {
        CloseAction?.Invoke(user);
    }
    
    private void ShowLoginView() => CurrentViewModel = _loginViewModel.Value;
    private void ShowRegisterView() => CurrentViewModel = _registerViewModel.Value;
}