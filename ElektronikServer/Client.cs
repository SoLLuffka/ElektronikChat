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
        }
    }
}
