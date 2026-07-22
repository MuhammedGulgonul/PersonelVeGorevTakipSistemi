# KURULUM KILAVUZU (INSTALL.md)

Bu kılavuz, Personel ve Görev Takip Sistemi projesini yerel bilgisayarınızda kurmak ve çalıştırmak için gerekli adımları içermektedir.

## 1. Sistem Gereksinimleri
Projeyi çalıştırmadan önce sisteminizde aşağıdaki bileşenlerin yüklü olduğundan emin olunuz:
* **.NET 8.0 SDK** (Uygulamanın derlenmesi ve çalıştırılması için gereklidir)
* **Microsoft SQL Server** (LocalDB veya Express sürümü veritabanı sunucusu olarak gereklidir)
* **Bir Web Tarayıcısı** (Google Chrome, Microsoft Edge vb.)

---

## 2. Veritabanı Yapılandırması
Uygulamanın veritabanını oluşturmak için iki farklı yöntem kullanabilirsiniz:

### Yöntem A: SQL Betiği ile Kurulum (Önerilen)
1. SQL Server Management Studio (SSMS) veya benzeri bir veritabanı yönetim aracı açınız.
2. Sunucunuza bağlanınız ve yeni bir veritabanı oluşturunuz (Örn: `PersonelDb`).
3. Proje ana dizininde bulunan `script.sql` dosyasını açıp bu veritabanı üzerinde çalıştırınız (Execute). Bu işlem tüm tabloları oluşturacak ve varsayılan yönetici hesabını ekleyecektir.

### Yöntem B: Entity Framework Migrations ile Kurulum
1. Bir terminal ekranı (PowerShell veya Komut İstemi) açınız.
2. Proje ana dizinine (`PersonelVeGorevTakipSistemi`) gidiniz.
3. Aşağıdaki komutu çalıştırarak veritabanının otomatik oluşturulmasını sağlayınız:
   ```bash
   dotnet ef database update --project PersonelVeGorevTakipSistemi.DataAccess --startup-project PersonelVeGorevTakipSistemi.WebUI
   ```

---

## 3. Bağlantı Dizesi (Connection String) Düzenlemesi
Uygulamanın veritabanına erişebilmesi için bağlantı dizesini kontrol etmelisiniz:
1. `PersonelVeGorevTakipSistemi.WebUI` klasörü altındaki `appsettings.json` dosyasını açınız.
2. `ConnectionStrings -> DefaultConnection` alanındaki bağlantı adresini kendi yerel SQL Server sunucu ayarlarınıza göre güncelleyiniz.
   * *Örnek (LocalDB için):* `"Server=(localdb)\\mssqllocaldb;Database=PersonelDb;Trusted_Connection=True;MultipleActiveResultSets=true"`
   * *Örnek (SQL Express için):* `"Server=.\\SQLEXPRESS;Database=PersonelDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"`

---

## 4. Uygulamayı Derleme ve Çalıştırma
Uygulamayı çalıştırmak için aşağıdaki adımları takip ediniz:
1. Terminalde proje ana dizinine gidiniz.
2. Projeyi derlemek için şu komutu çalıştırınız:
   ```bash
   dotnet build
   ```
3. Derleme başarılı olduktan sonra uygulamayı başlatmak için şu komutu yazınız:
   ```bash
   dotnet run --project PersonelVeGorevTakipSistemi.WebUI
   ```
4. Terminalde uygulamanın ayağa kalktığı port adresi görünecektir (Örn: `http://localhost:5000` veya `https://localhost:5001`).
5. Tarayıcınızı açıp bu adrese giderek uygulamaya erişebilirsiniz.

---

## 5. Varsayılan Giriş Bilgileri
Sisteme ilk girişte kullanabileceğiniz yönetici hesabı:
* **E-posta:** `admin@sirket.com`
* **Şifre:** `123456`
