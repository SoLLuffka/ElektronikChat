using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElektronikChat.Model
{
    internal class ContactModel
    {
        public string Name { get; set; }
        public List<string> Usernames { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; }
        public string LastMessage => CheckHaveMessages();
        public string id { get; private set; }
        public ContactModel()
        {
            id = Guid.NewGuid().ToString();
        }

        private string CheckHaveMessages()
        {
            if (Messages.Count != 0)
            {
                return Messages.Last().Message;
            }
            return string.Empty;
        }
    }
}