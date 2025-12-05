using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Typotrainer.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = "Server=localhost;Port=3306;Database=typotrainer_db;Uid=typotrainer;Pwd=LeerTypen6;";
        }

        // Try to retrieve testdata from database
        public async Task<string> DatabaseTest()
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = "SELECT message FROM test_data LIMIT 1";
                using var command = new MySqlCommand(query, connection);

                var result = await command.ExecuteScalarAsync();

                return result?.ToString() ?? "No data found";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
                return $"Connection failed: {ex.Message}";
            }
        }
    }
}