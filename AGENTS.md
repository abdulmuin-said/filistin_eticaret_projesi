# AGENTS.md — 7ANRPS48.com (Filistin e-ticaret)

## Project overview

- **Stack**: ASP.NET Core 10.0 MVC + PostgreSQL (Docker) + TailwindCSS
- **Architecture**: Clean Architecture (4 projeler: Web → Service → Data → Core)
- **Brand**: 7ANRPS48.com (Filistin e-ticaret, eski Canvasia/MeteorGaleri fork)
- **Para birimi**: ₪ (ILS - Yeni İsrail Şekeli)
- **Diller**: Arapça (varsayılan, RTL), İngilizce (LTR) — `IStringLocalizer<SharedResource>` kullanılır
- **Entity property isimleri**: Türkçe PascalCase (`Urun.Baslik`, `Kategori.Ad`, `SiteAyarlari.GirisZorunluMu`)
- **Kimlik doğrulama**: ASP.NET Core Identity (`AppUser : IdentityUser`)

## Quick start

```bash
docker-compose up -d db
cd FilistinProje.Web && dotnet watch run   # http://localhost:5002

# Full Docker
docker-compose build --no-cache && docker-compose up -d   # http://localhost:8080
```

## Critical gotchas

1. **DB unreachable at startup** → App doesn't crash, Hangfire + AbandonedCartService disabled. Check logs.

2. **Dual migration system** → EF migrations + `EnsureMissingMarch2026SchemaAsync` (hand-rolled SQL block, Program.cs ~satır 750-935). Yeni entity property'si eklenince **her ikisine de eklenmeli**.

3. **Turkish PascalCase + quotes** → Tüm raw SQL çift tırnak: `"Urunler"`, `"Kategoriler"`, `"AspNetUsers"`.

4. **Property name traps**:
   - `Urun.Baslik` — NOT "Ad" or "Name"
   - `UrunResim.ResimYolu` — NOT "Url" or "ImageUrl"
   - `Urun.EtkinFiyat`, `Urun.IndirimVarMi`, `Urun.SilindiMi`

5. **secrets.json** — DB connection string, gitignored, required for local dev.

6. **Rate limiter** — "auth" (10/5min IP), "general" (100/min IP).

7. **Maintenance mode** — `ISiteSecurityService` middleware blocks non-admin traffic.

8. **AdminBaseController** — `Admin` area'daki tüm controller'lar bundan türetilir. Yetkilendirme için `AdminPermissionMatrix` + `AdminSecurityRoles` kullanılır. Yeni admin controller eklerken buraya permission eklemeyi unutma.

9. **ViewBag bazlı yetkilendirme** — Admin layout'da `canManageUsers`, `canManageWholesale` vb. ViewBag değişkenleri `AdminBaseController.OnActionExecutionAsync` içinde set edilir. Yeni bir yetki eklersen burayı da güncelle.

10. **Admin layout sidebar** — `_AdminLayout.cshtml` satır ~77-170. Yeni bir admin sayfası eklerken navigasyon linkini buraya ekle.

## Commands

```bash
dotnet build FilistinProje.sln
dotnet ef migrations add <Name> --project FilistinProje.Data --startup-project FilistinProje.Web
dotnet ef database update --project FilistinProje.Data --startup-project FilistinProje.Web
cd FilistinProje.Web && npm run watch:storefront-css
```

## Key URLs

- Public: `/Urun`, `/Urun/Detay/{slug}-{id}`, `/Sepet`, `/Siparis/Odeme`, `/Hesap/GirisYap`, `/Profil`
- Admin: `/Admin/Home`, `/Admin/Toptanci`, `/Admin/Kullanici`, `/Admin/Ayarlar`
- Hangfire: `/admin/hangfire` (local only)

## DB conventions

- Tables/columns use **Turkish PascalCase** with double quotes in raw SQL
- Connection: `Host=localhost;Port=5434;Database=filistindb;Username=kanvasuser;Password=changeme_in_production`

## What NOT to do

- Test projesi oluşturma (yok)
- Linux shell komutları kullanma (PowerShell 5.1)
- secrets.json veya gerçek kimlik bilgilerini commit etme
- `cd <dir> && <cmd>` yapma, `workdir` parametresi kullan

## Session state (Faz 8 / Tüm admin controller'lar tamamlandı)

### Faz 4 (Üyelik, Profil ve Toptancı Onay Mekanizması)
- [x] **Adım 31**: Kayıt form alanları (KimlikNo, DogumTarihi, Telefon, Adres)
- [x] **Adım 32**: Kimlik resmi yükleme alanı (IFormFile, multipart/form-data)
- [x] **Adım 33**: Dosya servisi (`IDosyaServisi`/`DosyaServisi` - guid ile kaydetme, validasyon)
- [x] **Adım 34**: Kimlik fotoğrafı zorunluluk uyarıları
- [x] **Adım 35**: Giriş zorunluluk ayarı (`GirisZorunluMu` toggle + middleware)
- [x] **Adım 36**: Profilde kimlik bilgileri görüntüleme
- [x] **Adım 37**: Sipariş takibi ve iptali (filtre, iptal butonu)
- [x] **Adım 38**: Wholesale rolü + status + admin onay paneli (`/Admin/Toptanci`)
- [x] **Adım 39**: Toptancı kayıt formu ve admin onay mekanizması
- [x] **Adım 40**: Şifre sıfırlama ve hesap doğrulama e-postaları (3 dil)

### Faz 5 (Ürün Varyasyonları ve Stok Takip Geliştirmeleri)
- [x] **Adım 41**: Ürün detay sayfasında renk/boyut seçim alanları (dinamik varyasyon)
- [x] **Adım 42**: Seçilen varyasyona göre fiyat dinamik güncelleme (JS)
- [x] **Adım 43**: Stok 5'in altında "Son X Adet" uyarısı
- [x] **Adım 44**: Yıldızlı puanlama ve yorum sistemi
- [x] **Adım 45**: Site değerlendirme sistemi
- [x] **Adım 46**: Stoğu biten varyasyonların gray-out + admin panel ayarı (`StokBiteniGriGoster`)
- [x] **Adım 47**: Ürün detayında hediye paketi + özel not "Ekstra Hizmet" seçeneği (`HediyePaketiVarMi`, `HediyePaketFiyati`)
- [x] **Adım 48**: Akıllı Fiyat Aralığı Filtresi (noUiSlider) — mevcut input'lar slider ile değiştirildi, CDN CSS/JS, temayla uyumlu stil
- [x] **Adım 49**: Marka + özellik/nitelik filtre sistemi — sidebar'a marka listesi ve dinamik özellik filtreleri eklendi, `BuildOzellikFilterUrl` helper
- [x] **Adım 50**: Stokta biten ürünler "Tükendi" rozetiyle gösterilir, satın alma kapatılır — `Index.cshtml`'de `!StoktaVarMi` kontrolü ile gri rozet, `StoktaYokSatisIzni` ayarına saygılı

