using Proje_BlogTasarımı.Net.IO;
using Proje_BlogTasarımı;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Proje_BlogTasarımı
{
    public class Client
    {
        public string UserName { get; set; }
        public Guid UserId { get; set; }/*Globally unique identifier: programın id'si*/
        public TcpClient ClientSocket { get; set; }
        PacketReader _packetreader;
        public Client(TcpClient client)
        {
            Console.Write("t");
            ClientSocket = client;
            Console.Write("h");
            UserId = Guid.NewGuid();
            Console.Write("q");
            /*OpCode*/
            _packetreader = new PacketReader(ClientSocket.GetStream());
            Console.Write("k");
            var opcode = _packetreader.ReadByte();
            Console.Write("m");
            /*OpCode Doğrulama eklenmesi gerekiyor.*/
            UserName = _packetreader.ReadMessage();

            Console.WriteLine($"[{DateTime.Now}]: Client has connected with the username: {UserName}");

            Task.Run(() => Process());
        }
        void Process()
        {
            while (true)
            {
                try
                {/* bir paket olduğundan event kullanılmadı */
                    var opcode = _packetreader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = _packetreader.ReadMessage();
                            Console.WriteLine(msg.Insert(72, "<- Alici = ").Insert(36, "<- Gonderici,"));
                            /*msg =herhangi bir uzunluk, guid =36 , */
                            var msgGuidGonderici = msg.Substring(0, 36);
                            var msgGuidAlici = msg.Substring(36, 36);
                            var msgString = msg.Substring(72, msg.Length - 72);
                            /*if (msgGuidGonderici != UserId.ToString()) {*/
                            Form3.BroadcastMessage(UserId, Guid.Parse(msgGuidAlici), msgString);


                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    /*Network hataları ayıklanmadı*/
                    Console.WriteLine($"[{UserId.ToString()}]: Dissconeccted");
                    Form3.BroadcastDisconnect(UserId.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
