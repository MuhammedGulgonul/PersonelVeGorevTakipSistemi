# KULLANICI KILAVUZU (USER_GUIDE.md)

Bu kılavuz, Personel ve Görev Takip Sistemi'nin özelliklerini ve kullanıcı panellerini nasıl kullanacağınızı açıklamaktadır.

---

## 1. Sisteme Giriş ve Rol Tabanlı Yönlendirme
Sisteme e-posta ve şifrenizle giriş yaptıktan sonra, sistem sizi rolünüze göre otomatik olarak ilgili ana sayfaya yönlendirir:
* **Yöneticiler:** Doğrudan **Yönetici Gösterge Paneline (Dashboard)** yönlendirilir. Burada şirketteki departman, personel ve görev sayıları ile istatistiksel grafikleri görebilirler.
* **Çalışanlar:** Doğrudan **Görev Panosuna (Kanban)** yönlendirilir. Burada kendilerine atanmış olan güncel görevleri görebilirler.

---

## 2. Yönetici Gösterge Paneli (Dashboard)
Yalnızca yöneticilerin erişebildiği bu ekranda şu özellikler yer alır:
* **İstatistik Kartları:** Toplam departman, toplam personel ve toplam görev sayılarını gösteren özet kartlar.
* **Durum Dağılım Grafiği (Doughnut):** Görevlerin süreç durumlarına göre (Yapılacak, Yapılıyor, Tamamlandı) dağılımını gösteren renkli dairesel grafik.
* **Öncelik Dağılım Grafiği (Bar):** Görevlerin öncelik derecelerini (Düşük, Orta, Yüksek) gösteren çubuk grafik.

---

## 3. Görev Panosu (Kanban) ve Sürükle-Bırak Kullanımı
Görevlerin süreç durumları "Yapılacak", "Yapılıyor" ve "Tamamlandı" olmak üzere 3 sütunda listelenir.
* **Sürükle-Bırak:** Bir görevin durumunu değiştirmek için görev kartını mouse ile tutup istediğiniz sütuna sürükleyip bırakabilirsiniz. Fare imleci kartın üzerindeyken açık el simgesine dönüşür, kartı tuttuğunuzda ise kavrama simgesini alır.
* **Sayfa Yenilemesiz Dinamik Akış:** Sürükleme işlemi sonrasında sayfa yeniden yüklenmez. Sütunların başındaki görev sayıları anlık güncellenir ve kartın altındaki eylem butonları (Durdur, Tamamla vb.) yeni durumuna göre dinamik olarak anında yenilenir.
* **Güvenlik Sınırı:** Çalışanlar yetkileri dışındaki veya kendilerine ait olmayan görevleri sürüklemeye çalıştıklarında hata uyarısı alırlar ve kart otomatik olarak eski sütununa geri döner.

---

## 4. Görev Detayları ve Dosya Yönetimi
Görev başlığına tıklayarak detay sayfasına gidebilirsiniz. Bu sayfada:
* **Görev Künyesi:** Görevin açıklaması, önceliği, atanmış personel, son teslim tarihi ve oluşturulma tarihi listelenir.
* **Görevi Tamamen Silme:** Yöneticiler detay sayfasının sağ üstünde veya doğrudan Kanban kartının sağ üst köşesindeki kırmızı çöp kutusu simgesini kullanarak görevi tamamen silebilirler.
* **Dosya Yükleme (Maks. 5 MB):** Göreve dosya eklemek için dosya yükleme formu kullanılır. Yalnızca PDF belgeleri ile resim formatları (JPG, JPEG, PNG, GIF) kabul edilir. 5 MB boyut sınırını aşan veya geçersiz uzantılı dosyalar engellenir.
* **Dosya İndirme ve Silme:** Yüklenen dosyalar liste halinde gösterilir. Resimlerin veya PDF dosyalarının yanlarındaki indirme ikonuna tıklayarak bilgisayarınıza indirebilirsiniz. Dosyayı yükleyen personel veya yöneticiler çöp kutusu simgesiyle dosyayı fiziki sunucudan ve veritabanından silebilirler.

---

## 5. Departman ve Personel Yönetimi (Sadece Yönetici)
* **Departmanlar:** Yeni departman ekleme, mevcut departmanları düzenleme ve silme işlemleri yapılabilir.
* **Personeller:** Şirket çalışanlarının listelendiği ekrandır. Yeni personel eklenebilir, unvanı, departmanı ve sistem rolü (Yönetici / Çalışan) düzenlenebilir, şifre sıfırlama talepleri yönetilebilir.

---

## 6. Excel ve PDF Formatlarında Rapor Alma
* **Personel Listesi Excel Raporu:** Personeller sayfasının sağ üst köşesinde bulunan **"Excel'e Aktar"** butonuna basarak tüm personel listesini biçimlendirilmiş bir Excel tablosu olarak bilgisayarınıza indirebilirsiniz.
* **Görev Listesi PDF Raporu:** Görev Panosu sayfasının sağ üst köşesinde bulunan **"PDF Raporu İndir"** butonuna basarak görev listesini indirebilirsiniz.
  * Yöneticiler tıkladığında sistemdeki tüm görevlerin raporu gelir.
  * Çalışanlar tıkladığında ise sadece kendi üzerlerine atanmış görevlerin PDF raporunu alabilirler.
  * Raporda süresi geçen ve henüz bitirilmemiş olan görevlerin bitiş tarihleri kırmızı renk ile vurgulanır.

---

## 7. Sistem Hata Günlükleri (Log İzleme)
* Yöneticiler tarayıcının adres çubuğuna doğrudan **`/Log`** yazarak gizli sistem günlüklerine erişebilir.
* Burada uygulamada oluşan beklenmedik hatalar listelenir.
* **"Detay"** butonuna basılarak hatanın teknik yığın izi (Stack Trace) görüntülenebilir.
* **"Tümünü Temizle"** butonuyla hata günlükleri kalıcı olarak sıfırlanabilir.