### Faz 6 (Gelişmiş Sepet ve Checkout Sistemi)
- [x] **Adım 51**: Sepet sayfasında RTL uyumlu sınıflar ve tek tıkla sepet boşaltma butonu
- [x] **Adım 52**: Yüzen Sepet simgesi (`_Layout.cshtml`) — sol alt köşede sabit, gold adet sayacı
- [x] **Adım 53**: Ödeme sayfasında teslimat tipi seçeneği (Adrese Teslim / Mağazadan Teslim)
- [x] **Adım 54**: Bölge ve şehre göre dinamik kargo ücreti hesaplama motoru (`KargoBolge`, `KargoBolgeSehir`, `KargoBolgeFiyat`)
- [x] **Adım 55**: Ücretsiz kargo barajı bildirimi
- [x] **Adım 56**: WebRTC Kamera ile Kimlik Fotoğrafı Çek / Yükle
- [x] **Adım 57**: Sipariş notu ekleme kutusu
- [x] **Adım 58**: Kullanım şartları onay kutusu zorunluluğu
- [x] **Adım 59**: Banka Havalesi IBAN Yönetimi (`BankaHesap` entity, `BankalarController`)
- [x] **Adım 60**: Kapıda ödeme hizmet bedeli (`KapidaOdemeAktifMi`, `KapidaOdemeHizmetBedeli`)
- [x] **Adım 61**: 2000 ILS üzeri siparişlerde kapıda ödeme limiti (`KapidaOdemeLimiti`)

### Faz 7 (Admin Panel İyileştirmeleri ve Lokalizasyon Tamamlama)
- [x] **Adım 62**: Siparis/Index.cshtml bugfix (Admin_PageInfo, Admin_ShippingLabel)
- [x] **Adım 63**: Admin panel mobil iyileştirmesi (hamburger + slide-out drawer)
- [x] **Adım 64**: Search/Index.cshtml lokalizasyonu
- [x] **Adım 65**: UrunImport/Index.cshtml lokalizasyonu

### Faz 8 (Admin Controller'lar ve Özellik Ekleme — önceki session'larda AGENTS.md güncellenmeden yapıldı)
- [x] **Adım 66**: Kategori özellikleri — `ReceteGerekliMi` alanı (Kategoriler)
- [x] **Adım 67**: WhatsApp Sipariş + Fiyat Gizleme — `WhatsappSiparisVarMi`, `FiyatGizliMi` (Urunler)
- [x] **Adım 68**: Toptancı minimum sipariş tutarı — `ToptanciMinSiparisTutari` (SiteAyarlari)
- [x] **Adım 69**: Toptancı Ürün Grupları + İskonto Sistemi — `ToptanciUrunGrubu`, `ToptanciIskontoOrani` entity'leri, `Urun.ToptanciUrunGrubuId`
- [x] **Adım 70**: Başvuru Tarihi takibi — `AppUser.BasvuruTarihi`
- [x] **Adım 71**: Filistin kargo bölgeleri — `KargoBolge.Ulke`, `KargoBolge.Aciklama`
- [x] **Adım 72**: Slider çoklu dil desteği — `Slayt.BaslikEn/Ar`, `AltBaslikEn/Ar`, `AciklamaEn/Ar`
- [x] **Adım 73**: Admin Kargo Yönetimi — `KargoController` + `KargoFirmasi` entity
- [x] **Adım 74**: Admin Slider Yönetimi — `SlaytController` + CRUD view'lar
- [x] **Adım 75**: Admin Ürün Özellik Tanımları — `UrunOzellikController` + `UrunOzellikTanimi`/`UrunOzellikDegeri` entity'leri
- [x] **Adım 76**: Admin Raporlar — `RaporController`
- [x] **Adım 77**: Admin İletişim Mesajları — `IletisimController`
- [x] **Adım 78**: Admin İade Talepleri — `IadeController` + `IadeTalebi` entity
- [x] **Adım 79**: Admin Kupon Yönetimi — `KuponController` + `Kupon` entity
- [x] **Adım 80**: Admin Home Sections — `HomeSectionsController` + `HomePageSection` entity
- [x] **Adım 81**: Admin Bülten Aboneleri — `BultenController`
- [x] **Adım 82**: Admin Kurumsal Sayfalar — `SayfaController` + `KurumsalSayfa` entity
- [x] **Adım 83**: Admin Toplu Fiyat Güncelleme — `TopluFiyatGuncelleController`
- [x] **Adım 84**: Admin Slug Tool — `SlugToolController`
- [x] **Adım 85**: Admin Yorum Yönetimi — `YorumController`
- [x] **Adım 86**: Admin Ziyaretçi Log — `ZiyaretciController` + `ZiyaretciLog` entity
- [x] **Adım 87**: Admin XyzSecretMonitor — `XyzSecretMonitorController`
- [x] **Adım 88**: Admin Toptancı Yönetimi Detayı — `ToptanciController` tam CRUD, WholesaleStatus onay/red
- [x] **Adım 89**: Admin AnaSayfa Yönetimi — `AnaSayfaController`

### Migration'lar (tümü — 17 adet)
- `20260613194853_InitialCreate` — İlk veritabanı oluşturma
- `20260613195915_AddWholesalePrice` — WholesalePrice (Urunler)
- `20260613202928_AddCustomerIdentityFields` — KimlikNo, DogumTarihi, KimlikFotografYolu (AspNetUsers)
- `20260613203317_AddOrderDeliveryAndPrescriptionFields` — Reçete/teslimat alanları (Siparisler)
- `20260613221038_AddMultiLanguageFields` — Çoklu dil alanları (Urunler)
- `20260614073624_AddUserAddressField` — Adres (AspNetUsers)
- `20260614075244_AddLoginRequiredSetting` — GirisZorunluMu (SiteAyarlari)
- `20260614080356_AddWholesaleStatus` — WholesaleStatus (AspNetUsers)
- `20260614085324_AddSiteDegerlendirme` — SiteDegerlendirmeleri tablosu
- `20260614092149_AddStockOutGrayDisplay` — StokBiteniGriGoster (SiteAyarlari)
- `20260614092652_AddGiftWrapFields` — HediyePaketi alanları
- `20260614101244_AddKargoBolgeSistemi` — KargoBolge, KargoBolgeSehir, KargoBolgeFiyat tabloları
- `20260614102323_AddSiparisKimlikFoto` — KimlikFotoYolu (Siparisler)
- `20260614103327_AddBankaHesaplari` — BankaHesaplari tablosu
- `20260614222407_AddKapidaOdemeBedeli` — KapidaOdemeAktifMi, KapidaOdemeHizmetBedeli (SiteAyarlari) + OdemeYontemi, KapidaOdemeHizmetBedeli (Siparisler)
- `20260614223525_AddKapidaOdemeLimit` — KapidaOdemeLimiti (SiteAyarlari)
- `20260614225236_AddReceteGerekliKategori` — ReceteGerekliMi (Kategoriler)
- `20260615140117_AddWhatsappSiparisFields` — FiyatGizliMi, WhatsappSiparisVarMi (Urunler)
- `20260615212141_AddToptanciMinSiparisTutari` — ToptanciMinSiparisTutari (SiteAyarlari)
- `20260615221301_AddToptanciUrunGrubu` — ToptanciUrunGrubuId (Urunler), ToptanciUrunGruplari, ToptanciIskontoOranlari tabloları
- `20260616122634_AddBasvuruTarihi` — BasvuruTarihi (AspNetUsers)
- `20260616124117_AddFilistinKargoBolgeleri` — Ulke, Aciklama (KargoBolgeler)
- `20260619122952_AddSliderMultilingual` — BaslikEn/Ar, AltBaslikEn/Ar, AciklamaEn/Ar (Slaytlar)

