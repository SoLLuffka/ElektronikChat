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
            var placeholder = _packetReader.ReadMessage();


            Console.WriteLine($"[{DateTime.Now}]:{UID} has connected");

            Task.Run(() => Process());
        }

        async void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 2:
                            Console.WriteLine($"Klient {UID} jest gotowy do odbioru danych.");
                            /* to do zmiany nie moze odrazu wysylac tylko po sprawdzeniu danych
                            var msgPacket = new PacketBuilder();
                            msgPacket.WriteOpCode(3);
                            msgPacket.WriteMessage("ElO");

                            var message = msgPacket.GetPacketBytes();
                            await ClientScoket.GetStream().WriteAsync(message, 0, message.Length);*/
                            break;
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}]: Message received {msg} from ");
                            program.BroadcastMessage($"[{DateTime.Now}]: []: {msg}");
                            break;
                        case 6:
                            var name = _packetReader.ReadMessage();
                            var message = _packetReader.ReadMessage();
                            var usrname = _packetReader.ReadMessage();
                            Console.WriteLine($"name of group {name}, message {message}, from {usrname}");
                            program.BroadcastMessageContact(name, message, usrname, DateTime.Now.ToString());
                            break;
                        case 20:
                            var regData = _packetReader.ReadMessage();
                            var parts = regData.Split(';');
                            if (parts.Length == 5)
                            {
                                Console.WriteLine("Server received regData!");
                                Console.WriteLine($"Imie: {parts[0]},Nazwisko: {parts[1]},Email: {parts[2]},Login: {parts[3]},Password: {parts[4]}");
                                var success = await DBManager.RegisterUserAsync(parts[0], parts[1], parts[2], parts[3], parts[4]);
                            }
                            break;
                        case 25:
                            var logData = _packetReader.ReadMessage();
                            var logParts = logData.Split(';');
                            var logSuccess = await DBManager.UserExistsAsync(logParts[0], logParts[1], UID.ToString());
                            Console.WriteLine("Server received logData!");
                            break;
                        case 26:
                            var UsernameData = _packetReader.ReadMessage();
                            Username = UsernameData;
                            //Console.WriteLine($"New username of {UID} client is {Username}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"[{UID}]: Disconnected");
                    program.BroadcastDisconnect(UID.ToString());
                    ClientScoket.Close();
                    break;
                }
            }
        }
    }
}
