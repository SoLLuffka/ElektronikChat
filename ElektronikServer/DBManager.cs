using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Threading.Tasks;
using ElektronikServer.Net.IO;

namespace ElektronikServer
{
    public static class DBManager
    {
        private static string dbPath = "ElektronikChatDB.db";
        private static string connectionString = $"Data Source={dbPath};Version=3;";

        public static async Task<bool> RegisterUserAsync(string firstname, string lastname, string email, string login, string password)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO Users(login, password, firstname, lastname, email) VALUES (@Login, @Password, @FirstName, @LastName, @Email)";
                    cmd.Parameters.AddWithValue("@FirstName", firstname);
                    cmd.Parameters.AddWithValue("@LastName", lastname);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Password", password); // Make sure to hash this

                    Console.WriteLine($"Rejestrowanie użytkownika. Login: {login}, Hasło: {password}, Imie: {firstname}, Nazwisko: {lastname}, Email: {email}");
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
        public static async Task<bool> UserExistsAsync(string login, string password, string uid)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmdListUsers = new SQLiteCommand("SELECT login, password FROM Users", conn))
                {
                    using (var reader = await cmdListUsers.ExecuteReaderAsync())
                    {
                        Console.WriteLine("Wszyscy użytkownicy w bazie danych wraz z hasłami:");
                        while (await reader.ReadAsync())
                        {
                            var loginn = reader.GetString(0); // Pobranie loginu użytkownika
                            var passwordd = reader.GetString(1); // Pobranie hasła użytkownika
                            Console.WriteLine($"Login: {loginn}, Hasło: {passwordd}");
                        }
                    }
                }
                using (var cmd = new SQLiteCommand($"SELECT COUNT(*) FROM Users WHERE trim(login) = trim(@Login) AND trim(password) = trim(@Password)", conn))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Password", password);
                    var result = (long)await cmd.ExecuteScalarAsync();
                    if (result > 0)
                    {
                        program.DataMatch(true, uid);
                        Console.WriteLine("Login&Password data match");
                    }
                    else
                    {
                        program.DataMatch(false, uid);
                        Console.WriteLine("Login&Password data isn't match");
                    }
                    return result > 0;
                }
            }
        }

        /*public static void DataMatch(bool value)
        {
            var msgPacket = new PacketBuilder();
            msgPacket.WriteOpCode(30);
            msgPacket.WriteBoolean(value);
        }*/
    }
}
