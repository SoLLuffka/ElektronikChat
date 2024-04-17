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

        bool check_width = false;

        // Deklaruj publiczną właściwość, aby udostępnić dostęp do przycisku homeNav z innych miejsc w aplikacji
        public RadioButton HomeNavButton
        {
            get { return homeNav; }
        }

        public MainWindow()
        {
            InitializeComponent();
            dbConnection = new DBConnection();
            dbConnection.InitializeDatabase();
            ConnectToServer();

            MainViewModel viewModel = (MainViewModel)DataContext;// Pobieranie kontekstu danych jako instancję MainViewModel
            viewModel.CurrentViewChanged += ViewModel_CurrentViewChanged;
            DataContext = viewModel;
            LoginSecure(viewModel); //pierwsze sprawdzenie widoku
        }

        private void ConnectToServer()
        {
            try
            {
                _server = Server.Instance;
                _server.ConnectToServer("XD");
                _server.SignalReadyToServer();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udało się połączyć z serwerem");
                //Application.Current.Shutdown();
            }
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

        private void MenuDissapear(object sender, RoutedEventArgs e) //znikajace menu po lewej
        {
            ColumnDefinition columnDefinition = NavBar;


            if (check_width == false)
            {
                //MenuButton - ten przyciski co z trzema liniami do chowania i pojawiania menu
                columnDefinition.Width = new GridLength(0);
                Grid.SetColumn(MenuButton, 1);
                MenuButton.HorizontalAlignment = HorizontalAlignment.Left;
                check_width = true;
            }
            else
            {
                columnDefinition.Width = new GridLength(120);
                Grid.SetColumn(MenuButton, 0);
                MenuButton.HorizontalAlignment = HorizontalAlignment.Right;
                check_width = false;
            }
        }

        private void ViewModel_CurrentViewChanged(object sender, EventArgs e)
        {
            // Wywołujemy metodę LoginSecure() przy każdej zmianie widoku
            LoginSecure((MainViewModel)sender);
        }

        private void LoginSecure(MainViewModel viewModel) //zabezpieczenie przed zalogowaniem
        {
            if (viewModel.CurrentView is RegisterViewModel || viewModel.CurrentView is LoginViewModel)
            {
                HideNav();
            }
            else
            {
                ShowNav();
            }
        }

        private void HideNav()
        {
            ColumnDefinition columnDefinition = NavBar;
            columnDefinition.Width = new GridLength(0);
            MenuButton.Visibility = Visibility.Collapsed;
            check_width = true;
        }

        private void ShowNav()
        {
            ColumnDefinition columnDefinition = NavBar;
            columnDefinition.Width = new GridLength(120);
            MenuButton.Visibility = Visibility.Visible;
            check_width = false;
        }

    }
}
