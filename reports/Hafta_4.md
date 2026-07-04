# 4. Hafta Raporu: Departman ve Personel Yönetimi

**Tarih:** 4 Temmuz 2026  
**Proje Adı:** Personel ve Görev Takip Sistemi  

---

## Bu Hafta Yapılan Çalışmalar

### 1. Departman Yönetimi Modülü
Sistemdeki iş birimlerini yönetmek amacıyla Departman Yönetimi modülü tamamlanmıştır:
* Business katmanında departman listeleme, ekleme, güncelleme ve silme işlevlerini gerçekleştiren servis yazılmıştır.
* Departman silinirken, o departmana bağlı aktif çalışan personel olması durumunda silme işlemini engelleyen iş kuralı sisteme entegre edilmiştir.
* Sadece yöneticilerin erişebileceği kontrolcü sınıfı oluşturulmuştur.
* Bootstrap kütüphanesi kullanılarak departman listeleme tablosu, yeni departman ekleme formu ve departman düzenleme formu tasarlanmıştır.

---

### 2. Personel Yönetimi Modülü
Çalışanların kaydı ve hesap yönetimi için Personel Yönetimi modülü tamamlanmıştır:
* Business katmanında personel listeleme, ekleme, seçmeli güncelleme ve durum değiştirme işlevlerini yerine getiren servis yazılmıştır.
* Personel eklenirken veya güncellenirken e-posta adresinin veritabanında benzersiz olmasını sağlayan iş kuralı yazılmıştır.
* Yeni personel eklenirken girilen şifreyi otomatik olarak hash formatına dönüştüren altyapı eklenmiştir.
* Personel bilgileri güncellenirken şifre alanının boş bırakılması durumunda mevcut şifrenin korunması, yeni şifre girildiğinde ise güncellenmesi sağlanmıştır.
* Personel rollerinin yazılım genelinde Türkçe karşılıklarına ("Yönetici" ve "Çalışan") dönüştürülmesi tamamlanmıştır.
* Personel tablosuna "Ünvan" (Görevi) sütunu eklenerek çalışanların şirketteki hiyerarşik pozisyonlarının (Örn: "Genel Müdür", "Yazılım Mühendisi") kaydedilmesi ve listelenmesi sağlanmıştır.
* Personellerin işe başlangıç tarihleri ve varsayılan aktif durumları otomatik olarak yapılandırılmıştır.

---

### 3. Şifre Sıfırlama ve Durum Yönetimi Arayüzü
Şifresini unutan personellerin taleplerini onaylamak için yönetim arayüzü kurulmuştur:
* Personel tablosundaki şifre sıfırlama isteği alanını varsayılana çeken servis metodu yazılmıştır.
* Personel listesinde şifre sıfırlama talebi gönderen kişilerin yanında uyarı rozeti gösterilmesi sağlanmıştır.
* Yöneticinin tek tıkla personelin şifresini varsayılan değere çekebileceği şifre sıfırlama işlevi kontrolcüye ve arayüze eklenmiştir.
* Personeli tamamen silmek yerine durumunu aktif veya pasif yapmaya yarayan buton tasarımı yapılmıştır.

---

## Gelecek Haftanın Planı
* Görev yönetimi için gerekli iş mantığı servislerinin yazılması.
* Görev ekleme, düzenleme ve detay sayfalarının tasarlanması.
* Personel ve yönetici rolleri için görev görme ve güncelleme yetkilerinin sınırlandırılması.
* Görevleri durum, öncelik ve departmana göre filtreleyecek filtreleme motorunun yazılması.
