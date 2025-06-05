namespace BointApp.Models;

public class AppContext
{
    public DataStore DataStore { get; }

    public AppContext()
    {
        DataStore = new DataStore();
    }
}