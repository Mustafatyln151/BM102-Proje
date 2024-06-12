using Proje_BlogTasarımı.Net.IO;
using Proje_BlogTasarımı.TahminEt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.ML.Data;
using Microsoft.ML;



namespace Proje_BlogTasarımı
{
    public partial class Form3 : Form
    {
        /* DATA PATH */
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "C:\\Users\\my\\Documents\\GitHub\\Iskelet\\BM102_Proje_Repo\\Proje-BlogTasarımı\\Data\\user-time.txt");
       
        /**/

        static List<Client> _users;
        static Client klient;
        public struct Botlar
        {
            public string UserName;
            public Guid UserId;
            public Botlar(string userName, Guid userId)
            {
                UserName = userName;
                UserId = userId;
            }
        }
        public static Guid[] BotlarId { get; set; }
        static TcpListener _listener;

        

        public Form3()
        {
            InitializeComponent();


            /* Botlar */
            BotlarId = new Guid[8];
            Botlar ChatBot = new Botlar("ChatBot", Guid.Parse("00000000-0000-0000-0000-000000000001"));
            BotlarId[0] = ChatBot.UserId;

            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();
            



        }
        
       

        private void StartServer()
        {
            
            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);
                klient = client;
                AcknowledgeConnection(klient);
                Form3.BroadcastConnection();

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
                    broadcastpacket.WriteString(usr.UserId.ToString() + usr.UserName);
                    user.ClientSocket.Client.Send(broadcastpacket.GetPacketBytes());
                }
            }
        }
        public static void AcknowledgeConnection(Client klient)
        {

            var broadcastpacket = new PacketBuilder();
            broadcastpacket.WriteOpCode(15);
            broadcastpacket.WriteString(klient.UserId.ToString() + klient.UserName);
            klient.ClientSocket.Client.Send(broadcastpacket.GetPacketBytes());

        }
        public static void BroadcastMessage(Guid userGondericiId, Guid userAliciId, string message)
        {
            
            var msgPacket = new PacketBuilder();
            msgPacket.WriteOpCode(5);
            
            msgPacket.WriteString(userGondericiId.ToString() + userAliciId.ToString() + message);
            if (userAliciId != BotlarId[0])
            {
                var usersendingto = _users.Where(x => x.UserId == userAliciId).FirstOrDefault();
                usersendingto.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
                Console.WriteLine("Forwading Message: " + message + ", To User: " + usersendingto.UserName.ToString());
            }
            
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
            }
            
            /*BroadcastMessage(Guid.Parse("00000000-0000-0000-0000-000000000001"),$"[{disconnecteduser.UserName}] disconeccted.");*/
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
        /******************************************************************************************************/

                                                                                                 //KullaniciMesajlari veri tabanı kullanılacak
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-6H0UUBO\\SQLEXPRESS01;Initial Catalog =KullaniciMesajlari;Integrated Security=True");


        /*private void t8545extBox1_TextChanged(object sender, EventArgs e)
        {

        }*/

        private void button1_Click(object sender, EventArgs e)//YORUM EKLEME BUTONU VE yorumun da veri tabanına aktarılması
        {
            string yorum = textBox1.Text;

            string userName = KullancininBilgileriniTutanClass.UserName; // Form1'de başarılı giirş yapan kullanıcının kullanıcı adını class kullanarak form3'e aktardık

            baglanti.Open();

            SqlCommand komut = new SqlCommand(" INSERT INTO MesajlarDB (kullaniciAdi,Mesajlar) VALUES ('" + userName + "','" + yorum + "') ", baglanti);//  MesajlarDB tablosuna kullanıcı adının ve yorumun aktarılması

            komut.ExecuteNonQuery();

            baglanti.Close();

            listele();  // Datagridview'i kullanarak veri tabanındaki yorumların ve kullanıcı adlarının görüntülenmesi için bir fonksiyon çağırdik

            textBox1.Clear();
        
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            
        
        
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)// dATAgRİDView'in cell content click event'i 
        {

            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();  // Tıklanılan satırdaki mesajın textBox1'e aktardık

            KullancininBilgileriniTutanClass.AftCliUseName = dataGridView1.CurrentRow.Cells[0].Value.ToString();  // Tıklanan satırdaki Kullanıcı adını önceden oluşturduğumuz bir class'a attım

            KullancininBilgileriniTutanClass.UpMes = dataGridView1.CurrentRow.Cells[1].Value.ToString();    //ne içindi hatırlayamadım

            KullancininBilgileriniTutanClass.MesID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[2].Value); // Mesaj silme ve düzenleme için kullanılacak bir primary key i aldık.Eğer almassak kullanıcı bir mesajı sildiğinde ve düzenlediğinde kullanıcıya ait tüm mesajları silinir veya düzenlenirdi.

        }

        private void Form3_Load(object sender, EventArgs e)// Load event'i kullanılarak form3 her yüklendiğinde listele() fonksiyonu çağrılacak
        {
            listele();//FORM3 her açıldığında yorumlar görülecek
            
            
            Thread ThreadingServer = new Thread(StartServer);//Server kodları
            ThreadingServer.Start();
        }
    
        private void listele()//Data Grid View de yorumların görüntülenmesi için fonksiyon
        {
            baglanti.Open();

            SqlDataAdapter da = new SqlDataAdapter("Select kullaniciAdi AS [Kullanıcı Adı],Mesajlar AS [Mesaj], MesajID from MesajlarDB",baglanti); // MesajlarDB tablosundaki tüm veriler da depişkenine atandı 

            DataTable tablo = new DataTable(); // Hafızada bir tablo oluşturduk

            da.Fill(tablo);// Bu tabloyu MesajlarDB deki veiler ile doldurduk

            dataGridView1.DataSource = tablo; // DataGridView'in veri kaynağı olarak bu doldurulan tablo oldu

            dataGridView1.Columns["MesajID"].Visible = false;   // MesajID'nin kullanıcılar tarafından görünümünü kapattık 
            
            baglanti.Close();


        }

        private void button2_Click(object sender, EventArgs e)//Buton2 ye basıldığında mesaj düzenleme için
        {


            string KullaniciAdi = KullancininBilgileriniTutanClass.AftCliUseName; //Düzenlemek istediğimiz mesaja tıklıyoruz ve değiştirilmek mesajı atan kişinin kullanıcı adını alıyoruz

            string Mesaj = textBox1.Text; // Ve yerine geçecek metni yazıyoruz

            

            

            if (KullancininBilgileriniTutanClass.UserName == KullaniciAdi) // Eğer programa girdiğimiz kullanıcı adı ile tıkladığımız satırdaki kullanıcı adı aynıysa if in içi çalışır ve
            {
               

                baglanti.Open();

                SqlCommand comm = new SqlCommand("UPDATE  MesajlarDB SET Mesajlar = '" + Mesaj + "' WHERE MesajID =  '" + KullancininBilgileriniTutanClass.MesID + "'", baglanti);// MesajlarDB deki mesaj her mesaj için benzersiz olan  mesajID ile yazılan satırdaki  yeni metin ile update edilir
                
                comm.ExecuteNonQuery(); // Komutu çalıştırıyoruz.


                baglanti.Close();     

            }
                

            
            else
            {


                MessageBox.Show("Bu mesajı Değiştiremezsiniz", "Ulaşılamayan Bölge", MessageBoxButtons.OK, MessageBoxIcon.Warning);// Eğer tıklanan satırdaki kullanıcı adı ile programa girdiğimiz kullanıcı adı aynı değilse bu mesaj kutusu ile uyarı verir.

            
            }


            textBox1.Clear();// En son textBox temizlenir
            
            listele();// Ve database'in son hali görüntülenir.
        
        
        
        
        }

        private void button3_Click(object sender, EventArgs e)//Yorum Silme tuşuna basıldığında
        {

            if(KullancininBilgileriniTutanClass.AftCliUseName == KullancininBilgileriniTutanClass.UserName) // Eğer silmek istediğimiz satırdaki kullanıcı adı ile programa girdiğimiz kullanıcı adı aynı ise if in içi çalışır ve 
            {

                baglanti.Open();

                SqlCommand emir = new SqlCommand("DELETE FROM MesajlarDB WHERE MesajID = '"+KullancininBilgileriniTutanClass.MesID+"'", baglanti); // MesajlarDB tablosundan seçitiğimiz satırdaki attığımız yorumun mesajID si kullanılarak sadece bir satırın silinmesi sağlanır.

                emir.ExecuteNonQuery();

                baglanti.Close();

                textBox1.Clear();

                listele();//Database'in son hali görüntülenir

            }


            else
            {


                MessageBox.Show("Bu mesajı SİLEMEZSİNİZ", "Ulaşılamayan Bölge", MessageBoxButtons.OK, MessageBoxIcon.Warning);// Eğer  eşleşme olmazsa bu uyarı mesajı bizi karşılar.


            }



        }

        private void button4_Click(object sender, EventArgs e)// Kullanıcın aktiflik zamanını tahmin etme butonuna basıldığında
        {
            
            
            
            string t1 = MLCode(); //Return edilen string t1'e alınır
            
            string t2 = KullancininBilgileriniTutanClass.AftCliUseName;  // Aktiflik zamanı tahmin edilen kişinin adı t2'ye alınır
            
            string t3 = t2 + " kullanıcısının aktif olduğu tahmini zaman dilimi : "+ t1; // Ve alınan değerler mesaj kutusunda gösterilmek için birleştirilir.
            
            MessageBox.Show(t3,"Tahmin",MessageBoxButtons.OK,MessageBoxIcon.Information);
            
        }
      

        public string MLCode()
        {
            var mlContext = new MLContext(seed: 0);//Makine öğrenmesi işlemlerinin yapılandırılması

            // user-time.txt dosyasını  _dataPath yolunu kullanarak okur ve veriyi HourData sınıfına yükler
            IDataView dataView =

                    mlContext.Data.LoadFromTextFile<HourData>(_dataPath, hasHeader: false, separatorChar: ',');



            //Verinin yapısı belirleniyor
            
            string featuresColumnName = "Features";

            var pipeline = mlContext.Transforms
                .Concatenate(featuresColumnName, "Hour1", "Hour2", "Hour3")
                .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: 8));


            var model = pipeline.Fit(dataView); // Model eğitilir

          

            // Modelin HourData sınıfındaki verilerden tahminde bulunur ve HourPrediction sınıfının içindeki Time değişkenine tahminini atar.
            var predictor = mlContext.Model.CreatePredictionEngine<HourData, HourPrediction>(model);

            float hour1 = 0f;//Data base ile her kullanıcının en son 3 girişini kullanarak bir tahmin yapacağız
            float hour2 = 0f;
            float hour3 = 0f;


            //Tıklanılan kullanıcının kullanıcı adini alip databaseden kullancıyı bulunup mesaj1,mesaj2 ve mesaj3 sütunlarını hour1,hour2,hour3 e atayacak algoritma

                                                                                                  //KullaniciSonUcMesaj veritabanını kullanıyoruz
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-6H0UUBO\\SQLEXPRESS01; Initial Catalog = KullaniciSonUcMesaj ; Integrated Security= True ");

            conn.Open();

                                               
            SqlCommand komut = new SqlCommand("Select * from KullaniciSonAktiflik", conn);

            SqlDataReader dr = komut.ExecuteReader();

            while (dr.Read())
            {

                if (dr["KullaniciAdi"].ToString().Trim() == KullancininBilgileriniTutanClass.AftCliUseName)// Ne zaman aktif olduğunu merak ettiğimiz kişinin adına tıklarız ve bu kişinin databasedeki son aktif olduğu zamanları alırız
                {

                    hour1 = float.Parse(dr["Mesaj1"].ToString()); //Serverden gelecek normalde

                    hour2 = float.Parse(dr["Mesaj2"].ToString());

                    hour3 = float.Parse(dr["Mesaj3"].ToString());

                    MessageBox.Show(hour3.ToString());  //Kullanıcının aktif olduğu son zamanı gösterir

                }

            }
            
            conn.Close();

            dr.Close();
            
            
            
            var sampledata = new HourData  //Data base'den alınan saat verileri ile tahmin için gerekli veriler elde edilir
            {
                
                Hour1 = hour1,
                Hour2 = hour2,
                Hour3 = hour3

            };


            var prediction = predictor.Predict(sampledata);      //predictor tahmin motoru ile tahmin elde edilir
            
            
            return prediction.Time.ToString();// HourPrediction içindeki tahmin edilen zaman return edilir
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
