using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;

namespace BointApp.Models;

public partial class AppContext : ObservableObject 
{
    [ObservableProperty]
    private User? _currentUser;

    public DataStore DataStore { get; }
    public RentalService RentalService { get; }

    public AppContext()
    {
        DataStore = new DataStore();
        RentalService = new RentalService();
    }
}