using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;

namespace BointApp.Models;

public partial class AppContext : ObservableObject 
{
    [ObservableProperty]
    private User? _currentUser;

    public DataStore DataStore { get; }
    public RentalService RentalService { get; } // Dodajemy serwis wypożyczeń

    public AppContext()
    {
        DataStore = new DataStore();
        RentalService = new RentalService(); // Tworzymy instancję
        
        CurrentUser = DataStore.Users.FirstOrDefault();
    }
}