### Admin Controller'ları (27 adet)
| Controller | View'lar | Özellik |
|-----------|---------|---------|
| `AdminBaseController` | — | Base class, permission matrix, ViewBag yetkilendirme |
| `HomeController` | Index | Dashboard |
| `AnaSayfaController` | Index | Ana sayfa yönetimi |
| `UrunController` | Index, Ekle, Duzenle | Ürün CRUD |
| `KategoriController` | Index | Kategori yönetimi |
| `UrunOzellikController` | Index, Ekle | Ürün özellik tanımları |
| `UrunImportController` | Index | Toplu ürün import (JSON) |
| `TopluFiyatGuncelleController` | Index | Toplu fiyat güncelleme |
| `SlugToolController` | Index | Slug düzeltme aracı |
| `SiparisController` | Index, Detay | Sipariş yönetimi |
| `KullaniciController` | Index | Kullanıcı yönetimi |
| `ToptanciController` | Index | Toptancı başvuru/onay yönetimi |
| `BankalarController` | Index | IBAN yönetimi |
| `KargoController` | Index | Kargo firmaları |
| `AyarlarController` | Index | Site ayarları |
| `SlaytController` | Index, Ekle | Slider yönetimi |
| `RaporController` | Index | Raporlar |
| `IletisimController` | Index | İletişim mesajları |
| `IadeController` | Index | İade talepleri |
| `KuponController` | Index | Kupon yönetimi |
| `HomeSectionsController` | Index | Ana sayfa bölümleri |
| `BultenController` | Index | Bülten aboneleri |
| `SayfaController` | Index | Kurumsal sayfalar |
| `SearchController` | Index | Admin arama |
| `YorumController` | Index | Ürün yorumları |
| `ZiyaretciController` | Index | Ziyaretçi logları |
| `XyzSecretMonitorController` | Index | Secret monitor |

### Faz 9 (Marka Temizliği & Proje Lokalizasyonu — 23 Haziran 2026)
- [x] **Adım 90**: AGENTS.md güncellendi — 7 kayıp migration + 23 admin controller eklendi, Faz 8 tamamlandı
- [x] **Adım 91**: Türkiye şehirleri → Filistin şehirleri (Odeme.cshtml, Adreslerim.cshtml)
- [x] **Adım 92**: Admin Kullanici/Duzenle.cshtml placeholder "İstanbul" → "Ramallah"
- [x] **Adım 93**: Tüm "Canvasia" referansları .cs dosyalarından temizlendi (27 dosya, 50+ referans)
  - SiteSettingsService: fallback ad, URL, meta keywords, logo yolları, e-posta
  - Program.cs: ApplicationName, log dosya adı, SQL default değerleri
  - Tüm Controller'lar: EPPlus license, mail şablonları, rapor başlıkları
  - DbSeeder, KargoFirmasi entity, HomePageSettingsService slider metinleri
  - SmtpEmailService: logo ContentId
- [x] **Adım 94**: UrunController.cs encoding bozulması düzeltildi (Türkçe char map)

### Faz 10 (Türkçe Dil Desteğinin Kaldırılması & Lokalizasyon İyileştirmeleri — 7 Temmuz 2026)
- [x] **Adım 95**: `generate_resx.py`'daki Turkey/Canvasia referansları temizlendi (AllOverTurkey, TurkeyWideShipping, HomeSeoTitle)
- [x] **Adım 96**: `RaporController.cs` — `ToTurkeyLocal` → `ToPalestineLocal` (8 yerde)
- [x] **Adım 97**: `Admin/Siparis/Index.cshtml` — `GetTurkeyTimeZone`/`FormatTurkeyDateTime` → Palestine
- [x] **Adım 98**: **Türkçe dil desteği tamamen kaldırıldı**:
  - Program.cs fallback "tr" → "en"
  - DilController.cs'den "tr" çıkarıldı
  - `SharedResource.tr.resx` silindi
  - `_AdminLayout.cshtml` TR dropdown kaldırıldı
  - `Slayt.cs` `GetLocalized` TR fallback → EN/AR fallback
  - `Slayt/Ekle.cshtml`, `Slayt/Duzenle.cshtml` TR kolonları kaldırıldı
  - Email servisleri: default culture "tr" → "ar", varsayılan şablon `Sablon.en.html`
  - `ProfilController.cs` `CultureInfo("tr-TR")` → `CultureInfo.InvariantCulture`
  - `Admin/UrunController.cs` Excel import "tr-TR" → `CultureInfo.InvariantCulture` (3 yerde)
  - `generate_resx.py` tr_values dict + loop'tan tr entry kaldırıldı
- [x] **Adım 99**: IBANPlaceholder TR00 → PS00, PhonePlaceholder +90 → +970
- [x] **Adım 100**: Session.Id null-safety düzeltildi (`ISessionFeature?.Session?.Id`)
- [x] **Adım 101**: Eksik localizer key'leri eklendi (SpinWheelManagement, BankTransferPayment), `Admin_HeroSubtitle_Tr` temizlendi

### Faz 11 (Hassas Belge Güvenliği + Kamera WebRTC — 9 Temmuz 2026)
- [x] **Adım 102**: B25 kanıtı: `wwwroot/uploads/kimlikler/<guid>.png` dosyaları herkese açık anonim olarak servis ediliyordu; `AppUser.KimlikFotografYolu` DB kolonu `/uploads/kimlikler/...` web URL format'ında tutulmuştu.
- [x] **Adım 103**: Private storage mimarisi: `ContentRoot/secure-storage/hassas/{kategori}/<guid>.{ext}` (wwwroot dışı). `wwwroot/uploads/kimlikler|receteler` URL'leri gizli middleware ile kontrollü 404.
- [x] **Adım 104**: `IDosyaServisi.HassasBelgeKaydetAsync(IFormFile, HassasBelgeKategorisi)` (DosyaServisi.cs). Kategori bazlı MIME/uzantı/magic-byte + aktif içerik (`<script`, `<html`, `<!doctype`, `<svg`, `<?php`, `javascript:`) reddi. `MaksResimDosyaBoyutu` 8MB, `MaksDokumanDosyaBoyutu` 12MB. `BuildPrivateReference` → `private://kimlikler|receteler/<guid><.ext>`. `TryParsePrivateReference` ve `IsSafeStoredFileName` ile path injection/uzantı sızıntısı kapatıldı.
- [x] **Adım 105**: `FilistinProje.Web/Controllers/BelgeController.cs` (3 endpoint):
  - `GET /Belge/Kimlik?userId={guid}` → owner VEYA `Kullanici|Siparis|Toptanci` admin permission. Path değil, kullanıcı id kullanılıyor.
  - `GET /Belge/SiparisKimlik?siparisId={id}` → sipariş sahibi VEYA `Siparis` admin permission.
  - `GET /Belge/Recete?siparisId={id}` → sipariş sahibi VEYA `Siparis` admin permission.
  - `Cache-Control: no-store, max-age=0`, `X-Content-Type-Options: nosniff`, `Content-Disposition` (inline/attachment) güvenli header'lar.
  - Legacy eski public referans DB'de kaldıysa kontrollü 404 (dosya yoksa da 404, dosya varsa redirect etmeden okur).
