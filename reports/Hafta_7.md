# 7. Hafta Staj Raporu

Bu hafta yapılan çalışmalar, sistemin raporlama yeteneklerini güçlendirmeye, veri dışa aktarım süreçlerini tamamlamaya ve genel hata yönetimi ile günlükleme mekanizmalarını kurmaya odaklanmıştır.

## Gerçekleştirilen Faaliyetler

### 1. Excel ve PDF Formatlarında Raporlama Hizmeti
Sistemdeki verilerin yöneticiler ve ilgili personel tarafından indirilebilmesi amacıyla raporlama hizmeti geliştirilmiştir. 
* Personel listesinin Excel formatında indirilebilmesi sağlanmıştır. Bu kapsamda, tüm çalışanların özlük bilgileri, kayıt tarihleri, görev tanımları ve departman bilgileri düzenli bir tablo yapısıyla dışa aktarılmaktadır. Tablonun kolon genişlikleri içeriğe göre dinamik ayarlanmakta ve kurumsal renk temamıza uygun biçimlendirme uygulanmaktadır.
* Görev listesinin PDF formatında dışa aktarılması sağlanmıştır. Görevlerin başlıkları, açıklamaları, süreç durumları, öncelik seviyeleri, atanan personeller ve son teslim tarihleri yatay bir belge yapısında raporlanmaktadır. Türkçe karakterlerin sorunsuz gösterilmesi için sistem yazı tipleri entegre edilmiştir. Teslim tarihi geçmiş ve henüz tamamlanmamış olan geciken görevler, raporda kırmızı kalın yazı tipiyle vurgulanmaktadır.

### 2. Global Hata Yakalama Mekanizması
Uygulama genelinde oluşabilecek tüm beklenmeyen çalışma zamanı hatalarını kontrol altına almak amacıyla global bir hata yakalama ara yazılımı sisteme dahil edilmiştir.
* Sistemde meydana gelen her türlü beklenmedik çökme veya veritabanı bağlantı hatası, kullanıcıya yansıtılmadan önce arka planda yakalanmaktadır.
* Yakalanan hataların seviyeleri, hata mesajları, detaylı yığın izleri ve oluşma zamanları veritabanındaki günlük tablosuna kaydedilmektedir.
* Hata oluştuğunda kullanıcılar teknik detaylar içeren kafa karıştırıcı sayfalar yerine, kurumsal ve anlaşılır bir hata bilgilendirme sayfasına yönlendirilmektedir.

### 3. Yönetici İşlem ve Hareket Günlüğü İzleme Ekranı
Yöneticilerin sistemdeki kullanıcı faaliyetlerini ve sağlık durumunu takip edebilmesi için gizli bir günlük izleme paneli tasarlanmıştır.
* Bu panele yalnızca yönetici rolüne sahip kullanıcılar doğrudan adres satırından erişebilmektedir. Güvenlik gerekçesiyle üst menüde bu sayfanın bir bağlantısı bulunmamaktadır.
* Sayfa üzerinde veritabanına kaydedilen tüm kullanıcı hareketleri ve sistem hataları en yeniden en eskiye doğru listelenmektedir. Kullanıcıların giriş, çıkış, departman ekleme, personel güncelleme, şifre sıfırlama, görev atama, dosya yükleme ve silme gibi tüm kritik işlemleri kayıt altına alınmaktadır.
* Her bir günlük kaydının yanında bulunan detay butonu yardımıyla, işlemi yapan kullanıcının adı, e-postası, varsa işlemin teknik detayları veya sistem hatası yığın izi ayrı bir panelde genişletilerek incelenebilmektedir.
* Yöneticilerin günlük listesini tamamen temizleyebilmesi için güvenli bir temizleme seçeneği eklenmiştir.

## Sonuç
Yapılan çalışmalarla birlikte uygulamanın yönetimsel denetim yetenekleri artırılmış, veri analizi için gerekli raporlama servisleri devreye alınmış ve çalışma zamanı kararlılığı ile kullanıcı hareket takibi günlükleme mekanizmalarıyla güvence altına alınmıştır.
