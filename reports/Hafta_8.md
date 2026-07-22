# 8. Hafta Staj Raporu

Stajın bu son haftasında yapılan çalışmalar, projenin entegrasyon testlerinin gerçekleştirilmesi, veritabanı kurulum betiklerinin hazırlanması, kullanıcı ve kurulum kılavuzlarının yazılması ile genel teslimat süreçlerinin tamamlanmasına ayrılmıştır.

## Gerçekleştirilen Faaliyetler

### 1. Test Süreçleri ve Sistem Kararlılığı
Geliştirilen uygulamanın tüm fonksiyonel parçaları bir araya getirilerek entegrasyon ve akış testleri gerçekleştirilmiştir.
* Farklı kullanıcı rolleriyle sisteme giriş yapılarak yönlendirme kuralları test edilmiştir. Yöneticilerin gösterge paneline, çalışanların ise görev panosuna hatasız yönlendirildiği doğrulanmıştır.
* Görevler üzerinde gerçekleştirilen sürükle-bırak işlemleri, durum değişiklikleri ve yetki denetimleri kontrol edilmiştir. Yetkisiz bir personelin görevi kaydırma girişimlerinin engellendiği ve kartların eski konumlarına sorunsuzca döndüğü gözlemlenmiştir.
* Dosya yükleme ve dosya boyutu doğrulama senaryoları test edilmiştir. Belirlenen dosya boyutu limitini aşan veya geçersiz formattaki belgelerin yüklenmesinin engellendiği teyit edilmiştir.
* Excel ve PDF rapor üretme butonları çalıştırılarak indirilen belgelerin içerikleri, Türkçe karakter uyumlulukları ve biçimlendirmeleri incelenmiştir.

### 2. Veritabanı Kurulum Scriptinin Hazırlanması
Sistemin farklı ortamlarda kolayca ayağa kaldırılabilmesi amacıyla tüm tablo yapılarını, ilişkileri, kısıtları, indeksleri ve başlangıç verilerini içeren veritabanı script dosyası güncellenmiş ve proje kök dizinine aktarılmıştır. Bu sayede uygulamanın sıfırdan kurulum süreci basitleştirilmiştir.

### 3. Kurulum ve Kullanıcı Dokümantasyonu
Kullanıcıların ve sistemi kuracak teknik ekiplerin faydalanabilmesi amacıyla iki kapsamlı kılavuz yazılmıştır:
* **Kurulum Kılavuzu:** Projenin sistem gereksinimlerini, veritabanı ayarlarını, bağlantı dizesi yapılandırmasını ve uygulamanın terminal üzerinden derlenip çalıştırılması adımlarını içermektedir.
* **Kullanıcı Kılavuzu:** Uygulamanın sunduğu tüm özellikleri ekranlar düzeyinde açıklayan, rol bazlı panelleri, dosya yönetimi limitlerini ve raporlama işlevlerinin kullanımını anlatan rehberdir.

## Sonuç
Sekiz haftalık staj süreci boyunca planlanan tüm adımlar başarıyla tamamlanmıştır. Personel ve Görev Takip Sistemi; modern, güvenli, rol tabanlı yetkilendirmeye sahip, sürükle-bırak dinamikliğine ve raporlama yeteneklerine sahip kararlı bir web uygulaması olarak teslimata hazır hale getirilmiştir.
