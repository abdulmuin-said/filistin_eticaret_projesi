# Admin Paneli – Frontend Özellik Eşleşme Analizi

## 1. Yönetici Özeti

Bu analiz raporu, **7ANRPS48.com (Filistin E-Ticaret)** projesinin Admin paneli ile Kullanıcı (Frontend) tarafı arasındaki veri akışlarını, form alanlarını, controller/endpoint yapılarını ve veritabanı entity'lerini kod seviyesinde uçtan uca inceleyerek hazırlanmıştır.

* **İncelenen Admin Sayfası / Modülü Sayısı**: 25 Admin Controller, 29 Admin View Dizini (Toplam ~35 Yönetim Ekranı/Route)
* **İncelenen Frontend Sayfası Sayısı**: 14 Frontend Controller, 10 View Dizini (Toplam ~25 Kullanıcı Ekranı)
* **İncelenen Form Alanı Sayısı**: 140+ Form Alanı (Ürün, Kategori, Sipariş, Kargo, İndirim, Ayarlar, Kullanıcı vb.)
* **Tespit Edilen Toplam Bulgu Sayısı**: 22 Belirgin Uyumsuzluk / Kopukluk
  * 🔴 **Kritik Bulgu Sayısı**: 4
  * 🟠 **Yüksek Öncelikli Bulgu Sayısı**: 5
  * 🟡 **Orta Öncelikli Bulgu Sayısı**: 6
  * 🟢 **Düşük Öncelikli Bulgu Sayısı**: 4
  * ❓ **Doğrulama / İş Kararı Gereken Bulgu Sayısı**: 3
* **Kod / Derleme Doğrulaması**: `dotnet build FilistinProje.sln` çalıştırılarak 0 Hata, 0 Uyarı ile proje derleme durumu doğrulanmıştır.

---

## 2. Proje Mimarisi

* **Backend**: ASP.NET Core 10.0 MVC (Clean Architecture: Web, Service, Data, Core)
* **Veritabanı**: PostgreSQL + EF Core (Dual Migration & SQL Init `EnsureMissingMarch2026SchemaAsync`)
* **Ön Yüz (Frontend)**: Razor Views (`.cshtml`), Vanilla CSS, Tailwind CSS, JavaScript (RTL & LTR Çoklu Dil Desteği - Arapça `ar` varsayılan, İngilizce `en`)
* **Güvenlik & Yetkilendirme**: ASP.NET Core Identity (`AppUser`), Admin Permission Matrix (`AdminBaseController`), Güvenli Dosya Depolama (`private://` mimarisi & `BelgeController`)

---

## 3. Admin Sayfaları Envanteri

