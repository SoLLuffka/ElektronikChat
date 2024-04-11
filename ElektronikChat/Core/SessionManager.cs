using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElektronikChat.Core
{
    internal class SessionManager
    {
        public static string Uid {  get; private set; }
        public static string Username { get; private set; }
        public static DateTime SessionStart { get; private set; }

        public static void CreateSession(string uid, string username)
        {
            Uid = uid;
            Username = username;
            SessionStart = DateTime.Now;

            // MessageBox.Show($"Utworzono sesje z danymi {Uid}, {Username}, o czasie {SessionStart}");
        }
    }
}
