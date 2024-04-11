using ElektronikChat.Core;
using ElektronikChat.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElektronikChat.ViewModel
{
    class LoginViewModel
    {
        public string Login { get; set; }
        public string Password { get; set; }

        private Server _server;
        public bool IsLoggedIn;

        public RelayCommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            _server = Server.Instance;
            
            //LoginCommand = new RelayCommand(o => _server.LoginUser($"{Login};{Password}"));
            /**/
            LoginCommand = new RelayCommand(async o =>
            {
                await LoginUserAsync();
            });
        }

        private async Task LoginUserAsync()
        {
            _server.LoginUser($"{Login};{Password}");
            _server.ReadPackets();
            _server.DataMatchReceived += DataMatchReceivedHandler;
            //MessageBox.Show(IsLoggedIn.ToString());
            if (IsLoggedIn == true)
            {
                SessionManager.CreateSession(_server.uid, Login);

                SwitchToHomeView();
            }
            else
            {
                //MessageBox.Show("Dane sie nie zgadzaja!");
            }
        }

        private void DataMatchReceivedHandler(bool dataMatch)
        {
            IsLoggedIn = dataMatch;
        }

        private void SwitchToHomeView()
        {
            var mainViewModel = Application.Current.MainWindow.DataContext as MainViewModel;
            if (mainViewModel != null)
            {
                mainViewModel.CurrentView = mainViewModel.HomeVM;
            }
        }
    }
}
