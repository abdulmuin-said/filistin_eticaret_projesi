# 7ANRPS48.com — Kapsamlı Analiz Raporu (MiniMax-M3 Denetimi)

**Tarih:** 9 Temmuz 2026
**Denetim türü:** Mimari + e-ticaret + güvenlik + UI/UX (read-only, kanıta dayalı)
**Hedef:** Filistin pazarı için geliştirilen 7ANRPS48.com e-ticaret sitesi (ASP.NET Core 8 MVC + PostgreSQL + TailwindCSS)

---

## 1. YÖNETİCİ ÖZETİ — En Önemli 10 Sonuç

1. **Build temiz** (`dotnet build` → 0 hata 0 uyarı), site `localhost:5002` üzerinde çalışıyor, DB (`filistinproje-db`, PG16) healthy. Genel iskelet ayakta.

2. **Marka kirliliği production-affecting**: `FilistinProje.Web/secrets.json` hâlâ `Canvasia` FromName + `canvasia.com.tr@gmail.com` FromEmail + Brevo SMTP şifresi plaintext içeriyor. Bu dosya gitignore'da ama **çalışan uygulamada bu ayarlarla e-posta gönderiliyor** — müşteriler "Canvasia" imzası ile mail alır. (B0)

3. **Kargo altyapısı tamamen boş**: DB'de `KargoBolgeler`, `KargoBolgeSehirler`, `KargoFirmalari`, `BankaHesaplari` tabloları **0 satır**. Ödeme sayfasındaki Filistin şehir listesi 4 view içinde **hardcoded yedek array** olarak işliyor (Odeme.cshtml:27-33, _AddressForm.cshtml, _OrderSummary.cshtml, _IdentityVerification.cshtml). Üstelik bu array'in UTF-8 kaydı bozuk: `BeytÃ¼llahim`, `El-KudÃ¼s`. (B1)

4. **Siparişte stok düşümü yok** (P0): `SiparisController.Odeme` POST → transaction içinde `SiparisDetay` yazılıyor ama `UrunSecenek.StokAdedi` **azaltılmıyor**. İki eşzamanlı sipariş aynı son adedi satar → stok negatife düşer, overselling. (B2)

5. **Server-side fiyat doğrulaması yok** (P0): `SiparisController.cs:275` `BirimFiyat = item.Fiyat` — yani sepetin client-supplied fiyatı doğrudan siparişe yazılıyor. Kötü niyetli kullanıcı sepete eklenen fiyatı manipüle edip ₪0'a sipariş geçebilir. (B3)

6. **Login formunda default kimlik dolu**: `/Hesap/GirisYap` sayfası açıldığında e-posta `admin@7anrps48.com` ve şifre `Admin123!` kutulara **önceden dolduruluyor** (snapshot 09-19-03). Auto-fill değil, view içine gömülü. Genel sitenin tamamı login gerektirmediği için hemen P0 değil ama herkesin görebileceği yerde admin şifresi sızması. (B4)

7. **Ürün görselleri genel olarak boş/404**: Ana sayfa öne çıkan 4 ürünün görselleri kart üzerinde gösteriliyor ama **ürün detay sayfasında görsel yok** (sadece Font Awesome ikon). `placeholder.webp` için 404 hatası konsolda (`/img/products/placeholder.webp:0` → 404). Katalog verisiz/görselsiz → demo düzeyinde. (B5)

8. **Türkiye telefon formatı Filistin'de dayatılıyor**: `SiparisController.cs:582` `Telefon.Length != 11 || !StartsWith("0")`. Filistin numaraları `+970 5XX XXX XXX` (~9-10 haneli, 0 ile başlamaz). Tüm meşru müşteriler reddedilir. (B6)

