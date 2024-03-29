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
            _server = new Server();
            //LoginCommand = new RelayCommand(o => _server.LoginUser($"{Login};{Password}"));
            /**/
            LoginCommand = new RelayCommand(o =>
            {
                _server.LoginUser($"{Login};{Password}");
                if(IsLoggedIn == true)
                {
                    SwitchToHomeView();
                } else {
                    MessageBox.Show("Login error!");
                }
            });
        }

        public static void ProccessData(bool isLoggedIn)
        {
            //IsLoggedIn = isLoggedIn;
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
