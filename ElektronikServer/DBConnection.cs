using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;

public class DBConnection
{
    private SQLiteConnection sqliteConn;

    public void InitializeDatabase()
    {
        string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ElektronikChatDB.db");
        string connectionString = $"Data Source={dbPath};Version=3;";
        sqliteConn = new SQLiteConnection(connectionString);
        try
        {
            sqliteConn.Open();
            // Połączenie jest otwarte, tutaj możesz wykonywać zapytania
        }
        catch (Exception ex)
        {
            // Obsługa błędów
            Console.WriteLine($"Error connecting to database: {ex.Message}");
        }
    }

    public SQLiteConnection GetConnection()
    {
        return sqliteConn;
    }
}