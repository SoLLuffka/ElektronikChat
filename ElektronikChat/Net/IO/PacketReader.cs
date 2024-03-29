using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ElektronikChat.Net.IO
{
    internal class PacketReader : BinaryReader
    {
        public NetworkStream _ns;
        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }

        public string ReadMessage()
        {
            //byte[] msgBffer;
            var lenghtBytes = ReadBytes(4);
            var length = BitConverter.ToInt32(lenghtBytes, 0);
            var msgBffer = new byte[length];
            _ns.Read(msgBffer, 0, length);

            var msg = Encoding.UTF8.GetString(msgBffer);
            return msg;
        }

        public bool ReadBoolean()
        {
            return _ns.ReadByte() != 0;
        }
    }
}