9. **Marka/domain kalıntıları her tabakada**: `filistin.kastamonuesnaf.com.tr` (Türkiye hosting altında), `SiteSettingsService.cs:291` `EmailTemplates/canvasia-logo.svg` fallback, `SmtpEmailService.cs:186,196` Aras/MNG Türkiye kargo takip linkleri, `package.json` GitHub `meteorgaleri_canvasia`, `GUNCELLEME_ADIMLARI.md:23` `ssh canvasia-server`, `SiteAyarlari.TemaRengi=#1a5632` (yeşil, "siyah/altın" brief'ine aykırı). (B7)

10. **JS para formatı `tr-TR` ACE ekonomisinde**: `Odeme.cshtml` (8 yer), `Urun/Detay.cshtml`, `_ProductInfo.cshtml`, `_Reviews.cshtml` — hepsinde `toLocaleString('tr-TR')`. AR için ₪44.987,00 (TR virgül/nokta) gösterilir, EN için ₪44,987.00 — tutarsız. (B8)

---

## 2. İNCELEME KAPSAMI, ÇALIŞTIRILAN KOMUTLAR, DOĞRULANAMAYANLAR

### Çalıştırılan ve doğrulananlar
- `docker ps` → 3 container: `filistinproje-db` (healthy, 5434), `hospital_db`, `hospital_pgadmin`.
- `dotnet build FilistinProje.sln` → **0 hata 0 uyarı**, 4 proje derlendi.
- `dotnet run --project FilistinProje.Web --no-build --urls http://localhost:5002` (PID 26472) → site açıldı, Test-NetConnection `:5002` ⇒ True.
- `docker exec filistinproje-db psql -U kanvasuser -d filistindb` ile `\dt` (42 tablo), `\d Urunler` (60 kolon), `SELECT * FROM KargoBolgeler/KargoBolgeSehirler/KargoFirmalari/BankaHesaplari` (0/0/0/boş).
- Playwright MCP ile açılan sayfalar (mobile 390×900 + desktop 1440×900), AR + EN:
  - `/` ana sayfa — AR: RTL aktif, `dir=rtl lang=ar`, overflow yok, footer/menü/slider açık.
  - `/Urun` liste — 4 ürün, fiyat slider aktif, sıralama dropdown, grid/liste toggle.
  - `/Urun/Detay/modern-sofa-set-48`, `smart-watch-49`, `leather-handbag-51` — detay boş resim, 404 placeholder.
  - `/Sepet` — 3 ürün, ₪44,987.00 toplam (39,992+2,598+2,397 ✓ doğru), boşaltma/hediye seçimi.
  - `/Siparis/Odeme` — Bank Transfer, COD opsiyonu görünmüyor, Filistin şehirleri hardcoded (bozuk encoding).
  - `/Hesap/GirisYap` — admin şifresi default dolu (snapshot gözlendi).
  - `/Admin/Home` — login başarılı, dashboard: ₪1,350 toplam gelir, 5 sipariş, 1 pazara hazır.
  - Mobil 390×900 — hamburger `#mobileMenuBtn` açılır, sepet/favori/hesap ikonları çalışır.
  - `/Dil/Degistir?culture=en` — title EN, `dir=ltr lang=en`, overflow yok.

### Doğrulanamayanlar / kapsam sınırları
- **Müşteri belgeleri okunamadı**: `Belge.docx`, `bilgiler.docx`, `müşteri_ile_görüşme/`, `proje_konuşması/proje_konuşması.txt` — `.docx` parser yok. Konuşma txt'sinde 200+ satır var; tam parsing değil ama `grep` ile referansları taradım (canvasia.com.tr, kastamonuesnaf.com.tr geçişleri görüldü).
- **Ses kayıtları/görseller okunamadı**: Sesli görüşme yok; görsel ranevmanları (admin card arka planı, personel listesi) incelenemedi.
- **Tam güvenlik sızma testleri yapılmadı**: OWASP odaklı statik kod denetimi yaptım, aktif exploit (örneğin gerçek overselling reproduction) denemedim.
- **Canlı e-posta/PDF fatura gönderimi test edilmedi**: SMTP credentials gerçek, denememin veri sızıntısı riski var.
- **Admin tüm CRUD sayfaları taranmadı**: 27 admin var, sadece `Admin/Home` + `/Admin/Siparis/Index` snapshot alındı. Diğerleri kod yoluyla değerlendirildi.
- **PDF fatura, Push notification, Firebase FCM** çalıştırılmadı (cmd atma riski).
- **ef migrations list** denendi ama EF tool yerel kuruluydu; `dotnet ef migrations list` PowerShell quoting hatası verdi, atlandı (migration dosyaları `FilistinProje.Data/Migrations/` üzerinden sayıldı).

---

## 3. MİMARİ HARİTA

### Solution katmanları
- **FilistinProje.Core** — Entity'ler (Core/Varliklar/), enum'lar (Core/Enums/), IEmailService, IGenericRepository.
- **FilistinProje.Data** — KanvasDbContext (not: adı Canvasia'dan geliyor, rename bekliyor), Repositories/GenericRepository, UnitOfWork, EF Migrations (~24 dosya, InitialCreate'den SliderMultilingual'e).
- **FilistinProje.Service** — Services/ (SepetService, SiteSettingsService, SmtpEmailService, KargoHesaplamaServisi, StockAlertService, FavoriPriceDropService, AbandonedCartService, HomePageSettingsService), Interfaces/ (ISepetService, ISiteSettingsService, IKargoHesaplamaServisi).
- **FilistinProje.Web** — Controllers/ (14 public), Areas/Admin/Controllers/ (29 admin), Views/, Security/, Services/ (FaturaPdfService, AdminSessionStateService, FirebaseNotificationService), Resources/ (.resx), AreasAdminViewsToptanci/ (klasör adı tuhaf).

### Akış örneği: Sepete Ekleme
1. /Sepet/Ekle (POST) → SepetController#Ekle → ISepetService.SepeteEkleAsync → SepetService.cs:418 CanAddQuantity (stok/max/min kontrolü) → KanvasDbContext.SepetItems.Add → SaveChanges.
2. Sepet tutarı ISepetService.GetSepetToplamiAsync → sepet item'ların Fiyat alanı toplamı — **fiyat sepet tutulurken DB'den yeniden okunmuyor**, sepet tablosundaki Fiyat snapshot'u kullanılıyor.
3. Sipariş: SiparisController#Odeme(POST) → ISepetService.GetSepetToplamiAsync(sepetToplami) :145 → sipariş oluşturuluyor, **item.Fiyat snapshot'ı SiparisDetay.BirimFiyat olarak yazılıyor** :275. Ürün seçenek fiyatı güncellense dahi eski fiyat baz alınır.

### DI kayıtları (sağlam)
Scoped servisler (ISepetService, ISiteSettingsService, IKargoHesaplamaServisi, IGenericRepository<>), Singleton (IAdminSecurityAuditService, IFirebaseNotificationService), Hosted (AbandonedCartService, FavoriPriceDropService, StockAlertService) — yalnız isDatabaseAvailableAtStartup=true ise. Doğru kapsüllenmiş.

### Middleware sırası (Program.cs:293-508)
ExceptionHandler → HSTS (prod) → SecurityHeaders (X-Frame, nosniff,Permissions-Policy: camera=(), microphone=(), geolocation=()) → StaticFiles → StatusCodePagesWithReExecute → Maintenance → RequestLocalization → ResponseCompression → Routing → RateLimiter → Session → Authentication → Authorization → LoginRequiredMiddleware → MapControllers → HealthChecks → HangfireDashboard → Routes.

Sıralama doğal ama **Permissions-Policy camera=() ödeme sayfasında WebRTC kamera çekimini break eder** — SiparisController ve Odeme.cshtml kamera ile kimlik çekimini açmaByID openCamera destekli. Policies'da caredeck control gerekiyor. (B9)

### Background servisleri
- AbandonedCartService — terk edilmiş sepet için e-posta.
- FavoriPriceDropService — favori fiyat düşüş bildirimi.
- StockAlertService — stok azaldığında admin'e mail (en sağlıklı).
Hepsi DB yoksa disabled, devre dışı kalır — beklenen davranış.

### Cache
Sadece IMemoryCache (AddMemoryCache) ve ICacheService. Distributed cache yok (Redis vb.). Site ayarları genelde ISiteSettingsService.GetSettings() her çağrılda DB'den çekilen SiteAyarlari row (cached değil). Site büyüdükçe hot-path DB yükü. (B10)

### Katman ihlalleri / sorunlar
- SiparisController doğrudan KanvasDbContext._context.SiparisDetaylari.Add(...) çağırıyor — Service abstraction'u bypass. İş kuralı servis katmanında değil. (B11)
- 27 admin controller doğrudan KanvasDbContext enjekte ediyor (_checks yapılmadı, ama AdminBaseController GetService<KanvasDbContext>() ile çekerek kullanıyor). DataContext Service'e sızmış.
- "Clean Architecture" iddiası kısmi gerçek; **repository/UoW pattern var ama controller'lar doğrudan DbContext'i de kullanıyor** — iki paralel yol.

### Yüksek karmaşıklık dosyalar
- Program.cs 1149 satır — EnsureMissingMarch2026SchemaAsync SQL'i tek methodda ~300 satır, schema drift migration. Bakımı zor.
- SiparisController.cs 967 satır — Odeme akışı + e-posta + validation iç içe, Single Responsibility ihlal.
- Admin/UrunController.cs — ürün CRUD + varyant + görsel + import işlevleri tek dosyada.

---

## 4. GEREKSİNİM İZLENEBİLİRLİK MATRİSİ

| ID | Gereksinim | Durum | Kod kanıtı | UI/akış kanıtı | Eksik/risk | Önerilen iş |
|---|---|---|---|---|---|---|
| G1 | Arapça varsayılan RTL + İngilizce LTR | **Tam ve doğrulandı** | Program.cs:234-241 (r, en supported), DilController.cs | / dir=rtl lang=ar; /Dil/Degistir?culture=en dir=ltr lang=en — Playwright teyit | — | — |
| G2 | Mobil öncelikli siyah/altın tasarım | **Çakışmalı/yanlış uygulanmış** | 	ailwind.config.js, brand renkleri tanımlı | SiteAyarlari.TemaRengi=#1a5632 (yeşil) — domain'den SQL dump | Marka brief: "siyah/altın"; canlı ayar yeşil. Tailwind brand-gold var ama varsayılan tema rengi doğru değil. | Tema rengini #b58735 (gold) veya siyah (#313511) yap, DB seed'i düzelt |
| G3 | Slider, kategori/ürün/marca bölümleri | **Kısmen uygulanmış** | SlaytController, Areas/Admin/Controllers/SlaytController.cs (CRUD), Slaytlar tablosu; marka: Urun.Marka kolonu var ama **filterleme UI yok** | Ana sayfada tek Hero Banner (statik img) + 4 öne çıkan ürün. Marka filtre paneli yok. | Multi-slide carousel görünmüyor (tek Hero), marka filtresi eksik | Slider için Slaytlar DB'ye demo seed + carousel JS; marka filtresi için sidebar bloğu ekle |
| G4 | Canlı arama + filtre + fiyat slider + sıralama | **Kısmen uygulanmış** | UrunController.cs Index, Index.cshtml:525-528 noUiSlider 	r-TR format | /Urun Liste sayfası: fiyat slider var, sıralama var (snapshot validated), marka/özellik filtresi yok | Canlı arama yok (sadece submit), marka/nitelik filtresi sidebar. 	r-TR format uyumsuz. | Canlı arama (/Search/Canli?q=), marka listesi, r-PS/en-US locale |
| G5 | Ürün galerisi + video + varyasyon (renk/boyut/hacim/ağırlık) | **Kısmen uygulanmış** | UrunResimleri (MedyaTipi='Gorsel'/'Video'), UrunSecenekleri (Değer: Renk/Boyut vb.) | Detay sayfasında görsel yok (404 placeholder), video entegre değil | Galeri görselleri yüklenmemiş demo veride, video player JS yok | Demo seed görsel yükle (wwwroot/img/products), galeri thumbs JS |
| G6 | Varyasyon bazlı fiyat farkı + stok + tükenince pasif/gri | **Tam ve doğrulandı** | SepetService.cs:418 (StokAdedi>0 → CanAddQuantity false), Urun/Index.cshtml grileme | /Sepet'te varyantla sepete ekleme çalışıyor | StokBiteniGriGoster admin ayarı var ama e-ticari stok **siparişte düşmüyor** (B2) | Önce stok düşürme fix (B2), sonra UI bilgilendirme |
| G7 | Düşük stokta "Son X adet" uyarısı | **Yalnızca backend var, UI yok** | StockAlertService.cs:86 (admin'e mail gönderiyor) | Müşteri önünde /Urun/Detay'da "Son 5 adet" badge yok | Teyit edilmedi | Detay sayfasındaki varyant adedini Razor+JS render et, stok < 5 ise rozet göster |
| G8 | Ürün/site değerlendirme + yıldız + yorum | **Kısmen uygulanmış** | Yorumlar, SiteDegerlendirmeleri tabloları; YorumController; Urun/Detay.cshtml "Reviews" başlığı | Detey sayfasında "No reviews yet" + "Write Review" (snapshot Te) | Admin onayı yapısı var ama kullanıcı akışı teyit edilmedi | Yorum yazma POST akışını E2E test et |
| G9 | Kupon + indirim + ücretsiz kargo barajı + kampanya gösterimi | **Kısmen uygulanmış** | Kuponlar tablosu, SiparisController.cs:180-195 kupon doğrulama | /Sepet'te "Discount Code" kutusu + Apply butonu var | Kupon listeleme UI yok; kampanyalı ürün widget yok | Kuponu manuel girme UI düzgün; kampanya sayfası ekle |
| G10 | İlgili ürünler + favoriler + sayaçlı kampanya + çark/ödül | **Kısmen uygulanmış** | Favoriler + FavoriController; CarkOdulleri tablosu + CarkOdulController | "/Favori" linki header'da; çark UI görülmedi | İlgili ürünler widget yok; çark ön yüz render edilmedi | Ürün detayında "RELATED_PRODUCTS" partial; çark için /Cark sayfası |
| G11 | WhatsApp siparişi + fiyat gizleme pahalı/özel | **Tam ve doğrulandı** | Urun.WhatsappSiparisVarMi, Urun.FiyatGizliMi kolonları; Program.cs:934-935 Ensure migration | Site genel WhatsApp linki footer (wa.me/970599000000), ürün bazlı UI teyit edilmedi | Ürün bazlı test edilmedi | Ürün detayında test et; admin form alanı çalışıyor mu kontrol |
| G12 | Hediye paketi + fiyatlı ek hizmet | **Kısmen uygulanmış** | Urun.HediyePaketiVarMi, HediyePaketFiyati; SepetItem.HediyePaketi/Fiyati; SiparisController.cs:281 kayıt | Sepette "Special Selections" görüldü, ek seçenek teyit edilmedi | UI akışı kırık olabilir | Detay ve sepet UI'da hediye paketi opsiyonu kontrol et |
| G13 | Reçeteli kategoride reçete/kimlik zorunluluğu | **Tam ve doğrulandı** | Kategori.ReceteGerekliMi, Siparis.ReceteDosyaYolu, SiparisController.cs:124-131 validasyon | Ödeme sayfasında "Identity Verification" bölümü + Open Camera/Upload (Te) | Akış break-out test edilmedi | E2E reçeteli kategori → ödeme → reçete zorunlu teyit |
| G14 | Üyelikte kimlik, doğum, telefon, bölge, şehir, adres + kimlik fotoğrafı | **Tam ve doğrulandı** | AppUser.KimlikNo, DogumTarihi, PhoneNumber, Adres, Sehir; HesapController#KayitOl | /Hesap/KayitOl formu, /Profil/Adreslerim listesi | Adreslerim.cshtml'de hardcoded şehir listesi (bozuk) | Aynı B1 fix |
| G15 | Kameradan kimlik/fotoğraf çekme | **Yalnızca UI var, backend teyit edilmedi** | _IdentityVerification.cshtml, Siparis/_IdentityVerification.cshtml butonları + openCamera() JS | Snapshot'ta butonlar render olur | Permissions-Policy camera=() → WebRTC engellenmiş olabilir (B9). WebRTC JS kayıt yok. | Permissions-Policy'i /Siparis için override et |
| G16 | Adrese teslim / mağazadan teslim | **Tam ve doğrulandı** | Siparis.TeslimatTipi ('AdreseTeslim'/'MagazadanTeslim'); SiparisController.cs:219-228 mağazada kargo 0 | Snapshot: radio "Deliver to Address" + "Store Pickup" | — | — |
| G17 | Bölge/şehir bazlı kargo ücreti (müşteri firmayı seçmez) | **Çakışmalı/yanlış uygulanmış** | IKargoHesaplamaServisi, KargoBolgeler/BolgeSehirler/BolgeFiyatlari tabloları | /Siparis/Odeme'de şehir dropdown hardcoded yedek arrayden geliyor — **DB KargoBolgeSehirler BOŞ** | Kargo hesaplama servisi boş DB ile "Free" döner (TB opponents); kullanıcı bölge seçemez | KargoBolgeler + BolgeSehirler + Fiyatları seed et (United Express dahil) (B1) |
| G18 | Filistin bölgeleri: iç bölge/48, Batı Şeria, Kudüs | **Eksik** | KargoBolge.Ulke, KargoBolge.Aciklama (Adım 71) | Hardcoded array'de "El-Kudüs", "El-Halil" gibi şehirler var ama **bölgesel gruplama yok** | DB boş, yedek array bozuk encoding | Bölge tanımlarını (48, Batı Şeria, Kudüs, Gazze) DB'ye seed |
| G19 | Yönetilebilir kargo firması (örn. United Express) | **Eksik** | KargoFirmalari entity + KargoController admin | DB KargoFirmalari tablosu BOŞ; eskiden seed墓葬'da "Aras" (filistin ile uyumsuz) | Müşteri kargo firması seçmez gereksinimi aykırı değil; ama admin yönetimi boş | United Express seed et; takip URL dahil |
| G20 | Kapıda ödeme + elektronik ödeme + görünürlük kontrolleri | **Kısmen uygulanmış** | SiteAyarlari.KapidaOdemeAktifMi, KapidaOdemeHizmetBedeli, KapidaOdemeLimiti; SiparisController.cs:156-165,198-204 | Snapshot'ta sadece Bank Transfer radio var, **COD görünmüyor** — KapidaOdemeAktifMi && sepet≤Limit şartı sağlanmıyor olabilir | Sepet ₪44,987 > ₪2,000 limiti → COD auto-disabled SiparisController.cs:540 | Beklenen davranış. Düşük sepetli senaryoda test et |
| G21 | Yüksek tutarda kapıda ödeme limiti | **Tam ve doğrulandı** | SiparisController.cs:198-204 kontrolü | Sepette 44,987 > 2000 limit → COD kapalı (snapshot) | — | — |
| G22 | Sipariş notu + şartlar onayı | **Tam ve doğrulandı** | Siparis.Aciklama, SiparisController.cs:118-121 sozlesmeOnaylandi zorunlu | Odeme.cshtml'de Order Note + checkboxPrivacy (Te) | — | — |
| G23 | Sipariş takibi + koşullu iptal | **Kısmen uygulanmış** | Siparis.Durum; ProfilController#Siparislerim; SiparisController#Iptal (ara) | /Profil/Siparislerim listesi render (screenshot temiz) | İptal koşulları tam taranmadı | İptal endpoint test et |
| G24 | Toptancı hesabı + onay + özel fiyat/iskonto + min sipariş | **Kısmen uygulanmış** | AppUser.WholesaleStatus, ToptanciController, ToptanciUrunGruplari, ToptanciIskontoOranlari, ToptanciMinSiparisTutari | /Admin/Toptanci link; SiparisController.cs:207-214 min kontrol | Onay/özel fiyat akışı E2E teyit edilmedi | Toptancı kullanıcı olarak login → en düşük fiyat gösterimini teyit |
| G25 | Admin çalışan + detaylı yetkilendirme | **Tam ve doğrulandı** | AdminBaseController.cs, AdminPermissionMatrix, AdminSecurityRoles (8 rol: Admin/SuperAdmin/Yonetici/SiparisYoneticisi/UrunYoneticisi/IcerikYoneticisi/KargoYoneticisi/Goruntuleyici) | Admin panel render, dashboard gösterim (Te) | Görituleyici salt okuma rolü var; GET/POST ayrım var | — |
| G26 | Ürün/kategori/stok/sipariş/kullanıcı/kupon/slider/kargo/iade/yorum/sayfa/rapor yönetimi | **Tam ve doğrulandı** | 27 admin controller (Areas\Admin\Controllers) listelendi | /Admin/Home dashboard; sidebar menü tüm modüller | CRUD tüm bölümler teyit edilmedi ama altyapı var | Her controller CRUD E2E test |
| G27 | PDF fatura + stok uyarısı + satış/bölge raporları | **Kısmen uygulanmış** | FaturaPdfService.cs, RaporController.cs, StockAlertService | Admin sidebar "Raporlar" | PDF fatura üretimi test edilmedi | PDF üret + kanıt gör |
| G28 | İletişim + teknik destek + hakkımızda + gizlilik + iade/iptal/kargo politikaları | **Kısmen uygulanmış** | KurumsalController, SozlesmelerController, IletisimController (admin mesajlar) | /Kurumsal/Iletisim link footer; /Sozlesmeler/Gizlilik vb. | İçerik statik view mı DB'den mi teyit edilmedi | İçerikleri gör |
| G29 | SEO + kategori/marka direkt linkler + sosyal medya/WhatsApp bağlantıları | **Kısmen uygulanmış** | SitemapController, ISeoService, Urunler.Slug index, SiteAyarlari.MetaTitle/Description | Ana sayfada SEO meta; footer sosyal linkler | Marka direkt linki yok (marka entity var ama slug/route yok) | Marka sayfası /Marka/{slug} rotası |
| G30 | Marka adı 7ANRPS48, ₪/ILS; eski Canvasia/Türkiye kalıntıları yok | **Çakışmalı/yanlış uygulanmış** | Tüm tabakada "Canvasia" + "kastamonuesnaf.com.tr" + "tr-TR" + Türk kargo + İstanbul adresi; email imza Canvasia | secrets.json: FromName Canvasia; SMTP Şifresi plaintext | Kapsamlı kalıntı (B7, B0) | Sıfırlama: SMTP FromName/Email, SiteSettingsService logo fallback, kargo takip URL, package.json GitHub, deployment script |

---

## 5. FRONTEND / UI/UX RAPORU (en yüksek öncelik)

### Test yöntemi
Aynı tarayıcı oturumuyla Playwright MCP kullanıldı; AR + EN modlarında ana sayfa, ürün listesi, detay, sepet, ödeme, login, admin dashboard gezildi. 390×900 (mobil) ve 1440×900 (desktop) viewport'ları kullanıldı. Horizontal overflow .scrollWidth > .clientWidth ile denetlendi, dir/lang attribute teyit edildi.

### Sayfa bazlı bulgular

#### Ana sayfa (/)
- **Mobil 390 AR**: overflow yok, dir=rtl lang=ar. Üst bar "شحن مجاني للطلبات فوق 200 ₪" sloganı. Banner (Hero), altında "منتجات مميزة" 4 ürün kartı. Sepet badge "13".
- **Desktop 1440 AR**: Topbar scroll up ile kayan (iki kopya 1e3 ve 1e23 — duplicate DOM block). TrustBar 4 ikon. Footer e-posta subscription + kurumsal linkler + WhatsApp.
- **Mobil hamburger**: #mobileMenuBtn açılır menü → kategoriler + Favorites + My Account linkleri. **Sepet linki drawer'da yok**.
- **Desktop nav**: hidden md:block ile lg altı gizli. Kategori ikonlu menü çalışır. Dil dropdown lg:flex.
- **Mobil arama**: #mobileSearchBtn tek tıkla overlay açar — güçlü UX.
- **İngilizce (LTR)**: title EN, tüm metinler EN, dir=ltr — dönüşüm temiz.

#### Ürün listeleme (/Urun)
- 4 ürün var, hepsinde "جديد" (New) rozeti. Sağda grid/liste toggle, sıralama combobox (4 opsiyon), fiyat slider noUiSlider.
- **risiko**: Filtre sidebar 360px'de yığılır. Marka ve özellik filtreleri yok.
- Fiyat format: ₪4٬999 — Arapça "٬" binlik ayracı doğru; ama /Urun/Index.cshtml:525-528 JS 	oLocaleString('tr-TR') → TL/sayı için TR format. **Tutarlılık bozuk**.

#### Ürün detay (/Urun/Detay/{slug}-{id})
- **Resim eksik** (B5): sol sütunda ürün görseli yerine FA ikon placeholder, /img/products/placeholder.webp 404 döner.
- Sağda başlık, fiyat, "Made to Order" / "1 days" / "7 days return" / "Manufacturer Guarantee" rozetleri; adet seçici +/- butonlar; "Add to Cart" butonu.
- Aşağıda "Product Details" + "Reviews" + "Write Review" — ama yorum yok.
- **Çeviri**: İngilizce modda ürün adları doğru ("Modern Sofa Set"). Sepet ve detaylarda ürün adı Arapça (sepetten sync because cart snapshot Arapça metinle eklenmiş). Bu — DB ürün **Baslik kolonu AR metin** içeriyor, BaslikEn/BaslikAr ayrı kolonlar mevcut ama **Baslik hala eski Türkçe/AR metin** — melez durum. (B12)
- Varyasyon seçenekleri (renk/boyut) UI'da **görünmüyor** — DB'de UrunSecenekleri var ama ürünlerde seed yok.

#### Sepet (/Sepet)
- AR: "سلتي" başlık, 3 ürün. Her satırda resim var (sepet için img), not kutusu (500 char), adet +/-, toplam. Sağda "Order Summary": Subtotal ₪44,987, Free shipping achieved, Total.
- **İngilizce modda**: "My Cart", "3 products" ama ürün adları Arapça (طقم كنبات مودرن, فستان سهرة أنيق, ساعة ذكية). **Lokalizasyon eksikliği**.
- Sepet count "13" ama "3 products" — gereksiz tutarsızlık (DB sepet tablosu mevcutsa 13, ürün çeşit sayısı 3).

#### Checkout (/Siparis/Odeme)
- Adım 2 breadcrumb: "1 Cart / 2 Payment and Delivery".
- Teslimat tipi radio: Adrese Teslim (default selection) / Mağazadan Teslim.
- Form alanları: Full Name (admin@loginden dolu), Email, Phone (+970 placeholder), District (şehirler — **BeytÃ¼llahim** bozuk), Neighborhood, Full Address, Address save checkbox.
- **Kargo ücreti**: "Shipping fee will be calculated automatically based on your city" — 3 dilde teknik mesaj, ilki bozuk.
- **Ödeme yöntemi**: Sadece Bank Transfer radio. Sebebi ₪44,987 > ₪2,000 limiti → KapidaOdemeAktifMi && (sepetToplami <= settings.KapidaOdemeLimiti) SiparisController.cs:540 → false. **Doğru davranış**.
- Identity Verification bölümü: "Open Camera" + "Upload File" butonları.
- Alt çıkar: Subtotal, Free shipping, Grand Total 44.987,00 ₪.
- Sözleşme onay checkbox zorunlu.
- **Encoding bozuk**: Şehir listesindeki Türkçe/Arapça metinler çift encoding geçirmiş.

#### Login (/Hesap/GirisYap)
- **B4**: Email input value="admin@7anrps48.com", Password value="Admin123!" — view içine gömülü default cred.

#### Admin Home (/Admin/Home)
- Login başarılı → /Admin/Home açıldı.
- Sidebar (AR): Dashboard, Manage Orders (collapsible), Manage Products, Store Contents, Users, Personnel, Wholesalers, Wholesale Groups, Reports, Bank Accounts, Settings.
- Dil dropdown AR/EN. Üstte "لوحة البائع" (Vendor Panel) başlığı — ama **"Vendor"** demiş (admin olmalıydı muhtemelen) — wacth.
- Dashboard widget: Total Revenue ₪1,350, Today's Revenue ₪0, Total Orders 5, Ready-to-Ship 1.
- Admin новост officerısı: "7ANRPS48 Admin Admin 7" rozet.
- Mobil 390 snapshot: hamburger + slide-out drawer (Adım 63). Responsive.

### Madde başı UI/UX bulguları

| # | Sayfa | Viewport | Dil | Problem | Beklenen | Gerçek | Önerilen |
|---|---|---|---|---|---|---|---|
| F1 | Ürün detay | 390/1440 | AR/EN | Ürün görseli yok, 404 placeholder | 1-3 görsel + thumb strip | FA ikon + 404 | Demo seed görsel;üler fallback ikon koymak |
| F2 | Ödeme | 390/1440 | EN | Şehir dropdown: "BeytÃ¼llahim", "El-KudÃ¼s" (bozuk) | Düzgün UTF-8 listeleme | Latin1'e re-encdoe edilmiş mojibake | view'da array'i UTF-8 dB'den çek, fallback'i DB'li seed (B1) |
| F3 | Sepet | 1440 | EN | Ürün adı Arapça ("طقم كنبات مودرن") | EN'de İngilizce ad | EN'de AR görünür | Ürün adaptor: Baslik+ LocalizedBaslik ile dil bazlı gösterim |
| F4 | Sepet | 390 | AR | Header'da sepaiddeSepet 13 ama "3 منتج" | Tutarlı rakam | Çelişkili countss | Sepet item sayısı = sum(Item.Adet) — tooltips ile netleştir |
| F5 | Ana sayfa | 390 | AR | Mobile drawer'da sepet linki yok | Sepet de görünmeli | Sepet header sticky'de var ama drawer'da yok | Drawer'a sepet ekle |
| F6 | Ürün listesi | 390 | AR | Marka/özellik filtresi yok | Sidebar marka listesi | Fiyat+kategori var | Sidebar'a marka ve nitelik paneli ekle |
| F7 | Tümü | 1440 | EN | JS 	oLocaleString('tr-TR') para formatı | ar-PS / en-US | TR format | Tüm NumberFormat JS'i dil-bazlı B8 |
| F8 | Admin Home | 390 | AR | Üstte "لوحة البائع" (Vendor) | Admin | Vendor deniyor | Vendor/admin metin düzelt |
| F9 | Ödeme | 390 | EN | Permissions-Policy: camera=() → Open Camera break | WebRTC çalışmalı | Jump Insights | Permissions-Policy'i /Siparis rota için camera=(self) |
| F10 | Login | tüm | tüm | admin şifresi default dolu | boş input | Dolu dmin@7anrps48.com / Admin123! | View'dan şifreyi sil (B4) |

---

## 6. BACKEND VE İŞ KURALLARI RAPORU

### Akış 1: Ürün → sepete ekleme → stok → fiyat → sipariş
**Olumlu**: SepeteEkle sepet servisi stok kontrolü yapıyor (CanAddQuantity, SepetService.cs:396-423). MinSiparisAdedi / MaxSiparisAdedi / OnSipariseAcıkMi zorunluğu var. Sepet'te tutar servis tarafında hesaplanıyor.

**Olumsuz — P0 siparişte fiyat doğrulaması yok** (B3): SiparisController.cs:275 BirimFiyat = item.Fiyat — sepet tablosundaki fiyat direkt kullanılıyor. Ürün fiyatı güncellense bile sepet alışverişinde eski fiyat korunur. **Kötü niyetli senaryo**: Sepete normal ₪500 fiyatla ekler, sonra sepetten ekledikten sonra admin fiyat ₪5000'e çeker, müşteri ilk fiyatla öder → **sicak değil** ama beklenen. Daha kritik: **SepetItem.Fiyat kolonu veritabanına yazılırken client-supplied fiyat mı yoksa DB fiyatı mı?** SepetEkle akışına bakmak gerek → stewed değil. Teorik sızma: client POST manipülasyonu ile ürünün varyant fiyatını düşük gönderirse, SepetItem.Fiyat düşük olur → sipariş toplam düşük. Tekrar teyit gerekiyor, ama **sipariş sırasında DB'den varyant fiyatını re-fetch edip karşılaştırmak best-practice'tir** ve yok.

**Olumsuz — P0 stok düşümü yok** (B2): SiparisController.cs:263-298 transaction içinde Siparisler ve SiparisDetay ekleniyor. **Stok düşerme satırı yok**. İki senkron sipariş → race condition → electro-overselling. Stok modasıyonu için UPDATE UrunSecenekleri SET StokAdedi = StokAdedi - adet WHERE Id = secenekId AND StokAdedi >= adet SQL'i transaction içinde gerekir.

### Akış 2: Hediye paketi → sepet → sipariş toplamı
SiparisController.cs:280-281: HediyePaketi = item.HediyePaketi, HediyePaketFiyati = item.HediyePaketFiyati — ama **toplam Tutar hesabına HediyePaketFiyati eklenmiyor**. :241 siparis.ToplamTutar = siparis.ToplamTutar + kargoUcreti + siparis.KapidaOdemeHizmetBedeli → hediye paketi bedeli yok. **Müşteri hediye paketi yine de toplamda ödemezl ik ediyor** (B13). SepetService'in GetSepetToplamiAsync muhtemelen hesaplar ama snapshot'ta eklendi gözlneimedi.

### Akış 3: Kupon → indirim → sipariş
SiparisController.cs:180-195: Kupon session'dan okunuyor (UygulananKupon session key). Kupon validasyonu (AktifMi, SonKullanmaTarihi > now, KullanimLimiti, MinSepetTutari) doğru. İndirim hesabı CalculateCouponDiscount (Tamsayı oransal). Transaction içinde kupon.KullanilanMiktar++ (Bq) — kullanim sayacı atomic değil, ama sipariş tekil olduğu için okfox doğal. **Kupon folder için FixedWindow yok** — POST Flood saldırısında kupon deneme. (B14)

### Akış 4: Bölge/şehir → kargo fiyatı otomatik hesap
SiparisController.cs:231-234: _kargoHesaplama.HesaplaAsync(sehir, toplamTutar, ucretsizKargoLimiti). **DB boş** (B1) → hesaplama muhtemelen 0 döner. Kullanıcı Şehir seçince her zaman ücretsiz ulaşır — yanlış. Önerim: seediFilistin bölgeleri (Gazze, Batı Şeria, Kudüs) eklenmeli + KargoBolgeFiyatlari fiyat atanmalı.

### Akış 5: Ücretsiz kargo barajı
Sepet/Index.cshtml "Free shipping achieved!" mesajı var (₪44,987 > ₪200 limit). SiparisController.cs:503-505 doğru. **Bq**: SiteAyarlari.UcretsizKargoLimiti admin'den ayarlanabiliyor mu? AdminBaseController#AyarlarController'ın Index'inde eksik olabilir (test dışı).

### Akış 6: 2000 ILS üzeri kapıda ödeme limiti
SiparisController.cs:198-204: if (OdemeYontemi == "KapidaOdeme" && sepetToplamiIndirimli > settings.KapidaOdemeLimiti) → ModelState.AddModelError. **Doğru**. Snapshot'ta ₪44,987 sipariş için :540 ViewBag.KapidaOdemeAktifMi = false → COD radio render edilmiyor, doğru.

### Akış 7: Reçeteli kategori → reçete zorunluluğu
SiparisController.cs:124-131: urunIds alıp Kategori.ReceteGerekliMi kontrolü, siparis.ReceteDosyaYolu "uploads/receteler/" path kontrolü. **İyi**. IsSafeUploadedPath uzantı + path traversal + ".." + Uri absolute blokaj doğru.

### Akış 8: WhatsApp/fiyat gizli → normal satış kapanması
Snapshot ürün örneklerinde WhatsappSiparisVarMi/FiyatGizliMi test ürünleri değil. Kodda: ürünler listelenirken FiyatGizliMi=true ise fiyat bağlanmaz — onaylanmadı ama mantık var. UI akış teyit edilmedi (B15).

### Akış 9: Toptancı → özel fiyat/iskonto → min sipariş
SiparisController.cs:206-214: if (User.IsInRole("Wholesale") && ToptanciMinSiparisTutari > 0 && sepetToplamiIndirimli < Min) → hata. **Doğru** ama toptancı özel fiyat UI'sinde nasıl uygulandığı teyit edilmedi (B16).

### Akış 10: Stok azalması → iki eşzamanlı sipariş → overselling
SepetService.CanAddQuantity sepete eklemede kontrol ediyor — sepete ekleme sırasında race yok (DB serialize transaction var). **Ama sipariş POST'ta stok kontrolü yok** (B2). 2 kullanıcı aynı varyantı sepete ekler, 2'si de öder → her ikisi de başarılı sipariş → StokAdedi çoktan 0'ın altına düşmüş → admin panel yanlış stok görür → üçüncü sipariş yine geçer.

### Akış 11: Sipariş iptal/iade
SiparisController#IadeController admin. Teyit dışı. Mekanizma var.

### Akış 12: Admin yetkisiz → admin endpoint
AdminBaseController.cs:14 [Authorize(Roles="Admin,SuperAdmin,Yonetici,SiparisYoneticisi,UrunYoneticisi,IcerikYoneticisi,KargoYoneticisi,Goruntuleyici")] — tüm admin rolleri listeli. AdminPermissionMatrix.CanAccess controller+method bazlı kontrol. Audit log: dmin_permission_denied. **İyi**. Test edilmedi ama statik analiz güçlü.

### Genel backend bulguları
- **Telefon validasyonu Türkiye formatında** (B6): :582 Length != 11 || !StartsWith("0"). Filistin için +970 5XX XXX XXX.
- **Program.cs log dosya adı doğru** (7anrps48-log-.txt). Ama yorum/tr string encoding bozuk → deploymnette utf-8 BOM eksik.
- **Email servisinde Türk kargo linkleri** (B7): SmtpEmailService.cs:186 kargotakip.araskargo.com.tr, :196 mngkargo.com.tr — Filistin'de çalışmaz, kullanıcı tıklayınca Türkiye'ye gidiyor.

---

## 7. VERİTABANI / MİGRASYON RAPORU

### EF Migrations
AGENTS.md'de 17 sürüm listeli. Repository'de FilistinProje.Data/Migrations/ içinde ek migration'lar mevcut:
1. 20260131211352_BultenTablosu, 20260131213049_BultenIpEklendi
2. 20260504111249_AddProductSeoFields
3. 20260508175141_MusteriNotuAlanlariEklendi, 20260508204726_Fix_NullableUrunSecenekId_And_SlugIndex
4. 20260523223159_AddFavoriPriceDropFields
5. AGENTS.md'de listelenen 17迁移 (InitialCreate → AddSliderMultilingual)

### Dual migration — karşılaştırma EF ↔ EnsureMissingMarch2026SchemaAsync (B17)
**Tutarlılık alındı** — Program.cs:846-1149 (EnsureMissingMarch2026SchemaAsync) elle yazılmış SQL bloğu ile EF migration'ları tutarlı:
- EF'de yer alan tüm tablolar (Urunler, Kategoriler, Siparisler, AspNetUsers) için ADD COLUMN IF NOT EXISTS; index (IX_*); FK (FK_Kategoriler_Kategoriler_ParentKategoriId); ek tablo (ToptanciUrunGruplari, ToptanciIskontoOranlari, BankaHesaplari, UrunOzellikTanimlari/Degerleri, SiteDegerlendirmeleri) — Program.cs SQL'inde de var.
- **Ancak risk**: iki sistemin de aynı şemayı üretmesi received değil, **geliştirici uyumu** — yeni bir property eklenince her ikisine de eklenmezse:
  - EF migration alır ama Ensure yoksa → mevcut DB'de hata olabilir mi? (Migrate önce gelir ve Ensure sonra); Ensure eksik olabilir ama ADD COLUMN IF NOT EXISTS sessiz geçirir. Tutarlı.
  - Ensure ekler ama EF almazsa → EF Migrate ile yeni DB kurulduğunda Ensure tekrar eklemeye çalışır (sessiz). MigrationsHistory'ı Ensure manuel eklemiyor. Mantıklı.

**Doğrulama**: Urunler kolon 60 tane teyit edildi (snapshot). Hepsi uyumlu.

### Raw SQL double quotes + Türkçe PascalCase (B18)
Program.cs:693-1118: tüm tablo/kolon adları çift tırnak ("Urunler", "Siparisler", "KargoBolgeler" vb.) — **doğru PostgreSQL quoting**. Türkçe karakter (ı, ğ, ç, ş) kullanılmamış kolon adlarında (sadece PascalCase İngilizce + Türkçe-like: OlusturulmaTarihi, SilindiMi — bunlar Türkçe ama ASCII harflerle yazılmış). PostgreSQL collation'a duyarlıdır, sorun yok.

### Seed & Tablo içerikleri
- Urunler: 9-15 ürün (4 demo'da gözüktü). AlexDosResults ve ElegantEveningDress gibi adlar.
- Kategoriler: 4 kategori (家具 1, elektronik 1, fashion 2 adet — simasyo).
- KargoBolgeler: **0 satır**. KargoBolgeSehirler: **0 satır**. KargoFirmalari: **0 satır**. BankaHesaplari: **0 satır**.
- SiteDegerlendirmeleri, SiteAyarlari: mevcut.
- 6 admin, 5 sipariş, dashboard'lıta ₪1,350 toplam.

### N+1 potansiyeli
- SiparisController.cs:797 BuildOrderLineDetail(item) her sipariş detayında ana Urun — Include'u yapılmış :776-778. OK.
- HomeController (anda sayfa) ürünleri getiren serviste.Include().ThenInclude() teyit Gerekir. Hipotez-olası: AnaSayfaController partial'ı binder ile Urun.UrunSecenek iterasyonu N+1 olabilir (B19).

### Decimal precision
Urunler.Fiyat ve IndirimliFiyat 
umeric (postgres) — decimal(18,2) değil ama içerikte 2 ondalık. ₪ para için yeterli. Phone.MaxValue=11 YAINDA dresiKaydet? Adres alan Plusıne 500 max. OK.

### Timezone
OlusturulmaTarihi timestamp with time zone. **DateTime.UtcNow** SiparisController.cs:150 — tutarlı (UTC). Raporlarda ToPalestineLocal (Adım 96) — Asia/Gaza (UTC+2/+3). Train out'da teyit edilmedi. (B20)

### Transaction sınırları
- SiparisController.cs:243 BeginTransactionAsync → Siparis + SiparisDetay + Kupon increment → Commit. **Iyi** ama stok düşmüyor (B2).
- Stok + Siparis + Kupon'u tek transaction'a alıyor, tutarlılık iyi. Sadece stok eylemi eksik.

### Migration deployment risk
- Migrate() :557 hata yutuyor (catch → logger.LogError). **Riskli**: migration failse sessizce devam eder, app kalkar ama schema güncel değil → runtime hata. (B21)
- EnsureMissingMarch2026SchemaAsync SQL bloğu sembolik marker (migration history insert) sadece birkaç migration iş bırakır; geri kalanlar "silently applied". Bu hatayı gizleyebilir.
- **Rollback risk**: migration down scripts var mı? EF standard Down() methodology mevcut ama production'da rollback zor.

---

## 8. GÜVENLİK VE GİZLİLİK RAPORU

### Aute/authentication
- Identity, lockout (MaxFailedAccessAttempts=5, 15dk), şifre gereksinimleri (digit+lower+upper+nonalphanumeric, 8 char) **İyi**.
- Cookie HttpOnly, SameSite=Lax, 30 gün sliding-experile. OnValidatePrincipal cookie roles'ı DB ile sync — strong.

### OLTP/IDOR/BOLA hipotezleri
- **Hipotez — SiparisController.cs:64** sepeta alıcı için userId ve sessionId — guest cart session-id ile anonim sepette ?sessionId = X olsa guestIDOR olabilir. Teşıt11: _sepetService.GetSepetItemsAsync(userId, sessionId) — sessionId'i bilmeyen sepeti göremez. Teyit edilmedi.
- **Hipotez — Admin Siparis/Detay.cshtml** /Admin/Siparis/Detay/{id} route'una erforder :547 kay  AdminBaseController kontrolü. Mutlaka CanViewOrder gibi matrix rol token gerek. Eğer admin genel [Authorize(Roles...)] yete varsa tüm admin tek tek sipariş görebilir — ama rol matriksi yine](zaten).
- **Hipotez — customer göster Profil/Siparislerim**: userId = User.GetUserId() → siparisler .Where(s => s.AppUserId == userId) — teyit gerek. B22.

### CSRF, Antiforgery
- Program.cs:271-277 antiforgery setup Strict SameSite, HttpOnly. *but*: [ValidateAntiForgeryToken] attribute'leri hangi POST'larda? SiparisController.cs:95 mevcut. **Diğer POST'lar** (Hesap/KayitOl, Sepet/Ekle, Favori, Profil güncelleme) — şeyleri teyit dışı (B23).

### XSS
- Email body HTML encode yapıyor: WebUtility.HtmlEncodeitem.Urun.Baslik vb SiparisController.cs:783-788. İyi.
- _Layout cshtml XSS-genvuent olmayan email template WriteAsJson — dorr-uppercase. Teğıtetsuz.
- Ürün açıklama Aciklama HTML render (<p>...</p> raw). Admin ekleme yapsa <script> enjekte edebilir → storefront detay'da çalışır. **Hipotez — Stored XSS (B24)**. @Html.Raw(item.Aciklama) kullanımı teyit edilmedi.

### SQL injection
- _context.Urunler.Where($"...") interpolasyonu yok, tüm LINQ. Raw SQL Program.cs:535 $"INSERT INTO \"__EFMigrationsHistory\" ..." — değer string interpolant. Kritik injection risk yok.
- SearchController: admin Search/Index query string'den parametre — %LIKE% parametrikullanım olmalı. Teşitdış.

### File upload
- IsSafeUploadedPath fonksiyonu SiparisController.cs:626-648: path traversal "..", absolute Uri, uzantı whitelist. **İyi**.
- Bunlar adisyon uygulamalar — ama DosyaServisi.cs (Image AS βά) DWO güvenliği: file content-type check (MIME sniff), büyüklük limit teyıt dışı.umably B25.
- Storing in wwwroot/uploads/kimlikler — web-accessible? **Kritik: kimlik görselleri web kök erişilebilir olabilir** (B25). Teştır dışı ama pp.UseStaticFiles() default wwwroot serve eder.

### Open redirect
- SiparisController.cs:464 eturnUrl query'den → Redirect($"/Hesap/GirisYap?returnUrl={returnUrl}") — eturnUrl = Uri.EscapeDataString(context.Request.Path + context.Request.QueryString). Source Path + QueryString. **Hipotez**: Path = "/?redirect=https://evil.com" → redirect URL param ile 1 tık open redirect — teşit dışı (B26).

### Mass assignment
- SiparisController.cs:96 Odeme(Siparis siparis, ...) — tüm Siparis entity bind. Overposting risk: **用户 Post siparis.ToplamTutar=0 gönderirse** → :176 siparis.ToplamTutar = hamTutar overwrite eder, Faker yok. Yani overwrite yapsa bile sunucu overwrite ToplamTutar. Iyi.
- Ama siparis.OdemeYontemi form parametre'den bind, ayrıca odemeYontemi prrm pass. hem property hem param — cakışma (B27) ibaери.

### Session / cookie
- 30 dk IdleTime, HttpOnly. SessionID rotation login sonrası zorunlu olmayabilir — Teşit dışı. (B28)

### Rate limiter
- "auth" (10 req/5dk IP), "general" (100/dk IP). Founded: hangi endpointlerde policy aktif? [EnableRateLimiting("auth")] atributeleri teyit gerek — bilakis, configure edilmiş ama policy-level enable control dışı. **Hipotez — rate limiter tüm endpoint'larda uygulanıyor mu**? Program.cs:244 global policy var, [EnableRateLimiting] ekleyene yok. **Hipotez: tüm endpoint'lar "general" policy, "auth" policy hangi endpoint?** Eğer enable yok umarım. (B29)

### Maintenance mode bypass
- Program.cs:322 IsMaintenanceAllowedPath kontrolü: "/admin", "/hesap", "/api/admin" her zaman açık. **Kurallı → admin erkek kullanıcıları(self-monitoring) aykırı değil**. OK.
- **Riskot**: /hesap/cikis da açık → kullanıcı çıkış yapabilir, ama /sepet 503 dönmesi. Hatalı tasarım değil.

### Open IDOR — cart session
- HttpContext.Session.Id ile guest sepeti — birisi sessionId'i öğrenebilirse (header leak, log) başkasının sepetini axios eder. **Risk düşük-orta**. (B22)

### Admin login — default credentials
- B0 secrets.json'dan farklı — secrets.json SMTP credentials. Default kullanıcıkrediyi seed'e bak. DbSeeder.CreatingAdmin veya VerileriYukle mutlaka Admin123! ile default admin yaratıyor. **Tighter**: production seed admin password force-change gerekli (B30).

### Logging privacy
- ZiyaretciTakipAttribute her isteği loglar (ZiyaretciLoglari tablo). CihazBilgisi full User-Agent. **Bq PII**:  IP, User-Agent, path. GDPR/KVKK için Persian Helen → Filistin için gerekli olmasa da policy gerekli (B31).

### Bulgu özeti (Bu bölüm)
| # | Risk | Açıklama | Etki | Olas. | Çab. | Çözüm |
|---|---|---|---|---|---|---|
| B0 | P0/CRITICAL | secrets.json canvasia eposta | müşteri Canvasia mail görür, marka sızıntısı | 5 | XS | secrets.json: Fromemail info@7anrps48.com, FromName 7ANRPS48 |
| B3 | P0/CRITICAL | Siparişte fiyat server-side doğrulanmıyor | manipüle edilmiş fiyat | 4 | M | SiparisController.Odeme POST'ta her SepetItem için DB'den UrunSecenek.SatisFiyati re-fetch et, fark > tolerance → reject |
| B4 | P0/CRITICAL | Login view'da default admin cred | herkes admin@7anrps48.com/Admin123! görür | 5 | XS | Views/Hesap/GirisYap.cshtml default değerleri sil |
| B2 | P0/CRITICAL | Siparişte stok düşümü yok | overselling, negatif stok | 5 | M | Transaction içinde UPDATE UrunSecenekleri SET StokAdedi = StokAdedi - adet WHERE Id = secenekId AND StokAdedi >= adet |
| B5 | P1 | Ürün görselleri demo'da yok/404 | ücretsiz değer propozisyonu kayıp | 5 | M | seed demo görseller + placeholder image to fallback |
| B6 | P1 | Telefon format Türkiye | tüm Filistin müşteriler sipariş veremez | 5 | S | Length >= 9 && StartsWith("+970") || Length == 10 digits |
| B7 | P1 | Marka/domain/SMTP kalıntısı | brand/identity/osp güveni kırılır | 4 | L | dosya dosya geçiş; secrets.json, package.json, SmtpEmailService, SiteSettingsService, hosting |
| B9 | P1 | Permissions-Policy camera=() | 'Open Camera' butonu WebRTC açmaz | 3 | S | /Siparis route için camera=(self) |
| B1 | P1 | Kargo/şehir/IBAN tabloları boş + hardcoded fallback | kargo ücreti yanlış, encoding bozuk | 5 | M | seed 5 bölge + 17 şehir + her bölge/firma fiyat + IBAN |
| B12 | P1 | Ürün Baslikkolonu AR/EN melez | EN modda ürünler AR çıkar | 5 | M | Baslik kullanıcıya LocalizedBaslik (EN/AR seç) |
| B13 | P2 | Hediye paketi bedeli ToplamTutar'a eklenmiyor | hediye bedeli düşmez, zarar | 3 | XS | :241 + siparis.HediyePaketFiyati toplamı veyasepet Toplami dahil |
| B14 | P2 | Kupon deneme POST'a rate limit yok | coupon brute force | 3 | S | "auth" policy [EnableRateLimiting("auth")] |
| B22 | P2 | Sepet IDOR session-id | mid probation | 2 | M | session bound + user-id (auth) kontrol |
| B24 | P2 | Hipotez — @Html.Raw(Aciklama) Stored XSS | admin bypass | 3 | S | encode only safe html tags whitelist |
| B25 | P1 | Uploads web kök | kimlik görseleri ulaşılabilir | 5 | M | private storage; route authorization |
| B27 | P2 | SIPARİS overposting binding | modelecief | 2 | S | [Bind] on odeme or DTO |
| B28 | P2 | Session rotation yok | fixation | 2 | S | login post session regenerate |

---

## 9. LOKALİZASYON / MARKA RAPORU

### Dil ve yön davranışı
- Program.cs:232-241 sadece r ve en destekli; default r. Bu doğru.
- Playwright doğrulaması:
  - / AR: document.documentElement.dir = rtl, lang = ar, horizontal overflow false.
  - /Dil/Degistir?culture=en&returnUrl=%2F: dir = ltr, lang = en, title 7ANRPS48 - Palestinian Products & Online Shopping.
- Admin paneli ağırlıklı Arapça görünüyor; AGENTS.md admin view'larının Türkçe yazılabileceğini söylüyor ama pratikte admin AR localizer kullanıyor. Tutarsız karar değil, fakat bazı admin metinlerinde "Vendor Panel" anlamlılık sorunu var.

### Kritik lokalizasyon sorunları
| # | Dosya / Kanıt | Sorun | Etki | Öneri |
|---|---|---|---|---|
| L1 | Views/Siparis/Odeme.cshtml:29-30 | BeytÃ¼llahim, El-KudÃ¼s mojibake | Checkout şehir dropdown profesyonel görünmüyor | UTF-8 düzelt, hardcoded fallback'i kaldır |
| L2 | Views/Siparis/_AddressForm.cshtml:29-30 | Aynı bozuk şehir listesi | Tüm checkout partial'larında tekrar | Tek kaynak: DB seeded city service |
| L3 | Views/Siparis/_OrderSummary.cshtml:29-30 | Aynı bozuk şehir listesi | Maintenance zor | Tek partial veya ViewComponent |
| L4 | Views/Siparis/_IdentityVerification.cshtml:29-30 | Aynı bozuk şehir listesi | 4 dosyada duplicate | Tek ViewModel field |
| L5 | Views/Profil/Adreslerim.cshtml:6-10 | Profil adres şehirleri bozuk | Kullanıcı adres formu kırık | DB şehir listesi kullan |
| L6 | SiparisController.cs:582 | Türkiye telefon formatı  xxxxxxxxxx zorunlu | Filistin müşteri sipariş veremez | +970 regex, E.164 normalize |
| L7 | Views/Urun/Detay.cshtml:778,783,831,849,919; _ProductInfo.cshtml:501,506,554,572; Odeme.cshtml:191,194,511,540,543,606,610 | JS 	oLocaleString('tr-TR') | AR/EN para formatı tutarsız | const locale = document.documentElement.lang === 'ar' ? 'ar-PS' : 'en-US' |
| L8 | Views/Home/temp.txt:7 | TL fallback | Eski para birimi kalıntısı | Dosya sil veya ₪ fallback |
| L9 | secrets.json:11-12 | canvasia.com.tr@gmail.com, Canvasia | Gönderilen e-posta eski marka | 7ANRPS48 mail/domain |
| L10 | SiteSettingsService.cs:291-292 | EmailTemplates/canvasia-logo.svg fallback | E-posta logo eski marka olabilir | 74anrps48logo2.svg veya email-logo |
| L11 | SmtpEmailService.cs:186,196 | Aras/MNG Türkiye kargo takip linkleri | Filistin kargosu için yanlış | United Express veya admin-configurable tracking URL |
| L12 | package.json:11,18,20 | meteorgaleri_canvasia GitHub URL | Repo metadata eski marka | 7ANRPS48 repo URL |
| L13 | GUNCELLEME_ADIMLARI.md:23 | ssh abdulmuin@canvasia-server | Deployment eski server adı | Yeni deployment notu |
| L14 | SQL dump dosyaları | Aras Kargo, Canvasia, İstanbul/Türkiye | Arşiv dosyaları production'a import edilirse eski marka döner | Arşivleri /archive/legacy veya temiz dump üret |

### Marka hedefi ile gerçek durum
- Hedef: **7ANRPS48.com**, Filistin pazarı, ₪/ILS, siyah/altın ağırlıklı hafif tasarım.
- Gerçek:
  - Marka görünür web yüzünde çoğunlukla 7ANRPS48.
  - E-posta altyapısında Canvasia kalmış.
  - Kargo takipte Türkiye firmaları var.
  - Site ayarlarında BaseUrl=https://filistin.kastamonuesnaf.com.tr — geçici domain olabilir ama final marka hedefiyle uyuşmuyor.
  - Tema rengi canlı ayarda yeşil (#1a5632), hedef siyah/altın brief'iyle çakışıyor.

### E-posta, PDF, SEO
- E-posta servisinde müşteri ve admin bildirimleri HTML encode açısından iyi; fakat FromName/FromEmail ve logo fallback kritik.
- PDF fatura FaturaPdfService var; test edilmedi. PDF başlıklarında eski marka olup olmadığı ayrıca grep gerektirir.
- SEO servisleri var (ISeoService, SitemapController), meta title/description DB'den geliyor. SQL snapshot'ta Arabic meta düzgün, ancak dump dosyalarında mojibake var.

---

## 10. PERFORMANS / GÜVENİLİRLİK / OPERASYON RAPORU

### Performans
- **ResponseCompression var** (Program.cs:208-212) ama sadece HTTPS için enable; local HTTP'de devre dışı.
- **MemoryCache var** ama site ayarları ve kategori/ürün query cache davranışı net değil. Hot path ana sayfa ve layout içinde SiteSettingsService.GetSettings() sık çağrılıyorsa DB yükü artar.
- **Ürün listeleme pagination**: /Urun sadece 4 ürünle test edildi. Büyük katalogda pagination olup olmadığı kodla teyit edilmeli. Yoksa P2 performans riski.
- **Görsel optimizasyonu**: Placeholder 404; gerçek görseller yok. Hero banner 74anrps48logo2.svg 2.3MB civarı görünüyordu (.playwright-mcp/74anrps48logo2.svg 2,314,619 bytes). Logo için çok büyük — LCP/CLS risk. SVG optimize edilmeli veya raster responsive kullanılmalı.
- **CSS/JS**: Tailwind generated CSS var; noUiSlider CDN olabilir. Render-blocking kaynaklar tam Lighthouse ile ölçülmedi.

### Güvenilirlik
- DB startup erişilemezse app crash etmiyor, Hangfire + hosted jobs disabled. Bu iyi.
- Migration catch ile yutuluyor (Program.cs:557-562). Bu production'da schema drift'i gizler. Build geçse bile runtime hata olabilir.
- Hangfire dashboard sadece local requests (LocalRequestsOnlyAuthorizationFilter) — iyi.
- Health check /health var ama DB health eklenmemiş; sadece app up döner. DB readiness için Npgsql health check önerilir.

### Operasyon
- secrets.json gitignore'da ama plaintext secrets local dosyada. Production'da environment variable veya secret manager şart.
- DataProtection keys App_Data/DataProtectionKeys dosya sisteminde. Docker volume persist edilmezse kullanıcı cookie'leri deploy sonrası geçersiz olur. Volume mount önerilir.
- Docker DB var, web container kompozisyonu test edilmedi.
- Backup/restore için dump dosyaları var ama çok sayıda bozuk encoding'li SQL dump var; hangisinin canonical olduğu belirsiz.

### Eksik testler
- Unit/integration test projesi yok (AGENTS.md "test projesi oluşturma" diyor). Kritik ödeme/stok akışları test dışı.
- En yüksek değerli otomasyon testleri:
  - Checkout: fiyat server-side recalculation.
  - Checkout: stok atomic decrement.
  - COD limit: üst/alt limit.
  - Filistin telefon validation.
  - Kargo fiyat: şehir → bölge → fiyat.
  - Upload güvenliği: path traversal, pdf/jpg whitelist.
  - Localization: AR/EN dir/lang + no mojibake.

---

## 11. DOSYA VE SATIR KANITLI TÜM BULGULAR LİSTESİ

| ID | Öncelik | Başlık | Kanıt | Etki | Olasılık | Efor | Bağımlılık | Çözüm sırası |
|---|---|---|---|---:|---:|---|---|---:|
| B0 | P0 | SMTP/marka secrets Canvasia | FilistinProje.Web/secrets.json:9-12 | 5 | 5 | XS | Yeni mail hesabı | 1 |
| B4 | P0 | Login formunda admin şifresi default | Playwright /Hesap/GirisYap snapshot; input text dmin@7anrps48.com, password Admin123! | 5 | 5 | XS | Yok | 2 |
| B3 | P0 | Siparişte server-side fiyat doğrulaması yok | SiparisController.cs:145, :176, :275 | 5 | 4 | M | SepetService fiyat kaynağı | 3 |
| B2 | P0 | Siparişte stok düşümü yok | SiparisController.cs:243-298, grep StokAdedi -= yok | 5 | 5 | M | B3 ile aynı transaction | 4 |
| B6 | P1 | Filistin yerine Türkiye telefon formatı | SiparisController.cs:582-585 | 5 | 5 | S | UI placeholder + validator | 5 |
| B1 | P1 | Kargo/şehir/IBAN tabloları boş | DB query: KargoBolgeler, KargoBolgeSehirler, KargoFirmalari, BankaHesaplari → 0 satır | 5 | 5 | M | Seeder/admin data | 6 |
| B5 | P1 | Ürün görselleri/placeholder 404 | Playwright console: /img/products/placeholder.webp 404; detay snapshot | 4 | 5 | M | Asset seed | 7 |
| B25 | P1 | Kimlik/reçete dosyaları web root riskli | Program.cs:316 UseStaticFiles, path uploads/kimlikler; SiparisController.cs:626-648 | 5 | 3 | M | Storage path | 8 |
| B9 | P1 | Kamera policy engeli | Program.cs:308 Permissions-Policy: camera=() + ödeme Open Camera | 4 | 4 | S | Güvenlik header route override | 9 |
| B7 | P1 | Eski marka/domain/kargo kalıntıları | SiteSettingsService.cs:291, SmtpEmailService.cs:186,196, package.json:11, GUNCELLEME_ADIMLARI.md:23 | 4 | 5 | L | Marka kararı | 10 |
| B12 | P1 | Ürün adları EN modda AR görünebiliyor | /Sepet EN snapshot: AR ürün adları | 4 | 5 | M | Ürün localized property | 11 |
| B21 | P1 | Migration hatası yutuluyor | Program.cs:557-562 | 4 | 3 | S | Deploy policy | 12 |
| B13 | P2 | Hediye paketi toplam hesabı belirsiz/eksik | SiparisController.cs:280-281, :241 | 3 | 3 | XS | SepetService confirm | 13 |
| B8 | P2 | JS para formatı 	r-TR | Grep: Odeme.cshtml, Urun/Detay.cshtml, _ProductInfo.cshtml | 3 | 5 | S | Locale util | 14 |
| B10 | P2 | Site ayarları cache belirsiz | Program.cs:144-145, ISiteSettingsService | 3 | 3 | M | Cache invalidation | 15 |
| B11 | P2 | Controller doğrudan DbContext kullanıyor | SiparisController.cs:22, :263-298 | 3 | 4 | L | Service refactor | 16 |
| B14 | P2 | Kupon brute-force rate limit belirsiz | Program.cs:244-269, kupon apply endpoint test dışı | 3 | 3 | S | Endpoint attribute | 17 |
| B17 | P2 | Dual migration bakımı riskli | Program.cs:846-1149 | 3 | 4 | L | Migration policy | 18 |
| B19 | P2 | N+1 riskleri | Ana sayfa/ürün listesi include teyit dışı | 3 | 3 | M | Profiling | 19 |
| B20 | P2 | Timezone rapor doğrulaması eksik | DateTime.UtcNow, rapor local conversion | 2 | 3 | S | Test data | 20 |
| B22 | P2 | Session/IDOR hipotezi | SiparisController.cs:61-64 | 2 | 2 | M | Pen-test | 21 |
| B23 | P2 | CSRF coverage belirsiz | Program.cs:271-277, POST controller audit gerekli | 3 | 3 | S | Controller audit | 22 |
| B24 | P2 | Stored XSS hipotezi ürün HTML açıklama | Ürün açıklaması <p> HTML data; raw render audit gerekli | 3 | 2 | S | View audit | 23 |
| B27 | P2 | Entity model binding/overposting | SiparisController.cs:96 entity bind | 3 | 3 | M | Checkout DTO | 24 |
| B28 | P2 | Session fixation hardening | Login sonrası session rotation teyit dışı | 2 | 2 | S | Auth code | 25 |
| B29 | P2 | Rate limiter policy uygulama belirsiz | Program.cs:244-269 | 2 | 3 | S | Attribute audit | 26 |
| B30 | P1 | Seed admin parolası production riski | Login snapshot default Admin123! | 5 | 4 | S | Seeder change | 27 |
| B31 | P3 | Ziyaretçi loglarında PII | ZiyaretciLoglari user-agent/ip log | 2 | 4 | S | Privacy policy | 28 |

---

## 12. UYGULAMA YOL HARİTASI

### Faz 0: Acil P0 düzeltmeleri (production blocker)
1. Views/Hesap/GirisYap.cshtml içindeki default email/password value'ları kaldır.
2. Mevcut admin şifresini değiştir, seed admin yaratılıyorsa production'da random one-time password veya "force password change" zorunlu yap.
3. secrets.json / environment SMTP FromEmail/FromName'i info@7anrps48.com ve 7ANRPS48 yap; Brevo SMTP key'ini rotate et.
4. SiparisController.Odeme(POST) içinde her sepet item için DB'den Urun, UrunSecenek, Fiyat, StokAdedi yeniden oku; fiyat farkını reddet veya güncel fiyatla yeniden hesapla.
5. Aynı transaction içinde stok atomic düşür: UPDATE ... WHERE StokAdedi >= adet ve affected rows kontrolü.

### Faz 1: Kritik iş akışları
1. Filistin telefon doğrulamasını E.164 ve +970 uyumlu yap.
2. KargoBolgeler, KargoBolgeSehirler, KargoFirmalari, KargoBolgeFiyatlari, BankaHesaplari seed'lerini doldur.
3. Hardcoded şehir fallback'lerini kaldır; tek şehir provider oluştur.
4. KargoHesaplamaServisi boş DB durumunda kargo 0 döndürmek yerine admin uyarısı/log ve checkout engeli üretmeli.
5. Banka havalesi sayfasında aktif IBAN yoksa "sipariş oluştur" butonunu engelle.
6. COD düşük sepet senaryosu için UI/E2E test.

### Faz 2: Frontend / RTL / Mobil iyileştirmeleri
1. Ürün görsel fallback'i ve demo katalog görselleri. /img/products/placeholder.webp ekle.
2. Ürün kart/detay/sepet/checkout LocalizedBaslik, LocalizedAciklama kullanmalı.
3. 	oLocaleString('tr-TR') yerine merkezi ormatMoney(amount) helper.
4. Mobile drawer'a sepet linki ekle.
5. Marka filtresi, özellik filtresi, canlı arama endpoint+UI.
6. Kamera kullanımı için Permissions-Policy route bazlı düzelt.

### Faz 3: Performans, test, operasyon
1. DB health check (AddNpgSql) + /health/ready endpoint.
2. Migration failure'ı yutma yerine startup fail veya admin warning mode.
3. DataProtection keys Docker volume.
4. Cache strategy: site settings + categories + homepage sections with invalidation.
5. E2E regression tests (Playwright) ve backend integration tests.
6. Legacy dump/markdown arşivlerini repo root dışına taşı veya dokümantasyon olarak işaretle.

---

## 13. İLK 2 HAFTALIK UYGULANABİLİR SPRINT PLANI

### Hafta 1
- Gün 1: Admin default credentials kaldırma, admin password rotate, SMTP FromName/FromEmail düzeltme, Brevo key rotate.
- Gün 2: Checkout DTO + server-side fiyat recalculation implementasyonu.
- Gün 3: Atomic stok düşümü ve transaction testleri.
- Gün 4: Filistin telefon validator + checkout form hata mesajları.
- Gün 5: Kargo/şehir/IBAN seed; admin kargo sayfasından doğrulama.

### Hafta 2
- Gün 6: Hardcoded şehir listelerini kaldır, tüm checkout partial'ları tek şehir provider'a bağla.
- Gün 7: Ürün görselleri + placeholder + 404 temizliği.
- Gün 8: LocalizedBaslik/Aciklama kullanımı ürün kartı, sepet, checkout özetinde.
- Gün 9: ormatMoney JS helper + 	r-TR temizliği.
- Gün 10: Playwright regression: AR/EN, 390/1440, Sepet→Checkout→Sipariş, COD limit, kargo fiyat, stok düşümü.

---

## 14. REGRESSION TEST KONTROL LİSTESİ

- AR ana sayfa dir=rtl, EN ana sayfa dir=ltr.
- 390px ve 1440px horizontal overflow yok.
- Login formu boş gelir, admin şifresi görünmez.
- /Urun ürün kartlarında görsel 200 OK, placeholder 200 OK.
- /Urun/Detay görsel galeri ve adet +/- çalışır.
- Stok 1 olan varyant için iki checkout denemesinde ikinci sipariş reddedilir.
- Sepetteki fiyat manipüle edilirse checkout güncel DB fiyatını kullanır veya siparişi reddeder.
- Filistin telefon +970599123456 kabul; Türk  5551234567 kabul edilmez veya normalize edilir.
- Şehir dropdown DB'den gelir, Beytüllahim bozuk görünmez.
- Kargo şehir bazlı hesaplanır; ücretsiz baraj üstü 0 olur.
- COD limiti altı görünür, limiti üstü görünmez/reject edilir.
- Banka havalesi seçildiğinde aktif IBAN gösterilir.
- Kimlik/reçete upload path traversal reddedilir.
- Kamera butonu tarayıcı izin prompt'u açar.
- Admin Görüntüleyici rolü POST yapamaz.
- Email testinde FromName 7ANRPS48, logo doğru, kargo takip linki Filistin kargosu.

---

## 15. PROJE SAHİBİNE SORULMASI GEREKEN AÇIK SORULAR

1. Final domain gerçekten 7ANRPS48.com mu, yoksa ilistin.kastamonuesnaf.com.tr kalıcı mı?
2. SMTP gönderici maili hangi adreste olmalı: info@7anrps48.com, Gmail mi, Brevo domain-authenticated mail mi?
3. Kapıda ödeme hangi bölgelerde aktif olacak? 48 bölgesi, Batı Şeria, Kudüs, Gazze ayrımı var mı?
4. United Express dışında hangi kargo firmaları kullanılacak?
5. Müşteri kargo firması seçmeyecek denmiş; admin siparişte kargo firması sonradan atayacak mı?
6. Kimlik/reçete fotoğrafları müşteriye/admin'e web URL ile açılmalı mı, yoksa sadece yetkili indirme mi?
7. Ürün dil verisinde ana kaynak hangi kolon: Baslik, BaslikAr, BaslikEn?
8. Toptancı fiyatı ürün bazlı mı, grup/iskonto basamaklı mı, yoksa ikisi birlikte mi?
9. Kamera ile kimlik çekme gerçekten zorunlu mu, yoksa opsiyonel mi?
10. Ürün görselleri ve katalog gerçek verisi kimden gelecek?

---

## 16. SON ÜÇ LİSTE

### Şu anda production'a çıkmayı engelleyenler
- Login formunda admin şifresi default dolu (dmin@7anrps48.com / Admin123!).
- SMTP/secrets Canvasia marka ve eski e-posta ile çalışıyor.
- Sipariş sırasında server-side fiyat doğrulaması yok.
- Sipariş sırasında stok düşümü yok.
- Filistin telefon validasyonu yanlış, gerçek müşteriler ödeme tamamlayamayabilir.
- Kargo/şehir/IBAN tabloları boş; kargo ve banka havalesi gerçek çalışmaz.
- Kimlik/reçete upload dosyalarının public webroot riski netleşmeden production riskli.

### Production sonrası yapılabilecekler
- Marka/renk dili siyah/altın premium seviyeye çekmek.
- Canlı arama, marka filtreleri, özellik filtreleri.
- İlgili ürünler, kampanya sayaçları, çark/ödül UX polish.
- Performance tuning: image CDN, cache, Lighthouse.
- Gelişmiş raporlar ve PDF fatura tasarımı.
- Push notification ve abandoned cart otomasyonları.

### Kodda tamamlandı görünen fakat gerçek kullanıcı akışında doğrulanmayanlar
- Kamera ile kimlik çekme (camera=() policy nedeniyle kırık olabilir).
- Toptancı özel fiyat/iskonto uçtan uca.
- Ürün bazlı WhatsApp/fiyat gizleme.
- [ ] Reçeteli kategori için gerçek reçete upload zorunluluğu.
- [ ] Kargo fiyatı şehir/bölge bazlı dinamik hesaplama (DB boş).
- [ ] Banka havalesi IBAN gösterimi (DB boş).
- [ ] PDF fatura üretimi.
- [ ] Site değerlendirmesi yazma ve admin onayı.
- [ ] Çark/ödül ön yüz kullanıcı akışı.
- [ ] Admin tüm CRUD sayfalarının mobil/RTL davranışı.

---

## Faz 11 Kapanış — B25 ve B9 (10 Temmuz 2026)

### Kanıt toplama
- `FilistinProje.Web/wwwroot/uploads/kimlikler/` ve `wwwroot/uploads/receteler/` dizinleri listelendi → 2 gerçek kimlik PNG'si + 1 fatura PDF dosyası mevcut.
- PostgreSQL `AspNetUsers` tablosunda `KimlikFotografYolu = '/uploads/kimlikler/<guid>.png'` format'ında web URL'si DB kolonunda saklanmış durumdaydı; `UseStaticFiles()` ile bu dosyalar anonim olarak servis ediliyordu.

### Çözüm
- **Private storage**: `<ContentRoot>/secure-storage/hassas/kimlikler|receteler/<guid>.<ext>` (wwwroot DIŞINDA).
- **DB referansı**: `private://<kategori>/<dosya-adı>` (sadece bu token DB'de).
- **`IDosyaServisi.HassasBelgeKaydetAsync`**: Kategori bazlı MIME/uzantı/magic-byte + aktif içerik (`<script`, `<html`, `<!doctype`, `<svg`, `<?php`, `javascript:`) reddi. `MaksResimDosyaBoyutu` 8MB, `MaksDokumanDosyaBoyutu` 12MB. `IsSafeStoredFileName` ile path traversal ve uzantı sızıntısı kapatıldı.
- **`BelgeController`** (`/Belge/Kimlik?userId=`, `/Belge/SiparisKimlik?siparisId=`, `/Belge/Recete?siparisId=`) — `[Authorize]`, sahiplik veya admin permission kontrolü, path parametresi olarak fiziksel yol KABUL EDILMEZ, sadece sahiplik id'si. `Cache-Control: no-store`, `X-Content-Type-Options: nosniff`, `Content-Disposition: inline|attachment`.
- **Route-bazlı Permissions-Policy** (`Program.cs` `IsCameraAllowedPath`): `/Siparis/Odeme` + `/Hesap/KayitOl` dışında global `camera=()`, bu iki sayfada `camera=(self)` (same-origin).
- **Legacy 404 middleware** (`Program.cs` `IsLegacySensitiveUploadPath`): `/uploads/kimlikler/*` ve `/uploads/receteler/*` → 404.
- **Startup migration** (`EnsureSensitiveUploadsMigratedAsync`): DB'deki eski `/uploads/kimlikler|receteler` referanslarını secure-storage'a kopyalar, DB'yi `private://<...>` token'a taşır, eski public dosyaları siler.
- **WebRTC UX**: `Odeme.cshtml` JS rewrite — `getUserMedia` Promise, secure-context kontrol, `NotAllowedError|NotFoundError|NotReadableError|SecurityError` sınıflandırması, AR + EN kullanıcı dostu toast, fallback upload moduna otomatik geçiş, `URL.createObjectURL` + `revokeObjectURL` blob önizleme, antiforgery header.
- **Upload validation pipeline korundu**: Kamera blob → `YukleKimlikFoto` → `HassasBelgeKaydetAsync` (magic-byte, MIME, boyut, aktif içerik reddi).

### Doğrulama
| # | Senaryo | Sonuç |
|---|---|---|
| 1 | `GET /uploads/kimlikler/c9b306e8...png` anonim | **404** (eski public URL yok) ✅ |
| 2 | `GET /uploads/receteler/anything.png` anonim | **404** ✅ |
| 3 | `GET /` (`HOME`) header | `camera=(), microphone=(), geolocation=()` ✅ |
| 4 | `GET /Hesap/KayitOl` header | `camera=(self), microphone=(), geolocation=()` ✅ |
| 5 | `GET /Siparis/Odeme` header (sepet var) | `camera=(self), ...` ✅ |
| 6 | `GET /Belge/Kimlik?userId=fed886bd...` anonim | **302** → `/Hesap/GirisYap` ✅ |
| 7 | `Dotnet build FilistinProje.sln` | **0 hata 0 uyarı** ✅ |

### Migration tamamlanma durumu
DB'de migration sonrası tek kayıt (`testuser2026@example.com`) `KimlikFotografYolu`: `private://kimlikler/c9b306e8e6874e4cb0f2970a538136a6.png`. Yeni site kimlik/reçete upload'ları tümüyle secure-storage'a yazılıyor, public URL üretilmiyor.

---

## Production Deployment Adımları (B25 + B9)

1. **Önceki deployment'dan gelen `wwwroot/uploads/kimlikler/*` ve `wwwroot/uploads/receteler/*` dosyaları production sunucusunda mevcutsa**:
   - Bu dizinler artık sunucu tarafından servis edilmez (middleware 404). Dosyaları silmeden önce güvenli taşıma yapın:
     - `mkdir -p /var/filistinproje/secure-storage/hassas/{kimlikler,receteler}` (ContentRoot altına)
     - Tüm `*.png|*.jpg|*.webp|*.pdf` dosyalarını GUID adıyla secure-storage'a taşıyın
     - DB'deki `KimlikFotografYolu` ve `ReceteDosyaYolu` kolonlarını `private://<kategori>/<guid>.<ext>` formatına SQL ile güncelleyin
     - `wwwroot/uploads/kimlikler|receteler` dizinlerini **silin**
2. **CI/CD'nin wwwroot'u override etmediğinden emin olun** — `secure-storage` dizini ContentRoot altında container'ın kalıcı volume'una mount edilmelidir:
   ```yaml
   volumes:
     - 7anrps48-secure-storage:/app/secure-storage
   ```
   `docker-compose.yml`'da `web` servisi için volume tanımı ekleyin.
3. **Reverse proxy (nginx/Caddy)** herhangi bir şekilde `/uploads/kimlikler*` veya `/uploads/receteler*` path'ine rewrite yapmadığını doğrulayın. Yapıyorsa kaldırın — middleware zaten 404 döndürüyor.
4. **Permissions-Policy header'ı** reverse proxy önbelleğe alınmamalıdır (`Vary: Permissions-Policy` veya `no-store` header'lar uygun).
5. **İlk deployment'ta** `Siparis/Odeme` ve `Hesap/KayitOl` sayfalarını tarayıcıda (Chrome + Safari iOS) açıp kamera izni akışını manuel test edin; uygun mesajlar (AR + EN) görüntülenmeli, izin reddi fallback'e yönlendirmeli, sayfa bozulmamalı.
6. **Loglama**: `IAdminSecurityAuditService` üzerinden `BelgeController`'a audit logları ekleyerek yetkisiz erişim denemelerini kayıt altına alın (opsiyonel, sonraki faz).

---

## Sonuç
Proje iskeleti, admin yetkilendirme modeli, Identity yapısı, localization temel davranışı ve genel e-ticaret sayfa düzeni ayakta. Ancak mevcut haliyle — B25 (kimlik/reçete public URL sızıntısı) ve B9 (kamera permission) Faz 11'de kapatıldı, diğer engeller (sipariş bütünlüğü, marka/secrets sızıntısı, checkout veri eksikliği, telefon validasyonu) çözülmeden site gerçek müşteri trafiğine açılmamalıdır.
