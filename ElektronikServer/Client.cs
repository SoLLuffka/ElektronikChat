using ElektronikServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ElektronikServer
{
    internal class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientScoket { get; set; }

        PacketReader _packetReader;

        public Client(TcpClient client)
        {
            ClientScoket = client;
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(ClientScoket.GetStream());

            var opcode = _packetReader.ReadByte();
            Username = _packetReader.ReadMessage();

            Console.WriteLine($"[{DateTime.Now}]:{Username} has connected");

            Task.Run(() => Process());
        }

        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}]: Message received {msg} from {Username}");
                            program.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {msg}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception) 
                {
                    Console.WriteLine($"[{UID},{Username}]: Disconnected");
                    program.BroadcastDisconnect(UID.ToString());
                    ClientScoket.Close();
                    break;
                }
            }
        }
    }
}
