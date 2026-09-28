using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CodingTracker
{
    internal static class DatabaseManager
    {
        private static IConfiguration _config = null!;
        private static string _connectionString = "";

        internal static void SetConfiguration(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        internal static SqliteConnection CreateConnection()
        {
            return new(_connectionString);
        }

        internal static void Start()
        {
            using SqliteConnection connection = CreateConnection();
            string query =
                """
                CREATE TABLE IF NOT EXISTS CodingSessions ( 
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Project TEXT,
                    Language TEXT,
                    StartTime TEXT,
                    EndTime TEXT
                    )
                """;
            connection.Execute(query);
            Initialize();
        }

        /// <summary>
        /// Inserts some default values for the user to view
        /// </summary>
        private static void Initialize()
        {
            if (IsEmpty())
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
                CodingSessionRepository.InsertSessions(sessions);
            }
        }

        private static bool IsEmpty()
        {
            using SqliteConnection connection = CreateConnection();
            int count = connection.ExecuteScalar<int>("SELECT EXISTS (SELECT 1 FROM CodingSessions)");
            return count == 0;
        }
    }
}
