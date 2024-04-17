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
        //public RegisterViewModel _registerViewModel;

        public RegisterView()
        {
            InitializeComponent();
            //_registerViewModel = new RegisterViewModel();
            this.DataContext = new RegisterViewModel();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterViewModel data = new RegisterViewModel
            {
                FirstName = this.firstname.Text,
                LastName = this.lastname.Text,
                Email = this.email.Text,
                Login = this.login.Text,
                Password = this.password.Text,
                PasswordCheck = this.password_check.Text,
            };

            Window parentWindow = Window.GetWindow(this);
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = Application.Current.MainWindow.DataContext as MainViewModel;
            if (mainViewModel != null)
            {
                mainViewModel.CurrentView = mainViewModel.LoginVM;
            }
        }
    }
}
