# BM102-Proje
 Gazi 2.Dönem Proje
 Projenin Çalışması için gereklilikler:
1-)Database dosyalarının Sql Server 2022’ye import edilmesi 
  

 -Bu üç dosyanın import edilmesi gerekiyor -Connect yerine kendi server kodunuzu girin.
2-)Any CPU yerine x64 ün seçilmesi gerekiyor.
 






3-)user-time.txt nin dosya yolunun değiştirilmesi gerekiyor.
 
-Tam yolu kopyala diyip 
static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "C:\\Users\\my\\Documents\\GitHub\\Iskelet\\BM102_Proje_Repo\\Proje-BlogTasarımı\\Data\\user-time.txt");
Mavi yeri silip yerine sizin bilgisayarınızdaki yolun yapıştırılması gerekiyor.
 

4-)Makine Öğrenmesi modelinin çalışması için şu işlem adımları izlenmeli:
   1-) Proje-Blog Tasarımı üzerine sağ tık yapıp Nuget Paketlerini Yönet’e tıklanır.
 




2-) Çıkan ekranda gözat butonuna tıklanır ve arama çubuğuna ML.Net yazılır.
 
3-) Microsoft.ML’e tıklanır ve versiyon 1.3.1 indirilir.
 


5-) Gerekli kütüphaneler atacağımız github linkinde hazır olacaktır.

6-) Sql bağlantısı kurmak için kullanılan SqlConnection’ın parametresindeki Data Source değiştirilmeli
Aşağıdaki mavi yerin sizin bilgisayarınızdaki server adı ile değiştirilmesi gerekiyor.
SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-6H0UUBO\\SQLEXPRESS01; Initial Catalog = KullaniciBilgileri ; Integrated Security= True");
 
Kendi server adınıza buradan ulaşabilirsiniz.


