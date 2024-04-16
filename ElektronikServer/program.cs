using ElektronikServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElektronikServer
{
    class program
    {
        //public static DBConnection dbConnection;
        static List<Client> _users;
        static TcpListener _listener;
        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 22000);
            _listener.Start();

            //dbConnection = new DBConnection();
            //dbConnection.InitializeDatabase();
            //ExecuteSelectQuery();
            BroadcastConnection();
            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());

                var uidPacket = new PacketBuilder();  //przeslanie uid w celu utworzenia sesji
                uidPacket.WriteOpCode(2);
                uidPacket.WriteMessage(client.UID.ToString());
                client.ClientScoket.Client.Send(uidPacket.GetPacketBytes());

                _users.Add(client);

                /*Broadcast the connection to everyone on server*/
                BroadcastConnection();
            }
        }

        static void BroadcastConnection()
        {
            foreach (var user in _users)
            {
                foreach (var usr in _users)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    //broadcastPacket.WriteMessage(usr.Username);
                    broadcastPacket.WriteMessage(usr.UID.ToString());
                    user.ClientScoket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach (var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                user.ClientScoket.Client.Send(msgPacket.GetPacketBytes());
            }
            //Console.WriteLine(message);
        }

        public static void BroadcastMessageContact(string name, string message, string username, string daytime)
        {
            foreach(var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(6);
                string msgString = $"{name}←{message}←{username}←{daytime}";
                msgPacket.WriteMessage(msgString);
                user.ClientScoket.Client.Send(msgPacket.GetPacketBytes());
            }
        }

        public static void BroadcastDisconnect(string uid)
        {
            var disconnectedUser = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            _users.Remove(disconnectedUser);
            foreach (var user in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientScoket.Client?.Send(broadcastPacket.GetPacketBytes());
            }
            //BroadcastMessage($"[{disconnectedUser.Username}] Disconnected!");
            BroadcastMessage($"[] Disconnected!");
        }

        public static void DataMatch(bool value, string uid)
        {
            var desiredUser = _users.FirstOrDefault(x => x.UID.ToString() == uid);

            if (desiredUser != null)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(30);
                msgPacket.WriteBoolean(value);

                desiredUser.ClientScoket.Client.Send(msgPacket.GetPacketBytes());

                Console.WriteLine(msgPacket.GetPacketBytes());

                Console.WriteLine($"Wysłano informację o zgodności danych do klienta o UID: {uid}, wartość: {value}");
            }
            else
            {
                Console.WriteLine($"Nie znaleziono klienta o UID: {uid}");
            }         
        }

        /*
        public static void ExecuteSelectQuery()
        {
            string sql = "SELECT * FROM Users";
            using (var command = new SQLiteCommand(sql, dbConnection.GetConnection()))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Brak danych w tabeli Users.");
                        return;
                    }

                    while (reader.Read())
                    {
                        // Przetwarzaj wyniki, np.:
                        string id = reader["idUsers"].ToString();
                        string firstname = reader["firstname"].ToString();
                        string lastname = reader["lastname"].ToString();
                        Console.WriteLine($"{firstname} {lastname} {id}"); // Wyświetl wyniki w MessageBox
                                                                         // I tak dalej dla innych kolumn...
                    }
                }
            }
        }*/
    }
}
