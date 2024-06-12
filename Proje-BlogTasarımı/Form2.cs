using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proje_BlogTasarımı
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }                                                                                         //KullaniciBilgileri veritabanını kullanır.
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-6H0UUBO\\SQLEXPRESS01; Initial Catalog = KullaniciBilgileri ; Integrated Security= True");//Veri Tabanı Bağlantısı kurma

        private void button1_Click(object sender, EventArgs e)//Kaydolma ekranında girilen verilerin data base aktarılması
        {
            string t1 = textBox1.Text;

            string t2 = Sha256Converter.ComputeSha256Hash(textBox2.Text); // Parolanın şifrelenmesi için textBox2 ye girilen parolanın parametre olarak gönderilip şifrelenmesini sağlar

            
            
            baglanti.Open();//Bağlantının aktif hale getirilmesi

            SqlCommand komut = new SqlCommand("INSERT INTO Kullanicilar(kullaniciAdi,parola)  VALUES ('"+t1+"','"+t2+"')  ", baglanti); // Kullanicilar tablosuna Kullancının kullaniciAdi ve şifrelenmiş parolasının kaydedlmesi

            komut.ExecuteNonQuery();

            baglanti.Close();//Girilen değerler veri tabanına aktarıldı.

            
            
            Form1 frm1 = new Form1();//kaydolma tuşuna basıldığında Form1 açılacak

            frm1.Show();   //Kaydol tuşuna basıldığında yukarıdaki işlemler yapılır ve form1 açılır form2 kapatılır

            this.Hide();

            MessageBox.Show("Kayıt Olma Başarılı", "Onay", MessageBoxButtons.OK, MessageBoxIcon.Information); //Bilgilendirici bir metin gösterir
        
        
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {

        }
        
        
       
         ErrorProvider hata = new ErrorProvider();  //ErrorProvider sınıfı 
        
        private void textBox1_TextChanged(object sender, EventArgs e)//Kullanıcı Adı Uzunluğu
        {
            

            if(textBox1.Text.Length < 3)
            {

                hata.SetError(textBox1, "Kullanıcı Adı En Az 4 Karakter Olmalı"); // TextBox' a girilen metnin uzunluğuna göre uyarı işareti verir.

                hata.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;//Uyarı işretinin her zaman durmasını sağlar

               

            }
            else
            {

                hata.Clear();  //TextBox'a girilen metinin uzunluğu gerekli şartı sağladığında uyarı simgesi kaybolur.

            }
        
        
        
        
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)//Kullanıcı Adı Sadece Harflerden oluşabilmesini sağlar
        {
            if(((int)e.KeyChar >= 65 && (int)e.KeyChar <= 90)||((int)e.KeyChar >= 97 && (int)e.KeyChar <=122)|| (int)e.KeyChar == 8)
            {

                e.Handled = false;//Sadece harf içeren tuşların çalışmasını sağlar



            }
            else
            {

                e.Handled = true;// Harf içeren tuşlar dışındakilerin çalışmasını engeller.
            }
        
        
        
        }

        private void textBox2_TextChanged(object sender, EventArgs e)//Şifrenin uzunluğunun en az 8 Karkter olmasını sağlar
        {

       

           if(textBox2.Text.Length < 7)
           {
                hata.SetError(textBox2, "Şifre En Az Sekiz Haneli Olmalı");
                
                hata.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;

            }

            else
            {

                hata.Clear();

            }
            
        
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)//Şifrede boşluk olamaz
        {
            if(char.IsSeparator(e.KeyChar)== true) // Klavyeden bir tuşa basıldığında bu tuşun boşluk tuşu oldupunu anlar ve boşluk karakterinin şifrede olmasını engeller.
            {
                e.Handled = true;


                hata.SetError(textBox2, "Şifre Boşluk İçeremez");
               
                
                hata.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;
            }
            else
            {
                hata.Clear();



            }
        
        
        }
    }
}
