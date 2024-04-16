using ElektronikChat.Core;
using ElektronikChat.Core.Net;
using ElektronikChat.Model;
using ElektronikChat.Net.IO;
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
            //_Messages = new ObservableCollection<string>();  
            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();




            Messages.Add(new MessageModel
            {
                Username = "Sigma",
                Message = "Uuu, sigma",
                Time = DateTime.Now.ToString(),
            });

            for (int i = 0; i < 3; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "Sigma",
                    Message = $"Uuu, sigma{i+1}",
                    Time = DateTime.Now.ToString(),
                });
            }

            for (int i = 0; i < 4; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "Cwigkla",
                    Message = $"Uuu, sigma{i + 1}",
                    Time = DateTime.Now.ToString(),
                });
            }

            for (int i = 0; i < 5; i++)
            {
                Contacts.Add(new ContactModel
                {
                    Name = $"Pokój {i+1}",
                    Usernames = new List<string> { "Sigma", "Cwigkla"},
                    Messages = new ObservableCollection<MessageModel>()
                });
            }

            _server = Server.Instance;
            _server.connectedEvent += UserConnected;
            _server.msgReceivedEvent += MessageReceived;
            _server.userDisconnectedEvent += RemoveUser;
            _server.msgReceivedContactEvent += MessageReceivedContact;

            SendCommand = new RelayCommand(o => SendMessageToServer(), o => !string.IsNullOrEmpty(Message));
            //ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message)); 
        }

        private void MessageReceivedContact()
        {
            try
            {
                var data = _server.PacketReader.ReadMessage();
                data = data.Replace("\0", "");
                var fragData = data.Split("←");
                var name = fragData[0];
                var message = fragData[1];
                var username = fragData[2];
                var datetime = fragData[3];
                //var message = _server.PacketReader.ReadMessage();
                //var username = _server.PacketReader.ReadMessage();
                //var datetime = _server.PacketReader.ReadMessage();
                var contact = Contacts.FirstOrDefault(c => c.Name == name);
                if (contact != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    contact.Messages.Add(new MessageModel
                    {
                        Username = username,
                        Message = message,
                        Time = DateTime.Now.ToString(),
                    }));
                }
            }
            catch (Exception ex) 
            {
                //MessageBox.Show($"Błąd: {ex}");
            }
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
                Time = DateTime.Now.ToString()
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

        private void SendMessageToServer()
        {
            var name = SelectedContact.Name;
            var message = Message;
            Message = "";
            _server.SendMessageToContact(name, message, Username);
        }
    }
}
