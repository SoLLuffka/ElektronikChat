using ElektronikChat.Net.IO;
using ElektronikChat.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ElektronikChat.Core.Net
{
    class Server
    {
        TcpClient _client;
        public PacketReader PacketReader;

        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public event Action userDisconnectedEvent;
        public event Action userRegistered;
        public event Action userLogged;
        public event Action userDataMatch;

        public Server()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(string username)
        {
            if(!_client.Connected)
            {
                _client.Connect("127.0.0.1", 22000);
                PacketReader = new PacketReader(_client.GetStream());

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

        private void ReadPackets()
        {
            Task.Run(() => 
            {
                while (true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            msgReceivedEvent?.Invoke();
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
                            userDataMatch?.Invoke();
                            if(PacketReader.ReadBoolean() == true)
                            {
                                LoginViewModel.ProccessData(true);
                            } else
                            {
                                LoginViewModel.ProccessData(false);
                            }
                            break;
                        default:
                            Console.WriteLine("ah yes..");
                            break;
                    }
                }
            });
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
    }
}
