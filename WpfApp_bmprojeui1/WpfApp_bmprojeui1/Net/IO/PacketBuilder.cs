using System;
using System.IO;
using System.Text;


namespace WpfApp_bmprojeui1.Net.IO
{
    class PacketBuilder
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }
        /*server ile OpCode(Operational Code) iletimi sağlanması için.*/
        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }
        public void WriteString(string msg)
        {
            Int32 msgLen = Encoding.UTF8.GetByteCount(msg);
            _ms.Write(BitConverter.GetBytes(msgLen),0,4);
            /*TODO: UTF-8 Encoding TESTİ YAP----------------------------Kontrol edilmedi*/
            _ms.Write(Encoding.UTF8.GetBytes(msg),0,msgLen);
            /*_ms.Write(Encoding.UTF8.GetBytes(msg));*/
        }
        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
