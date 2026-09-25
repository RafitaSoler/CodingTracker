using CodingTracker;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        DatabaseManager.SetConfiguration(config);
        DatabaseManager.Start();
        InitializeDatabase();

        UIController ui = new(config);
        ui.MainMenu();
    }

    private static void InitializeDatabase()
    {
        if(DatabaseManager.IsEmpty())
        {
            List<CodingSession> sessions = new()
            {
                new(".NET Course", "C#", new DateTime(2026, 9, 22, 18, 30, 0), null),
                new() { Project = "Coding Tracker", Language = "C#",
                    StartTime = new DateTime(2026, 9, 21, 18, 30, 0),
                    EndTime   = new DateTime(2026, 9, 21, 20, 15, 0) },
                new() { Project = "Platformer", Language = "C++",
                    StartTime = new DateTime(2026, 9, 22, 10, 0, 0),
                    EndTime   = new DateTime(2026, 9, 22, 12, 45, 0) }
            };
            DatabaseManager.InsertSessions(sessions);
        }
    }
}