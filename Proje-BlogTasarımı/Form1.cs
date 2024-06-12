using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proje_BlogTasarımı
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
                                                                                                  //KullaniciBilgileri veri tabanı kullanılır
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-6H0UUBO\\SQLEXPRESS01; Initial Catalog = KullaniciBilgileri ; Integrated Security= True");//Veri Tabanı Bağlantısı kurma

        private void button1_Click(object sender, EventArgs e)//Giriş Yap butonuna basıldığında
        {

            SqlCommand komut = new SqlCommand("Select * from Kullanicilar", baglanti);//Kullanicilar tablosundakitüm satır ve sütunlar komut nesnesine kopyalanır

            baglanti.Open();

            SqlDataReader dr = komut.ExecuteReader(); // dr objesine kopyalanan bu veriler atanır

            bool sonuc = false;

            string kullaniciAdi = textBox1.Text; // TextBox1'e girilen metnin veri tabanındaki kullanıcı adı ile uyuşuyor mu diye kontrol etmek için metnin alınması

            KullancininBilgileriniTutanClass.UserName = kullaniciAdi;  //Kullanıcı adını  formlar arasi aktarmak için bir class ta tutacağız.


            string parola = Sha256Converter.ComputeSha256Hash(textBox2.Text); // TextBox2 ye girilen şifrenin şifrelenip databasedeki şifrelenmiş parola ile kontrol edilmak üzere alınması

            while (dr.Read())//Veri olduğu sürece true döndürür
            {

                if (dr["kullaniciAdi"].ToString().Trim() == kullaniciAdi && dr["parola"].ToString().Trim() == parola)//Veri tabanında böyle bir kullanıcı adı ve  parola olup olmadığına bakar.
                {

                    sonuc = true;

                    break;// Eğer bulursa döngüden çıkar

                }

            }
            if (sonuc) // Girilen kullanıcı adı ve parola veritabanındaki bir kayıtla eşleşmiştir 
            {

                MessageBox.Show("Giriş Başarılı", "Giriş Ekranı");


                baglanti.Close();

                Form3 frm3 = new Form3();

                frm3.Show(); //Form3 açılır ve form1 kapanır
                this.Hide();

            }
            else
            {

                MessageBox.Show("Giriş Başarısız!", "Giriş Ekranı");
                baglanti.Close();
            }

            baglanti.Close();

        }

        ErrorProvider hata = new ErrorProvider();// ErrorProvider sınıfı
        
        private void textBox1_TextChanged(object sender, EventArgs e)//TextBox daki veriye dair uyarı verir Text changed event'i
        {

           

            if (textBox1.Text.Length < 3 )
            {
                 
               hata.SetError(textBox1, "Kullanıcı Adı En Az 4 karakter olmalı");


            }
            else
            {

                hata.Clear();


            }
        
        
        }

        private void button2_Click(object sender, EventArgs e)//Form1 deki kaydol tuşu
        {

            Form2 frm2 = new Form2();//Kaydol tuşuna basıldığında From2 deki kaydolma formuna bizi atacak

            frm2.Show();

            this.Hide();

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) // KeyPress eventi ile herhangi bir tuşa basıldığında hangi tuş olduğunu anlayıp kullanıcı adının sadece harflerden oluştuğunu kullanıcıya hatırlattık
        {


            if (((int)e.KeyChar >= 65 && (int)e.KeyChar <= 90) || ((int)e.KeyChar >= 97 && (int)e.KeyChar <= 122) || (int)e.KeyChar == 8)
            {

                e.Handled = false;



            }
            else
            {

                e.Handled = true;
            }


        }

        private void textBox2_TextChanged(object sender, EventArgs e) //Text changed event'ini kullanarak textBox2'ye girilen metnin uzunluğunu kontrol eder
        {
            if (textBox2.Text.Length < 7)
            {
                hata.SetError(textBox2, "Şifre En Az Sekiz Haneli Olmalı");
            }

            else
            {

                hata.Clear();

            }


        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)// Herhangi bir tuşa basıldığında bu çalışır ve Boşluk tuşunu algılayıp şifrede boşluk tuşu olmasını engeller
        {
            if (char.IsSeparator(e.KeyChar) == true)
            {
                e.Handled = true;


                hata.SetError(textBox2, "Şifre Boşluk İçeremez");

            }
            else
            {
                hata.Clear();



            }
        }
    }


}


















