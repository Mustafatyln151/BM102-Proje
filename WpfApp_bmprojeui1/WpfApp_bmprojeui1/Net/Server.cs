using System;
using System.Net;
using System.Net.Sockets;
using WpfApp_bmprojeui1.Net.IO;
/*28.05.2024 hüseyin*/

namespace WpfApp_bmprojeui1.Net
{
    class Server
    {
        public TcpClient _client;
        public PacketReader _packetReader;
        public event Action connectedEvent;/*event ve Action ikisi de tip*/
        public event Action messageRecievedEvent;
        public event Action userdisconnectedEvent;
        public event Action acknowledgedEvent;
        public Server()
        {
            _client = new TcpClient();
        }
        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                /*Daha sonra servere bağlanılacak ip addres VE portun bulunup bağlanması gerekiyor ile değiştirilmesi gerekiyor
                 !!TODO!!*/
                _client.Connect(IPAddress.Parse("127.0.0.1"), 7891);
                _packetReader = new PacketReader(_client.GetStream());
                /*Packet Builder code*/
                if (!string.IsNullOrEmpty(username))
                {
                    var connectpacket = new PacketBuilder();
                    connectpacket.WriteOpCode(0);/*0 -> gelişigüzel*/
                    connectpacket.WriteString(username);
                    _client.Client.Send(connectpacket.GetPacketBytes());
                }
                ReadPackets();
            }
        }
        private void ReadPackets()
        {
            /*Thread'ın açılıp kapatılması ve bakımı eklenmedi*/
            Task.Run(() => {
                while (true)
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            messageRecievedEvent?.Invoke();
                            break;
                        case 10:
                            userdisconnectedEvent?.Invoke();
                            break;
                        case 15:
                            acknowledgedEvent?.Invoke();
                            break;
                        default:
                            break;
                    }
                }
            });
        }
        public void SendMessageToServer(string message){
            var messagepacket = new PacketBuilder();
            messagepacket.WriteOpCode(5);
            /*messagepacket.WriteString(UserId.ToString());*/
            messagepacket.WriteString(message);
            _client.Client.Send(messagepacket.GetPacketBytes());
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