- [x] **Adım 106**: `Program.cs` — global `Permissions-Policy: camera=()` kaldırıldı, `IsCameraAllowedPath` route-bazlı `camera=(self)` (`/Siparis/Odeme` ve `/Hesap/KayitOl`). Diğer tüm sayfalar `microphone=(), geolocation=()` ile kapatıldı.
- [x] **Adım 107**: `Views/Siparis/Odeme.cshtml` JS rewrite: `getUserMedia` Promise, secure-context kontrol (`isSecureContext` + localhost), hata sınıflandırma (`NotAllowedError`, `NotFoundError`, `NotReadableError`, `AbortError`, `SecurityError`) → AR + EN kullanıcı dostu toast; fallback upload moduna otomatik geçiş; canvas.toBlob null kontrolü; blob URL önizleme (`createObjectURL` + `revokeObjectURL`); antiforgery header eklendi.
- [x] **Adım 108**: `Views/Siparis/_IdentityVerification.cshtml` — yeni AR/EN kimlik doğrulama mesaj kutusu + fallback upload CTA. `Views/Profil/Index.cshtml` kimlik fotoğrafı → `/Belge/Kimlik?userId=...` URL'sine çevrildi (anonim public URL kaldırıldı). `Areas/Admin/Views/Siparis/Detay.cshtml` reçete önizleme/görsel linki `BelgeController` üzerinden. `SiparisController.cs` `IsSafeUploadedPath` artık `private://` referansını da doğruluyor (geriye dönük uyumluluk korundu).
- [x] **Adım 109**: `Program.cs` startup'ta `EnsureSensitiveUploadsMigratedAsync` — eski `/uploads/kimlikler|receteler` DB kayıtları `private://` referansa taşınır ve dosya secure-storage'a kopyalanır; eski public dosyalar migration sonrası silinir. Migration sonrası eski URL artık 404 döner (eski URL'leri zaten middleware blokajlı).
- [x] **Adım 110**: `WebRTC blob → upload validation hattı`: canvas.toBlob → xhr → `YukleKimlikFoto` → aynı `IDosyaServisi.HassasBelgeKaydetAsync` (magic-byte, MIME, boyut, aktif içerik). Upload validation bypass yapılmadı.
### Faz 12 (Yönetim Paneli Sipariş & Kargo Hata Düzeltmeleri — 3 Eylül 2026)
- [x] **Adım 112**: `Admin/Siparis` içerik sütunundaki çift UTF-8 bozulması ve Türkçe metin ("1 Ã¼rÃ¼n / 1 adet") giderildi; resx'e `Admin_OrderSummaryContent` eklendi, Arapça ("1 منتج / 1 قطعة") ve İngilizce dinamik formatlama yapıldı.
- [x] **Adım 113**: Tablo içi 3 nokta hızlı işlem dropdown menülerinin `.ca-table-wrapper` içinde kırpılması/arkaplanda kalması sorunu çözüldü (`data-bs-popper-config='{"strategy":"fixed"}'`, `admin.js` dropdown popperConfig, `admin.css` `.ca-dropdown-menu` `z-index: 1065 !important`).
- [x] **Adım 114**: Dropdown menüsündeki "ملصق الشحن" (Kargo Etiketi) linki doğrudan `SiparisController.EtiketYazdir(id)` action'ına bağlandı, `target="_blank"` ile tek tıkla yeni sekmede kargo etiketi yazdırma şablonu açılması sağlandı.
- [x] **Adım 115**: `Admin/Siparis/Detay/{id}` başlığındaki Türkçe durum ("Sipariş Alındı") `GetStatusLabel(Model.Durum)` çok dilli fonksiyonu ile "الحالة: تم استلام الطلب" yapıldı; tarayıcının yerel OS dilinde görünen native fatura dosya seçicisi yerine çok dilli buton ve etiket ("اختيار ملف" / "لم يتم اختيار ملف") eklendi.
- [x] **Adım 116**: Resx dosyalarına eksik olan `Admin_Variation` anahtarı eklendi (Arapça: `المتغير`, İngilizce: `Variation`); sipariş açıklamasındaki ("Cash on Delivery Pending") sistem notları `LocalizeOrderNote` ile kullanıcı diline bağlandı; reçete durumları çok dilli yapıldı.
- [x] **Adım 117**: Detay sayfasındaki müşteri kartına `dir="ltr"` telefon formatı, e-posta/telefon linkleri ve misafir sipariş rozeti (`زائر`) eklendi; Sipariş #12 veritabanındaki eski test adresi (Istanbul/Fatih) Ramallah / Al-Masyoun olarak güncellendi.
- [x] **Adım 118**: `Admin/Kargo` sayfası baştan sona onarıldı: eksik `Admin_LogoUrl` çağrısı `Admin_LogoURL` ("رابط الشعار") ile eşitlendi; inline düzenleme satırındaki başlıksız inputlara açık etiketler (`<label class="ca-label">`) ve placeholder'lar eklendi; veritabanındaki `Ulke = 'Filistin'` kayıtları `'Palestine'` olarak güncellendi; filtre JS'si case-insensitive ve çift dil destekli yapıldı; inline JS metinleri güvenli JSON serializer değişkenlerine taşındı.

### Faz 13 (Yönetim Paneli Menü, Lokalizasyon, Ürün & Lisans Düzeltmeleri — 3 Eylül 2026)
- [x] **Adım 119**: `_AdminLayout.cshtml` menü optimizasyonu: `إدارة الطلبات` dropdown'undaki mükerrer kargo linki tekil `إدارة الشحن والمناطق` bağlantısına dönüştürüldü; `محتوى المتجر` altındaki mükerrer `أقسام الصفحة الرئيسية` linki temizlendi.
- [x] **Adım 120**: `Admin/Iade` tablosundaki `Admin_Siparis` başlığı düzeltildi; `@Localizer["Admin_Order"]` anahtarına bağlanarak Arapça ("الطلب") ve İngilizce ("Order") gösterimi sağlandı.
- [x] **Adım 121**: `Admin/Urun` tablosunda vitrin görünürlük butonuna (`ca-btn-icon view`) onay penceresi (`onsubmit="return confirm(...)"`) eklendi, kazara vitrine açma/kapatma engellendi, `Admin_ProductVisibilityShowConfirm` ve `Admin_ProductVisibilityHideConfirm` onay anahtarları AR/EN resx dosyalarına eklendi.
- [x] **Adım 122**: `Admin/Urun/Duzenle/112` sayfasının 6 sekmesi (الأساسيات, السعر والعمليات, المحتوى, الخصائص, المتغيرات, الصور) uçtan uca test edildi; modeldeki 8 varyasyon, form inputları, medya yükleme alanı ve dinamik sekmeler doğrulandı.
- [x] **Adım 123**: `Admin/Urun/Ekle` sayfasındaki Türkçe `urun-adi-otomatik-olusturulur` ve `ornek.webp` placeholder kalıntıları çok dilli dinamik metinlerle değiştirildi, sayfa doğrulandı.
- [x] **Adım 124**: `Admin/UrunOzellik` tablosundaki kullanılmayan 24 eski Türkçe mobilya/kanvas özelliği arşivlendi; genel e-ticaret için `اللون (Color)`, `المادة (Material)`, `بلد المنشأ (Country of Origin)`, `الوزن (Weight)`, `الضمان (Warranty)` tanımları eklendi; sayfaya ürün düzenleme sayfasındaki `الخصائص` sekmesini yönettiğini belirten açık ve şık bir rehber kutusu eklendi.
- [x] **Adım 125**: `Admin/Kullanici` sayfasındaki `Kullanıcı` tablo başlığı `@Localizer["Admin_User"]` ile değiştirilerek Arapça `المستخدم` ve İngilizce `User` yapıldı; veritabanındaki "Test Kullanıcı" kaydı "Test User" olarak güncellendi.
- [x] **Adım 126**: `Admin/Toptanci` sayfasındaki boş `تاريخ الطلب` (Başvuru Tarihi) sorunu çözüldü; `HesapController.cs`'de toptancı kaydında `BasvuruTarihi = DateTime.UtcNow` ataması yapıldı ve veritabanındaki mevcut kayıt `NOW()` ile dolduruldu.
- [x] **Adım 127**: `Admin/Toptanci/UrunGruplari` sayfasındaki `إضافة نسبة خصم` kart başlığında metin ve `%` ikonunun RTL modunda üst üste binme hatası `d-flex align-items-center gap-2` flex konteyner ile düzeltildi.
- [x] **Adım 128**: `Admin/Ziyaretci` tablosunda görünen `/Hata/404` Türkçe route'u uluslararası standart olan `/Error/{0}` standardına taşındı; `Program.cs` ve `HataController.cs` güncellendi, veritabanındaki 23 log kaydı `/Error/` ile güncellendi.
- [x] **Adım 129**: `Admin/Ziyaretci/Export` Excel indirmedeki `LicenseNotSetException` (500) hatası giderildi; `ZiyaretciController.cs` ve `Program.cs`'de `ExcelPackage.License.SetNonCommercialOrganization("7ANRPS48");` tanımlanarak 200 OK ile XLSX indirilmesi sağlandı.

### Faz 14 (Bankalar ve Ayarlar Ekranı İyileştirmeleri — 3 Eylül 2026)
- [x] **Adım 130**: `Admin/Bankalar` sayfasındaki unlocalized anahtarlar (`Admin_RegisteredAccounts`, `Admin_NoBankAccountsYet`, `Admin_AccountOwnerPlaceholder`, `Admin_BankNamePlaceholder`, `Admin_BankOrAccountOwner`, `Admin_Branch`, `Admin_DeleteBankAccountConfirm`) AR ve EN resx kaynaklarına eklendi; `L` fallback metodu ile Türkçe `Örn:` kalıntısı ve ham key görüntülenmesi tamamen giderildi.
- [x] **Adım 131**: `Admin/Ayarlar?tab=sosyal` ve `_SosyalMedyaForm.cshtml` içerisindeki manuel Font Awesome sınıfı yazma zorunluluğu kaldırıldı; 11 popüler sosyal ağ seçeneği (Facebook, Instagram, WhatsApp, TikTok, X, YouTube, Telegram, Snapchat, LinkedIn, Pinterest, Custom), canlı renkli ikon rozeti ve otomatik form tamamlama sağlandı.
- [x] **Adım 132**: `Admin/Ayarlar` sekme çubuğundaki içeriksiz boş `odeme` sekme butonu kaldırıldı; `AyarlarController.cs`'de geriye dönük uyumluluk için `tab=odeme` istekleri `tab=kapida-odeme` sekmesine yönlendirildi.
- [x] **Adım 133**: `Admin/Ayarlar?tab=seo` sekmesindeki ham/teknik alanlar kullanıcı dostu 3 karta dönüştürüldü: 1) Arama Motoru Optimizasyonu (SEO Title, Description, Keywords, OG Share Image); 2) Analitik ve Piksel Takip Araçları (Google Analytics 4, Meta Pixel ID); 3) Çerez Bildirimi (Cookie Consent Banner) çift dilli rehber metinlerle yapılandırıldı.

