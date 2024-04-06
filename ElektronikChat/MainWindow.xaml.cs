using ElektronikChat.Core;
using ElektronikChat.Core.Net;
using ElektronikChat.ViewModel;
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

        private Server _server;

        public MainWindow()
        {
            InitializeComponent();
            dbConnection = new DBConnection();
            dbConnection.InitializeDatabase();

            _server = Server.Instance;
            _server.ConnectToServer("XD");
        }
    }
}
