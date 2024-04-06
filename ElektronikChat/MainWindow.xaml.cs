using ElektronikChat.Core;
using ElektronikChat.Core.Net;
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

        //private Server _server;

        public MainWindow()
        {
            InitializeComponent();
            dbConnection = new DBConnection();
            dbConnection.InitializeDatabase();

            //_server = new Server();
            //_server.ConnectToServer("x");
        }

        private void Border_MouseDown(object sender, MouseEventArgs e) //ruszanie oknem trzymajac headbar'a
        {
            if(e.LeftButton == MouseButtonState.Pressed) 
            {
                DragMove();
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e) //minimalizacja okna
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Scale_Click(object sender, RoutedEventArgs e) //skalowanie okna
        {
            if(Application.Current.MainWindow.WindowState != WindowState.Maximized) 
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e) //wylaczenie calej aplikacji
        {
            Application.Current.Shutdown();
        }
    }
}
