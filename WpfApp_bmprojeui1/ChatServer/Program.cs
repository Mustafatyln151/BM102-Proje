using ChatServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
/*ChatServer Programı*/
namespace ChatServer
{
    class Program
    {
        static List<Client> _users;
        static Client klient;
        public struct Botlar
        {
            public string UserName;
            public Guid UserId;
            public Botlar(string userName,Guid userId)
            {
                UserName = userName;
                UserId = userId;
            }
        }
        public static Guid[] BotlarId { get; set; }
        static TcpListener _listener;
        static void Main(string[] args)
        {

            /* Botlar */
            BotlarId = new Guid[8];
            Botlar ChatBot = new Botlar("ChatBot",Guid.Parse("00000000-0000-0000-0000-000000000001"));
            BotlarId[0]= ChatBot.UserId;
            /*İP ADDRESİ TEST İÇİN SONRADAN DEĞİŞTİR
             TODO:*/

            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse(/*GetLocalIPAddress()*/"127.0.0.1"), 7891);
            Console.WriteLine(GetLocalIPAddress());
            _listener.Start();
            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);
                klient = client;
                AcknowledgeConnection(klient);

                /*Broadcast the connection to everyone*/
                BroadcastConnection();

            }
        }

        static void BroadcastConnection()
        {
            foreach (var user in _users)
            {
                foreach (var usr in _users)
                {
                    var broadcastpacket = new PacketBuilder();
                    broadcastpacket.WriteOpCode(1);
                    broadcastpacket.WriteString(usr.UserId.ToString()+usr.UserName);
                    user.ClientSocket.Client.Send(broadcastpacket.GetPacketBytes());
                }
            }
        }
        public static void AcknowledgeConnection(Client klient)
        {
            
            var broadcastpacket = new PacketBuilder();
            broadcastpacket.WriteOpCode(15);
            broadcastpacket.WriteString(klient.UserId.ToString()+klient.UserName);
            klient.ClientSocket.Client.Send(broadcastpacket.GetPacketBytes());
                
        }
        public static void BroadcastMessage(Guid userGondericiId,Guid userAliciId,string message){
            /*foreach (var user in _users)
            {*/
                
                /* Birazını ben uydurdum*/
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                /*msgPacket.WriteString(userId.ToString());*/
                msgPacket.WriteString(userGondericiId.ToString()+ userAliciId.ToString() + message);

            if (userAliciId != BotlarId[0] && userAliciId != BotlarId[0]) {
                var usersendingto = _users.Where(x => x.UserId == userAliciId).FirstOrDefault();
                usersendingto.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
                Console.WriteLine("Forwading Message: " + message + ", To User: " + usersendingto.UserName.ToString());
            }
            else if(userGondericiId == BotlarId[0])
            {

            }
            /*}*/
        }
        public static void BroadcastDisconnect(string userId)
        {
            var disconnecteduser = _users.Where(x => x.UserId.ToString() == userId).FirstOrDefault();
            _users.Remove(disconnecteduser);
            foreach (var user in _users)
            {
                var discPacket = new PacketBuilder();
                discPacket.WriteOpCode(10);
                discPacket.WriteString(userId);
                user.ClientSocket.Client.Send(discPacket.GetPacketBytes());
                /*BroadcastMessage(Guid.Parse("00000000-0000-0000-0000-000000000001"),user.UserId, $"[{disconnecteduser.UserName}] disconeccted.");*/
            }
            /*Ben uydurdum*//*****************************************ALICI GUİD EKLENMEDİ TODO************************************/
            
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                Console.WriteLine(ip.ToString());
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
