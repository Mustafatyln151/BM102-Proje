using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_bmprojeui1.Net.IO
{
    /*Serverin içindeki PacketReader ile Kopyalanıp Yapıştırılmıştır*/
    class PacketReader : BinaryReader
    {
        private NetworkStream _ns;
        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }
        public string ReadMessage()
        {
            byte[] msgbuffer;
            var length = ReadInt32();
            msgbuffer = new byte[length];
            /*UTF-8 Uzunluk Gözükmüyor Sanırım*/
            _ns.Read(msgbuffer, 0, length);

            var msg = Encoding.UTF8.GetString(msgbuffer);
            /*var msg = Encoding.UTF8.GetString(msgbuffer);*/
            return msg;
        }
    }
}