| No | Admin Modülü | Sayfa / Ekran | Route | İlgili Controller / View | Frontend Karşılığı | Durum |
|---|---|---|---|---|---|---|
| 1 | Dashboard | Ana Ekran | `/Admin/Home` | `HomeController.cs` -> `Home/Index.cshtml` | Yok (İç Operasyon) | Tam çalışıyor |
| 2 | İçerik Yönetimi | Ana Sayfa Yönetimi (Eski) | `/Admin/AnaSayfa` | `AnaSayfaController.cs` -> `AnaSayfa/Index.cshtml` | `Views/Home/Index.cshtml` | ❌ **Hatalı / Kullanılmıyor** (JSON ayarlarını okumuyor) |
| 3 | İçerik Yönetimi | Ana Sayfa Bölümleri | `/Admin/HomeSections` | `HomeSectionsController.cs` -> `HomeSections/*` | `Views/Home/Index.cshtml` | Tam çalışıyor |
| 4 | İçerik Yönetimi | Slider Yönetimi | `/Admin/Slayt` | `SlaytController.cs` -> `Slayt/*` | `Views/Home/Index.cshtml` (Hero Slider) | Tam çalışıyor |
| 5 | Ürün Yönetimi | Ürün Listesi | `/Admin/Urun` | `UrunController.cs` -> `Urun/Index.cshtml` | `Views/Urun/Index.cshtml` | Tam çalışıyor |
| 6 | Ürün Yönetimi | Ürün Ekle / Düzenle | `/Admin/Urun/Ekle`, `/Admin/Urun/Duzenle/{id}` | `UrunController.cs` -> `Urun/Ekle.cshtml`, `Duzenle.cshtml` | `Views/Urun/Detay.cshtml` | ⚠️ **Kısmen Çalışıyor** (KisaAd, SKU, Barkod, KampanyaBitis gösterilmiyor) |
| 7 | Ürün Yönetimi | Ürün Özellik Tanımları | `/Admin/UrunOzellik` | `UrunOzellikController.cs` -> `UrunOzellik/*` | `Views/Urun/_ProductInfo.cshtml` | Tam çalışıyor |
| 8 | Kategori Yönetimi | Kategoriler | `/Admin/Kategori` | `KategoriController.cs` -> `Kategori/*` | `Views/Urun/Index.cshtml`, `_Header.cshtml` | ⚠️ **Kısmen Çalışıyor** (BannerUrl, UstMetin, AltMetin gösterilmiyor) |
| 9 | Sipariş Yönetimi | Sipariş Listesi & Detay | `/Admin/Siparis`, `/Admin/Siparis/Detay/{id}` | `SiparisController.cs` -> `Siparis/*` | `Views/Profil/Siparislerim.cshtml`, `SiparisDetay.cshtml` | Tam çalışıyor |
| 10 | Sipariş Yönetimi | Toplu Etiket Basımı | `/Admin/Siparis/TopluEtiket` | `SiparisController.cs` -> `TopluEtiket.cshtml` | Yok (İç Operasyon) | Tam çalışıyor (Menüde gizli route) |
| 11 | Kargo Yönetimi | Kargo Firmaları & Bölgeleri | `/Admin/Kargo` | `KargoController.cs` -> `Kargo/Index.cshtml` | `Views/Siparis/Odeme.cshtml` (Kargo hesaplama) | Tam çalışıyor |
| 12 | İade Yönetimi | İade Talepleri | `/Admin/Iade` | `IadeController.cs` -> `Iade/*` | `Views/Profil/IadeOlustur.cshtml` | Tam çalışıyor |
| 13 | Müşteri Yönetimi | Kullanıcılar | `/Admin/Kullanici` | `KullaniciController.cs` -> `Kullanici/*` | `Views/Profil/Index.cshtml` | Tam çalışıyor |
| 14 | Müşteri Yönetimi | Toptancılar & Başvurular | `/Admin/Toptanci` | `ToptanciController.cs` -> `Toptanci/Index.cshtml` | `Views/Profil/Index.cshtml`, `_ProductInfo.cshtml` | Tam çalışıyor |
| 15 | Müşteri Yönetimi | Toptancı İskonto Grupları | `/Admin/Toptanci/UrunGruplari` | `ToptanciController.cs` -> `UrunGruplari.cshtml` | `Services/OrderPricingService.cs`, `SepetService.cs` | ❌ **Frontend Karşılığı Yok / İskonto Uygulanmıyor** |
| 16 | Kupon Yönetimi | İndirim Kuponları | `/Admin/Kupon` | `KuponController.cs` -> `Kupon/*` | `Views/Sepet/Index.cshtml`, `Controllers/SepetController.cs` | ⚠️ **Kısmen Çalışıyor** (Kullanılan miktar artırılmıyor) |
| 17 | Sistem Ayarları | Genel Site Ayarları | `/Admin/Ayarlar` | `AyarlarController.cs` -> `Ayarlar/Index.cshtml` | `Views/Shared/_Header.cshtml`, `_Footer.cshtml`, `_Layout.cshtml` | ⚠️ **Kısmen Çalışıyor** (UstBarMesaji, TemaRengi, Analytics, Cookie gösterilmiyor) |
| 18 | İletişim Yönetimi | İletişim Mesajları | `/Admin/Iletisim` | `IletisimController.cs` -> `Iletisim/*` | `Views/Kurumsal/Iletisim.cshtml` | Tam çalışıyor |
| 19 | Yorum Yönetimi | Ürün Değerlendirmeleri | `/Admin/Yorum` | `YorumController.cs` -> `Yorum/Index.cshtml` | `Views/Urun/_Reviews.cshtml` | Tam çalışıyor |
| 20 | Bülten Yönetimi | E-Posta Aboneleri | `/Admin/Bulten` | `BultenController.cs` -> `Bulten/Index.cshtml` | `Views/Shared/_Footer.cshtml` | Tam çalışıyor |
| 21 | Kurumsal Sayfalar | Statik Sayfa Yönetimi | `/Admin/Sayfa` | `SayfaController.cs` -> `Sayfa/*` | `Views/Kurumsal/*` | ❌ **Frontend Karşılığı Yok** (Hakkımızda/Gizlilik statik view'lara hardcoded bağlanmış) |
| 22 | Personel Yönetimi | Yetki Matrisi & Roller | `/Admin/Personel` | `PersonelController.cs` -> `Personel/*` | Yok (İç Operasyon) | Tam çalışıyor |
| 23 | Raporlama | Satış & Stok Raporları | `/Admin/Rapor` | `RaporController.cs` -> `Rapor/Index.cshtml` | Yok (İç Operasyon) | Tam çalışıyor |
| 24 | Analiz & İzleme | Ziyaretçi Logları | `/Admin/Ziyaretci` | `ZiyaretciController.cs` -> `Ziyaretci/Index.cshtml` | Yok (İç Operasyon) | Tam çalışıyor |
| 25 | Banka Yönetimi | IBAN & Banka Hesapları | `/Admin/Bankalar` | `BankalarController.cs` -> `Bankalar/Index.cshtml` | `Views/Siparis/_PaymentOptions.cshtml` | Tam çalışıyor |
| 26 | Güvenlik İzleme | Secret Monitor | `/Admin/XyzSecretMonitor` | `XyzSecretMonitorController.cs` | Yok (İç Operasyon) | Tam çalışıyor (Menüde gizli route) |
| 27 | Çark Ödülleri (Eski) | Çark Yönetimi | `/Admin/CarkOdul` | Fiziksel Görünüm Yok (Boş Dizin) | Kaldırıldı (Migration 20260713120000) | ❌ **Kullanılmıyor / Boş Dizin** |
| 28 | Push Bildirim (Eski) | Web Push Abonelikleri | `/Admin/PushBildirim` | Fiziksel Görünüm Yok (Boş Dizin) | `PushAbonelik` entity var, UI yok | ❌ **Kullanılmıyor / Boş Dizin** |
| 29 | Toplu Fiyat (Eski) | Toplu Güncelleme | `/Admin/TopluFiyatGuncelle` | Fiziksel Görünüm Yok (Boş Dizin) | Yok | ❌ **Kullanılmıyor / Boş Dizin** |
| 30 | Ürün Import (Eski) | Excel / JSON Import | `/Admin/UrunImport` | Fiziksel Görünüm Yok (Boş Dizin) | `UrunController.cs` içinde yapılıyor | ❌ **Kullanılmıyor / Boş Dizin** |

---

## 4. Alan Bazlı Eşleşme Matrisi

| Admin Alanı | Veritabanına Kaydediliyor mu? | Frontend'e Taşınıyor mu? | UI'da Görünüyor mu? | Sepete Yansıyor mu? | Siparişe Yansıyor mu? | Durum |
|---|---|---|---|---|---|---|
| `Urun.Baslik` / `BaslikAr` / `BaslikEn` | ✅ Evet | ✅ Evet | ✅ Evet | ✅ Evet | ✅ Evet | 🟢 Tam Çalışıyor |
| `Urun.KisaAd` | ✅ Evet | ❌ Hayır | ❌ Görünmüyor | ❌ Hayır | ❌ Hayır | 🟡 Frontend'de Kullanılmıyor |
| `Urun.SKU` | ✅ Evet | ❌ Hayır | ❌ Görünmüyor | ❌ Hayır | ❌ Hayır | 🟡 Frontend'de Kullanılmıyor |
| `Urun.Barkod` | ✅ Evet | ❌ Hayır | ❌ Görünmüyor | ❌ Hayır | ❌ Hayır | 🟡 Frontend'de Kullanılmıyor |
| `Urun.KisaAciklama` / `Aciklama` | ✅ Evet | ✅ Evet | ✅ Evet | ❌ Gerek yok | ❌ Gerek yok | 🟢 Tam Çalışıyor |
| `Urun.Fiyat` / `IndirimliFiyat` | ✅ Evet | ✅ Evet | ✅ Evet | ✅ Evet | ✅ Evet | 🟢 Tam Çalışıyor |
| `Urun.TopFiyat` (Toptan Fiyat) | ✅ Evet | ✅ Evet | ✅ Evet (Toptancıysa) | ✅ Evet | ✅ Evet | 🟢 Tam Çalışıyor |
| `Urun.Maliyet` | ✅ Evet | ❌ İç Alan | ❌ İç Alan | ❌ İç Alan | ❌ İç Alan | 🟢 İç Operasyon (Doğru) |
| `Urun.KdvOrani` | ✅ Evet | ❌ Hayır | ❌ Görünmüyor | ❌ Hesaba Katılmıyor | ❌ Hesaba Katılmıyor | 🟠 Fiyat motorunda vergi kırılımı yapılmıyor |
| `Urun.MinSiparisAdedi` | ✅ Evet | ✅ Evet | ✅ Evet | ✅ Kontrol ediliyor | ✅ Kontrol ediliyor | 🟢 Tam Çalışıyor |
| `Urun.MaxSiparisAdedi` | ✅ Evet | ✅ Evet (UI limit) | ⚠️ UI uyarısı var | ❌ **Sepete eklemede backend aşılıyor** | ❌ Aşılabiliyor | 🔴 Backend sepete ekleme kontrolü eksik |
| `Urun.KampanyaBitisTarihi` | ✅ Evet | ❌ Hayır | ❌ Görünmüyor | ❌ **Süre dolsa da indirim devam ediyor** | ❌ İndirim düşüyor | 🔴 `EtkinFiyat` tarih kontrolü yapmıyor |
| `Urun.HediyePaketiVarMi` / `Fiyati` | ✅ Evet | ✅ Evet | ✅ Evet | ✅ Evet | ✅ Evet | 🟢 Tam Çalışıyor |
| `Urun.WhatsappSiparisVarMi` / `FiyatGizliMi` | ✅ Evet | ✅ Evet | ✅ Evet | ❌ WhatsApp CTA | ❌ WhatsApp CTA | 🟢 Tam Çalışıyor |
| `Kategori.BannerUrl` | ✅ Evet | ❌ Hayır | ❌ Görünmüyor | ❌ Gerek yok | ❌ Gerek yok | 🟠 Kategori sayfasında gösterilmiyor |
| `Kategori.UstMetin` / `AltMetin` | ✅ Evet | ❌ Hayır | ❌ Görünmüyor | ❌ Gerek yok | ❌ Gerek yok | 🟠 SEO ve kategori açıklama alanları gösterilmiyor |
| `Kategori.UrunSiralamaTipi` | ✅ Evet | ❌ Hayır | ❌ Dikkate Alınmıyor | ❌ Gerek yok | ❌ Gerek yok | 🟠 Frontend sıralama varsayılan sorguyu kullanıyor |
| `SiteAyarlari.UstBarMesaji` / `Etkin` / `Hizi` | ✅ Evet | ❌ Hayır | ❌ **Sabit metinler gösteriliyor** | ❌ Gerek yok | ❌ Gerek yok | 🟡 Üst bar ayarları frontend header'a yansımıyor |
| `SiteAyarlari.TemaRengi` | ✅ Evet | ❌ Hayır | ❌ Uygulanmıyor | ❌ Gerek yok | ❌ Gerek yok | 🟡 CSS tema rengine bağlanmamış |
| `SiteAyarlari.CookieMetni` | ✅ Evet | ❌ Hayır | ❌ Çerez Barı Yok | ❌ Gerek yok | ❌ Gerek yok | 🟡 Çerez rıza barı render edilmiyor |
| `SiteAyarlari.GoogleAnalyticsId` / `FacebookPixelId` | ✅ Evet | ❌ Hayır | ❌ Script Eklenmiyor | ❌ Gerek yok | ❌ Gerek yok | 🟡 Layout `<head>` script etiketleri eklenmemiş |
| `ToptanciIskontoOrani` (Gruplar) | ✅ Evet | ❌ Hayır | ❌ Görünmüyor | ❌ **Hesaplanmıyor** | ❌ İskonto Düşmüyor | 🔴 Toptancı kademeli iskonto motoru pasif |
| `Kupon.KullanilanMiktar` | ✅ Evet | ❌ **Artırılmıyor** | ❌ Statik 0 kalıyor | ❌ Kontrol bypass ediliyor | ❌ Miktar güncellenmiyor | 🔴 Kupon kullanım limiti aşılabiliyor |

---

## 5. Kritik Bulgular

### `ADMIN-FRONTEND-001` — Kupon Kullanım Sayacı (`KullanilanMiktar`) Sipariş Tamamlandığında Artırılmıyor

* **Önem Seviyesi**: 🔴 Kritik
* **Modül**: Kampanya & İndirim / Sipariş Akışı
* **Admin Tarafı**: `KuponController.cs` (`Ekle.cshtml`, `Duzenle.cshtml`), Form Alanı: `KullanimLimiti` (Örn: 100 kişi).
* **Frontend Tarafı**: `SiparisController.cs` (`CreateOrder` POST action methodu).
* **Mevcut Davranış**: Kullanıcı ödeme yapıp sipariş oluşturduğunda `UygulananKupon` koduna göre indirim tutarı hesaplanıyor ve sipariş kaydediliyor. Ancak `_context.Kuponlar` tablosundaki ilgili kuponun `KullanilanMiktar` property'si artırılmıyor (`kupon.KullanilanMiktar += 1` yapılmıyor).
* **Beklenen Davranış**: Sipariş başarıyla veritabanına yazıldığında kuponun `KullanilanMiktar` değeri 1 artırılmalı ve kaydedilmelidir.
* **Teknik Kök Neden**: `SiparisController.cs` içerisindeki `CreateOrder` işleminde kupon indirimi hesaplandıktan sonra kupon varlığının güncellenmesi unutulmuştur.
* **Veri Akışındaki Kopma Noktası**: `SiparisController.cs` -> DB Save (`Kupon.KullanilanMiktar` persist adımı).
* **Yeniden Üretme Adımları**:
  1. Admin panelinden `KullanimLimiti = 1` olan bir test kuponu oluşturun.
  2. Kullanıcı tarafında sepeti doldurup bu kuponu uygulayın ve siparişi tamamlayın.
  3. Admin panelinde Kuponlar listesine bakın (`KullanilanMiktar` hâlâ `0` olarak görünür).
  4. İkinci bir siparişte aynı kupon tekrar başarıyla kullanılır (Limit aşıldığı hâlde engellenemez).
* **Etkilenen Senaryolar**: Tüm indirim kuponlu siparişler, sınırlı sayıda kupon kampanyaları.
* **Önerilen Çözüm**: `SiparisController.cs` `CreateOrder` metodunda sipariş kaydedilmeden hemen önce kupon aktifse `kupon.KullanilanMiktar++` yapılarak DB'ye yazılmalıdır.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Web/Controllers/SiparisController.cs`
* **Test Planı**: Sepette limitli bir kuponla sipariş oluşturulup DB ve Admin ekranında `KullanilanMiktar` artışı doğrulanmalıdır.
* **Riskler**: Yok (Düzeltme veritabanı bütünlüğünü korur).

---

### `ADMIN-FRONTEND-002` — Toptancı Kademeli Ürün Grubu İskonto Oranları (`ToptanciIskontoOranlari`) Fiyat Motorunda Hesaplanmıyor

* **Önem Seviyesi**: 🔴 Kritik
* **Modül**: Toptancı Yönetimi & Fiyatlandırma
* **Admin Tarafı**: `ToptanciController.cs` -> `UrunGruplari.cshtml`, Entity: `ToptanciUrunGrubu`, `ToptanciIskontoOrani` (Örn: 10-50 adet arası %15, 51-100 adet arası %25 iskonto).
* **Frontend Tarafı**: `SepetService.cs`, `OrderPricingService.cs`, `SiparisController.cs`.
* **Mevcut Davranış**: Admin panelinde ürünlere Toptancı Ürün Grubu atanabilmekte ve adet bazlı iskonto oranları girilebilmektedir. Fakat toptancı müşterisi sepete 100 adet ürün eklediğinde sepet ve ödeme fiyatlandırma servisleri yalnızca `TopFiyat` veya `EtkinFiyat` değerini okumakta; `ToptanciIskontoOranlari` tablosundaki adet bazlı iskonto yüzdelerini tamamen görmezden gelmektedir.
* **Beklenen Davranış**: Toptancı rolündeki kullanıcı sepete ürün eklediğinde, sepetteki ilgili grubun toplam adedine karşılık gelen iskonto oranı `ToptanciIskontoOranlari` tablosundan çekilerek birim fiyata veya sepet toplamına yansıtılmalıdır.
* **Teknik Kök Neden**: `OrderPricingService.cs` ve `SepetService.cs` sınıfları yazılırken `ToptanciIskontoOrani` tablosuna `Include` atılmamış ve iskonto hesaplama mantığı eklenmemiştir.
* **Veri Akışındaki Kopma Noktası**: `ToptanciIskontoOrani` DB -> `OrderPricingService.cs` / `SepetService.cs` -> Frontend Fiyat Görüntüleme.
* **Yeniden Üretme Adımları**:
  1. Admin panelinden bir toptancı ürün grubu oluşturup %20 iskonto tanımlayın.
  2. Bir ürünü bu toptancı grubuna bağlayın.
  3. Onaylı bir toptancı hesabı ile giriş yapıp sepete tanımlı adette ürün ekleyin.
  4. Fiyatın düz toptan fiyat üzerinden hesaplandığını, kademeli grup iskontosunun düşmediğini görün.
* **Etkilenen Senaryolar**: Toptancı müşterilerinin toplu siparişleri.
* **Önerilen Çözüm**: `OrderPricingService.cs` ve `SepetService.cs` içerisine toptancı rolü kontrolü ve kademeli grup iskontosu hesaplama fonksiyonu entegre edilmelidir.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Service/Services/OrderPricingService.cs`, `FilistinProje.Service/SepetService.cs`, `FilistinProje.Web/Controllers/SepetController.cs`
* **Test Planı**: Toptancı kullanıcısı ile farklı adetlerde ürün eklenerek iskontonun sepet ve ödeme adımına doğru yansıdığı test edilmelidir.
* **Riskler**: Toptancı fiyat hesaplamalarında doğru öncelik sırası (`TopFiyat` vs. `Grupİskontosu`) belirlenmelidir.

---

### `ADMIN-FRONTEND-003` — Sepete Ekleme İşleminde `MaxSiparisAdedi` Backend Doğrulaması Eksik

* **Önem Seviyesi**: 🔴 Kritik
* **Modül**: Ürün Yönetimi / Sepet Akışı
* **Admin Tarafı**: `UrunController.cs` (`Duzenle.cshtml`), Form Alanı: `MaxSiparisAdedi` (Örn: Müşteri başına maks 2 adet).
* **Frontend Tarafı**: `SepetController.cs` (`Ekle` POST action methodu).
* **Mevcut Davranış**: Ürün detay sayfasında HTML `max` attribute'u ile miktar kutusu sınırlandırılsa da, kullanıcı tarayıcı konsolundan veya doğrudan HTTP request ile `adet=500` gönderdiğinde `SepetController.cs` backend tarafında `MaxSiparisAdedi` kontrolü yapmadığı için sepet 500 adet ürün ile oluşturulmaktadır.
* **Beklenen Davranış**: `SepetController.cs` içerisindeki `Ekle` ve `Guncelle` metotları, eklenmek istenen toplam adedin ürünün `MaxSiparisAdedi` sınırını aşıp aşmadığını veritabanından doğrulamalı; aşıyorsa işlemi reddedip uyarı dönmelidir.
* **Teknik Kök Neden**: `SepetController.cs` action metodunda `urun.MaxSiparisAdedi.HasValue` kontrolü yapılmamıştır.
* **Veri Akışındaki Kopma Noktası**: `Urun.MaxSiparisAdedi` DB -> `SepetController.cs` validation adımı.
* **Yeniden Üretme Adımları**:
  1. Admin panelinden bir ürünün `MaxSiparisAdedi` alanını `2` yapın.
  2. Frontend'de F12 / Postman üzerinden `/Sepet/Ekle` adresine `urunId=X, adet=50` POST edin.
  3. Sepete 50 adet ürünün başarıyla eklendiğini ve ödeme adımına geçebildiğinizi görün.
* **Etkilenen Senaryolar**: Stok sınırlı özel kampanyalı ürünler, karaborsa/stok kapatma engelleme mekanizmaları.
* **Önerilen Çözüm**: `SepetController.cs` `Ekle` ve `MiktarGuncelle` metotlarına backend `MaxSiparisAdedi` kontrolü eklenmelidir.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Web/Controllers/SepetController.cs`
* **Test Planı**: Limit üzeri adetlerle HTTP request atılarak backend engellemesi ve kullanıcı uyarısı doğrulanmalıdır.
* **Riskler**: Yok.

---

### `ADMIN-FRONTEND-004` — Süresi Dolan `KampanyaBitisTarihi` Ürün Fiyat Motoru (`EtkinFiyat`) Tarafından Kontrol Edilmiyor

* **Önem Seviyesi**: 🔴 Kritik
* **Modül**: Ürün Yönetimi & Fiyatlandırma
* **Admin Tarafı**: `UrunController.cs` (`Duzenle.cshtml`), Form Alanı: `KampanyaBitisTarihi` (Örn: 2026-07-20 23:59).
* **Frontend Tarafı**: `Urun.cs` (`EtkinFiyat` property'si) & `OrderPricingService.cs`.
* **Mevcut Davranış**: Admin panelinden bir ürüne indirimli fiyat ve kampanya bitiş tarihi tanımlandığında; kampanya bitiş tarihi geçmiş (örneğin dünün tarihi) olsa bile `Urun.cs` modelindeki `EtkinFiyat` getter metodu yalnızca `IndirimliFiyat.HasValue && IndirimliFiyat.Value > 0 && IndirimliFiyat.Value < Fiyat` şartına bakmakta ve müşteriye süresi geçmiş indirimli fiyatı sunmaya devam etmektedir.
* **Beklenen Davranış**: `KampanyaBitisTarihi` dolmuşsa `EtkinFiyat` normal `Fiyat` değerine dönmeli ve indirim sonlandırılmalıdır.
* **Teknik Kök Neden**: `Urun.cs` içerisindeki `EtkinFiyat` ve `IndirimVarMi` property'lerine `!KampanyaBitisTarihi.HasValue || KampanyaBitisTarihi.Value > DateTime.UtcNow` mantıksal kontrolü eklenmemiştir.
* **Veri Akışındaki Kopma Noktası**: `Urun.KampanyaBitisTarihi` DB -> `Urun.EtkinFiyat` hesaplama mantığı.
* **Yeniden Üretme Adımları**:
  1. Admin panelinden bir ürünün `IndirimliFiyat`ını 100 ILS yapın ve `KampanyaBitisTarihi` alanını dün akşamın tarihine ayarlayın.
  2. Müşteri tarafında ürün detay ve liste sayfalarını açın.
  3. Ürünün hâlâ 100 ILS indirimli fiyatla satıldığını ve sepete eklenebildiğini görün.
* **Etkilenen Senaryolar**: Süreli indirim kampanyaları, flaş indirimler.
* **Önerilen Çözüm**: `Urun.cs` sınıfındaki `EtkinFiyat` ve `IndirimVarMi` hesaplamalarına tarih kontrolü dahil edilmelidir.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Core/Varliklar/Urun.cs`
* **Test Planı**: Kampanya tarihi geçmiş ürünlerde indirimin otomatik olarak kalktığı doğrulanmalıdır.
* **Riskler**: Yok.

---

## 6. Yüksek Öncelikli Bulgular

### `ADMIN-FRONTEND-005` — Admin Panelindeki Eski Ana Sayfa Yönetimi (`/Admin/AnaSayfa`) Frontend Tarafından Okunmuyor

* **Önem Seviyesi**: 🟠 Yüksek
* **Modül**: İçerik Yönetimi
* **Admin Tarafı**: `/Admin/AnaSayfa` (`AnaSayfaController.cs`), JSON bazlı Hero Slider ayarları.
* **Frontend Tarafı**: `HomeController.cs` -> `Index.cshtml`.
* **Mevcut Davranış**: Admin panelinde "Store Content -> Home Page Content" (`/Admin/AnaSayfa`) başlığı altında bir yönetim ekranı bulunmakta ve kaydedilen slider/hero ayarları JSON dosyasına yazılmaktadır. Ancak frontend `HomeController.cs`, ana sayfayı oluştururken bu JSON ayarlarını hiç okumamakta; veriyi DB'deki `Slaytlar` ve `HomePageSections` tablolarından çekmektedir. Bu durum yöneticilerin `/Admin/AnaSayfa` üzerinden yaptığı değişikliklerin sitede hiç görünmemesine neden olmaktadır.
* **Beklenen Davranış**: Admin menüsündeki tutarsız/eski olan bu sayfa ya kaldırılmalı ya da aktif `HomeSections` / `Slayt` yönetimi ile birleştirilmelidir.
* **Teknik Kök Neden**: Sitede iki farklı ana sayfa içerik mimarisi kalmıştır (Eski JSON mimarisi vs. Yeni EF Core DB mimarisi).
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Web/Areas/Admin/Views/Shared/_AdminLayout.cshtml`, `FilistinProje.Web/Areas/Admin/Controllers/AnaSayfaController.cs`

---

### `ADMIN-FRONTEND-006` — Kurumsal Sayfa Yönetimi (`/Admin/Sayfa`) Değişiklikleri Statik Frontend Sayfalarına Yansımıyor

* **Önem Seviyesi**: 🟠 Yüksek
* **Modül**: Kurumsal İçerik Yönetimi
* **Admin Tarafı**: `/Admin/Sayfa` (`SayfaController.cs`), Entity: `KurumsalSayfa`.
* **Frontend Tarafı**: `KurumsalController.cs` (`Hakkimizda`, `Gizlilik`, `KullaniciSozlesmesi`, `MesafeliSatis`, `IadeKosullari` action'ları).
* **Mevcut Davranış**: Admin panelinden "Hakkımızda", "Gizlilik Politikası" veya "İade Koşulları" metinleri düzenlenip DB'ye yazılabilmektedir. Fakat frontend `KurumsalController.cs` içerisindeki ilgili action'lar veritabanındaki `KurumsalSayfalar` tablosunu okumak yerine sabit Razor view dosyalarını (`Hakkimizda.cshtml`, `Gizlilik.cshtml` vb.) render etmektedir.
* **Beklenen Davranış**: Statik view'lar yerine DB'deki `KurumsalSayfa` içeriği okunmalı veya dinamik `Detay(slug)` route'una yönlendirilmelidir.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Web/Controllers/KurumsalController.cs`, `FilistinProje.Web/Views/Kurumsal/*`

---

### `ADMIN-FRONTEND-007` — Toptancı Minimum Sipariş Tutarı (`ToptanciMinSiparisTutari`) İçin Backend POST Doğrulaması Eksik

* **Önem Seviyesi**: 🟠 Yüksek
* **Modül**: Toptancı Yönetimi & Ödeme Akışı
* **Admin Tarafı**: `/Admin/Ayarlar` (`AyarlarController.cs`), Form Alanı: `ToptanciMinSiparisTutari` (Örn: 1000 ILS).
* **Frontend Tarafı**: `SiparisController.cs` (`CreateOrder` POST action methodu).
* **Mevcut Davranış**: `SiparisController.cs` `Odeme` GET action metodunda `ViewBag.ToptanciMinSiparisTutari` set edilerek JS ile arayüzde kontrol sağlansa da, sipariş oluşturma POST isteğinde (`CreateOrder`) kullanıcının toptancı olup olmadığı ve sepet tutarının `ToptanciMinSiparisTutari` sınırını karşılayıp karşılamadığı backend tarafında doğrulanmamaktadır.
* **Beklenen Davranış**: Toptancı rolündeki kullanıcı sipariş POST ettiğinde backend tarafında sepet tutarı `ToptanciMinSiparisTutari` değerinden küçükse sipariş reddedilmelidir.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Web/Controllers/SiparisController.cs`

---

### `ADMIN-FRONTEND-008` — Ürün `KdvOrani` Fiyat ve Fatura Hesaplamalarında Dikkate Alınmıyor

* **Önem Seviyesi**: 🟠 Yüksek
* **Modül**: Ürün Yönetimi & Sipariş / Fatura
* **Admin Tarafı**: `UrunController.cs`, Form Alanı: `KdvOrani` (Varsayılan %20).
* **Frontend Tarafı**: `OrderPricingService.cs`, `SepetService.cs`, `SiparisController.cs`, SiparisDetay View'ları.
* **Mevcut Davranış**: Admin panelinde her ürün için KDV oranı girilebilmektedir. Ancak ne sepet hesaplamalarında, ne ödeme detayında, ne de sipariş özetlerinde KDV tutarı/kırılımı hesaplanmamakta veya gösterilmemektedir. Bütün fiyatlar vergisiz/dahil ayrımı olmadan ham tutar olarak işlenmektedir.
* **Beklenen Davranış**: KDV oranı fiyat hesaplama servislerinde dikkate alınmalı, sipariş özetinde veya detayında vergi matrahı/KDV tutarı opsiyonel olarak belirtilmelidir.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Service/Services/OrderPricingService.cs`, `FilistinProje.Web/Views/Siparis/_OrderSummary.cshtml`

---

### `ADMIN-FRONTEND-009` — Kategori `BannerUrl`, `UstMetin`, `AltMetin` ve `UrunSiralamaTipi` Alanları Kategori Sayfasında Gösterilmiyor/Kullanılmıyor

* **Önem Seviyesi**: 🟠 Yüksek
* **Modül**: Kategori Yönetimi & Ürün Listeleme
* **Admin Tarafı**: `KategoriController.cs` (`Duzenle.cshtml`), Form Alanları: `BannerUrl`, `UstMetin`, `AltMetin`, `UrunSiralamaTipi`.
* **Frontend Tarafı**: `UrunController.cs` -> `Views/Urun/Index.cshtml`.
* **Mevcut Davranış**: Kategori düzenleme sayfasından yüklenen Hero Banner görseli (`BannerUrl`), kategoriye özel ek açıklama metinleri (`UstMetin`, `AltMetin`) ve kategori içi ürün sıralama kuralı (`UrunSiralamaTipi`), frontend kategori sayfasında render edilmemekte ve ürün sorgusunda sıralama parametresi olarak kullanılmamaktadır.
* **Beklenen Davranış**: Kategori detay/listeleme sayfasının üst kısmında banner görseli ve üst metin gösterilmeli, ürünler kategoride tanımlanan sıralama kuralına göre listelenmelidir.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Web/Controllers/UrunController.cs`, `FilistinProje.Web/Views/Urun/Index.cshtml`

---

## 7. Orta Öncelikli Bulgular

### `ADMIN-FRONTEND-010` — Site Ayarları Üst Bar Mesajı (`UstBarMesaji`), Etkinlik Durumu (`UstBarEtkin`) ve Hızı (`UstBarHizi`) Header'a Yansımıyor

* **Önem Seviyesi**: 🟡 Orta
* **Modül**: Sistem Ayarları & Header UI
* **Admin Tarafı**: `/Admin/Ayarlar`, Form Alanları: `UstBarMesaji`, `UstBarEtkin`, `UstBarHizi`.
* **Frontend Tarafı**: `Views/Shared/_Header.cshtml`.
* **Mevcut Davranış**: Admin panelinden üst bar kapatılsa (`UstBarEtkin = false`) veya özel bir duyuru metni yazılsa da (`UstBarMesaji`), `_Header.cshtml` dosyası bu ayarları okumamakta; hardcoded olarak tanımlanmış sabit güven mesajlarını ("Ücretsiz Kargo", "256 Bit SSL" vb.) döner bir şeritte göstermeye devam etmektedir.
* **Beklenen Davranış**: `UstBarEtkin = false` ise üst şerit gizlenmeli, `UstBarMesaji` doluysa bu duyuru metni öncelikli olarak şeritte gösterilmelidir.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Web/Views/Shared/_Header.cshtml`

---

### `ADMIN-FRONTEND-011` — Site Ayarları `TemaRengi` Seçimi CSS Tasarımına Bağlanmamış

* **Önem Seviyesi**: 🟡 Orta
* **Modül**: Sistem Ayarları & Tema
* **Admin Tarafı**: `/Admin/Ayarlar`, Form Alanı: `TemaRengi` (Color Picker, Örn: `#313511`).
* **Frontend Tarafı**: `Views/Shared/_Layout.cshtml` & CSS Dosyaları.
* **Mevcut Davranış**: Admin panelinden ana tema rengi değiştirilebilmekte ve DB'ye kaydedilmektedir. Ancak frontend tarafında `TemaRengi` hiçbir `<style>` etiketine veya CSS variable'ına (`--primary-color`) enjekte edilmediği için site renkleri hardcoded Tailwind sınıflarında kalmaktadır.
* **Beklenen Davranış**: Layout `<head>` alanına dinamik CSS değişkeni (`:root { --brand-primary: @siteSettings.TemaRengi; }`) enjekte edilmelidir.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Web/Views/Shared/_Layout.cshtml`

---

### `ADMIN-FRONTEND-012` — Site Ayarları Analytics (`GoogleAnalyticsId`, `FacebookPixelId`) İstemciye Render Edilmiyor

* **Önem Seviyesi**: 🟡 Orta
* **Modül**: Sistem Ayarları & Analitik
* **Admin Tarafı**: `/Admin/Ayarlar`, Form Alanları: `GoogleAnalyticsId` (G-XXXXX), `FacebookPixelId` (123456789).
* **Frontend Tarafı**: `Views/Shared/_Layout.cshtml`.
* **Mevcut Davranış**: Admin panelinden Google Analytics ve Facebook Pixel ID'leri girilip kaydedilebilmektedir. Fakat `_Layout.cshtml` içerisinde ilgili `gtag` veya `fbq` izleme kodları bulunmamaktadır.
* **Beklenen Davranış**: `GoogleAnalyticsId` ve `FacebookPixelId` dolu olduğunda layout `<head>` kısmında ilgili script etiketleri otomatik olarak render edilmelidir.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Web/Views/Shared/_Layout.cshtml`

---

### `ADMIN-FRONTEND-013` — Site Ayarları Çerez Metni (`CookieMetni`) İçin Frontend Rıza Barı Bulunmuyor

* **Önem Seviyesi**: 🟡 Orta
* **Modül**: Sistem Ayarları & Çerez Yönetimi
* **Admin Tarafı**: `/Admin/Ayarlar`, Form Alanı: `CookieMetni`.
* **Frontend Tarafı**: `Views/Shared/_Layout.cshtml`.
* **Mevcut Davranış**: Admin panelinde çerez bilgilendirme metni alanı yer almaktadır. Ancak kullanıcı tarafında ilk girişte çıkan bir çerez rıza (Cookie Consent) barı bulunmamaktadır.
* **Beklenen Davranış**: Kullanıcı siteye ilk girdiğinde `CookieMetni` içeriğini gösteren ve onaylandığında gizlenen bir çerez onay banner'ı eklenmelidir.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Web/Views/Shared/_Layout.cshtml`

---

### `ADMIN-FRONTEND-014` — Site Ayarları Footer Açıklaması (`FooterAciklamasi`), Çalışma Saatleri ve Adres Bilgisi Footer'da Görünmüyor

* **Önem Seviyesi**: 🟡 Orta
* **Modül**: Sistem Ayarları & Footer UI
* **Admin Tarafı**: `/Admin/Ayarlar`, Form Alanları: `FooterAciklamasi`, `CalismaSaatleri`, `Adres`.
* **Frontend Tarafı**: `Views/Shared/_Footer.cshtml`.
* **Mevcut Davranış**: `SiteAyarlari` içerisinde kaydedilen özel footer metni, çalışma saatleri ve mağaza adres bilgileri `_Footer.cshtml` görünümünde kullanılmamaktadır.
* **Beklenen Davranış**: `_Footer.cshtml` içerisinde kurumsal blok altında veya yanında bu bilgiler dinamik olarak basılmalıdır.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Web/Views/Shared/_Footer.cshtml`

---

### `ADMIN-FRONTEND-015` — Ürün `KisaAd`, `SKU` ve `Barkod` Bilgileri Ürün Detay Sayfasında Gösterilmiyor

* **Önem Seviyesi**: 🟡 Orta
* **Modül**: Ürün Yönetimi / Ürün Detay
* **Admin Tarafı**: `UrunController.cs`, Form Alanları: `KisaAd`, `SKU`, `Barkod`.
* **Frontend Tarafı**: `Views/Urun/_ProductInfo.cshtml`, `Views/Urun/Detay.cshtml`.
* **Mevcut Davranış**: Admin panelinde ürün eklerken/düzenlerken SKU, Barkod ve Kısa Ad alanları doldurulabilmektedir. Ürün detay sayfasında ise bu bilgiler hiçbir yerde gösterilmemektedir.
* **Beklenen Davranış**: SKU ve Barkod bilgileri ürün teknik detay/bilgi tablosunda veya ürün başlığının altında ikincil kod olarak gösterilmelidir.
* **Değiştirilmesi Muhtemel Dosyalar**: `FilistinProje.Web/Views/Urun/_ProductInfo.cshtml`

---

## 8. Düşük Öncelikli Bulgular

### `ADMIN-FRONTEND-016` — Repository İçerisinde Kalan Boş Admin Görünüm Dizinleri (`CarkOdul`, `PushBildirim`, `TopluFiyatGuncelle`, `UrunImport`)

* **Önem Seviyesi**: 🟢 Düşük
* **Modül**: Proje Temizliği
* **Açıklama**: Geçmiş sürümlerden kalan veya refaktör edilen modüllere ait 4 adet boş klasör (`Areas/Admin/Views/CarkOdul`, `PushBildirim`, `TopluFiyatGuncelle`, `UrunImport`) proje dizininde yer almaktadır.
* **Önerilen İşlem**: Boş dizinler temizlenmelidir.

---

### `ADMIN-FRONTEND-017` — Gizli Admin Route'larının (`XyzSecretMonitor`, `TopluEtiket`) Menüde Yer Almaması

* **Önem Seviyesi**: 🟢 Düşük
* **Modül**: Admin Navigasyon
* **Açıklama**: `/Admin/XyzSecretMonitor` ve `/Admin/Siparis/TopluEtiket` fonksiyonel olarak çalışan admin endpoint'leridir ancak `_AdminLayout.cshtml` navigasyon menüsünde doğrudan linkleri bulunmamaktadır.
* **Önerilen İşlem**: İlgili yetkili rollere görünür ikincil menü bağlantısı eklenmesi değerlendirilebilir.

---

### `ADMIN-FRONTEND-018` — `PushAbonelik` Veritabanı Varlığının Admin ve Frontend Arayüzünün Bulunmaması

* **Önem Seviyesi**: 🟢 Düşük
* **Modül**: Bildirim Altyapısı
* **Açıklama**: `FilistinProje.Core` içerisinde `PushAbonelik` entity'si ve EF Core migration'ı mevcuttur ancak ne istemci tarafında FCM token toplayan JS kodu, ne de admin panelinde bildirim gönderme ekranı bulunmaktadır.
* **Önerilen İşlem**: Web Push özelliği aktif edilecekse arayüzler tamamlanmalı, edilmeyecekse dokümante edilmelidir.

---

### `ADMIN-FRONTEND-019` — Footer İçi Kurumsal Bağlantı Yollarındaki Hardcoded URL Yapıları

* **Önem Seviyesi**: 🟢 Düşük
* **Modül**: Footer UI
* **Açıklama**: `_Footer.cshtml` içerisindeki kurumsal bağlantılarda `/pages/about` ve `/pages/return-policy` gibi static linkler kullanılmaktadır.
* **Önerilen İşlem**: Bağlantılar standart MVC route helper'larına (`/Kurumsal/Detay/...` veya `/pages/...`) dinamik olarak bağlanmalıdır.

---

## 9. Frontend’de Bulunup Admin Panelinden Yönetilemeyen Alanlar

1. **Header Üst Şerit Güven Mesajları**: `_Header.cshtml` içerisindeki "256 Bit SSL", "Banka Havalesi İle Ödeme", "Özel Paketleme" gibi duyuru metinleri kod içine sabit yazılmıştır; Admin panelindeki `UstBarMesaji` alanından bağımsızdır.
2. **Kargo Bedelsiz Teslimat Sloganları**: Footer üstündeki trust bar metinleri (Örn: "Filistin Genelinde Ücretsiz Kargo") doğrudan Razor view içine yazılmıştır.
3. **Çerçeve Modelleri Ve Metre Fiyatı**: `_ProductInfo.cshtml` (Satır 88 & 443) içerisinde kanvas tablo çerçeve seçenekleri (`Çerçevesiz`, `Siyah`, `Beyaz`, `Gold`, `Gümüş`, `Meşe`, `Ceviz`) ve metre başı çerçeve fiyatı (`FRAME_PRICE_PER_METER = 250`) JavaScript kodunun içine sabit (hardcoded) yazılmıştır. Admin panelinden dinamik olarak yönetilememektedir.

---

## 10. Kullanılmayan veya Yarım Kalmış Özellikler

1. **Eski JSON Tabanlı Ana Sayfa Yönetimi (`HomePageSettingsModel`)**: `AnaSayfaController.cs` tarafından yönetilen fakat `HomeController.cs` tarafından okunmayan JSON ayar yapısı.
2. **`PushAbonelik` Entity'si**: Veritabanı tablosu olup arayüzü ve servisi tamamlanmamış altyapı.
3. **Kategori İçi Ürün Sıralama Tipi (`UrunSiralamaTipi`)**: DB kolon değeri bulunup sorgularda kullanılmayan özellik.
4. **Boş Admin View Klasörleri**: `CarkOdul`, `PushBildirim`, `TopluFiyatGuncelle`, `UrunImport`.

---

## 11. Güvenlik ve Yetkilendirme Bulguları

1. **Sepet `MaxSiparisAdedi` Backend Bypass Riski**: İstemci tarafından gönderilen isteklerde backend adedi sınırlandırmadığı için kötü niyetli kullanıcılar stok kapatma veya aşırı sipariş denemesi yapabilir.
2. **Toptancı Minimum Sipariş Tutarı (`ToptanciMinSiparisTutari`) Server-Side Doğrulama Eksikliği**: Toptancı kullanıcısının HTTP isteğini manipüle ederek belirlenen limitin altında sipariş geçebilme riski.
3. **Süresi Dolan Kampanya Fiyatı Bypass Riski**: Bitiş tarihi geçen kampanyalı ürünlerin veritabanında `IndirimliFiyat` değeri temizlenmediği sürece indirimli satılmaya devam etmesi.
4. **Kupon Kullanım Limiti Artırılmama Riski**: `KullanilanMiktar` artırılmadığı için sınırlı kuponların sonsuz defa kullanılabilmesi.

---

## 12. Test Sonuçları

* **Statik Kod Analizi**: Tüm Controller, View, Service, Entity ve DTO sınıfları veri akışı bazında taranmıştır.
* **Derleme Testi**: `dotnet build FilistinProje.sln` komutu ile projenin sorunsuz derlendiği doğrulanmıştır.
* **WebRTC & Hassas Belge Güvenliği**: Faz 11 kapsamında eklenen `private://` storage ve `BelgeController` güvenlik yapısı incelenmiş, anonim erişime kapalı olduğu doğrulanmıştır.

---

## 13. Önerilen Düzeltme Sırası

1. **Aşama 1: Kritik Veri, Güvenlik ve İndirim Düzeltmeleri** (`ADMIN-FRONTEND-001` - `004`)
   - Kupon `KullanilanMiktar` artırma mantığının `SiparisController`'a eklenmesi.
   - `SepetController` backend `MaxSiparisAdedi` doğrulamasının eklenmesi.
   - `Urun.EtkinFiyat` ve `IndirimVarMi` property'lerine `KampanyaBitisTarihi` kontrolünün eklenmesi.
   - Toptancı kademeli grup iskontolarının `OrderPricingService` ve `SepetService`'e entegre edilmesi.

2. **Aşama 2: Yüksek Öncelikli İş Mantığı ve İçerik Düzeltmeleri** (`ADMIN-FRONTEND-005` - `009`)
   - `KurumsalController`'ın DB'deki `KurumsalSayfa` verilerini okuyacak şekilde güncellenmesi.
   - `SiparisController` `CreateOrder` işlemine backend `ToptanciMinSiparisTutari` doğrulamasının eklenmesi.
   - Admin Ana Sayfa menü karmaşasının giderilmesi (`AnaSayfaController` vs `HomeSectionsController`).
   - Kategori `BannerUrl`, `UstMetin`, `AltMetin` ve `UrunSiralamaTipi` alanlarının `UrunController` / `Index.cshtml` tarafına yansıtılması.

3. **Aşama 3: Sistem Ayarları ve Frontend Entegrasyonları** (`ADMIN-FRONTEND-010` - `015`)
   - `_Header.cshtml` üst şerit duyuru mesajlarının `SiteAyarlari`'na bağlanması.
   - `_Layout.cshtml` dosyasına `TemaRengi`, Analytics scriptleri ve Çerez onay barının eklenmesi.
   - `_Footer.cshtml` içerisine footer açıklaması, adres ve çalışma saatlerinin yansıtılması.
   - Ürün detay sayfasına SKU ve Barkod gösteriminin eklenmesi.

4. **Aşama 4: Proje Temizliği ve Kod İyileştirmeleri** (`ADMIN-FRONTEND-016` - `019`)
   - Boş görünüm klasörlerinin silinmesi.
   - Hardcoded URL ve çerçeve fiyatı sabitlerinin yapılandırmaya/admin ayarlarından yönetilebilir yapıya kavuşturulması.

---

## 14. Değiştirilmesi Muhtemel Dosyalar

* **Core Katmanı**:
  * `FilistinProje.Core/Varliklar/Urun.cs`
  * `FilistinProje.Core/Varliklar/Kupon.cs`
* **Service Katmanı**:
  * `FilistinProje.Service/Services/OrderPricingService.cs`
  * `FilistinProje.Service/SepetService.cs`
* **Web Admin Katmanı**:
  * `FilistinProje.Web/Areas/Admin/Controllers/AnaSayfaController.cs`
  * `FilistinProje.Web/Areas/Admin/Views/Shared/_AdminLayout.cshtml`
* **Web Frontend Katmanı**:
  * `FilistinProje.Web/Controllers/SiparisController.cs`
  * `FilistinProje.Web/Controllers/SepetController.cs`
  * `FilistinProje.Web/Controllers/KurumsalController.cs`
  * `FilistinProje.Web/Controllers/UrunController.cs`
  * `FilistinProje.Web/Views/Shared/_Header.cshtml`
  * `FilistinProje.Web/Views/Shared/_Footer.cshtml`
  * `FilistinProje.Web/Views/Shared/_Layout.cshtml`
  * `FilistinProje.Web/Views/Urun/_ProductInfo.cshtml`
  * `FilistinProje.Web/Views/Urun/Index.cshtml`

---

## 15. Regresyon Test Planı

1. **Sepet & Ödeme Testleri**: Kupon kullanımı, stok düşümü, limit kontrolleri, kargo hesaplama, kapıda ödeme ve havale ödeme akışları.
2. **Toptancı Satış Testleri**: Toptancı girişi, minimum sipariş tutarı engeli, grup iskontolu fiyatlandırma.
3. **İçerik & SEO Testleri**: Dinamik kurumsal sayfalar, kategori bannerları, dil değişimi (AR/EN), meta etiketleri.
4. **Güvenlik Testleri**: B25 hassas belge erişimleri (`/Belge/Kimlik`, `/Belge/Recete`), yetki matrisi denetimleri.

---

## 16. İş Kararı Gerektiren Konular

1. **Eski Admin Ana Sayfa Ekranı (`/Admin/AnaSayfa`)**: Bu ekran tamamen kaldırılıp navigasyondan çıkarılmalı mıdır, yoksa `HomeSections` ekranı ile birleştirilmeli midir?
2. **Web Push Notification (`PushAbonelik`)**: Web Push özelliği aktif bir şekilde geliştirilecek midir, yoksa kullanılmayan veritabanı tablosu kaldırılmalı mıdır?
3. **Ürün SKU ve Barkod Görünürlüğü**: SKU ve Barkod bilgisi son kullanıcıya ürün detay sayfasında gösterilmeli midir, yoksa yalnızca admin/fatura çıktılarında mı kalmalıdır?
4. **Çerçeve Metre Fiyatı Ve Seçenekleri**: Kanvas tablo çerçeve fiyatlandırması (`FRAME_PRICE_PER_METER`) admin panelinde Ayarlar modülüne mi taşınmalıdır?

---

## 17. Genel Sonuç

Yapılan kapsamlı analiz sonucunda, projenin veritabanı ve admin altyapısının oldukça zengin ve detaylı kurgulandığı görülmüştür. Ancak bazı yönetimsel ayarların (kupon sayacı, toptancı kademeli iskontoları, kampanya bitiş tarihleri, ürün sepet limitleri, kurumsal içerikler ve üst şerit ayarları) frontend tarafında tam olarak tüketime bağlanmadığı veya backend doğrulamasından geçirilmediği tespit edilmiştir.

Raporda sunulan **14 Aşamalı Düzeltme Planı** sırasıyla uygulandığında, projenin Admin-Frontend entegrasyonu %100 uyumlu, güvenli ve eksiksiz hale gelecektir.
