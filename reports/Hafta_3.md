# 3. Hafta Raporu: Kullanıcı Giriş Sistemi

**Tarih:** 2 Temmuz 2026  
**Proje Adı:** Personel ve Görev Takip Sistemi  

---

## Bu Hafta Yapılan Çalışmalar

### 1. Kimlik Doğrulama Altyapısının Kurulması
Kullanıcıların sistemde oturum açabilmeleri için çerez tabanlı kimlik doğrulama altyapısı yapılandırılmıştır:
* WebUI projesindeki Program.cs dosyasına çerez kimlik doğrulama servisleri eklenmiştir.
* Giriş yapmamış kullanıcıların yönlendirileceği giriş sayfası tanımlanmıştır.
* Kimlik doğrulama ve yetkilendirme ara yazılımları Program.cs dosyasına entegre edilmiştir.

---

### 2. Şifre Güvenliği ve Doğrulama Mantığı
Kullanıcı şifrelerinin güvenliğini sağlamak ve giriş bilgilerini doğrulamak için arka plan kodları yazılmıştır:
* Business katmanında şifreleri SHA-256 algoritmasıyla şifreleyen ve doğruluğunu kontrol eden yardımcı sınıf oluşturulmuştur.
* E-posta adresi ve şifre bilgilerini veritabanından doğrulayan giriş servisi yazılmıştır.
* Giriş servisi Program.cs dosyasına bağımlılık enjeksiyonu ile kaydedilmiştir.

---

### 3. Form Modelleri ve Doğrulama Yapıları
Kullanıcılardan alınan verilerin sunucu tarafında doğrulanması amacıyla form modelleri hazırlanmıştır:
* Giriş formu verilerini taşımak ve boş bırakma durumlarını kontrol etmek için giriş modeli yazılmıştır.
* Şifre değiştirme ekranında eski şifre, yeni şifre ve yeni şifre tekrarı bilgilerini doğrulamak için şifre değiştirme modeli yazılmıştır.
* Şifresini unutan kullanıcıların e-posta adreslerini doğrulamak amacıyla şifremi unuttum modeli oluşturulmuştur.

---

### 4. Kontrolcü Yönlendirmeleri, Sayfa Tasarımları ve Veritabanı Güncellemesi
Arayüz yönlendirmelerini yapacak kontrolcü sınıfları, kullanıcı ekranları ve veritabanı altyapısı hazırlanmıştır:
* Giriş, çıkış, şifre değiştirme ve şifremi unuttum yönlendirmelerini yöneten kontrolcü sınıfı güncellenmiştir.
* Giriş yapmamış kullanıcıların ana sayfaya erişmesini engellemek için ana sayfa kontrolcüsü koruma altına alınmıştır.
* Şifre sıfırlama taleplerini veritabanında saklayabilmek için personel tablosuna yeni bir sütun eklenmiş ve veritabanı şeması güncellenmiştir.
* Bootstrap kütüphanesi kullanılarak şık ve mobil uyumlu giriş ekranı, şifre değiştirme ekranı ve şifremi unuttum ekranı tasarlanmıştır.
* Sitenin ana şablonundaki menüler, giriş yapan kullanıcının rolüne göre dinamikleştirilmiştir. Yönetici rolündeki kullanıcılara tam yönetim menüsü, çalışan rolündeki kullanıcılara ise sadece kendi görev menüsü gösterilmiştir.
* Giriş yapan kullanıcıların karşılandığı ana sayfa ekranı rol bazlı kılavuz kartlarıyla güncellenmiştir.

---

## Gelecek Haftanın Planı
* Departman yönetimi için gerekli iş mantığı servislerinin yazılması.
* Departman listeleme, ekleme, güncelleme ve silme ekranlarının tasarlanması.
* Personel yönetimi için gerekli iş mantığı servislerinin yazılması.
* Personel listeleme, ekleme ve güncelleme ekranlarının tasarlanması.
