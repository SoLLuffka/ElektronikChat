using ElektronikChat.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using ElektronikChat.ViewModel;

namespace ElektronikChat.Core
{
    public class Register
    {
        public static void Registration(RegisterViewModel data, Window parentWindow)
        {
            string sql = "INSERT INTO Users(login, password, firstname, lastname, email) VALUES (@Login, @Password, @FirstName, @LastName, @Email);";
            MessageBox.Show($"Registration Data: {data.FirstName}, {data.LastName}, {data.Email}, {data.Login}, {data.Password}, {data.PasswordCheck}");
        }
    }
}
