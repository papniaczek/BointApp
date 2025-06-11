using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using BointApp.Models;
using BointApp.ViewModels;
using BointApp.ViewModels.Authentication;
using BointApp.Views;
using BointApp.Views.Authentication;

namespace BointApp;

public partial class App : Application
{
    private static IClassicDesktopStyleApplicationLifetime? _desktop;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public static new AppContext Context { get; private set; } = null!;

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _desktop = desktop;
            
            Context = new AppContext();
            DisableAvaloniaDataAnnotationValidation();
            
            desktop.ShutdownRequested += (sender, args) =>
            {
                Context.DataStore.SaveChanges();
            };
            
            ShowAuthWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    public static void RequestLogout()
    {
        if (_desktop is null) return;
        
        _desktop.MainWindow?.Close();
        ShowAuthWindow();
    }
    
    private static void ShowAuthWindow()
    {
        if (_desktop is null) return;

        var authViewModel = new AuthViewModel(user =>
        {
            Context.CurrentUser = user;
        
            var mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };

            var oldWindow = _desktop.MainWindow;
            _desktop.MainWindow = mainWindow;
            _desktop.MainWindow.Show();
            oldWindow?.Close();
        });
        
        _desktop.MainWindow = new AuthWindow
        {
            DataContext = authViewModel
        };
        _desktop.MainWindow.Show();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}