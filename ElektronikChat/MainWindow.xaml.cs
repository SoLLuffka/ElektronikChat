using ElektronikChat.Core;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElektronikChat
{
    public partial class MainWindow : Window
    {
        private DBConnection dbConnection;
        private Core.Register register;

        public MainWindow()
        {
            InitializeComponent();
            dbConnection = new DBConnection();
            dbConnection.InitializeDatabase();
            ExecuteSelectQuery(); // Dodaj to wywołanie metody tutaj
            register = new Core.Register();
        }

        private void ExecuteSelectQuery()
        {
            string sql = "SELECT * FROM Users";
            using (var command = new SQLiteCommand(sql, dbConnection.GetConnection()))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        MessageBox.Show("Brak danych w tabeli Users.");
                        return;
                    }

                    while (reader.Read())
                    {
                        // Przetwarzaj wyniki, np.:
                        string id = reader["idUsers"].ToString();
                        string firstname = reader["firstname"].ToString();
                        string lastname = reader["lastname"].ToString();
                        MessageBox.Show($"{firstname} {lastname} {id}"); // Wyświetl wyniki w MessageBox
                                                            // I tak dalej dla innych kolumn...
                    }
                }
            }
        }
    }
}
