using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    internal static class CodingSessionRepository
    {
        internal static List<CodingSession> GetSessions()
        {
            using SqliteConnection connection = DatabaseManager.CreateConnection();
            var sessions = connection.Query<CodingSession>("SELECT * FROM CodingSessions");
            return sessions.ToList();
        }

        internal static int InsertSessions(List<CodingSession> sessions)
        {
            using SqliteConnection connection = DatabaseManager.CreateConnection();
            connection.Open();
            using SqliteTransaction transaction = connection.BeginTransaction();

            string query = "INSERT INTO CodingSessions (Project, Language, StartTime, EndTime) VALUES (@Project, @Language, @StartTime, @EndTime)";
            int rowsInserted = connection.Execute(query, sessions, transaction);

            transaction.Commit();
            return rowsInserted;
        }

        internal static int DeleteSession(int id)
        {
            using SqliteConnection connection = DatabaseManager.CreateConnection();
            string query = "DELETE FROM CodingSessions WHERE Id = @Id";
            return connection.Execute(query, new { Id = id });
        }
    }
}
