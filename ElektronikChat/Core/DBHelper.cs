using ElektronikChat.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElektronikChat.Core
{
    public class DBHelper
    {
        private static string dbPath = "ElektronikChatDB.db"; // Zaktualizuj ścieżkę do swojej bazy danych
        private static string connectionString = $"Data Source={dbPath};Version=3;";

        public static void InsertUser(RegisterViewModel data)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "INSERT INTO Users (login, password, firstname, lastname, email) VALUES (@Login, @Password, @FirstName, @LastName, @Email);";
                    cmd.Parameters.AddWithValue("@Login", data.Login);
                    cmd.Parameters.AddWithValue("@Password", data.Password);
                    cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", data.LastName);
                    cmd.Parameters.AddWithValue("@Email", data.Email);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}