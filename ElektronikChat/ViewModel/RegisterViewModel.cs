using ElektronikChat.Core;
using ElektronikChat.Core.Net;
using ElektronikChat.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElektronikChat.ViewModel
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PasswordCheck { get; set; }

        private Server _server;

        private RelayCommand RegisterUserCommand { get; set; }

        public RegisterViewModel()
        {
            _server = new Server();

            RegisterUserCommand = new RelayCommand(o => _server.RegisterUser("xd"));
        }
        public void Register()
        {
            string combinedMessage = $"{FirstName};{LastName};{Email};{Login};{Password}";
            _server.RegisterUser(combinedMessage);
        }
    }
}
