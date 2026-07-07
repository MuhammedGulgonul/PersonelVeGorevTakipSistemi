# 5. Hafta Raporu: Görev Yönetimi ve Kanban Pano Sistemi

**Tarih:** 7 Temmuz 2026  
**Proje Adı:** Personel ve Görev Takip Sistemi  

---

## Bu Hafta Yapılan Çalışmalar

### 1. Görev Yönetimi İş Mantığı
Görev süreçlerini arka planda koordine etmek amacıyla Görev Servisi yazılmıştır:
* Business katmanında tüm görevleri departman ve personel bilgisiyle çeken metot yazılmıştır.
* Giriş yapan çalışanların sadece kendilerine atanmış görevleri görebilmeleri için filtreleme metotları eklenmiştir.
* Görev ekleme, güncelleme ve silme iş kuralları tanımlanmıştır.
* Görev durumlarını hızlıca değiştiren durum güncelleme işlevi servise dahil edilmiştir.

---

### 2. Kanban Panosu ve Arayüz Tasarımı
Görevlerin görsel olarak takip edilebilmesi için Kanban Pano arayüzü tasarlanmıştır:
* Görevler "Yapılacak", "Yapılıyor" ve "Tamamlandı" olarak adlandırılan 3 bağımsız süreç sütununda listelenmektedir.
* Görev kartlarının üst kısmında öncelik derecelerini belirten renkli etiketler yer almaktadır.
* Görev kartı tasarımı kısmi görünüm altyapısına bölünerek kod karmaşıklığı önlenmiştir.
* Görevin son teslim tarihi geçtiğinde ve görev henüz tamamlanmadığında, tarih bilgisini otomatik olarak kırmızı renkle vurgulayan görsel uyarı sistemi eklenmiştir.

---

### 3. Rol Yetkilendirmesi ve Hızlı İşlemler
Yönetici ve çalışanların yetki sınırları arayüz düzeyinde ayrıştırılmıştır:
* Yöneticiler yeni görev oluşturma, düzenleme, silme ve görev atama yetkilerine sahip kılınmıştır.
* Çalışanlar sadece kendilerine atanmış görevleri görüntüleyebilir ve bu görevleri hızlı durum butonlarıyla sütunlar arasında kaydırabilir.
* Görev kartlarının altına yerleştirilen "Görevi Başlat", "Durdur", "Tamamla" ve "Yeniden Aç" butonlarıyla tek tıkla süreç güncelleme imkanı sağlanmıştır.

---

### 4. Filtreleme Sistemi
Görevlerin kolayca bulunabilmesi için gelişmiş arama motoru eklenmiştir:
* Görevler öncelik derecesine (Düşük, Orta, Yüksek) göre filtrelenebilmektedir.
* Görevler güncel durumlarına göre filtrelenebilmektedir.
* Yöneticilerin tüm şirketi görebilmesi amacıyla görevler atanan personelin bağlı olduğu departmana göre filtrelenebilmektedir.

---

## Gelecek Haftanın Planı
* Sistem genelinde yapılan kritik işlemlerin ve hataların kaydedileceği Log Yönetimi modülünün yazılması.
* Log verilerini tarihsel olarak listeleyen ve detaylarını gösteren log izleme ekranlarının tasarlanması.
* Staj projesinin genel güvenlik, kod kalitesi ve performans testlerinin yapılması.
