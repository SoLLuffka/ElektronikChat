using ElektronikChat.Net.IO;
using ElektronikChat.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace ElektronikChat.Core.Net
{
    class Server
    {
        private static Server _instance;

        TcpClient _client;
        public PacketReader PacketReader;

        public TcpClient Client => _client;

        public string uid;

        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public event Action msgReceivedContactEvent;
        public event Action userDisconnectedEvent;
        public event Action userRegistered;
        public event Action userLogged;
        public event Action<bool> DataMatchReceived;

        //public delegate void DataMatchHandler(bool dataMatch);
        //public event DataMatchHandler DataMatch;

        public Server()
        {
            _client = new TcpClient();
        }

        public static Server Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Server();
                }
                return _instance;
            }
        }

        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("192.168.140.102", 22000);


                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteMessage(username);
                _client.Client.Send(connectPacket.GetPacketBytes());

                // Now username can be empty, because for us only connection matter
                // Username will be taken from database with implementation of
                // logging in system
                //if (!string.IsNullOrEmpty(username))
                //{

                //}
                ReadPackets();
            }
        }

        public void ReadPackets()
        {
            PacketReader = new PacketReader(_client.GetStream());
            Task.Run( () =>
            {
                try
                {
                    while (true)
                    {
                        //MessageBox.Show("sss");
                        var opcode = PacketReader.ReadByte();
                        //MessageBox.Show($"Odebrano pakiet z opcodem: {opcode}");
                        switch (opcode)
                        {
                            case 1:
                                connectedEvent?.Invoke();
                                break;
                            case 2:
                                uid = PacketReader.ReadMessage();
                                break;
                            case 5:
                                msgReceivedEvent?.Invoke();
                                break;
                            case 6:
                                msgReceivedContactEvent?.Invoke();
                                break;
                            case 10:
                                userDisconnectedEvent?.Invoke();
                                break;
                            case 20:
                                userRegistered?.Invoke();
                                break;
                            case 25:
                                userLogged?.Invoke();
                                break;
                            case 30:
                                var dataMatch = PacketReader.ReadBoolean();
                                //HandleDataMatch(dataMatch);
                                OnDataMatchReceived(dataMatch);
                                break;
                            default:
                                //MessageBox.Show("ah yes..");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas odczytywania pakietów: {ex.Message}");
                }
            });
        }

        private void HandleDataMatch(bool dataMatch)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Zgodność danych: {dataMatch}");
            });
        }

        protected virtual void OnDataMatchReceived(bool dataMatch)
        {
            DataMatchReceived?.Invoke(dataMatch);
        }

        public void SignalReadyToServer()
        {
            if (_client.Connected)
            {
                var readyPacket = new PacketBuilder();
                readyPacket.WriteOpCode(2); // Przykładowy opcode sygnalizujący gotowość
                _client.Client.Send(readyPacket.GetPacketBytes());
            }
        }

        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }

        public void RegisterUser(string message)
        {
            //ConnectToServer("");
            //PacketReader = new PacketReader(_client.GetStream());
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(20);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }

        public void LoginUser(string message)
        {
            //ConnectToServer("x");
            //PacketReader = new PacketReader(_client.GetStream());
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(25);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }

        public void SendUsername(string username)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(26);
            messagePacket.WriteMessage(username);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }

        public void SendMessageToContact(string name, string message, string username)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(6);
            messagePacket.WriteMessage(name);
            messagePacket.WriteMessage(message);
            messagePacket.WriteMessage(username);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }

        public void WritePlainBool(bool value)
        {
            var packet = new PacketBuilder();
            packet.WriteBoolean(value);
            _client.Client.Send(packet.GetPacketBytes());
        }
    }
}
