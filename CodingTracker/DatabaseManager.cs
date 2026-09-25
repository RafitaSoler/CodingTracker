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

        private static SqliteConnection CreateConnection()
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
        }

        internal static bool IsEmpty()
        {
            using SqliteConnection connection = CreateConnection();
            int count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM CodingSessions");
            return count == 0;
        }

        internal static List<CodingSession> GetSessions()
        {
            using SqliteConnection connection = CreateConnection();
            var sessions = connection.Query<CodingSession>("SELECT * FROM CodingSessions");
            return sessions.ToList();
        }

        internal static void InsertSessions(List<CodingSession> sessions)
        {
            using SqliteConnection connection = CreateConnection();
            connection.Open();
            using SqliteTransaction transaction = connection.BeginTransaction();

            string query = "INSERT INTO CodingSessions (Project, Language, StartTime, EndTime) VALUES (@Project, @Language, @StartTime, @EndTime)";
            connection.Execute(query, sessions, transaction);

            transaction.Commit();
        }
    }
}
