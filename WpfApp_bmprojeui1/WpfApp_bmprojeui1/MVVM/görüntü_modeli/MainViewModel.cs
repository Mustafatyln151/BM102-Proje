using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_bmprojeui1.Core;
using WpfApp_bmprojeui1.Net;  
using WpfApp_bmprojeui1.MVVM.Model;
using System.Windows;
using System.Windows.Data;
using System.Net.WebSockets;
using System.ComponentModel;

namespace WpfApp_bmprojeui1.MVVM.görüntü_modeli
{
    class MainViewModel : ObservableObject 
    {
        /*Servercode*/
        public RelayCommand ConnectToServerCommand { get; set; }
        /*Connect to servere kullanıcı ismi olarak parametre gidecek*/
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        private Server _server;
        /*servercode*/
        public UserModel SelfUser { get; set; }
        public ContactModel SelfContact { get; set; }
        public ObservableCollection<UserModel> Userslist { get; set; }
        public ObservableCollection<MessageModel> Messagess { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }

        /*Commands */
        public RelayCommand SendCommand { get; set; }
        private ContactModel _selectedcontact;
        
        public ContactModel SelectedContact
        {
            get { return _selectedcontact; }
            set { _selectedcontact = value;
                OnPropertyChanged();
            }
        }


        private string _message;

        public string Message
        {
            get { return _message; }
            set 
            { 
                _message = value;
                OnPropertyChanged();
            }
            
        }
        private string essage;

        public string Essage
        {
            get { return essage; }
            set
            {
                essage = value;
                OnPropertyChanged();
            }

        }
        public MainViewModel()
        {
            Userslist = new ObservableCollection<UserModel>();
            SelfUser = new UserModel();
            SelfContact = new ContactModel();

            _server = new Server();
            _server.connectedEvent += UserConnected;
            _server.messageRecievedEvent += MessageRecieved;
            _server.userdisconnectedEvent += UserDisconnected;
            _server.acknowledgedEvent += AcknowledgedEvent;
            /*Relay kullanmamızın sebebi null olmayan string gönderebilmek için*/
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));
            /*servercoede*/
            INotifyPropertyChanged
            Messagess = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();

            SendCommand = new RelayCommand(o =>{
                _selectedcontact.Messages.Add(new MessageModel {
                    Username = SelfUser.UserName,
                    Time = DateTime.Now,
                    ImageSource = SelfContact.ImageSource,
                    Message = Message,
                    FirstMessage = false,
                    IsNativeOrigin = true,
                });
                /* server *//* TODO: Serverden User Id yi Al ve Mesaj atarken ekle*/
                _server.SendMessageToServer(SelfUser.UserId+_selectedcontact.User.UserId+Message);
                
                /* Server */
                Message = "";
            });
            /* Botları sanırım buraya atabiliriz*/
            