### Faz 15 (Storefront & Admin İyileştirmeleri — 3 Eylül 2026)
- [x] **Adım 134**: `Admin/Ayarlar?tab=sosyal` grid sütunları yeniden yapılandırılarak `social-save` ve `social-delete` butonlarının üst üste binmesi tamamen giderildi.
- [x] **Adım 135**: Font Awesome 6.4.0'da eksik olan yeni `X (Twitter)` ikonu için `admin.css` ve storefront `_Layout.cshtml` dosyalarına SVG mask tabanlı `.fa-x-twitter` sınıfı eklendi.
- [x] **Adım 136**: Veritabanındaki sahte/test kaydı olan `Test Ürün 1787679181417` (ID: 142) ve 11 ilişkili tablodaki bağımlılıkları temizlendi.
- [x] **Adım 137**: `/favorites` sayfasındaki fiyat düşüş alarmı zil ikonuna tıklandığında oluşan 404 hatası giderildi; `FavoriController.cs`'de `TogglePriceNotification` action'ına açık route eşleştirmeleri eklendi.
- [x] **Adım 138**: Header arama kutusundaki arama temizleme (X) ikonunun Arapça (RTL) modunda sola yapışıp metni kapatması `ms-3 me-1 p-1` ile düzeltildi.
- [x] **Adım 139**: `/profile` karşılama metni resx anahtarına bağlandı (`مرحباً، {0}`); veritabanındaki admin kullanıcısının adı `مدير 7ANRPS48` olarak güncellendi.
- [x] **Adım 140**: `/profile` sayfasına "تعديل المعلومات" (Bilgileri Güncelle) ve "تغيير كلمة المرور" (Şifre Değiştir) butonları ile modern, yüksek z-index'li (`z-[100000]`) modallar eklendi; e-posta alanı salt okunur yapıldı ve güvenlik uyarısı eklendi; `ProfilController.cs`'de `update` ve `change-password` action'ları yazıldı.
- [x] **Adım 141**: `profile/Adreslerim` sayfasındaki adres ekleme modalının (`#adresModal`) header altında kalma sorunu `z-[100000]` ile çözüldü.
- [x] **Adım 142**: Ekranın sağ/sol altında yüzen sepet butonunun (`_FloatingButtons.cshtml`) footer ile görsel çakışması lüks altın degradesi (`linear-gradient(135deg, #c5a880...)`), beyaz kontrast halkası ve koyu yeşil/altın rozet ile çözüldü.
- [x] **Adım 143**: `Admin/Kategori` düzenleme ve ekleme ekranlarına kategori hero banner yönetimi eklendi; canlı banner görsel önizlemesi, kampanya etiketi ve üst metin alanları yerleştirildi; mobil (600x400), masaüstü (1400x450) ve kart/menü (600x600) için piksel boyut rehberi eklendi. Kategori 78 (أثاث منزلي) örnek banner ve kampanya etiketiyle yapılandırıldı.
- [x] **Adım 144**: Ürün detayında tek varyantlı ürünlerde (ör. Yumurta #134) gereksiz yere görünen "Standart / Standard" seçim butonu `@if (secenekler.Count > 1)` koşuluyla gizlendi; varyant etiketlerindeki Türkçe "Standart" kalıntıları temizlendi.
- [x] **Adım 145**: `Urunler.TeknikOzellikler` kolonundaki import kalıntısı İngilizce başlıklar ve çift dilli pipe strings temizlendi; Razor motoruna akıllı parser eklenerek ürün detayındaki teknik özellikler tablosu saf Arapça (`🏷️ العلامة التجارية`, `🔖 رمز المنتج`, `📦 الفئة`, `✅ المتوفر`) olarak render edildi.



### Hassas dosya mimarisi (B25)
- **Storage root**: `<ContentRoot>/secure-storage/hassas/{kategori}/` (wwwroot dışında).
  - `kategori` ∈ `kimlikler` (jpg/jpeg/png/webp, max 8MB), `receteler` (jpg/jpeg/png/webp/pdf, max 12MB).
  - Dosya adı: `Guid.NewGuid().ToString("N") + lowercase uzantı`.
  - DB referansı: `private://<kategori>/<dosya-adı>` (sadece bu token kullanıcıya gösterilir).
- **Okuma**: `BelgeController` (`/Belge/Kimlik`, `/Belge/SiparisKimlik`, `/Belge/Recete`) yalnızca owner veya yetkili admin için dosya akışı verir. Path parametresi olarak fiziksel yol KABUL EDILMEZ, sadece sahiplik id'si.
- **Legacy uyumluluk**: DB'de eski `/uploads/kimlikler/...` veya `/uploads/receteler/...` referansı varsa `BelgeController` mevcut dosyayı okur, fakat middleware aynı URL için zaten 404 döndürür; yeni DB referansları `private://` ile taşınır.
- **Public blokaj**: `Program.cs` route-bazlı 404 middleware (`/uploads/kimlikler/*` ve `/uploads/receteler/*`).

### Kamera (WebRTC) politika (B9)
- **Global**: `Permissions-Policy: camera=(), microphone=(), geolocation=()`.
- **Ödeme + Kayıt sayfaları**: `camera=(self)` (yalnızca same-origin).
- **JS davranışı** (`Odeme.cshtml`): Secure context ön-kontrolü, NotAllowed/NotFound/NotReadable/Security hata sınıflandırması, AR + EN mesaj, upload moduna otomatik geçiş. Hata durumunda sayfa bozulmaz.

### Program.cs (önemli satırlar)
- `~satır 784-1060` — `EnsureMissingMarch2026SchemaAsync`: hand-rolled SQL, tüm ek kolonları/tabloları kapsar (ReceteGerekliMi, WhatsappSiparisVarMi, FiyatGizliMi, ToptanciMinSiparisTutari, ToptanciUrunGrubuId, ToptanciUrunGruplari, ToptanciIskontoOranlari, BasvuruTarihi, KargoBolge.Ulke/Aciklama, Slayt dil alanları)
- `~satır 460-510` — Migration + Seed uygulama mantığı
- `~satır 572-578` — `EnsureSensitiveUploadsMigratedAsync` (Faz 11)
- `~satır 302-313` — Security header'lar (route-bazlı Permissions-Policy)
- `~satır 320-330` — Legacy hassas upload path 404 middleware
- `~satır 700+` — Login middleware (GirisZorunluMu kontrolü)
- `~satır 40-45` — DB erişilebilirlik kontrolü

## Developer notları (AI için)

1. **WholesaleStatus** enum'u `FilistinProje.Core/Enums/` altında. Yeni enum'lar da aynı yere eklenmeli.
2. **View'da enum referansı**: `FilistinProje.Core.Enums.WholesaleStatus` — tam nitelikli kullan.
3. **DbSeeder** tüm rolleri `AdminSecurityRoles.AllRoles` listesinden seed eder. Yeni rol eklenirse bu listeye ekle.
4. **Wholesale** rolü admin rolü DEĞİLDİR. `AllAdminRoles`'a ekleme, `AllRoles`'a ekle.
5. **AdminBaseController** tüm admin controller'ların base class'ıdır. Attribute'ları (`[Authorize]`, `[Area]`) zaten içerir. Sadece `: AdminBaseController` yap yeter.
6. **Yeni bir admin controller eklerken**: (a) `: AdminBaseController` yap, (b) `AdminPermissionMatrix`'e controller adını ekle, (c) `AdminBaseController`'da ViewBag değişkenini set et, (d) `_AdminLayout.cshtml`'e link ekle.
7. **View'lar Türkçe** yazılır (admin paneli için localizer kullanılmıyor). Türkçe dil desteği projeden kaldırılmıştır, sadece AR/EN vardır.
8. **Build'den önce** `dotnet build FilistinProje.sln` ile kontrol et. 0 hata 0 uyarı hedefi.
9. **StokBiteniGriGoster** (`SiteAyarlari`): Admin panelden yönetilir. true=stoğu biten varyasyonlar gri+tükendi rozeti gösterilir, false=tamamen gizlenir. `StoktaYokSatisIzni` true ise bu ayar devre dışı kalır (tüm varyasyonlar seçilebilir).
10. **HediyePaketi** akışı: `Urun.HediyePaketiVarMi` + `HediyePaketFiyati`. SepetItem ve SiparisDetay'da `HediyePaketi` (bool) ve `HediyePaketFiyati` (decimal) alanları.
11. **WhatsappSiparisVarMi** — Ürün bazında WhatsApp sipariş modu. `FiyatGizliMi` ile birlikte çalışır: true ise ürün fiyatı gizlenir, "WhatsApp ile Sipariş Ver" butonu gösterilir.
12. **ToptanciUrunGrubu** — Toptancı ürün grupları, `ToptanciIskontoOrani` ile adet bazlı iskonto yüzdesi tanımlanır. `ToptanciMinSiparisTutari` toptancı için minimum sipariş tutarıdır.
13. **ReceteGerekliMi** (Kategori) — Bu kategoriye ait ürünler için reçete yükleme zorunluluğu.
14. **Slayt dil alanları** — `BaslikEn/Ar`, `AltBaslikEn/Ar`, `AciklamaEn/Ar`. Slayt entity'sinde `LocalizedBaslik`, `LocalizedAltBaslik`, `LocalizedAciklama` computed property'leri mevcut.
15. **Tüm view'lar güncel** — `Detay.cshtml`, `Siparislerim.cshtml`, `Adreslerim.cshtml`, `Basarili.cshtml`, `Basarisiz.cshtml` modern, IStringLocalizer kullanır durumda.
16. **Admin URL'leri**: `/Admin/Slayt`, `/Admin/Kargo`, `/Admin/Rapor`, `/Admin/Iletisim`, `/Admin/Iade`, `/Admin/Kupon`, `/Admin/HomeSections`, `/Admin/Bulten`, `/Admin/Sayfa`, `/Admin/UrunOzellik`, `/Admin/TopluFiyatGuncelle`, `/Admin/SlugTool`, `/Admin/Yorum`, `/Admin/Ziyaretci`, `/Admin/XyzSecretMonitor`
17. **Hassas dosya referansı**: DB kolonlarında path değil `private://<kategori>/<guid><ext>` token bulunur. Dosya `ContentRoot/secure-storage/hassas/{kategori}/` altındadır. Okuma için `BelgeController` endpoint'leri. `DosyaServisi.HassasBelgeKaydetAsync` upload doğrulamayı zorunlu kılar. Yeni hassas alan eklenirse sadece bu servis ile yazılmalı.
18. **Kamera (WebRTC) politika**: `Program.cs` global kamera iznini kapatır; sadece `/Siparis/Odeme` ve `/Hesap/KayitOl` için `camera=(self)` route-bazlı açılır. Yeni kamera kullanan sayfa eklenirse `IsCameraAllowedPath`'e path eklenmeli.
19. **Belge endpoint'leri**: Yetkisiz isteklerde `[Authorize]` → 302 login, login sonrası ise `Forbid()`. Sahiplik kontrolü: kimlik için `AppUser.Id == currentUserId`, reçete/sipariş kimlik için sipariş sahipliği. Admin için matrix'te `Kullanici`, `Siparis`, `Toptanci` izinleri olmalı.
20. **Migration startup (B17 + B21)**: `StartupReadinessState` singleton state machine. Production'da kritik migration/seed hata → `app.Lifetime.StopApplication()` fail-fast, container exit. Dev'te log + 503 readiness. `/health/live` her zaman "alive"; `/health/ready` DB + startup gate. `EnsureMigrationHistoryConsistencyAsync` sadece mevcut schema'ya karşılık gelen migration'ları history'ye yazar; uygulanmamış migration'ı "applied" olarak işaretlemez.
21. **DataProtection key kalıcılığı**: Container'da `DATA_PROTECTION_KEYS_PATH=/app/secure-storage/dataprotection-keys` env ile volume mount altına yazılır. Yerel geliştirmede `App_Data/DataProtectionKeys`. Volume kaybı = tüm auth cookie'ler geçersiz, kullanıcı yeniden login olur.

### Faz 12 (Sipariş Bütünlüğü — B2, B3, B13, B27 — 10 Temmuz 2026)
- [x] **Adım 112**: `IOrderPricingService` interface'i (`FilistinProje.Core/Interfaces/IOrderPricingService.cs`) — server-side tek fiyat hesaplama yolu. `HesaplaAsync(sepetItems, sehir, odemeYontemi, isWholesale, kuponKodu)` → `OrderPricingResult`; `StokDusAsync(satirlar)` → `StockDeductionResult`; `CalculateCouponDiscount(Kupon, tutar)`.
- [x] **Adım 113**: `FilistinProje.Core/DTOs/OrderPricingModels.cs` — `OrderLinePricing`, `OrderPricingResult`, `PriceChangedEntry`, `StockShortageEntry`, `StockDeductionResult`. DTO'larda sadece server tarafından hesaplanan güvenli property'ler.
- [x] **Adım 114**: `FilistinProje.Core/DTOs/CheckoutRequestDto.cs` (B27) — checkout formundan bind edilen DTO. Server-owned alanlar (ToplamTutar, IndirimTutari, AppUserId, Durum, SiparisNo, KuponKodu) bind edilmez.
- [x] **Adım 115**: `FilistinProje.Service/Services/OrderPricingService.cs` — implementasyon.
  - B3: Her satır Urun.UrunSecenek DB'den yeniden okunur; SepetItem.Fiyat (snapshot) **hiçbir koşulda** SiparisDetay.BirimFiyat'a yazılmaz.
  - wholesale: `Urun.EtkinTopFiyat` (TopFiyat > 0) + `ToptanciIskontoOrani` (adete göre en yüksek iskonto).
  - hediye: server-side `Urun.HediyePaketFiyati` kullanılır (SepetItem'daki değil).
  - kupon: kupon tarih, limit, min kontrol + `CalculateCouponDiscount` (Tip 0=yüzde, 1=tutar) yuvarlanmış.
  - stok kontrolü: varyant varsa ve `OnSipariseAcikMi` değilse `StokAdedi >= Adet` zorunlu; yetersizse `StockShortageEntry` döner.
- [x] **Adım 116**: `OrderPricingService.StokDusAsync` — transaction içinde `ExecuteSqlInterpolatedAsync` ile atomik koşullu UPDATE:
  - `UPDATE "UrunSecenekleri" SET "StokAdedi" = "StokAdedi" - @adet WHERE "Id" = @id AND "StokAdedi" >= @adet`
  - Read-then-write yarışı yok; birden fazla eşzamanlı checkout'ta yalnızca biri başarılı olur.
- [x] **Adım 117**: `SiparisController.Odeme` POST komple yeniden yazıldı (B27 + B2 + B3 + B13):
  - `[Bind]` attribute yerine doğrudan `CheckoutRequestDto dto` parametresi (overposting engeli).
  - Transaction `ReadCommitted`; sipariş → stok düşümü → sipariş detayları → kupon.KullanilanMiktar++ → sepet temizleme aynı transaction.
  - Stok düşümü başarısızsa veya transaction içinde exception olursa `transaction.RollbackAsync()` + form + anlaşılır hata.
- [x] **Adım 118**: Kaynaklar (Resources): `Siparis_StockShortage`, `Siparis_StockShortageGeneric`, `Siparis_PriceChangedNotice`, `Siparis_OrderFailed` AR/EN eklendi.
- [x] **Adım 119**: Views `Odeme.cshtml`, `_AddressForm.cshtml`, `_IdentityVerification.cshtml` → `@model FilistinProje.Core.DTOs.CheckoutRequestDto`. Bind edilen alanlar aynı.
- [x] **Adım 120**: `scripts/siparis_butunluk_test.sql` — manuel doğrulama betiği (stok race sim, fiyat manipülasyon sim, hediye paketi tekil tutar).

### Fiyat formülü (CheckoutPOST sonrası, B3/B13 doğrulanmış):
```
Her satır için (grouped by UrunId + UrunSecenekId + CerceveModeli + HediyePaketi):
  BirimFiyat = (secenek?.SatisFiyati > 0) ? secenek.SatisFiyati
            : isWholesale ? urun.EtkinTopFiyat
            : urun.EtkinFiyat
  + çerçeve_farkı (boyut × çevre × 250/m, varsa)
  - toptancı_iskonto (wholesale ise ve adet eşik aşarsa)
  HediyePaketBirim = urun.HediyePaketFiyati (eğer urun.HediyePaketiVarMi && dto.HediyePaketi)
  SatirToplam = BirimFiyat × Adet + HediyePaketBirim × Adet

AraToplam = Σ SatirToplam
IndirimTutari = (kupon varsa ve geçerli) ? kupon_indirim(AraToplam) : 0
SepetIndirimli = AraToplam - IndirimTutari
KargoUcreti = (magazadan teslim) ? 0 : KargoHesapla(sehir, SepetIndirimli, UcretsizKargoLimiti)
KapidaOdemeHizmetBedeli = (COD && sepet <= CODLimiti) ? settings.KapidaOdemeHizmetBedeli : 0

siparis.ToplamTutar = GenelToplam
                   = (AraToplam - IndirimTutari) + KargoUcreti + KapidaOdemeHizmetBedeli
                   ≥ 0
```

### Transaction davranışı (B2 + rollback garantisi):
```
BEGIN (Isol: ReadCommitted)
  INSERT Siparisler
  WAIT sql_save → @Id
  foreach (satır in satirlar):                              -- atomik, sıralı
    affected = UPDATE UrunSecenekleri SET Stok -= adet
                WHERE Id=@id AND StokAdedi >= adet
    if affected != 1 → Rollback + FormHata (StokShortage)
  INSERT SiparisDetaylari × n (server-side BirimFiyat + HediyePaketBirim)
  if (kupon): kupon.KullanilanMiktar++
  SepetItems.SilindiMi = true (cart clear)
  COMMIT
if ANY exception → ROLLBACK + FormHata (OrderFailed)
```
Hiçbir durumda sipariş, kupon veya stok tek başına commit etmez; ya hep birlikte ya hiçbiri.

### Faz 13 (Migration & Production Startup Güvenliği — B17, B21, DataProtection — 10 Temmuz 2026)
- [x] **Adım 121**: Package: `Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore` 8.0.10 (Web).
- [x] **Adım 122**: `StartupReadinessState` singleton (HealthChecks/StartupReadinessState.cs) — phase machine: Booting → DatabaseUnavailable | SchemaDriftFailed | MigrationPending | MigrationFailed | SeedFailed | Ready. LastErrorType/Message/UpdatedAtUtc.
- [x] **Adım 123**: `StartupReadinessHealthCheck` (IHealthCheck) + `HealthCheckResponseWriter` — liveness `"alive"` plain text; readiness `{status, results[{check, status, description, durationMs}]}` JSON. Hiçbir yerde connection string/exception stack/body expose edilmez.
- [x] **Adım 124**: Program.cs — `/health/live` (predicate=false, no checks, "alive" body), `/health/ready` (tag:ready → DB + startup), `/health` (tag:ready alias).
- [x] **Adım 125**: DB available + Production'da kritik migration/seed hata → `app.Lifetime.StopApplication()` fail-fast. Development'ta log + devam.
- [x] **Adım 126**: DB unavailable → state=DatabaseUnavailable, `/health/live=200` "alive", `/health/ready=503`. Hangfire + hosted services disabled (mevcut).
- [x] **Adım 127**: `EnsureMigrationHistoryConsistencyAsync` — tüm elle uygulanan schema drift'leri __EFMigrationsHistory'ye yalnız MEVCUT schema karşılığı varsa ekler; `ProductVersion="8.0.4"` (snapshot uyumlu). Uygulanmamış migration'ı applied gibi işaretlemez. WHERE NOT EXISTS + bilinen kolon/tablo koşulu.
- [x] **Adım 128**: `EnsureMissingMarch2026SchemaAsync`'e 3 eksik tablo eklendi: `CarkOdulleri`, `PushAbonelikleri` (FK+2 index), `StokBildirimLoglari` (FK+3 index). Tümü `CREATE TABLE IF NOT EXISTS` + `ADD CONSTRAINT IF NOT EXISTS`. ProductVersion 8.0.0 → 8.0.4 normalleşti.
- [x] **Adım 129**: docker-compose.yml — `DATA_PROTECTION_KEYS_PATH=/app/secure-storage/dataprotection-keys` env; `filistin_app_secure_storage:/app/secure-storage` named volume. `.env.example` DATA_PROTECTION_KEYS_PATH satırı + açıklama eklendi.
- [x] **Adım 130**: docker-compose.yml — web servisi volumes'a `filistin_app_secure_storage:/app/secure-storage` satırı. Volume declaration: `filistin_app_secure_storage: driver: local`.

### Startup davranış matrisi

| Ortam | DB Available | Migration hatası | Sonuç |
|---|---|---|---|
| Production | Yes | Hayır | Phase=Ready; /health/live=200; /health/ready=200 ✅ |
| Production | Yes | Evet | fail-fast: app.Lifetime.StopApplication(); container exit; /health/ready=503 |
| Production | No | — | Phase=DatabaseUnavailable; /health/live=200 "alive"; /health/ready=503 |
| Development | Yes | Hayır | Phase=Ready; 200/200 ✅ |
| Development | Yes | Evet | Logged; Phase=SchemaDriftFailed|MigrationFailed|SeedFailed; /health/ready=503 |
| Development | No | — | Log+warn; /health/ready=503; liveness=200 |

### Production Deployment Sırası (PowerShell + Docker Desktop)

```powershell
# === 1) Yedek al ===
docker exec filistinproje-db pg_dump -U kanvasuser filistindb | Out-File -Encoding utf8 backup_$(Get-Date -Format 'yyyyMMdd_HHmmss').sql

# === 2) Image build ===
docker-compose build --no-cache web

# === 3) Yeni container'ı başlat (depends_on db healthy, Compose sırayla başlatır) ===
docker-compose up -d web

# === 4) Log'ları izle, migration başarılı mı ===
docker logs -f --tail 200 filistinproje-web

# === 5) Readiness'i doğrula (migration bitene kadar poll) ===
$readyUrl = "http://localhost:8080/health/ready"
while ((Invoke-WebRequest -UseBasicParsing -Uri $readyUrl -TimeoutSec 3).StatusCode -ne 200) { Start-Sleep 2 }
# "alive" liveness ayrı:
$liveUrl = "http://localhost:8080/health/live"
Invoke-WebRequest -UseBasicParsing -Uri $liveUrl | Select-Object StatusCode,Content

# === 6) Trafik aç ===
```

### Rollback koşulları ve komutları

**Koşul 1: `/health/ready` 503 dönmeye başlarsa** (production monitor alarm):
```powershell
docker logs --tail 500 filistinproje-web | Select-String -Pattern "Migration|Schema|Seed|LogCritical" -Context 2
docker exec filistinproje-db psql -U kanvasuser -d filistindb -c "SELECT ""MigrationId"" FROM ""__EFMigrationsHistory"" ORDER BY ""MigrationId"" DESC LIMIT 10;"
```

**Koşul 2: Önceki image'a dön (zero-downtime rollback)**
```powershell
# Önceki image zaten varsa (docker images ile bak):
docker tag filistinproje-web:previous filistinproje-web:latest
docker-compose up -d --no-deps web
```

**Koşul 3: Tam veritabanı geri alma (DROP SCHEMA + restore)**
```powershell
# BU KOMUT TÜM VERİYİ SİLER. Sadece tam yedek olduğunda çalıştır.
docker exec filistinproje-db psql -U kanvasuser -d filistindb -c "DROP SCHEMA public CASCADE; CREATE SCHEMA public;"
Get-Content backup_YYYYMMDD_HHmmss.sql | docker exec -i filistinproje-db psql -U kanvasuser -d filistindb
docker-compose restart web
```

**DataProtection key kaybı** (auth cookie'leri geçersiz olur; yeniden login gerek):
```powershell
docker run --rm -v filistin_app_secure_storage:/data -v ${PWD}\dataprotection-backup:/backup alpine cp -a /backup/. /data/
docker-compose restart web
```

### Gizli değerler (secrets) kuralları
- `secrets.json` gitignore'da; **production'da ASLA kullanılmaz**.
- SMTP username/password, connection string → yalnız environment variable (docker-compose `environment:` veya `.env`).
- DataProtection key dosyaları gizli değil ama kalıcı volume şart.
- `.env` **commit edilmez**; sadece `.env.example` repoda.

### Migration History consistency (B17 karar)

Dual migration sistemi (EF + EnsureMissingMarch2026SchemaAsync) korunur. Yeni entity property eklenince **her ikisine de eklenmeli** (mevcut kural). Ek olarak:
- `EnsureMigrationHistoryConsistencyAsync` tüm elle uygulanan schema'lere karşılık gelen EF migration ID'lerini history'ye ekler (ProductVersion 8.0.4).
- Bu method **schema kolon/tablo MEVCUTSA** ekler; yoksa eklemez. Bu sayede uygulanmamış migration'ı "applied" gibi işaretleme riski yoktur.
- EF `MigrateAsync()` kendi history insert'ini snapshot ProductVersion ile yapar; çift insert `WHERE NOT EXISTS` ile engellenir.
- `EnsureMissingMarch2026SchemaAsync` içinde tüm kolon/tablo değişiklikleri `ADD COLUMN IF NOT EXISTS` / `CREATE TABLE IF NOT EXISTS` / `ADD CONSTRAINT IF NOT EXISTS` ile idempotent.

### Health endpoint güvenliği

- `/health/live` → body `"alive"` (plain text). Yalnız process alive kontrolü. Hiçbir DB/CONN/stack yok.
- `/health/ready` → JSON `{status: "Healthy|Degraded|Unhealthy", results: {"database": {status, description, durationMs}, "startup": {status, description, durationMs}}}`. description'da sadece "ready" / "booting" / "db_unavailable" / "schema_drift_failed" / "migration_failed" / "seed_failed" gibi enum etiketleri. Exception message dahil DEĞİLDİR.
- Hiçbir endpoint body'sinde connection string, exception stack trace, IP, port, password yoktur.
