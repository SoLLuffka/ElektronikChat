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
            _server.SignalReadyToServer();
        }

        
        private void Border_MouseDown(object sender, MouseButtonEventArgs e) //poruszanie oknem poprzez belke gorna
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Normal)
                {
                    DragMove();
                }
                else if (WindowState == WindowState.Maximized && e.GetPosition(this).Y < 40) // 40 to wysokość paska tytułowego
                {
                    // Przywróć okno do poprzedniego stanu
                    if (RestoreBounds.Width > 0 && RestoreBounds.Height > 0)
                    {
                        Left = RestoreBounds.Left;
                        Top = RestoreBounds.Top;
                        Width = RestoreBounds.Width;
                        Height = RestoreBounds.Height;
                    }

                    WindowState = WindowState.Normal;
                }
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

        bool check_width = false;
        private void MenuDissapear(object sender, RoutedEventArgs e) //znikajace menu po lewej
        {
            ColumnDefinition columnDefinition = NavBar;

            if (check_width == false)
            {
                
                columnDefinition.Width = new GridLength(0);
                check_width = true;
            }
            else
            {
                
                columnDefinition.Width = new GridLength(120);
                check_width = false;
            }
        }
    }
}