            /*
            Messagess.Add(new MessageModel
            {
                Username = "Kıvanç",
                ImageSource = "https://mrwallpaper.com/wallpapers/cool-profile-picture-minion-13pu7815v42uvrsg.html",
                Message = "Test",
                Time = DateTime.Now,
                IsNativeOrigin = false,
                FirstMessage = true
            });
            
            for(int i = 0; i < 4; i++)
            {
                Messagess.Add(new MessageModel
                {
                    Username = "Kıvanç",

                    ImageSource = "https://mrwallpaper.com/wallpapers/cool-profile-picture-minion-13pu7815v42uvrsg.html",
                    Message = "Test",
                    Time = DateTime.Now,
                    IsNativeOrigin = false,
                    FirstMessage = false
                });
            }
            for (int i = 0; i < 4; i++)
            {
                Messagess.Add(new MessageModel
                {
                    Username = "Bunny",

                    ImageSource = "https://mrwallpaper.com/wallpapers/cool-profile-picture-minion-13pu7815v42uvrsg.html",
                    Message = "Test",
                    Time = DateTime.Now,
                    IsNativeOrigin = true,
                });
            }

            Messagess.Add(new MessageModel
            {
                Username = "Bunny",

                ImageSource = "https://mrwallpaper.com/wallpapers/cool-profile-picture-minion-13pu7815v42uvrsg.html",
                Message = "Last",
                Time = DateTime.Now,
                IsNativeOrigin = false,
                FirstMessage = true
            });
            

            for(int i=0; i< 5; i++)
            {
                Contacts.Add(new ContactModel
                {
                    User=new UserModel
                    {
                        UserName = $"Kıvanç{i}",
                        UserId= Guid.NewGuid().ToString()
                    },
                    ImageSource = "https://external-content.duckduckgo.com/iu/?u=http%3A%2F%2Fcdn.carbuzz.com%2Fgallery-images%2F1600%2F394000%2F200%2F394249.jpg&f=1&nofb=1&ipt=c39290135cd9ff86cbd8b58058ae467421447a18db59a9ba3e2aa55203270bc8&ipo=images",
                    Messages = Messagess
                });

            }*/
            Userslist.Add(new UserModel
            {
                UserName = "Chatbot",
                UserId = "00000000-0000-0000-0000-000000000001"
            });
            Contacts.Add(new ContactModel
            {
                User = new UserModel
                {
                    UserName = "Chatbot",
                    UserId = "00000000-0000-0000-0000-000000000001"
                },
                Messages = new ObservableCollection<MessageModel>(),
                ImageSource = "https://www.clipartmax.com/png/middle/118-1184192_virtual-assistants-and-chat-bots-bot-icon-bot.png",
                /*LastMessage = */

            });
        }
        

        private void MessageRecieved()
        {
            var msg = _server._packetReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Essage = msg);


            var msgguidGonderici = msg.Substring(0, 36);
            var msgguidAlici = msg.Substring(36, 36);
            var msgcut = msg.Substring(72, msg.Length - 72);
            var user = Userslist.Where(x => x.UserId == msgguidGonderici).FirstOrDefault();
            var contact = Contacts.Where(x => x.User == user).FirstOrDefault();
            //if (contact != null && user != null){
                Application.Current.Dispatcher.Invoke(() => contact.Messages.Add(new MessageModel
                {
                    Message = msgcut,
                    Username = user.UserName,
                    Time = DateTime.Now,
                    IsNativeOrigin = false,

                }));
            /*}
            else {
                Essage = msg;
            }*/
            /*Application.Current.Dispatcher.Invoke(() => Messages.Add(new MessageModel{
             Message = msgcut,
             Username = user.UserName,
             Time = DateTime.Now,
             IsNativeOrigin = false,

            }));*/

        }
        private void UserDisconnected() {
            var userid = _server._packetReader.ReadMessage();
            var user = Userslist.Where(x => x.UserId == userid).FirstOrDefault();
            var contact = Contacts.Where(x => x.User == user).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Userslist.Remove(user));
            Application.Current.Dispatcher.Invoke(() => Contacts.Remove(contact));
        }

        private void UserConnected()
        {
            var msg = _server._packetReader.ReadMessage();
            var msgguid = msg.Substring(0, 36);
            var msgcut = msg.Substring(36, msg.Length - 36);
            var user = new UserModel
            {
                UserName = msgcut,
                UserId = msgguid
            };
            if (!Userslist.Any(x => x.UserId == user.UserId))
            {
                /*Serverdede çifte bağlanan kullanıcıların olmadığını kontrol edilmesi gerekiyor*/
                Application.Current.Dispatcher.Invoke(() => Contacts.Add(new ContactModel
                {
                    User=user,
                    Messages = new ObservableCollection<MessageModel>()
                })
                );
                Application.Current.Dispatcher.Invoke(()=>Userslist.Add(user));
                
            }
        }
        private void AcknowledgedEvent(){
            var ackbuff = _server._packetReader.ReadMessage();/*TODO: ReadMessage ile ReadStringin farkı ne?*/
            var UserId = ackbuff.Substring(0,36);
            var UserName = ackbuff.Substring(36,ackbuff.Length-36);
            SelfUser.UserName =UserName;
            SelfUser.UserId = UserId;
            /* Contactın kodlarını ekle */
            SelfContact.User = SelfUser;
            SelfContact.Messages = new ObservableCollection<MessageModel>();
            SelfContact.ImageSource = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fupload.wikimedia.org%2Fwikipedia%2Fcommons%2Fthumb%2F1%2F12%2FUser_icon_2.svg%2F1024px-User_icon_2.svg.png&f=1&nofb=1&ipt=e7cd2b5c901d54fe090a7ceb64987b3d6e467e849fcc468da1c26a06d27df474&ipo=images";
        }
    }
}
