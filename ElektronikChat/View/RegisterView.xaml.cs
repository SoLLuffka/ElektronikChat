using ElektronikChat.Core;
using ElektronikChat.ViewModel;
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

namespace ElektronikChat.View
{
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
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
        }
    }
}
