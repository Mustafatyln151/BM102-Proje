using System;
using System.IO;
using System.Net.Sockets;
using System.Text;


namespace Proje_BlogTasarımı.Net.IO
{
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

            /*var msg = Encoding.ASCII.GetString(msgbuffer);*/
            var msg = Encoding.ASCII.GetString(msgbuffer);
            return msg;
        }
    }
}
