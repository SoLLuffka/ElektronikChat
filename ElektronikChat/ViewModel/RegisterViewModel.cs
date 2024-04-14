using ElektronikChat.Core;
using ElektronikChat.Core.Net;
using ElektronikChat.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using ElektronikChat.View;
using ElektronikChat.ViewModel;
using System.Windows.Controls;

namespace ElektronikChat.ViewModel
{
    class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PasswordCheck { get; set; }

        private Server _server;

        public RelayCommand RegisterCommand { get; set; }

        public RegisterViewModel()
        {
            _server = Server.Instance;
            RegisterCommand = new RelayCommand(o =>
            {
                if (ValidateForm())
                {
                    _server.RegisterUser($"{FirstName};{LastName};{Email};{Login};{Password}");
                    SwitchToLoginView();
                }
            });
        }

        private void SwitchToLoginView()
        {
            var mainViewModel = Application.Current.MainWindow.DataContext as MainViewModel;
            if (mainViewModel != null)
            {
                mainViewModel.CurrentView = mainViewModel.LoginVM;
            }
        }

        private bool ValidateForm()
        {
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            var passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
            var namePattern = @"^[A-Z][a-z]+$";
            var surnamePattern = @"^[A-Z][a-z]+(?:[-' ][A-Z][a-z]+)*$";
            var loginPattern = @"^[a-zA-Z0-9]+$";

            if (!Regex.IsMatch(Email, emailPattern))
            {
                MessageBox.Show("Proszę podać prawidłowy adres email.");
                return false;
            }

            if (!Regex.IsMatch(Password, passwordPattern))
            {
                MessageBox.Show("Hasło jest nieprawidłowe. Musi zawierać co najmniej 8 znaków, w tym literę i cyfrę.");
                return false;
            }

            if (!Regex.IsMatch(Login, loginPattern)) // Do poprawy w bazie do jebniecia n uq i zmienienie tego if'a
            {
                MessageBox.Show("Login jest nieprawidłowe. Musi on byc unikalny.");
                return false;
            }

            if (!Regex.IsMatch(FirstName, namePattern))
            {
                MessageBox.Show("Imie jest nieprawidłowe. Musi zaczynac sie ono z duzej litery.");
                return false;
            }

            if (!Regex.IsMatch(LastName, surnamePattern))
            {
                MessageBox.Show("Nazwisko jest nieprawidłowe. Musi zaczynac sie ono z duzej litery.");
                return false;
            }

            if(Password != PasswordCheck)
            {
                MessageBox.Show("Hasła muszą być takie same!");
                return false;
            }

            return true;
        }
    }
}
