# 6. Hafta Raporu: Dosya Yönetimi, İstatistik Paneli ve Animasyonlar

**Tarih:** 8 Temmuz 2026  
**Proje Adı:** Personel ve Görev Takip Sistemi  

---

## Bu Hafta Yapılan Çalışmalar

### 1. Görevlere Dosya Yükleme Servisi ve Güvenlik Limitleri
Görev süreçlerinde bilgi alışverişini artırmak amacıyla dosya yükleme altyapısı tamamlanmıştır:
* Business katmanında dosyaları sunucu diskine yükleyen ve veritabanı kayıtlarını yöneten dosya servisi yazılmıştır.
* Görev detayları sayfasında dosya yükleme formu ve yüklenen dosyaların listesi tasarlanmıştır.
* Güvenlik kontrolü olarak sadece görevle yetkilendirilmiş personelin ve yöneticilerin dosya yüklemesine, indirmesine veya silmesine izin verilmiştir.
* Dosya boyutu maksimum 5MB olarak sınırlandırılmış, yalnızca PDF ve görsel uzantılı dosyaların (JPG, JPEG, PNG, GIF) yüklenmesine izin veren doğrulama kodları yazılmıştır.

---

### 2. Yönetici İstatistik Paneli (Dashboard)
Yöneticilerin şirketin genel durumunu izleyebilmesi amacıyla İstatistik Paneli kurulmuştur:
* Toplam departman, toplam personel ve toplam görev sayılarını gösteren gösterge kartları tasarlanmıştır.
* Grafik entegrasyonu için Chart.js kütüphanesi sisteme entegre edilmiştir.
* Görevlerin süreç dağılımlarını gösteren dairesel grafik ile görevlerin öncelik derecelerini gösteren çubuk grafik arayüze eklenmiştir.
* Yönetici menüsündeki Dashboard yönlendirmesi bu yeni panele bağlanmıştır.

---

### 3. Arayüz Tasarım ve Animasyon İyileştirmeleri
Sitenin kullanıcı deneyimini daha akıcı ve profesyonel hale getirmek için CSS animasyonları eklenmiştir:
* Sayfaların ve kartların yüklenirken hafifçe yukarı kayarak belirmesini sağlayan sayfa geçiş efekti eklenmiştir.
* Butonlara üzerine gelindiğinde süzülme ve yumuşak gölge efekti uygulanmıştır.
* Form elemanlarına odaklanıldığında sınır çizgisi ve renk geçişi eklenmiştir.
* Tablolardaki satırların üzerine gelindiğinde hafifçe vurgulanması ve büyümesi sağlanmıştır.
* Görev kartlarının başlıklarına renk geçişi animasyonu uygulanmıştır.

---

## Gelecek Haftanın Planı
* Excel ve PDF formatlarında kurumsal raporlar üretecek raporlama servislerinin yazılması.
* Personel listesini Excel dosyası olarak, görev listesini ise PDF olarak indirmeyi sağlayan buton ve işlevlerin eklenmesi.
* Sistem genelinde oluşabilecek beklenmedik hataları yakalayıp günlük log dosyalarına yazacak hata yakalama ara yazılımının geliştirilmesi.
