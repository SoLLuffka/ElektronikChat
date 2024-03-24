using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace ElektronikServer
{
    public static class DBManager
    {
        private static string dbPath = "ElektronikChatDB.db";
        private static string connectionString = $"Data Source={dbPath};Version=3;";

        public static async Task<bool> RegisterUserAsync(string login, string password, string firstname, string lastname, string email)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO Users(login, password, firstname, lastname, email) VALUES (@Login, @Password, @FirstName, @LastName, @Email)";
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Password", password); // Make sure to hash this
                    cmd.Parameters.AddWithValue("@FirstName", firstname);
                    cmd.Parameters.AddWithValue("@LastName", lastname);
                    cmd.Parameters.AddWithValue("@Email", email);
                    int result = await cmd.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        Console.WriteLine($"User {login} registered successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to register user {login}.");
                    }
                    return result > 0;
                }
            }
        }
        public static async Task<bool> UserExistsAsync(string login)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SQLiteCommand($"SELECT COUNT(1) FROM Users WHERE login = @Login", conn))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    var result = (long)await cmd.ExecuteScalarAsync();
                    return result > 0;
                }
            }
        }
    }
}
