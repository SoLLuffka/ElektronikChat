using ElektronikChat.Core;
using ElektronikChat.Core.Net;
using ElektronikChat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElektronikChat.ViewModel
{
    class TextChatVIewModel : ObservableObject
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> _Messages { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }

        private ContactModel _selectedContact;

        public ContactModel SelectedContact
        {
            get { return _selectedContact; }
            set { 
                _selectedContact = value; 
                OnPropertyChanged();
            }
        }

            
        private string _message;

        public string Message 
        { 
            get { return _message; }
            set { 
                _message = value;
                OnPropertyChanged(); 
            } 
        }

        public RelayCommand SendCommand { get; set; }
            
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        public static string Username { get; set; }
        //public string Message { get; set; }

        private Server _server;
        public TextChatVIewModel()
        {
            Users = new ObservableCollection<UserModel>();
            _Messages = new ObservableCollection<string>();  
            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();




            Messages.Add(new MessageModel
            {
                Username = "Katarzyna",
                Message = "Hej",
                Time = DateTime.Now,
            });

            Messages.Add(new MessageModel
            {
                Username = "Katarzyna",
                Message = "Zrobiłeś zadanie domowe z Matematyki?",
                Time = DateTime.Now,
            });


            Contacts.Add(new ContactModel
            {
                Name = "Katarzyna",
                Usernames = new List<string> { "Sigma", "Cwigkla"},
                Messages = Messages
            });
            

            _server = Server.Instance;
            _server.connectedEvent += UserConnected;
            _server.msgReceivedEvent += MessageReceived;
            _server.userDisconnectedEvent += RemoveUser;

            SendCommand = new RelayCommand(o =>
            {
                SelectedContact.Messages.Add(new MessageModel
                {
                    Message = Message,
                    Username = Username,
                    Time = DateTime.Now
                });
                Message = "";

            });
            //ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message)); 
        }

        private void RemoveUser()
        {
            var uid = _server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }

        private void MessageReceived()
        {
            var msg = _server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(new MessageModel
            {
                Username = Username,
                Message = msg,
                Time = DateTime.Now
            }));
        }

        private void UserConnected()
        {
            var user = new UserModel
            {
                Username = "", //_server.PacketReader.ReadMessage()
                UID = _server.PacketReader.ReadMessage(),
            };

            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }

        }
        public static void SetUsername(string username)
        {
            Username = username;
        }
    }
}
