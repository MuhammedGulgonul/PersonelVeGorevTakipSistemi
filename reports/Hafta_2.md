# 2. Hafta Raporu: Veritabanı Tasarımı ve EF Core Entegrasyonu

**Tarih:** 1 Temmuz 2026  
**Proje Adı:** Personel ve Görev Takip Sistemi  

---

## Bu Hafta Yapılan Çalışmalar

### 1. Veritabanı Modellerinin Yazılması
Projenin ihtiyaç duyduğu veri yapısını oluşturmak amacıyla Core katmanında gerekli sınıflar ve ilişkileri tanımlanmıştır:
* **Department:** Departman bilgilerini ve bu departmana bağlı personellerin listesini tutar.
* **Employee:** Personel adı, soyadı, e-posta, yetki rolü, aktiflik durumu ve şifre hash bilgilerini tutar.
* **Task:** Atanan görevlerin başlık, açıklama, durum, öncelik ve tarih bilgilerini tutar.
* **TaskFile:** Görevlere eklenecek PDF veya resim gibi dosyaların sunucu yollarını tutar.
* **Log:** Sistemde oluşacak yazılımsal hataların kayıtlarını tutar.
* **TaskState ve TaskPriority:** Görev durumlarını ve öncelik derecelerini yönetmek için C# Enum yapıları oluşturulmuştur.

---

### 2. Entity Framework Core Bağlantısının Yapılması
C# kodlarımız ile veritabanını konuşturmak için gerekli entegrasyonlar tamamlanmıştır:
* Projemize .NET 8.0 ile uyumlu EF Core SQL Server ve araç NuGet paketleri yüklenmiştir.
* DataAccess katmanında veritabanı tablolarımızı yönetecek olan **AppDbContext** sınıfı oluşturulmuştur.
* WebUI projesindeki appsettings.json dosyasına yerel SQL Server bağlantı adresimiz tanımlanmıştır.
* Program.cs dosyasına AppDbContext servisi ve SQL Server ayarları kaydedilmiştir.

---

### 3. Veritabanının Oluşturulması ve Başlangıç Verileri
* EF Core Migrations aracı kullanılarak ilk göç dosyası oluşturulmuş ve SQL Server üzerinde **PersonelGorevDb** adında veritabanı ile tüm ilişkili tablolar otomatik üretilmiştir.
* Sisteme ilk girişi yapabilmek amacıyla veritabanına varsayılan 3 departman ve 1 yönetici personel başlangıç verisi olarak eklenmiştir.

---

## Gelecek Haftanın Planı
* Üye giriş ve çıkış sisteminin kodlanması.
* Şifrelerin hash'lenerek güvenli hale getirilmesi için gerekli iş mantığının yazılması.
* Admin ve çalışan panellerinin yetkilendirme altyapısının kurulması.
