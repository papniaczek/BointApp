using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BointApp.Models;

public partial class AppContext : ObservableObject 
{
    [ObservableProperty]
    private User? _currentUser;

    public DataStore DataStore { get; }

    public AppContext()
    {
        DataStore = new DataStore();
        CurrentUser = DataStore.Users.FirstOrDefault(); // First user from list is logged
    }
}