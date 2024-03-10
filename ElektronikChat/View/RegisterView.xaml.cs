using ElektronikChat.Core;
using ElektronikChat.ViewModel;
using ElektronikChat.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace ElektronikChat.View
{
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private bool ValidateForm(RegisterViewModel data)
        {
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            var passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
            var namePattern = @"^[A-Z][a-z]+$";
            var surnamePattern = @"^[A-Z][a-z]+(?:[-' ][A-Z][a-z]+)*$";

            if (!Regex.IsMatch(data.Email, emailPattern))
            {
                MessageBox.Show("Proszę podać prawidłowy adres email.");
                return false;
            }

            if (!Regex.IsMatch(data.Password, passwordPattern))
            {
                MessageBox.Show("Hasło jest nieprawidłowe. Musi zawierać co najmniej 8 znaków, w tym literę i cyfrę.");
                return false;
            }

            if (!Regex.IsMatch(data.Login, namePattern)) // Do poprawy w bazie do jebniecia n uq i zmienienie tego if'a
            {
                MessageBox.Show("Login jest nieprawidłowe. Musi on byc unikalny.");
                return false;
            }

            if (!Regex.IsMatch(data.FirstName, namePattern))
            {
                MessageBox.Show("Imie jest nieprawidłowe. Musi zaczynac sie ono z duzej litery.");
                return false;
            }

            if (!Regex.IsMatch(data.LastName, surnamePattern))
            {
                MessageBox.Show("Nazwisko jest nieprawidłowe. Musi zaczynac sie ono z duzej litery.");
                return false;
            }

            return true;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterViewModel data = new RegisterViewModel
            {
                FirstName = this.firstname.Text,
                LastName = this.lastname.Text,
                Email = this.email.Text,
                Login = this.login.Text,
                Password = this.password.Password,
                PasswordCheck = this.password_check.Password,
            };

            Window parentWindow = Window.GetWindow(this);
            Register.Registration(data, parentWindow);

            if (ValidateForm(data))
            {
                DBHelper.InsertUser(data);
            } else {
                MessageBox.Show("Internal Server Error");
            }
        }
    }
}
