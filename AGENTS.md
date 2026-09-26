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
- Central Remote DB: `Host=canvasia-server;Port=5434;Database=filistindb;Username=kanvasuser;Password=changeme_in_production` (canvasia-server üzerinde yerel PostgreSQL 18 servisi, Tailscale üzerinden bağlı)
- Local Docker Compose: Sadece `web` servisi çalışır (DB konteyneri kaldırıldı).

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

### Faz 16 (Proje Sahibi Video Talepleri & Vitrin/Admin İyileştirmeleri — 20 Eylül 2026)
- [x] **Adım 142**: Paketleme Seçenekleri UI Güncellemesi (`Views/Urun/Detay.cshtml`):
  - Radyo buton kutuları yerine modern açılır menü (`<select id="giftPackageSelect">`) formatına geçildi.
  - Seçenekler `Ad (+Fiyat ₪ / adet)` formatında dinamik listelendi.
  - Başlığa proje sahibinin istediği `(يتم إضافة سعر التغليف على المنتج)` / `(Packaging fee is added to product price)` notu eklendi.
  - Sepet ve özet hesaplama JS fonksiyonları (`updateSelectionSummary`, `ajaxSepetBtn`) select menüsüne bağlandı.
- [x] **Adım 143**: Toptan Fiyat Kademeleri Tablo Tasarımı (`Views/Urun/Detay.cshtml`):
  - Dağınık 2 kolonlu kart gridi yerine derli toplu, şık ve okunaklı bir mini tablo (`table`) tasarımına dönüştürüldü.
  - Sütunlar: `@Localizer["WholesaleMinQty"]` (الكمية / Quantity), `@Localizer["WholesaleUnitPrice"]` (سعر القطعة / Unit Price).
- [x] **Adım 144**: Vitrin Varyasyon Tıklama Hatası & Güvenlik Düzeltmeleri (`Views/Urun/Detay.cshtml`):
  - Stoğu biten varyasyonlardaki `disabled` input kısıtlaması kaldırılarak `data-purchasable="false"` modeline geçildi; kullanıcı artık varyasyona tıklayıp resmini ve "Tükendi" uyarısını görebiliyor, sepete ekleme butonu kontrollü olarak devre dışı kalıyor.
  - `updateDiscountDisplays` fonksiyonunda `window.caDiscountCalculator` için null-safety fallback eklendi (`calculatePercentage` tanımsızlık hatası engellendi).
  - `updateVariantCardStyles` içinde renk regex'i büyük/küçük harf duyarsız yapıldı (`/^#[0-9A-Fa-f]{6}$/`).
  - Varsayılan varyant seçimi mantığı onarıldı; admin panelde `VarsayilanMi` seçilmediyse hiçbir varyant öne çıkarılmıyor, ana ürünün temel fiyatı/resmi görüntüleniyor.
- [x] **Adım 145**: İndirim Bitiş Tarihine Saatlik Hızlı Butonlar:
  - `Admin/Urun/Duzenle.cshtml`, `Admin/Urun/_VariantEditor.cshtml` ve `Admin/Urun/Ekle.cshtml` dosyalarında gün butonlarının (`+1d, +3d, +1w, +1m`) önüne saat butonları (`+1h, +3h, +6h, +12h` / `+1 س, +3 س, +6 س, +12 س`) eklendi.
  - JS motoru `data-hours` desteğiyle `now.setHours(now.getHours() + hours)` dinamik saat hesaplamasına kavuşturuldu.
  - `SharedResource.ar.resx` ve `SharedResource.en.resx` dosyalarına `QuickTime_1Hour`, `QuickTime_3Hours`, `QuickTime_6Hours`, `QuickTime_12Hours`, `GiftPackagePriceNote` anahtarları eklendi.
- [x] **Adım 146**: Admin Ürün Düzenleme Sabit Görsel Yükleme Kolaylığı (`Duzenle.cshtml`):
  - Sağ taraftaki sabit ürün önizleme kartının hemen altına `Admin_UploadNewMainImage` butonu eklendi; admin kullanıcısı hangi sekmede (Fiyat, Temel Bilgiler vb.) olursa olsun sekmeler arası geçiş yapmadan tek tıkla ana ürün görselini güncelleyebiliyor.
- [x] **Adım 142**: Ekranın sağ/sol altında yüzen sepet butonunun (`_FloatingButtons.cshtml`) footer ile görsel çakışması lüks altın degradesi (`linear-gradient(135deg, #c5a880...)`), beyaz kontrast halkası ve koyu yeşil/altın rozet ile çözüldü.
- [x] **Adım 143**: `Admin/Kategori` düzenleme ve ekleme ekranlarına kategori hero banner yönetimi eklendi; canlı banner görsel önizlemesi, kampanya etiketi ve üst metin alanları yerleştirildi; mobil (600x400), masaüstü (1400x450) ve kart/menü (600x600) için piksel boyut rehberi eklendi. Kategori 78 (أثاث منزلي) örnek banner ve kampanya etiketiyle yapılandırıldı.
- [x] **Adım 144**: Ürün detayında tek varyantlı ürünlerde (ör. Yumurta #134) gereksiz yere görünen "Standart / Standard" seçim butonu `@if (secenekler.Count > 1)` koşuluyla gizlendi; varyant etiketlerindeki Türkçe "Standart" kalıntıları temizlendi.
- [x] **Adım 145**: `Urunler.TeknikOzellikler` kolonundaki import kalıntısı İngilizce başlıklar ve çift dilli pipe strings temizlendi; Razor motoruna akıllı parser eklenerek ürün detayındaki teknik özellikler tablosu saf Arapça (`🏷️ العلامة التجارية`, `🔖 رمز المنتج`, `📦 الفئة`, `✅ المتوفر`) olarak render edildi.

### Faz 16 (Admin Ürün Görünürlük & Yönlendirme Düzeltmeleri — 4 Eylül 2026)
- [x] **Adım 146**: `Admin/Urun` tablosundaki vitrin görünürlük butonundaki Razor boolean attribute hatası (`value="value"`) düzeltildi; `value="@(item.YayindaMi ? "false" : "true")"` ve `DurumGuncelle` için `value="@(item.AktifMi ? "false" : "true")"` yapılarak her tıklamada ürünün istisnasız gizlenmesi sorunu giderildi.
- [x] **Adım 147**: `Admin/UrunController.cs` içerisindeki tüm action yönlendirmelerine (`GizleGoster`, `DurumGuncelle`, `Sil`, `TopluSil` vb.) `new { area = "Admin" }` rotası eklendi; LinkGenerator'ın admin isteklerini vitrindeki `/Urun` rotasına yönlendirip admini dışarı atması ve 404 üretmesi engellendi.
- [x] **Adım 148**: Eksik `Admin_Product_Published`, `Admin_Product_Hidden`, `Admin_Product_Activated`, `Admin_Product_Deactivated`, `Admin_Product_NotFound` resx anahtarları Arapça ve İngilizce kaynaklara eklendi; ham toast metinleri ("Admin_Product_Hidden") düzeltildi.
- [x] **Adım 149**: Vitrin görünürlük butonunun ikon ve tooltip mantığı düzeltildi; ürün yayındayken açık göz (`fa-eye`), gizliyken üzeri çizgili göz (`fa-eye-slash`) gösterilmesi sağlandı.
- [x] **Adım 150**: Vitrin `UrunController.Detay` sorgusunda admin kullanıcılar için `YayindaMi` zorunluluğu bypass edildi; yöneticilerin taslak/gizli ürünleri sitede 404 almadan önizleyebilmeleri sağlandı.

### Faz 17 (Veritabanı Türkçe Veri Temizliği & Çok Dilli Varyasyon Sistemi — 4 Eylül 2026)
- [x] **Adım 151**: `UrunSecenekleri` tablosundaki Ürün #112'ye ait Türkçe ve test amaçlı 8 varyant temizlendi; 4 temiz kozmetik varyantı (`أسود` / Black #111827, `أزرق` / Blue #2563EB, `أخضر` / Green #16A34A, `بنفسجي` / Purple #7C3AED) olarak yapılandırıldı, `Beden` alanlarındaki "VFIX" ve "RENK" kalıntıları silindi.
- [x] **Adım 152**: `VaryantRenkYardimcisi.cs` içine `RenkCevirileri` sözlüğü ve `GetLocalizedRenk(string? renkAdi, bool isAr)` metodu eklendi; Türkçe veya İngilizce girilse dahi renklerin daima aktif dilde (Arapça / İngilizce) render edilmesi garanti altına alındı.
- [x] **Adım 153**: `UrunSecenek.cs` içine `GetLocalizedVaryantBasligi(bool isAr)` metodu eklendi; `Views/Urun/Detay.cshtml` bu metoda bağlanarak ürün detayında varyant etiketlerinin ve seçili varyant özetinin dinamik lokalizasyonu sağlandı.
- [x] **Adım 154**: `KargoBolgeler` tablosundaki 5 bölge Arapça standart isim ve açıklamalara güncellendi (`المناطق الداخلية 48 (شمال)`, `الضفة الغربية`, `القدس`, `قطاع غزة`), ülke alanı `Palestine` yapıldı.
- [x] **Adım 155**: `KargoBolgeSehirler` tablosundaki 11 mükerrer satır temizlendi; 24 Filistin şehrinin `SehirAdi` alanları saf Arapça (`القدس`, `الناصرة`, `بيت لحم`, vb.), `SehirAdiEn` alanları ise saf İngilizce (`Nazareth`, `Bethlehem`, `Jerusalem`) yapıldı.
- [x] **Adım 156**: `UrunHediyePaketSecenekleri` ("تغليف قياسي"), `KargoFirmalari` ("Ramallah, Palestine"), `AspNetUsers`, `IletisimMesajlari` ve `Siparisler` tablolarındaki tüm Türkçe veri kalıntıları temizlendi.
- [x] **Adım 157**: `DbSeeder.cs` içindeki bölge ve şehir tohumlama verileri Arapça/İngilizce yapıya geçirildi, uygulamanın yeniden başlatıldığında Türkçe kayıt üretmesi kalıcı olarak engellendi.

### Faz 18 (Varyant İndirimleri, Teslimat Toggle Düzeltmesi, Kurumsal Sayfa CRUD & Misafir Bölge Seçimi — 11 Eylül 2026)
- [x] **Adım 158**: `_VariantEditor.cshtml`'e İndirim Türü (بدون خصم / مباشر, نسبة مئوية %, مبلغ ثابت ₪), Yüzde (%) ve Sabit Tutar (₪) alanları hem kart döngüsüne hem template'e eklendi; çift yönlü anlık dinamik hesaplama sağlandı.
- [x] **Adım 159**: `Ekle.cshtml` ve `Duzenle.cshtml` ana ürün fiyatlandırma paneline İndirim Türü, % ve ₪ indirim inputları ve anlık otomatik hesaplama entegre edildi.
- [x] **Adım 160**: Teslimat Seçenekleri (Toggle) hatası giderildi; `Odeme.cshtml` içinde hidden input desteği sunan `getDeliveryType()` tanımlandı, `:checked` seçici bağımlılığı kaldırıldı; `toggleDeliveryType()`, `togglePaymentMethod()`, `sehirKargoHesapla()` ve `updateCheckoutTotals()` senkronize edildi.
- [x] **Adım 161**: `SiteSettingsService.cs` ve `Ayarlar/Index.cshtml` içine her iki teslimat türünün aynı anda kapatılmasını engelleyen güvenlik önlemleri ve kullanıcı uyarıları eklendi.
- [x] **Adım 162**: Kurumsal Sayfalar (`SayfaController.cs`) sessiz kayıt ve güncelleme başarısızlığı giderildi; `NormalizeContent` öne alındı, otomatik slug üretiminde `ModelState.Remove(nameof(model.UrlSlug))` uygulandı; mevcut sayfaların diller arası içerik güncellemesi sağlandı; `Form.cshtml`'e validation summary ve `UrlSlug` alanı eklendi.
- [x] **Adım 163**: Misafir sipariş akışında şehir (`Sehir`) öncesine Bölge (`BolgeSelect`) kademeli dropdown'u eklendi; `regionCities` sözlüğü ile şehirlere dinamik filtreleme ve anlık kargo hesaplaması bağlandı; kayıtlı adresten seçimde (`selectAddress`) bölge eşleştirmesi eklendi; `CheckoutRequestDto.cs` ve `SiparisController.ValidateCheckoutInput` güncellendi.

### Faz 19 (Google OAuth Profil Tamamlama & Toptancı Entegrasyonu — 25 Eylül 2026)
- [x] **Adım 164**: Google ile kayıtta "Toptancı Olarak Kaydol" entegrasyonu (Video 2):
  - `ProfilTamamlaViewModel.cs`: `ToptanciMi` boolean bayrağı ve `[Display(Name = "RegisterAsWholesale")]` eklendi.
  - `HesapController.cs`: `ProfilTamamla` GET metodunda mevcut toptancı durumu (`user.BasvuruTarihi.HasValue || user.WholesaleStatus == WholesaleStatus.Approved`) modele aktarıldı. POST metodunda `model.ToptanciMi` seçildiyse ve kullanıcı henüz onaylı/başvurulu değilse `WholesaleStatus.Pending` ve `BasvuruTarihi = DateTime.UtcNow` ataması sağlandı.
  - `ProfilTamamla.cshtml`: `KayitOl.cshtml` tasarımıyla birebir eşleşen toptancı onay kutusu (`التسجيل كتاجر جملة`) kimlik fotoğrafı alanının altına eklendi.

### Faz 20 (Profil Tamamlama & Kayıt Dosya Kaybı UX Çözümü — 25 Eylül 2026)
- [x] **Adım 165**: Form Doğrulama Hatasında Kimlik Fotoğrafı Kaybını Önleme (Video 3):
  - `ProfilTamamla.cshtml` & `KayitOl.cshtml` tam kapsamlı istemci taraflı JavaScript doğrulaması (Client-side validation): Form submit edildiğinde Ad Soyad, Kimlik No, Doğum Tarihi, Telefon, E-posta, Bölge, Şehir, Adres ve Kimlik Fotoğrafı alanları denetlenir. Eksik bir alan varsa `e.preventDefault()` ile formun sunucuya gitmesi ve sayfanın yenilenmesi engellenir; eksik alanın altına kırmızı uyarı mesajı yazılır ve ilk geçersiz öğeye `scrollIntoView` ile odaklanılır. Bu sayede sayfa yenilenmediği için dosya inputundaki seçili kimlik resmi asla kaybolmaz.
  - Canlı hata temizleme (Real-time input clearing): Kullanıcı eksik bir alanı doldurmaya veya select'i değiştirmeye başladığı anda kırmızı kenarlık ve hata mesajı dinamik olarak temizlenir.
  - Canlı dosya görsel geri bildirimi: Kimlik fotoğrafı seçildiği anda boyut (max 8MB) ve format (PNG, JPG, JPEG, WEBP) kontrolü yapılır, onay durumunda `border-emerald-500` ve `bg-emerald-50/20` ile belirginleştirilir, dosya adı yeşil onay ikonuyla (`<i class="fas fa-check-circle"></i> File selected: xxxx.png ✓`) gösterilir.
  - Sunucu taraflı geçici dosya koruması (Server-side fallback): Sunucu tarafında `ModelState.IsValid == false` olsa bile yüklenen yeni dosya hemen `_dosyaServisi.KaydetAsync` ile güvenli depolamaya kaydedilir, `model.MevcutKimlikFotoUrl` içine alınır ve form geri döndüğünde yeşil onay rozeti ("تم إرفاق صورة الهوية مسبقاً (يمكنك تغييرها إن أردت)") devreye girer. Kullanıcı eksik alanı düzeltip tekrar gönderdiğinde dosyayı yeniden arayıp yüklemek zorunda kalmaz.

### Faz 21 (Buton Metni Encoding Bozulması & Toptancı Fiyat Kademesi İnteraktif Dropdown — 25 Eylül 2026)
- [x] **Adım 166**: Buton Metnindeki HTML Entity / Encoding Bozulması Giderildi (Video 4):
  - `Views/Urun/Detay.cshtml` içerisindeki tüm istemci taraflı Razor `@Localizer[...]` ifadeleri JavaScript string context'inde çift HTML-encode edilip ekranda `&#X83A;&#X64A;&#X631; &#X62A;&#X648;&#X641;&#X631;` oluşturması engellendi.
  - Script bloğunun başında `@Html.Raw(System.Text.Json.JsonSerializer.Serialize(...))` ile güvenli JSON sabitleri (`TXT_OUT_OF_STOCK`, `TXT_ADD_TO_CART`, `TXT_PREORDER`, `TXT_MADE_TO_ORDER`, `TXT_SELECT_FRAME`, `TXT_GIFT_PACKAGING`, `TXT_DISCOUNT`, `TXT_FRAME_SELECTION_REQUIRED`, `TXT_ERROR_OCCURRED`, `TXT_CONNECTION_ERROR`, `TXT_LOW_STOCK`, `TXT_COPIED`, `TXT_PRODUCT_LINK`) tanımlandı.
  - Stokta olmayan varyasyon seçildiğinde veya sepete ekleme butonunda temiz ve net Arapça "غير متوفر" gösterilmesi sağlandı.
- [x] **Adım 167**: Toptan Fiyat Kademeleri İnteraktif Açılır Menüye (Dropdown) Dönüştürüldü (Video 4):
  - `Views/Urun/Detay.cshtml` içindeki toptan fiyatlar statik yapıdan çıkarılıp modern, yeşil tonlarında `<select id="wholesaleTierSelect">` açılır listesine dönüştürüldü.
  - Seçenekler `MinAdet+ Adet — Fiyat ₪ / Adet (Varyant)` formatında dinamik listelendi; varsayılan seçenek olarak `StandardRetailPrice (SelectWholesaleTier)` tanımlandı.
  - `SharedResource.ar.resx` ve `SharedResource.en.resx` dosyalarına `StandardRetailPrice`, `SelectWholesaleTier`, `WholesaleTierSelectHelp` anahtarları eklendi.
  - JavaScript dinleyicisi eklenerek toptancı müşteri kademeyi seçtiğinde:
    1) Kademe belirli bir varyanta aitse o varyant kartı otomatik seçildi.
    2) Sipariş adedi kutusu (`productQuantity` ve hidden `selectedQuantity`) kademenin minimum adedine (örn. 50+) otomatik ayarlandı, max sınır gerekiyorsa dinamik genişletildi.
    3) Fiyat göstergesi toptan birim fiyatıyla güncellendi.
    4) Sipariş seçim özeti (`updateSelectionSummary`) otomatik tetiklenerek toptan toplam tutar dinamik hesaplandı.
    5) Standart fiyata dönüldüğünde (`value=""`) `updateProductPricing()` ile perakende fiyat ve varyant durumuna pürüzsüz geri dönülmesi sağlandı.

### Faz 22 (Genel Hediye Paketleme Yönetimi & Vitrin/Sepet Entegrasyonu — 25 Eylül 2026)
- [x] **Adım 168**: Hediye Paketleme Entity & DB Katmanı Genel (Global) Yapıya Taşındı (Video 5):
  - `UrunHediyePaketSecenegi.cs`: `UrunId` alanı nullable (`int?`) yapıldı; `UrunId == null` olan kayıtlar tüm sitedeki ürünlerde geçerli genel hediye paketi olarak tanımlandı.
  - `KanvasDbContext.cs`: `UrunId` ve `Urun` navigation property'si `IsRequired(false)` olarak yapılandırıldı.
  - `Program.cs`: `EnsureMissingMarch2026SchemaAsync` içine `ALTER TABLE "UrunHediyePaketSecenekleri" ALTER COLUMN "UrunId" DROP NOT NULL;` eklendi; tabloda hiç genel paket yoksa varsayılan 3 genel paket (Standart Paket 5 ₪, Lüks Kutu 15 ₪, Özel Tasarım Paket 25 ₪) otomatik tohumlandı.
  - `20260925191912_MakeGiftPackageUrunIdNullable.cs` EF Core migration'ı üretildi ve snapshot güncellendi.
- [x] **Adım 169**: Admin Ürün Ekleme ve Düzenleme Ekranlarından Paketleme Kartı Temizlendi (Video 5):
  - `Areas/Admin/Views/Urun/Duzenle.cshtml` ve `Ekle.cshtml` içerisinden `@await Html.PartialAsync("_GiftPackageOptionEditor", Model)` kaldırıldı.
  - `Areas/Admin/Controllers/UrunController.cs`: `Ekle` ve `Duzenle` POST metotlarındaki ürün bazlı paket eşzamanlama (`SyncGiftPackageOptionsAsync`) bağımlılıkları temizlendi.
- [x] **Adım 170**: Admin Panelinde Tek Noktadan Genel Hediye Paketleme Yönetimi (Video 5):
  - `Areas/Admin/Views/Ayarlar/Index.cshtml` içine yeni `خيارات التغليف العامة` (Genel Hediye Paketleme Seçenekleri) sekmesi eklendi.
  - Mağaza sahibinin paket adı (Arapça / İngilizce), fiyat (₪), sıra ve aktif/pasif anahtarıyla paket ekleyip düzenleyebileceği, satır silme ve toplu kaydetme destekli dinamik yönetim tablosu inşa edildi.
  - `Areas/Admin/Controllers/PaketlemeController.cs` yazılarak `/Admin/Paketleme`, `KaydetJson`, `SilJson`, `TopluKaydet` endpoint'leri sağlandı; `AdminPermissionMatrix.cs` içine yetkileri eklendi.
  - `_AdminLayout.cshtml` masaüstü Ürünler dropdown'una ve mobil menüye doğrudan `خيارات التغليف العامة` bağlantısı eklendi.
- [x] **Adım 171**: Vitrin Ürün Detay & Sepet/Sipariş Süreçleri Genel Paket Desteği (Video 5):
  - `Controllers/UrunController.cs` `Detay` action'ında aktif genel paketler (`UrunId == null && AktifMi && !SilindiMi`) çekilip `ViewBag.HediyePaketleri` ile aktarıldı.
  - `Views/Urun/Detay.cshtml`: Ürünün kendine özel paketi yoksa genel paketler sitedeki istisnasız tüm ürünlerin detay sayfasında `giftPackageSelect` menüsünde otomatik listelendi.
  - `SepetService.cs`: `SepeteEkleAsync`, `GetSepetAsync` ve misafir sepeti birleştirme (`BirlestirAsync`) metotlarında genel hediye paketlerinin seçimi, doğrulaması ve sepet satırına eklenmesi desteklendi.
  - `OrderPricingService.cs`: Sipariş fiyat hesaplamasında genel paketler `globalGiftPackages` olarak çekilerek sepetteki satırlarla eşleştirildi ve güvenli fiyat doğrulaması sağlandı.
  - `GiftPackagePricingTests.cs` içine `Pricing_AcceptsGlobalPackage_ForAnyProduct` birim testi eklendi; tüm 103 test başarıyla geçti.

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

### Faz 18 (Fiyat Rakamlarının Standart / Latin 0-9 Formatına Getirilmesi — 4 Eylül 2026)
- [x] **Adım 146**: `Program.cs` — Arapça kültür (`ar`) yapılandırmasına `DigitSubstitution = DigitShapes.None`, `NativeDigits = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"]` ve `NumberDecimalSeparator = "."`, `CurrencyDecimalSeparator = "."`, `PercentDecimalSeparator = "."` tanımlandı. Böylece sunucu taraflı tüm `@amount.ToString(...)` çağrıları standart 0-9 Latin rakamları ve nokta ayırıcı ile basılır.
- [x] **Adım 147**: `Views/Urun/Detay.cshtml` — `priceDisplay`, `oldPriceDisplay`, hediye paketi, toptan fiyat kademeleri, birlikte alınanlar ve önerilen ürünlerin fiyat çağrıları `ToString("N2", CultureInfo.InvariantCulture)` ile garanti altına alındı. Satır 1326'daki Türkçe `(birim: ...)` metni `isAr ? "للقطعة" : "unit"` dinamik etiketine çevrildi.
- [x] **Adım 148**: `Views/Shared/_Layout.cshtml` — `window.formatMoney` istemci fonksiyonu baştan yazılarak `num.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) + ' ₪'` çıktısı vermesi sağlandı; varyant ve seçenek seçimlerinde fiyatların Doğu Arap rakamlarına (`٩٫٩٩`) dönüşmesi engellendi.
- [x] **Adım 149**: `wwwroot/js/admin.js` — admin arayüzündeki `formatMoney` fonksiyonu da standart Latin rakam formatına getirildi.

### Faz 19 (Twitter / X İkonu & Doğal Varyant Görselleri — 4 Eylül 2026)
- [x] **Adım 150**: `_Footer.cshtml` — Sosyal medya ikon döngüsünde `fa-x-twitter` / `twitter` kontrolü ile doğrudan saf inline SVG X logosu render edildi; vitrinde Twitter / X logosunun görünmemesi sorunu tamamen çözüldü.
- [x] **Adım 151**: `_Layout.cshtml` & `admin.css` — FontAwesome `.fa-x-twitter` fallback mask CSS tanımındaki SVG URI URL-encode (`%3Csvg...`) edilerek modern tarayıcılardaki CSS sözdizimi hatası giderildi.
- [x] **Adım 152**: Yapay şeritli ve metinli görseller (`essence-mascara-*.webp`) diskten silindi; veritabanındaki test varyantları saf, müdahalesiz orijinal ürün fotoğraflarıyla güncellendi.
- [x] **Adım 153**: `Views/Urun/Detay.cshtml` — Fotoğrafı yüklenmemiş varyantlar için `MAIN_PRODUCT_IMAGE` (`Model.AnaGorselUrl`) otomatik fallback mantığı entegre edildi; varyanta fotoğraf yüklenmemişse ürünün ana görselinin gösterilmesi garanti altına alındı.

### Faz 20 (Canlı Öncesi 17 Kritik Hata Düzeltmesi ve Production Hazırlığı — 7 Eylül 2026)
- [x] **Adım 154 (Madde 1)**: `Admin/UrunController.cs` — `IndirimliFiyat <= 0` ve `TopFiyat <= 0` girildiğinde `Admin_Product_PriceGreaterThanZero` ve `DiscountPriceLowerThanOriginal` validasyon hatası vermesi engellendi; değerler validasyondan önce `null` olarak normalize edildi.
- [x] **Adım 155 (Madde 2)**: `Admin/Urun/Duzenle.cshtml` — Ürün düzenleme sayfasındaki `إعدادات الجملة` (Wholesale Settings) kartı tamamen kaldırıldı. Toptancı fiyatlandırması `/Admin/Toptanci/UrunGruplari` sayfasına devredildi.
- [x] **Adım 156 (Madde 3)**: `/Admin/Toptanci/UrunGruplari` — Veritabanındaki Id=5 grubunun açıklaması `المجموعة الأساسية لمنتجات الجملة وخصومات الكميات` olarak güncellendi.
- [x] **Adım 157 (Madde 4)**: `Admin/Kategori/Ekle.cshtml` ve `Duzenle.cshtml` — Kırık HTML grid (`col-lg-6` kapanış eksikliği) düzeltildi, İngilizce etiketler Arapça/İngilizce resx anahtarlarına bağlandı, şık `.ca-file-upload` bileşeni eklendi.
- [x] **Adım 158 (Madde 5)**: `Admin/UrunOzellik` — `UrunOzellikTanimlari` tablosundaki soru işaretli (`?`) kayıtlar Arapça ve İngilizce adlarıyla güncellendi: `اللون (Color)`, `المادة / الخامة (Material)`, `بلد المنشأ (Country of Origin)`, `الوزن / الحجم (Weight / Volume)`, `الضمان والكfالة (Warranty)`.
- [x] **Adım 159 (Madde 6)**: `Admin/Kargo/Index.cshtml` — Bölge ve şehir ekleme/düzenleme formlarındaki çift `__RequestVerificationToken` (FormData + manual append) kaldırıldı; 400 Bad Request hatası tamamen çözüldü.
- [x] **Adım 160 (Madde 7)**: `Siparis/Odeme.cshtml` & `SiteAyarlari` — Checkout başlığındaki zeytin yeşili üzerine koyu metin kontrastı `text-white` ile düzeltildi. `AdreseTeslimAktifMi` ve `MagazadanTeslimAktifMi` ayarları `SiteAyarlari` ve `Admin/Ayarlar` paneline entegre edildi.
- [x] **Adım 161 (Madde 8)**: Banka Havalesi Dekont Yükleme — `HassasBelgeKategorisi.Dekont` (4) eklendi. `IDosyaServisi` üzerinden güvenli depolama ve `POST /checkout/YukleDekont` ile geçici/kalıcı dosya mekanizması kuruldu. `GET /Belge/Dekont?siparisId={id}` güvenli indirme ucu açıldı. `Admin/Siparis/Detay` sayfasına dekont kartı (görsel/PDF önizleme + indirme linki) eklendi.
- [x] **Adım 162 (Madde 9)**: Checkout Kimlik Adımı — `Siparis/Odeme.cshtml` sayfasından `_IdentityVerification.cshtml` kaldırıldı, Sipariş Onayı 3. adım olarak düzenlendi.
- [x] **Adım 163 (Madde 10)**: Mobil Sepet Tasarımı & Yüzen Buton İyileştirmesi — `Sepet/Index.cshtml` mobil cihazlar için yeniden tasarlandı (`pb-28 lg:pb-10`, 9x9 touch kontroller, temiz fiyatlandırma ve yapışkan alt ödeme barı). `_FloatingButtons.cshtml`'de sepet ve ödeme sayfalarında yüzen sepet butonu gizlendi ve WhatsApp butonu yapışkan barın üzerine kaydırıldı.
- [x] **Adım 164 (Madde 11)**: Mail Test Hata Mesajı — `SmtpEmailService.cs`'deki Türkçe sabit hata `"EMAIL_DISABLED"` koduna çevrildi ve `AyarlarController.cs`'de `_localizer["Admin_TestMailDisabled"]` ile çok dilli yapıldı.
- [x] **Adım 165 (Madde 12)**: Footer Ücretsiz Kargo Limiti — `_Footer.cshtml`'deki statik metin dinamik `siteSettings.UcretsizKargoLimiti` değerine bağlandı.
- [x] **Adım 166 (Madde 13)**: Kayıt Formunda Kimlik Fotoğrafı — `Hesap/KayitOl.cshtml`'de kimlik fotoğrafı alanı kırmızı yıldız (*) ve istemci doğrulaması (`IDPhotoRequiredError`) ile zorunlu hale getirildi.
- [x] **Adım 167 (Madde 14)**: Kayıt Formunda Şehir Sıralaması — `Hesap/KayitOl.cshtml`'de Şehir (`Sehir`) alanı Adres (`Adres`) alanının üzerine taşındı.
- [x] **Adım 168 (Madde 15)**: Yorum Baş Harfleri & Yıldızlar — `Views/Urun/Detay.cshtml`'deki `IListSkipTakeSelectIterator` string hatası giderildi ve soru işaretli (`?`) rating gösterimi FontAwesome yıldızlarıyla değiştirildi.
- [x] **Adım 169 (Madde 16)**: Varyant Varsayılan Eşitleme — `Admin/Urun/_VariantEditor.cshtml`'de varsayılan seçilen varyantın fiyat, indirimli fiyat, maliyet, SKU ve stok durumunu ana ürün inputlarıyla dinamik eşitleyen JS geliştirildi.
- [x] **Adım 170 (Madde 17)**: Toptancı İndirim Modeli & Ana Sayfa Fırsatlar Bölümü — Sabit tutarlı indirim desteği (`IskontoTipi = SabitTutar`, `IskontoTutari`) eklendi. `OrderPricingService`, `SepetService` ve admin paneli güncellendi. Ürün detay sayfasına toptan indirim rozetleri eklendi. Ana sayfadaki `AutoFirsatUrunleri` bölümünün indirimli ürünleri eksiksiz getirmesi ve Arapça `اختيارات الحملات` / `منتجات العروض` başlığıyla görüntülenmesi sağlandı.
- [x] **Adım 171**: Genel Test ve Canlı Öncesi Uçtan Uca Doğrulama (7 Eylül 2026):
  - `Views/Shared/_FloatingButtons.cshtml` Razor parser `@media` kaçış hatası (`@@media`) düzeltildi.
  - `Views/Siparis/Odeme.cshtml` içerisinde mükerrer kalan script parçası temizlendi ve 0 syntax hatasına ulaşıldı.
  - `BankaHesaplari` tablosuna Filistin bankaları (Bank of Palestine & Arab Bank) tohumlandı; havale seçildiğinde dinamik IBAN kopyalama ve zorunlu dekont yükleme akışı doğrulandı.
  - `KargoHesaplamaServisi.cs` — `SehirdeAktifKargoVarMiAsync` metodunda tanımlı bölgelerdeki şehirlerin sipariş verebilmesi sağlandı; veritabanında Filistin kargo bölgelerine standart bölgesel teslimat ücretleri (Batı Şeria 20 ₪, Kudüs 25 ₪, 48 Alanları 35 ₪, Gazze 30 ₪) atandı.
  - Mağaza üzerinden uçtan uca gerçek sipariş (Sipariş #14 - 20260907165541164388) oluşturuldu, `/Siparis/Beklemede` onay ekranı ve `/Admin/Siparis` yönetim panelindeki yansıması test edildi.
  - Tüm 26 Admin Controller ekranı (`/Admin/Home`, `Urun`, `Kategori`, `Siparis`, `Kargo`, `Bankalar`, `Personel`, `Rapor`, `Ziyaretci`, vb.) Playwright ile gezildi, 0 konsol hatası ile doğrulandı.
  - Tüm birim testler (`dotnet test`) 98/98 başarılı olarak doğrulandı.

### Faz 21 (Mobil Sepet Mimarisi, Kayıt Bölge/Şehir Motoru, Havale Master Toggle & E-posta Lokalizasyonu — 9 Eylül 2026)
- [x] **Adım 172 (Mobil Sepet Düzeni)**: `Views/Sepet/Index.cshtml` sayfasında mobilde ekranı kaplayan yapışkan alt ödeme çubuğu (`fixed bottom-0`) tamamen kaldırıldı; Sipariş Özeti (`<aside>`) kartı doğrudan sepet ürünlerinin hemen altına ve `اختيارات خاصة` (Özel Seçimler / Çapraz Satış) bölümünün üzerine taşınarak kullanıcının kargo seçimi, kupon girişi ve ödeme butonunu doğrudan görebilmesi sağlandı.
- [x] **Adım 173 (Kayıt Bölge & Şehir Dinamik Motoru)**: `Views/Hesap/KayitOl.cshtml` ve `KayitViewModel.cs` — Şehir alanının üzerine Filistin Bölgeleri (`القدس`, `الضفة الغربية`, `قطاع غزة`, `المناطق الداخلية 48`) dropdown'ı eklendi. Seçilen bölgeye göre şehirlerin istemci tarafında dinamik yüklenmesi JS ile sağlandı; hem Arapça hem İngilizce çift dilli şehir desteği ve model dönüşü için otomatik seçim entegre edildi; zorunlu kimlik fotoğrafı uyarısındaki HTML entity kodlaması saf Arapça `يرجى تحميل صورة بطاقة الهوية لإتمام التسجيل.` mesajına çevrildi.
- [x] **Adım 174 (Teslimat & Havale Ayarları Kalıcılığı)**: `SiteSettingsService.SaveSettings` içerisindeki eksik `AdreseTeslimAktifMi` ve `MagazadanTeslimAktifMi` atamaları düzeltildi; `SiteAyarlari` tablosuna ve modeline `BankaHavalesiAktifMi` eklendi, `Admin/Ayarlar?tab=satis` altından yönetilebilir hale getirildi ve veritabanı yansıması test edildi.
- [x] **Adım 175 (Banka Havalesi Master Toggle)**: `FilistinProje.Web/Areas/Admin/Controllers/BankalarController.cs` ve `Views/Bankalar/Index.cshtml` — Tek tıkla banka havalesini açıp kapatan toggle eklendi. Banka havalesi kapatıldığında `Views/Siparis/_PaymentOptions.cshtml`'de havale radyo seçeneği, banka hesap listesi ve dekont yükleme alanı tamamen gizlendi; `PurchaseOrderService.cs`'de backend tarafında havale ile sipariş verilmesi engellendi.
- [x] **Adım 176 (E-posta Test Mesajlarının Çok Dilli Yapılması)**: `SmtpEmailService.cs` ve `AyarlarController.cs` — Tüm Türkçe exception ve log metinleri teknik sabitlere çevrildi; test e-postası tetiklendiğinde SMTP ayarları eksikse veya servis kapalıysa doğrudan kullanıcının seçili diline (Arapça: `إعدادات SMTP غير مكتملة...`, İngilizce: `SMTP settings are incomplete...`) göre SweetAlert toast gösterimi sağlandı; test butonu tıklandığında kullanıcının aynı `tab=mail` sekmesinde kalması garanti altına alındı.
### Faz 22 (Toptancı Ürüne Özel İndirim, Kategori & Kargo Yönetimi İyileştirmeleri — 9 Eylül 2026)
- [x] **Adım 178 (Toptancı Ürüne Özel İndirim)**: `ToptanciIskontoOrani` entity'sine `UrunId` ve `Urun` navigation eklendi. `KanvasDbContext` ve `Program.cs` schema DDL'i güncellendi. `ToptanciController.cs`'e `GetGrupUrunleri(grupId)` AJAX ucu eklendi. `Toptanci/UrunGruplari.cshtml` view'ına grup seçildiğinde dinamik ürün seçimi dropdown'ı eklendi. `OrderPricingService.cs` ve `SepetService.cs` iskonto motorunda öncelik sırası oluşturuldu (önce ürüne özel iskonto, yoksa grup geneli iskonto).
- [x] **Adım 179 (Kategori Sayfası Tam Lokalizasyon)**: `Admin/Kategori/Index.cshtml` sayfasındaki Türkçe `Yeni Kategori` butonu `@Localizer["Admin_Yeni_Kategori"]` (`فئة جديدة`) yapıldı. `Arama` ve `Kategori Tipi` etiketleri çok dilli resx anahtarlarına bağlandı, sayfada 0 Türkçe metin bırakıldı.
- [x] **Adım 180 (Admin Kargo Sayfası İyileştirmeleri & Şehir Silme Kalıcılığı)**: Şehir silme onay modalındaki eksik `Admin_Delete` yerelleştirme anahtarı `@Localizer["Admin_Sil"]` (`حذف` / `Delete`) ile düzeltildi. `DbSeeder.cs` içindeki `mevcutSehir.SilindiMi = false;` kaldırılarak ve `KargoController.BolgeListesi()`'ne `s => !s.SilindiMi` filtresi eklenerek silinen şehirlerin uygulama yeniden başlayınca geri gelme hatası kalıcı olarak çözüldü. Taşıyıcı formundaki gereksiz takip URL, gönderici unvan, adres, telefon alanları ve kullanılmayan `مصفوفة أسعار المحافظات` (İl Fiyat Matrisi) tamamen temizlendi.
- [x] **Adım 181 (Doğrulama ve Birim Testler)**: `dotnet build FilistinProje.sln` 0 hata 0 uyarı ile derlendi; 98 birim testin tümü eksiksiz geçti (`98/98 passed`); Playwright ile toptancı ürüne özel iskonto tanımlama, kategori arayüzü ve şehir silme kalıcılığı canlı olarak test edildi.

### Faz 23 (Google OAuth Entegrasyonu, Profil Tamamlama Akışı & Kurumsal Sayfalar CMS — 9 Eylül 2026)
- [x] **Adım 182 (Google OAuth Kurulumu & Kimlik Doğrulama)**: `Microsoft.AspNetCore.Authentication.Google` (v10.0.12) paketi `FilistinProje.Web` projesine kuruldu; Google ClientId ve ClientSecret `secrets.json` ve `appsettings.json`'a eklendi; `Program.cs`'de `AddGoogle(...)` servisi yapılandırıldı.
- [x] **Adım 183 (Google ile Giriş & Kayıt Butonları Lokalizasyonu)**: `SharedResource.ar.resx` ve `SharedResource.en.resx` dosyalarına `LoginWithGoogle` (`تسجيل الدخول بواسطة Google` / `Sign in with Google`), `RegisterWithGoogle` (`التسجيل بواسطة Google` / `Sign up with Google`), `Or` (`أو` / `OR`) anahtarları eklendi; `GirisYap.cshtml` ve `KayitOl.cshtml` sayfalarındaki statik butonlar Google SVG ikonuyla birlikte doğrudan `Hesap/ExternalLogin` post formuna bağlandı.
- [x] **Adım 184 (Kayıt Tamamlama / ProfilTamamla Akışı)**: `HesapController.cs`'de `ExternalLogin` ve `ExternalLoginCallback` güncellendi; Google ile giriş yapıldığında bu e-posta adresiyle sistemde **mevcut bir hesap varsa doğrudan giriş yapılır**, kayıt tamamlama ekranı açılmaz; eğer sistemde **hesap yoksa (üye değilse)** otomatik hesap oluşturulup **kayıt tamamlama ekranı (`/Hesap/ProfilTamamla`) açılır**.
- [x] **Adım 185 (Profil Tamamlama Arayüzü & Filistin Şehir Motoru)**: `Views/Hesap/ProfilTamamla.cshtml` oluşturuldu; Tailwind CSS ile markaya uyumlu, RTL/LTR destekli arayüz sağlandı; Filistin bölgelerine (`القدس`, `الضفة الغربية`, `قطاع غزة`, `المناطق الداخلية 48`) göre anlık şehir getiren dinamik JS motoru ve güvenli kimlik kartı yükleme bileşeni entegre edildi.
- [x] **Adım 186 (Kurumsal Sayfalar CMS & PostgreSQL Tohumlama)**: 6 temel kurumsal sayfa (`hakkimizda`, `gizlilik`, `kullanici-sozlesmesi`, `mesafeli-satis`, `iade-kosullari`, `sss`) çift dilli (Arapça & İngilizce) zengin içeriklerle PostgreSQL `"KurumsalSayfalar"` tablosuna ve `DbSeeder.cs`'e tohumlandı.
- [x] **Adım 187 (Kurumsal Sayfalar Yönetim Paneli & Dinamik Vitrin)**: `/Admin/Sayfa` ve `/Admin/Sayfa/Form/{id}` sayfalarındaki Türkçe etiketler çok dilli resx anahtarlarına bağlandı; `KurumsalController.cs` hem `/pages/{slug}` hem `/Kurumsal/Detay/{slug}` rotalarını destekleyecek şekilde dinamik `Detay` aksiyonuna bağlandı; `Detay.cshtml` Tailwind CSS zengin tipografiyle yenilendi.
- [x] **Adım 188 (Uçtan Uca Doğrulama)**: Playwright ile `/Admin/Sayfa`, `/Admin/Sayfa/Form/1`, `/pages/about`, `/Kurumsal/Detay/hakkimizda`, `/Kurumsal/Detay/gizlilik`, `/account/GirisYap`, `/account/KayitOl` ve `/Hesap/ProfilTamamla` canlı olarak test edilip ekran görüntüleri kaydedildi; 98 birim testin tümü başarıyla geçti (`98/98 passed`).

### Faz 24 (Varyant İndirimleri, Teslimat Toggle, Google OAuth Prod Düzeltmeleri & Sıfır Türkçe Log Standardı — 11 Eylül 2026)
- [x] **Adım 189 (Varyant İndirim Türleri & Dinamik Hesaplama)**: `_VariantEditor.cshtml` içinde her varyant için yüzde (%) ve sabit tutar indirim inputları eklendi. `variant-editor.js` ve form submit akışı güncellenerek son satış fiyatının bu indirim alanlarına göre frontend ve backend tarafında senkronize hesaplanması sağlandı.
- [x] **Adım 190 (Teslimat Seçenekleri Toggle Fix)**: `Admin/Ayarlar` teslimat toggle'larının form post ve save mekanizması onarıldı, switch durumlarının veritabanında doğru kalıcı olması sağlandı.
- [x] **Adım 191 (Google OAuth Production & Reverse Proxy Onarımı)**: Canlı sunucuda Google ile giriş tıklandığında 500 hatası alınması sorunu çözüldü:
  - `docker-compose.yml` ve `.env.example` içerisine `GOOGLE_CLIENT_ID` ve `GOOGLE_CLIENT_SECRET` ortam değişkenleri eklendi.
  - `Program.cs`'de Nginx/Docker arkasındaki SSL sonlandırmasını desteklemek için `ForwardedHeadersOptions` güvenilir container/reverse proxy IP ağları (`10.0.0.0/8`, `172.16.0.0/12`, `192.168.0.0/16`, `127.0.0.1`) ile donatıldı.
  - Google Correlation Cookie için `SameSiteMode.Lax` ve CallbackPath (`/signin-google`) güvenli politikası yapılandırıldı.
  - `HesapController.cs`'de `GetExternalAuthenticationSchemesAsync` kontrolü ve try/catch loglama eklenerek sağlayıcı eksik olduğunda 500 çökmesi yerine kullanıcı dostu uyarı gösterildi.
- [x] **Adım 192 (Sıfır Türkçe Log & Katı Dil Kuralı Standardizasyonu)**: Proje genelindeki tüm Türkçe log mesajları, startup uyarıları ve arka plan servis günlükleri İngilizceye çevrildi:
  - `Program.cs` startup uyarıları ve migration logları (PostgreSQL bağlantı uyarısı, schema drift, EF migration, hassas dosya taşıma, seed logları).
  - `DbSeeder.cs`, `SiparisController.cs`, `HesapController.cs`, `HomeController.cs`, `KurumsalController.cs`, `ProfilController.cs`, `UrunController.cs`, `Areas/Admin/Controllers/UrunController.cs`.
  - `SepetService.cs`, `AbandonedCartService.cs`, `FavoriPriceDropService.cs`, `FirebaseNotificationService.cs`, `OrderPricingService.cs`, `PurchaseOrderService.cs`, `SmtpEmailService.cs`, `StockAlertService.cs`.
  - `TurkceIdentityErrorDescriber` sınıfı `LocalizedIdentityErrorDescriber` olarak yeniden adlandırıldı ve refactor edildi.

### Faz 25 (İndirim Geçerlilik Süresi & Vitrin Canlı Geri Sayım Mekanizması — 15 Eylül 2026)
- [x] **Adım 193 (Entity & Dual Migration Uyumlu Şema)**:
  - `FilistinProje.Core/Varliklar/Urun.cs`: `[NotMapped] IndirimBitisTarihi` alias'ı eklendi (`KampanyaBitisTarihi` ile eşzamanlı). `EtkinFiyat`, `IndirimVarMi` ve `IndirimYuzdesi` süre kontrolü doğrulandı.
  - `FilistinProje.Core/Varliklar/UrunSecenek.cs`: `IndirimBitisTarihi` (`DateTime?` UTC) kolonu eklendi; `IndirimVarMi` sürenin geçerliliğini (`!IndirimBitisTarihi.HasValue || IndirimBitisTarihi.Value > DateTime.UtcNow`) kontrol edecek şekilde güncellendi; `EtkinFiyat` ve `IndirimYuzdesi` entegre edildi.
  - EF Core Migration `20260915120603_AddDiscountEndDateToVariants.cs` idempotent `IF NOT EXISTS` kontrolleriyle oluşturuldu ve veritabanına uygulandı (`dotnet ef database update`).
  - `Program.cs` içerisindeki `EnsureMissingMarch2026SchemaAsync` raw SQL bloğuna `"UrunSecenekleri"` için `ALTER TABLE ... ADD COLUMN IF NOT EXISTS "IndirimBitisTarihi"` DDL komutu eklendi.
- [x] **Adım 194 (Çok Dilli Lokalizasyon — AR & EN)**:
  - `SharedResource.ar.resx` ve `SharedResource.en.resx` dosyalarına `DiscountEndsIn`, `DiscountEnded`, `Admin_DiscountEndDate`, `Admin_DiscountEndDateHelp`, `Admin_Product_DiscountEndDateRequired`, `Admin_Product_VariantDiscountEndDateRequired`, `QuickTime_1Day`, `QuickTime_3Days`, `QuickTime_1Week`, `QuickTime_1Month` anahtarları eklendi.
- [x] **Adım 195 (Yönetim Paneli Formları, Hızlı Butonlar & Zorunluluk Validasyonu)**:
  - `UrunController.cs`: `optionalVariantFields` listesine `IndirimBitisTarihi` eklendi; `ValidateProductAsync` ve `ValidateVariantsAsync` metodlarına indirimli fiyat girildiğinde geçerlilik süresinin zorunlu olması şartı eklendi; zaman dilimi dönüşümleri `BusinessTimeZoneService` ile Kudüs yerel saatinden UTC'ye normalize edildi.
  - `Areas/Admin/Views/Urun/Duzenle.cshtml` ve `Ekle.cshtml`: İndirim bitiş tarihi alanı indirimli fiyatın hemen yanına konumlandırıldı; `+1 Gün`, `+3 Gün`, `+1 Hafta`, `+1 Ay` hızlı seçim butonları eklendi; indirimli fiyat girildiğinde zorunluluk yıldızı gösteren ve boş bırakıldığında formu engelleyen JS submit guard entegre edildi.
  - `Areas/Admin/Views/Urun/_VariantEditor.cshtml`: Varyant döngüsüne ve dinamik varyant şablonuna (`#variantCardTemplate`) indirim bitiş tarihi ve hızlı butonlar eklendi.
- [x] **Adım 196 (Vitrin Canlı Geri Sayım Sayacı & Otomatik Fiyat Dönüşü)**:
  - `wwwroot/js/site-countdown.js`: Süre bittiğinde (`diff <= 0`) `countdown:ended` CustomEvent'i fırlatacak ve `#priceDisplay` fiyatını canlı olarak normal satış fiyatına çevirecek şekilde güçlendirildi.
  - `Views/Shared/_Layout.cshtml`: Küresel scriptlere `site-countdown.js` eklendi.
  - `Views/Urun/Detay.cshtml`: Fiyat bloğuna `#productDetailCountdown` rozeti eklendi; varyant radio butonlarına `data-enddate` bağlandı; varyant değiştiğinde sayaç ve fiyatın anlık güncellenmesi sağlandı.
- [x] **Adım 197 (Uçtan Uca Playwright E2E Doğrulama)**:
  - Admin panelinden Ürün #112'ye 200 ₪ normal fiyat, 150 ₪ indirimli fiyat ve +3 gün süre tanımlanarak kaydedildi; vitrinde 150 ₪, üstü çizili 200 ₪, %25 indirim rozeti ve canlı geri sayım sayacı doğrulandı (`step-b-active-countdown.png`).
  - İndirim bitiş tarihi geçmiş tarihe çekilerek kaydedildi; vitrinde fiyatın otomatik normal fiyata (200 ₪) döndüğü, eski fiyatın ve geri sayım sayacının gizlendiği doğrulandı (`step-d-expired-revert-normal-price.png`).
  - Varyant seviyesinde siyah varyanta (`Id: 117`) indirim ve süre tanımlanıp mavi varyant (`Id: 148`) indirimsiz bırakılarak canlı geçiş test edildi; siyah seçildiğinde indirim ve sayaç belirdi, maviye geçildiğinde normal fiyata dönüp sayaç gizlendi (`variant-black-discount-active.png`, `variant-blue-normal-price.png`).
  - Zorunlu alan kontrolü test edildi; indirimli fiyat girilip süre boş bırakıldığında form gönderimi başarıyla engellendi.

### Faz 26 (Kurumsal Sayfalar Görsel WYSIWYG Editör Entegrasyonu & Form Sadeleştirme — 15 Eylül 2026)
- [x] **Adım 198 (Mükerrer Alanların Temizlenmesi & Çok Dilli Sekme Mimarisi)**:
  - `Areas/Admin/Views/Sayfa/Form.cshtml`: Proje sahibinin kafasını karıştıran mükerrer `المحتوى (HTML)` ve genel `عنوان الصفحة` kutuları arayüzden gizlendi (`hidden`); arka planda Arapça içerikle otomatik senkronize edilmesi sağlandı.
  - İki dilli modern sekme yapısı kuruldu: 🇸🇦 `اللغة العربية (الأساسية)` ve 🇬🇧 `English (اختياري)`.
- [x] **Adım 199 (Quill.js Görsel Zengin Metin Düzenleyici Entegrasyonu)**:
  - HTML kodlama zorunluluğu tamamen ortadan kaldırıldı; Word benzeri görsel araç çubuğu (Kalın, İtalik, Başlıklar H1-H4, Listeler, Hizalama, RTL/LTR Yönü, Link Ekleme) entegre edildi.
  - İhtiyaç duyulduğunda kaynak kod görebilmek için "عرض / تعديل كود HTML" toggle butonu eklendi.
  - `SharedResource.ar.resx` ve `SharedResource.en.resx` dosyalarına `Admin_SayfaIcerik`, `Admin_SayfaIcerikHint`, `Admin_ToggleHtml`, `Admin_ArabicContentTab`, `Admin_EnglishContentTab` anahtarları güncellendi/eklendi.
- [x] **Adım 200 (SayfaController Geriye Dönük Uyumluluk & E2E Doğrulama)**:
  - `SayfaController.cs`: `Form` GET aksiyonunda eski sayfalarda `BaslikAr` veya `IcerikAr` boşsa ana `Baslik` ve `Icerik` kolonlarından otomatik doldurulması sağlandı.
  - Playwright MCP ile `/Admin/Sayfa/Form/1` üzerinde yeni görsel editör, sekme geçişleri ve HTML kod toggle butonları test edildi (`sayfa-form-quill-editor.png`); form kaydedilip vitrin sayfasında (`/Kurumsal/Detay/hakkimizda`) içeriğin sorunsuz görüntülendiği doğrulandı (`storefront-corporate-page-verified.png`).

### Faz 27 (Dinamik Kurumsal Sayfalar Footer & ViewComponent Entegrasyonu — 15 Eylül 2026)
- [x] **Adım 201 (KurumsalSayfalarViewComponent Mimarisi)**:
  - `FilistinProje.Web/ViewComponents/KurumsalSayfalarViewComponent.cs`: Kurumsal sayfaları veritabanından (`_context.KurumsalSayfalar`) asenkron ve `AsNoTracking()` ile sorgulayan, silinmemiş (`!SilindiMi`) ve geçerli slug'a sahip (`!string.IsNullOrWhiteSpace(UrlSlug)`) kayıtları sıra numarasına (`OrderBy(x => x.Sira).ThenBy(x => x.Id)`) göre listeleyen bağımsız ve yüksek performanslı bir ViewComponent geliştirildi.
  - `Views/Shared/Components/KurumsalSayfalar/Default.cshtml`: Çoklu dil desteği (`@sayfa.LocalizedBaslik`) ve `/pages/@sayfa.UrlSlug` URL yapısıyla dinamik link render eden şablon oluşturuldu.
- [x] **Adım 202 (_Footer.cshtml Entegrasyonu & Statik Link Temizliği)**:
  - `Views/Shared/_Footer.cshtml`: "الروابط المؤسسية" (Kurumsal / Corporate) başlığı altındaki eski hardcoded/statik linkler (`/pages/about`, `/pages/contact`, `/products`, `/Sozlesmeler/Gizlilik`, `/Sozlesmeler/MesafeliSatis`) kaldırılarak `@await Component.InvokeAsync("KurumsalSayfalar")` çağrısıyla tamamen dinamik hale getirildi.
  - Alt bilgi barındaki (bottom bar) eski `/Sozlesmeler/Gizlilik` linki de modern standart `/pages/gizlilik` URL'sine güncellendi.
- [x] **Adım 203 (Uçtan Uca Playwright E2E Doğrulama)**:
  - Admin panelinden (`/Admin/Sayfa/Form`) yeni bir test sayfası eklendi (Başlık: "شروط الضمان التجريبية", Slug: "test-warranty-policy", Sıra: 7, İçerik: "هذه صفحة اختبار تجريبية لشروط الضمان والخدمة.").
  - Vitrin ana sayfasına (`http://localhost:5002/`) gidilerek sayfa sonuna (Footer) inildi; eklenen yeni kurumsal sayfa linkinin dinamik olarak listelendiği doğrulandı (`footer-corporate-links.png`).
  - Linke tıklandı; tarayıcının `/pages/test-warranty-policy` adresine yönlendiği, sayfa başlığı (`شروط الضمان التجريبية`) ve içeriğinin zengin metin formatında başarıyla görüntülendiği test edildi ve görsel olarak doğrulandı (`corporate-page-detail-verified.png`).
  - Test verisi temizlendi.

### Faz 28 (Değişim Yapılamaz Politikası & Dinamik Renk Seçici Rozeti — 17 Eylül 2026)
- [x] **Adım 204 (Dual Migration & Entity Katmanı)**:
  - `FilistinProje.Core/Varliklar/Urun.cs`: `IsExchangeable` (bool default `true`), `NoExchangeBadgeColor` (string default `'#DC2626'`) ve ters mantığı form bağlamayla köprüleyen `DegisimYapilamazMi` getter/setter eklendi.
  - EF Core Migration `20260917163207_AddNoExchangeFieldsToUrunler.cs` oluşturuldu ve DB'ye uygulandı (`dotnet ef database update`).
  - `Program.cs` `EnsureKnownSchemaDriftAsync` raw SQL bloğuna `ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "IsExchangeable" boolean NOT NULL DEFAULT true;` ve `ALTER TABLE "Urunler" ADD COLUMN IF NOT EXISTS "NoExchangeBadgeColor" text NOT NULL DEFAULT '#DC2626';` eklendi.
- [x] **Adım 205 (Çok Dilli Lokalizasyon — AR & EN)**:
  - `SharedResource.ar.resx` ve `SharedResource.en.resx`: `Admin_NoExchangePolicy`, `Admin_NoExchangePolicyHelp`, `Admin_NoExchangeBadgeColor`, `Admin_NoExchangeBadgeColorHelp`, `Product_NoExchangeNotice`, `Admin_FastPalette`, hazır renk etiketleri eklendi.
- [x] **Adım 206 (Yönetim Paneli Formları & Dinamik Renk Seçici)**:
  - `Areas/Admin/Controllers/UrunController.cs`: `ApplyProductFields` metodunda `IsExchangeable` ve `NoExchangeBadgeColor` eşlendi; `NormalizeProductInput` metodunda `NormalizeBadgeColor` ile renk doğrulaması yapıldı.
  - `Areas/Admin/Views/Urun/Duzenle.cshtml` ve `Ekle.cshtml`: Fiyat ve işlemler sekmesine "غير قابل للاستبدال أو الإرجاع" switch tile'ı, açılır-kapanır yumuşak animasyonlu renk seçici kutusu, HTML5 color input, hex text input, hızlı hazır palet butonları (`#DC2626`, `#EA580C`, `#1F2937`, `#CA8A04`) ve canlı rozet önizlemesi eklendi.
- [x] **Adım 207 (Vitrin Detay Sayfası Şık Uyarı Rozeti)**:
  - `Views/Urun/Detay.cshtml`: Satın alma aksiyonlarının altına ürün `!Model.IsExchangeable` olduğunda adminin seçtiği arka plan rengiyle şık `<i class="fas fa-ban"></i>` ikonlu, kontrastı ayarlanmış (açık/koyu otomatik) çok dilli uyarı rozeti eklendi. Güven rozetleri alanındaki iade garantisi kutusu da bu ürünlerde dinamik olarak "غير قابل للاستبدال أو الإرجاع" olarak güncellendi.
- [x] **Adım 208 (Playwright Uçtan Uca E2E Doğrulama)**:
  - Admin panelinden Ürün #112 düzenlenerek toggle açıldı, `#7C3AED` özel mor renk seçilip kaydedildi.
  - Vitrin detay sayfasında (`/Urun/Detay/test-1-essence-mascara-lash-princess-112`) rozetin mor arka plan (`rgb(124, 58, 237)`), beyaz metin ve Arapça/İngilizce olarak başarıyla görüntülendiği doğrulandı (`element-2026-09-17T16-40-38-641Z.png`).
  - Panelden toggle kapatılıp kaydedildiğinde rozetin vitrinden anında kaybolduğu doğrulandı (`badgeExists: false`).

### Faz 29 (Varyasyon Seviyesinde Toptan Satış İskontosu & Hiyerarşik Fiyatlandırma Motoru — 17 Eylül 2026)
- [x] **Adım 209 (Entity, Dual Migration & Şema)**:
  - `FilistinProje.Core/Varliklar/ToptanciIskontoOrani.cs`: `UrunSecenekId` (`int?`) ve `UrunSecenek` navigation property'si eklendi.
  - `FilistinProje.Core/Varliklar/UrunSecenek.cs`: Dropdown ve tablo rozet gösterimi için `GetDetailedVariantBadge(bool isAr)` metodu eklendi (`اللون: ... / المقاس: ... / القياس: ...`).
  - `FilistinProje.Data/KanvasDbContext.cs`: `ToptanciIskontoOrani` için `UrunSecenekId` indeksi ve `SetNull` silme kuralı yapılandırıldı.
  - EF Core Migration `20260917165846_AddUrunSecenekIdToToptanciIskontoOrani.cs` oluşturuldu ve DB'ye uygulandı (`dotnet ef database update`).
  - `Program.cs` `EnsureMissingMarch2026SchemaAsync` raw SQL bloğuna `ALTER TABLE "ToptanciIskontoOranlari" ADD COLUMN IF NOT EXISTS "UrunSecenekId" integer NULL;` ve `CREATE INDEX IF NOT EXISTS "IX_ToptanciIskontoOranlari_UrunSecenekId"` eklendi.
- [x] **Adım 210 (Backend & Controller)**:
  - `Areas/Admin/Controllers/ToptanciController.cs`: `[HttpGet] GetUrunVaryantlari(int urunId)` endpoint'i eklendi (`Id`, `VaryantBasligi`, `Renk`, `Beden`, `Olcu`, `StokAdedi` JSON).
  - `UrunGruplari()` action'ında `IskontoOranlari` sorgusuna `.ThenInclude(i => i.UrunSecenek)` dahil edildi.
  - `IskontoKaydet` action'ında `UrunSecenekId` parametresi modele bağlandı, varlığı doğrulandı ve veritabanına kaydedildi/güncellendi.
- [x] **Adım 211 (Admin Panel Arayüzü & Çok Dilli Lokalizasyon)**:
  - `SharedResource.ar.resx` ve `SharedResource.en.resx`: `Admin_WholesaleAllVariants` ve `Admin_WholesaleVariantOptionalHint` anahtarları eklendi.
  - `Areas/Admin/Views/Toptanci/UrunGruplari.cshtml`: `#mainUrunSelect` altına dinamik `#mainVaryantContainer` ve `#mainVaryantSelect` alanı eklendi; AJAX ile varyantların yüklenmesi ve sıfırlanması sağlandı.
  - "Kayıtlı İskontolar" tablosunda kural spesifik bir varyanta aitse ürün başlığının yanında `.ca-badge-warning` ile varyant etiketi gösterildi.
- [x] **Adım 212 (Hiyerarşik Fiyatlandırma & Sepet Motoru)**:
  - `OrderPricingService.cs` ve `SepetService.cs`: 3 seviyeli hiyerarşik iskonto önceliği entegre edildi: (1) `UrunSecenekId == secenek.Id`, (2) `UrunId == urun.Id && !UrunSecenekId.HasValue`, (3) `!UrunId.HasValue && !UrunSecenekId.HasValue`.
- [x] **Adım 213 (Birim Testler & Playwright E2E Doğrulama)**:
  - `WholesaleTierPricingTests.cs` içine `CartPricing_HierarchicalWholesaleDiscountPriority_AppliesCorrectly` testi eklendi; varyant kuralının ürün ve grup kurallarını öncelikli olarak ezdiği hem sepet hem sipariş motorunda doğrulandı (`99/99 passed`).
  - Playwright MCP ile `/Admin/Toptanci/UrunGruplari` üzerinde grup ve çoklu varyantlı ürün seçildi, varyant dropdown'ının dinamik olarak yüklendiği (`اللون: أسود`, `اللون: أزرق` vb.), siyah varyanta (`Id: 117`) min. 15 adet ve %18 iskonto girilerek kaydedildiği ve kayıtlı iskontolar tablosunda varyant rozetiyle (`اللون: أسود`) listelendiği test edilip ekran görüntüsü alındı (`wholesale_variant_discount_verified.png`).

### Faz 30 (Hediye ve Özel Paketleme Ücretinin Ayrıştırılması & Şeffaf Fiyatlandırma — 17 Eylül 2026)
- [x] **Adım 214 (Frontend Dinamik Fiyat JS Mantığı & Vitrin Fiyatı İzolasyonu)**:
  - `Views/Urun/Detay.cshtml` & `_ProductInfo.cshtml`: `updatePriceWithFrame()` fonksiyonundaki ek fiyat hesaplamasından `giftPrice` tamamen çıkarıldı; `#priceDisplay` ve `#oldPriceDisplay` hediye paketi seçildiğinde değişmez, saf `basePrice + cerceveFarki` değerinde sabit kalır.
  - `#selectionSummary` özet bileşeni eklendi: varyant, çerçeve farkı, saf ürün birim fiyatı (`#summaryProductPrice`), seçilen hediye paketleme satırı (`#summaryGiftRow`), genel toplam (`#summaryFiyat`) ve matematiksel formül dökümü (`#summaryBreakdown` — örn. `(7.50 ₪ × 2) + (50.00 ₪ × 2) = 115.00 ₪`).
- [x] **Adım 215 (Alışveriş Sepeti & Ödeme Şeffaf Ayrımı)**:
  - `Views/Sepet/Index.cshtml`: Ürün satırında birim fiyat saf ürün fiyatı olarak tutuldu (`7.50 ₪ / قطعة`); hediye paketi ürün başlığı altında altın sarısı kutu ile ayrıştırıldı (`تغليف قياسي: +50.00 ₪ / قطعة (100.00 ₪)`).
  - Sipariş özetinde `المجموع الفرعي` (Subtotal) yalnızca ürünleri kapsar (`15.00 ₪`); `رسوم التغليف` (Packaging Fee) bağımsız bir ara toplam satırı olarak dökülür (`+100.00 ₪`).
  - `Views/Siparis/_OrderSummary.cshtml`: Ödeme adımında `#checkoutPackagingFeeRow` ile paketleme bedeli ayrı bir ara toplam kalemi olarak eklendi.
- [x] **Adım 216 (Müşteri Sipariş Detayı & Fatura PDF Senkronizasyonu)**:
  - `Views/Profil/SiparisDetay.cshtml`: Tablo özetinde ürünler toplamı ve paketleme bedeli ayrıştırıldı.
  - `FilistinProje.Web/Services/FaturaPdfService.cs`: PDF faturada ürünler toplamından paketleme bedeli ayrılarak `"رسوم التغليف / Packaging Fee"` kalemi eklendi.
- [x] **Adım 217 (Çok Dilli Lokalizasyon — AR & EN)**:
  - `SharedResource.ar.resx` ve `SharedResource.en.resx`: `PackagingFee`, `GiftPackaging`, `SelectionSummary`, `ProductUnitPrice`, `TotalPayable`, `CalculationFormula` anahtarları eklendi.
- [x] **Adım 218 (Birim Testler & Playwright E2E Doğrulama)**:
  - `dotnet test FilistinProje.Tests` 99 test 0 hata ile doğrulandı.
  - Playwright MCP ile ürün detayında hediye paketi seçildiğinde `#priceDisplay`'in sabit kaldığı (`7.50 ₪`), seçim özetinde şeffaf formülün basıldığı, sepete eklendiğinde `Subtotal: 15.00 ₪`, `Packaging Fee: +100.00 ₪`, `Total: 115.00 ₪` olarak hesaplandığı, ödeme sayfasında dökümün korunduğu ve İngilizce dilinde hatasız çalıştığı doğrulanarak ekran görüntüleri kaydedildi (`product_detail.png`, `cart_page.png`, `checkout_page.png`).

### Faz 31 (Toptan Satış Grupları Ürün Seçim Listesi ve Ürün-Grup İlişkilendirme Onarımı — 17 Eylül 2026)
- [x] **Adım 219 (Tüm Aktif Ürünleri Getiren Güvenli API Endpoint'i)**:
  - `Areas/Admin/Controllers/ToptanciController.cs`: `[HttpGet] GetTumAktifUrunler()` endpoint'i eklendi (`!u.SilindiMi && u.AktifMi`, `Id`, `Baslik`, `ToptanciUrunGrubuId`, `GrupAdi`).
  - `UrunGruplari()` GET action'ına `ViewBag.TumUrunler` eklenerek sayfa yüklenirken ürün verisinin hazır sunulması sağlandı.
- [x] **Adım 220 (Otomatik Ürün-Grup İlişkilendirme & Grup Ürün Yönetimi)**:
  - `IskontoKaydet` action'ında iskonto tanımlanan ürünün (`model.UrunId`) `ToptanciUrunGrubuId` değeri otomatik olarak hedef gruba eşitlendi.
  - `[HttpPost] GrubaUrunAta(int grupId, int urunId)` ve `[HttpPost] GruptanUrunCikar(int urunId)` action'ları eklenerek grup seviyesinde doğrudan ürün atama/çıkarma desteği getirildi.
- [x] **Adım 221 (Çok Dilli Lokalizasyon — AR & EN)**:
  - `SharedResource.ar.resx` ve `SharedResource.en.resx`: `Admin_CurrentGroup` (`المجموعة الحالية` / `Current Group`), `Admin_GrubaUrunAta` (`تعيين منتج للمجموعة` / `Assign Product to Group`), `Admin_GruptanCikar` (`إزالة من المجموعة` / `Remove from Group`), `Admin_GruptakiUrunler` (`المنتجات في هذه المجموعة` / `Products in this Group`), `Admin_GruptaUrunYok` anahtarları eklendi.
- [x] **Adım 222 (Frontend & Dropdown Dinamik Akışı)**:
  - `Areas/Admin/Views/Toptanci/UrunGruplari.cshtml`: JavaScript akışı yeniden yapılandırıldı; `fetchAllActiveProducts` önbellek mekanizması, `DOMContentLoaded` anında ürünlerin yüklenmesi, grup seçildiğinde (`onMainGroupChange`) tüm aktif ürünlerin listelenmesi ve başka bir grupta olan ürünlerin yanında parantez içinde `(المجموعة الحالية: Grup Adı)` bilgisinin basılması sağlandı.
  - Grup kartları altında "المنتجات في هذه المجموعة" listesi, silme butonu ve hızlı ürün atama dropdown'ı entegre edildi; satır içi iskonto formuna sistemdeki tüm ürünler dahil edildi.
- [x] **Adım 223 (Uçtan Uca Playwright E2E Doğrulama)**:
  - Playwright MCP ile yeni oluşturulan "مجموعة 50 التوفيرية" seçildi; `#mainUrunSelect` açılır menüsünün artık boş gelmediği, 31 ürünün tamamının başarıyla listelendiği doğrulandı.
  - "أحمر الشفاه" ürünü seçilip min. 10 adet ve %15 iskonto kaydedildi; ürünün gruba otomatik bağlandığı, grup altında listelendiği, başka bir grup seçildiğinde ise ürünün yanında mevcut grup adının parantez içinde gösterildiği test edildi ve ekran görüntüsü alındı (`wholesale_product_groups_fixed.png`).

### Faz 32 (Genel Ayarlar Sekmeli Kısmi Güncelleme [Partial Update] & Veri Ezilme Koruması — 17 Eylül 2026)
- [x] **Adım 224 (Servis Katmanı Kısmi Güncelleme [Patch] Mimarisi)**:
  - `FilistinProje.Service/Services/SiteSettingsService.cs`: `ISiteSettingsService` arayüzüne `SaveSettings(SiteAyarlari settings, string? activeTab)` metodu eklendi (geriye dönük tam uyumluluk için varsayılan interface gövdesi tanımlandı).
  - Veritabanındaki mevcut ayarları okuyan (`existing`), yalnızca post edilen sekmenin (`NormalizeTabName`: `genel`, `iletisim`, `sosyal`, `kargo`, `kapida-odeme`, `seo`, `mail`, `bakim`) alanlarını güncelleyen, diğer tüm sekmelerin verilerini ve formda yer almayan boolean alanları koruyan kısmi güncelleme alt metotları (`UpdateGenelSettings`, `UpdateIletisimSettings`, `UpdateSosyalSettings`, `UpdateKargoSettings`, `UpdateKapidaOdemeSettings`, `UpdateSeoSettings`, `UpdateMailSettings`, `UpdateBakimSettings`) geliştirildi.
- [x] **Adım 225 (Admin AyarlarController Sekme Yönetimi & Kargo Güncelleme İzolasyonu)**:
  - `Areas/Admin/Controllers/AyarlarController.cs`: `[HttpPost] Index(SiteAyarlari model, string? aktifSekme, [FromQuery] string? tab)` action'ı güncellendi.
  - `NormalizeTab` helper'ı ile `satis`/`shipping` -> `kargo`, `odeme`/`payment` -> `kapida-odeme` eşitlemesi yapıldı.
  - `VarsayilanKargoFirmasiniGuncelleAsync` işlemi yalnızca kargo sekmesi kaydedildiğinde (`normalizedTab == "kargo"`) çalışacak şekilde izole edildi; diğer sekmelerde gereksiz DB kargo güncellemesi engellendi.
  - Form kaydedildikten sonra kullanıcının işlem yaptığı sekmede kalması sağlandı (`RedirectToAction(..., new { tab = normalizedTab })`).
- [x] **Adım 226 (Admin/Ayarlar/Index.cshtml Bağımsız Form Mimarisi & Senkronize JS)**:
  - Tek ve devasa dış `<form>` yapısı tamamen kaldırıldı.
  - Her sekme paneli (`#panel-genel`, `#panel-iletisim`, `#panel-sosyal`, `#panel-kargo`, `#panel-kapida-odeme`, `#panel-seo`, `#panel-mail`, `#panel-bakim`) kendi bağımsız `<form method="post" asp-action="Index">`, `@Html.AntiForgeryToken()`, `<input type="hidden" name="aktifSekme" value="..." />` ve kendi kart içi "Kaydet" butonuna kavuşturuldu.
  - Ekranın altındaki yapışkan (sticky) bar butonu (`#btnSaveActiveTab`), JavaScript ile o an ekranda aktif olan sekmenin formunu `activeForm.requestSubmit()` yöntemiyle tetikleyecek şekilde bağlandı.
  - Sekme geçişleri (`activateTab`) URL query parametresi (`?tab=...`) ve `window.history.pushState` ile çift yönlü senkronize edildi.
- [x] **Adım 227 (Birim Testler & Çözüm Derleme)**:
  - `FilistinProje.Tests/SiteSettingsPartialUpdateTests.cs`: Kargo sekmesi, Kapıda Ödeme sekmesi ve Genel sekme güncellemelerinin diğer sekmelerdeki alanları ezmediğini (SiteAdi, Telefon, KargoBedeli, KapidaOdemeLimiti, boolean anahtarlar) doğrulayan birim testler yazıldı.
  - `dotnet test` çalıştırıldı, tüm 102 birim test sıfır hata ile başarıyla geçti.
- [x] **Adım 228 (Playwright E2E Uçtan Uca Tarayıcı Doğrulaması)**:
  - Playwright MCP ile `http://localhost:5002/Hesap/GirisYap` üzerinden admin girişi yapıldı, `/Admin/Ayarlar?tab=genel` sayfasındaki ilk değerler okundu (`siteAdi: 7ANRPS48`, `telefon: +970-599-000-000`, `kargoBedeli: 15`, `kapidaOdemeLimit: 1500`).
  - Kargo sekmesine geçilip kargo bedeli 45, ücretsiz kargo limiti 350 yapıldı ve yapışkan bar üzerinden kaydedildi; sayfanın `tab=kargo` sekmesinde kaldığı, kargo değerlerinin güncellendiği, Genel ve Ödeme sekmelerindeki verilerin kesinlikle ezilmediği doğrulandı.
  - Kapıda Ödeme sekmesine geçilip kapıda ödeme limiti 1800 yapıldı ve kart içi kaydet butonuyla kaydedildi; sayfanın `tab=kapida-odeme` sekmesinde kaldığı, limitin güncellendiği, önceki kargo güncellemesi (45 ₪) ve genel ayarların bozulmadan korunduğu tam olarak kanıtlandı.

### Faz 33 (Ürün ve Varyant Fiyatlandırma Mimarisi Refactoring — 18 Eylül 2026)
- [x] **Adım 229 (Admin UI Varyant Editörü & Önizleme Mekanizması)**:
  - `Areas/Admin/Views/Urun/_VariantEditor.cshtml`: Bağımsız `SatisFiyati` inputu satırın sonundan kaldırılıp `FiyatFarki` (+/- Fark) alanı birincil odak alanı haline getirildi. Yanına salt-okunur (`readonly`, `#f1f5f9`) "Nihai Efektif Fiyat Önizlemesi" eklendi.
  - `variantCardTemplate` şablonu da aynı yapıya güncellendi; yeni eklenen varyantlar varsayılan fark 0 ve ana taban fiyat önizlemesi ile oluşturulur.
  - `syncDefaultCheckbox` içindeki `mainFiyat.value = satisFiyati;` ataması kaldırılarak varyant seçiminin ana taban fiyatı ezmesi engellendi.
  - İstemci JS: `input[name="Fiyat"]` ve varyant `FiyatFarki` alanlarına anlık `input` dinleyicileri bağlanarak `(Ana Taban Fiyat + Fiyat Farkı)` formülüyle önizlemelerin ve indirimlerin canlı güncellenmesi sağlandı.
- [x] **Adım 230 (Backend Mantığı & Çift Yönlü Fiyat Senkronizasyonu)**:
  - `Areas/Admin/Controllers/UrunController.cs`: `ResolveVariantSalePrice` metodu `(urun.Fiyat + variant.FiyatFarki > 0 ? urun.Fiyat + variant.FiyatFarki : urun.Fiyat)` mantığıyla baştan yazıldı.
  - `SyncVariantsAsync`: Varyantlar kaydedilirken `SatisFiyati` değeri ana fiyat + fiyat farkı üzerinden otomatik eşitlendi.
  - `SyncProductPricesWithVariantsAsync`: Ana ürün fiyatı (`urun.Fiyat`) güncellendiğinde altındaki tüm aktif varyantların `SatisFiyati` değerleri `ResolveVariantSalePrice` ile otomatik senkronize edildi.
- [x] **Adım 231 (Playwright E2E Uçtan Uca Tarayıcı Doğrulaması)**:
  - Playwright MCP ile `/Admin/Urun/Duzenle/112` sayfasına gidildi, taban fiyat 100 ₪, Varyant 1 farkı 0 ₪, Varyant 2 farkı +25 ₪ olarak ayarlanıp kaydedildi.
  - Vitrin detay sayfasında (`/Urun/Detay/...-112`) Varyant 1 seçildiğinde 100.00 ₪, Varyant 2 seçildiğinde 125.00 ₪ olduğu doğrulandı.
  - Admin paneline dönülüp ana taban fiyat 150 ₪ yapılıp kaydedildi; vitrin sayfasında Varyant 1'in 150.00 ₪'ye, Varyant 2'nin ise otomatik olarak 175.00 ₪'ye yükseldiği teyit edilerek ekran görüntüleri alındı.

### Faz 34 (Toptan Satış Varyant Bazlı İskonto Tanımlama & Hiyerarşik Fiyatlandırma — 18 Eylül 2026)
- [x] **Adım 232 (Entity & Dual Migration Uyumlu Şema Genişletmesi)**:
  - `FilistinProje.Core/Varliklar/ToptanciIskontoOrani.cs`: `UrunSecenekId` (`int?`) ve `UrunSecenek` navigation property'si tanımlandı.
  - `FilistinProje.Data/KanvasDbContext.cs`: `ToptanciIskontoOrani` için `UrunSecenekId` indeksi ve `SetNull` silme kuralı yapılandırıldı.
  - EF Core Migration: `20260917165846_AddUrunSecenekIdToToptanciIskontoOrani.cs` oluşturuldu ve DB'ye uygulandı.
  - `Program.cs` `EnsureMissingMarch2026SchemaAsync`: `ALTER TABLE "ToptanciIskontoOranlari" ADD COLUMN IF NOT EXISTS "UrunSecenekId" integer NULL;`, `CREATE INDEX IF NOT EXISTS "IX_ToptanciIskontoOranlari_UrunSecenekId"` ve `FK_ToptanciIskontoOranlari_UrunSecenekleri_UrunSecenekId` foreign key kontrolü eklendi.
- [x] **Adım 233 (Backend & Servis Katmanı Hiyerarşik Fiyatlandırma)**:
  - `Areas/Admin/Controllers/ToptanciController.cs`: `[HttpGet] GetUrunVaryantlari(int urunId)` endpoint'i (`Id`, `VaryantBasligi`, `Renk`, `Beden`, `Olcu`, `StokAdedi`) optimize edildi; `IskontoKaydet` action'ında formdan gelen `UrunSecenekId` doğrulandı; `UrunGruplari()` sorgusunda `UrunSecenek` ilişkisi `.Include` ile bağlandı.
  - `SepetService.cs` ve `OrderPricingService.cs`: Toptancı sepet ve sipariş fiyatlandırmasında 3 seviyeli katı öncelik sırası uygulandı:
    1. Varyanta Özel İskonto (`UrunSecenekId == secenek.Id`)
    2. Ürüne Özel İskonto (`UrunId == urun.Id && !UrunSecenekId.HasValue`)
    3. Grup Geneli İskonto (`!UrunId.HasValue && !UrunSecenekId.HasValue`)
- [x] **Adım 234 (Admin UI Dinamik Varyant Seçimi & Tablo Rozet Entegrasyonu)**:
  - `Areas/Admin/Views/Toptanci/UrunGruplari.cshtml`: Ürün seçimi (`#mainUrunSelect`) altına dinamik `#mainVaryantContainer` ve `#mainVaryantSelect` eklendi; varsayılan `-- Tüm Varyantlar İçin Geçerli --` seçeneği oluşturuldu; ürün seçildiğinde AJAX ile varyantlar yüklenip varyant yoksa alanın gizlenmesi sağlandı; casing-tolerant (camelCase / PascalCase) güvenliği sağlandı.
  - "Kayıtlı İskontolar" tablosunda kural spesifik bir varyanta aitse ürün başlığının yanında `.ca-badge-warning` rozeti ve etiket ikonu ile varyant başlığı (`GetDetailedVariantBadge`) gösterildi.
- [x] **Adım 235 (Birim Testler & Playwright E2E Uçtan Uca Tarayıcı Doğrulaması)**:
  - `dotnet test FilistinProje.Tests`: 102 test sıfır hata ile başarıyla geçti (`WholesaleTierPricingTests` varyant hiyerarşisi dahil).
  - Playwright MCP ile `/Admin/Toptanci/UrunGruplari` sayfasına gidildi; "مجموعة الجملة الأساسية" ve alt varyantları olan "ايسنس ماسكارا لاش برينسيس" seçildi; varyant açılır menüsünün otomatik dolduğu ve varyantsız ürünlerde gizlendiği test edildi.
  - Varyant 117 (`اللون: أسود`) için 15 adet / %12.5 ve Varyant 148 (`اللون: أزرق`) için 20 adet / %25 iskonto kuralları kaydedildi; her iki varyantın da kayıtlı iskontolar tablosunda bağımsız satırlar halinde ve şık sarı rozetleriyle listelendiği tarayıcıda görsel olarak doğrulandı.
### Faz 35 (Eksik Resx Lokalizasyonları, Varyant & Hediye Paketi E2E Doğrulaması — 19 Eylül 2026)
- [x] **Adım 236 (Lokalizasyon Anahtarları & UTF-8 Bütünlüğü)**:
  - `SharedResource.ar.resx` ve `SharedResource.en.resx`: Eksik olan 47 adet yerelleştirme anahtarı (`Admin_Variation`, `Variant`, `Admin_AreYouSure`, `Admin_DocumentUploaded`, `Profile_*`, `Contract_*`, `UserAgreement_*` vb.) eklendi.
  - `Admin/Siparis/Detay` sayfasındaki raw `Admin_Variation` başlığı Arapça `المتغير` olarak çözümlendi.
  - Vitrin ürün detayındaki `Variant:` etiketi Arapça modda `المتغير:` olarak dinamik ve doğru lokalizasyonla gösterildi.
- [x] **Adım 237 (Uçtan Uca Playwright Testleri & Sepet/Checkout Doğrulaması)**:
  - Ürün Detay: Varyant seçimi ile fiyat dinamik değişimi (150 ₪ -> 175 ₪), hediye paketi seçimi (+50 ₪ / parça) ve seçim özeti (`ملخص الاختيار`) matematiksel olarak doğrulandı.
  - Sepet (`/Sepet`): 2 adet siyah (400 ₪) + 1 adet mavi (225 ₪) ürün, 150 ₪ paketleme ücreti, 475 ₪ ara toplam, 625 ₪ genel toplam ve ücretsiz kargo barajı testi hatasız geçti.
  - Ödeme (`/Siparis/Odeme`): Filistin şehirleri, Bank of Palestine / Arab Bank IBAN seçenekleri, kapıda ödeme bedeli (+15 ₪), belge yükleme formatları ve özet kartı doğrulandı.
  - `dotnet test`: 102/102 test sıfır hata ile geçti. Çözüm derlemesi `0 Hata` ile tamamlandı.

### Faz 36 (Proje Sahibi Video Talepleri & İndirim Sayacı Tasarımı — 20 Eylül 2026)
- [x] **Adım 238**: Paketleme Seçenekleri UI Güncellemesi (`Views/Urun/Detay.cshtml`): Radyo buton kutuları yerine modern açılır menü (`<select id="giftPackageSelect">`) formatına geçildi; seçenekler `Ad (+Fiyat ₪ / adet)` formatında dinamik listelendi; başlığa `(يتم إضافة سعر التغليف على المنتج)` notu eklendi; sepet ve özet hesaplama JS motorları select menüsüne bağlandı.
- [x] **Adım 239**: Toptan Fiyat Kademeleri Tablo Tasarımı (`Views/Urun/Detay.cshtml`): Dağınık 2 kolonlu kart gridi yerine derli toplu, şık ve okunaklı bir mini tablo (`table`) tasarımına dönüştürüldü; sütunlar `@Localizer["WholesaleMinQty"]` ve `@Localizer["WholesaleUnitPrice"]` olarak yapılandırıldı.
- [x] **Adım 240**: Vitrin Varyasyon Tıklama & Out-of-Stock Satın Alma Durumu Onarımı (`Views/Urun/Detay.cshtml`): Stoğu biten varyasyonlardaki `disabled` input kısıtlaması kaldırılarak `data-purchasable="false"` modeline geçildi; kullanıcı artık varyasyona tıklayıp resmini ve "Tükendi" uyarısını görebiliyor, sepete ekleme butonu kontrollü olarak devre dışı kalıyor; `updateDiscountDisplays` null-safety sağlandı, varsayılan varyant seçimi mantığı onarıldı.
- [x] **Adım 241**: İndirim Bitiş Tarihine Saatlik Hızlı Butonlar: `Admin/Urun/Duzenle.cshtml`, `Admin/Urun/_VariantEditor.cshtml` ve `Admin/Urun/Ekle.cshtml` dosyalarında gün butonlarının (`+1d, +3d, +1w, +1m`) önüne saat butonları (`+1h, +3h, +6h, +12h` / `+1 س, +3 س, +6 س, +12 س`) eklendi; JS motoru `data-hours` desteğiyle `now.setHours(...)` formülüne kavuşturuldu.
- [x] **Adım 242**: Admin Ürün Düzenleme Sabit Görsel Yükleme Kolaylığı (`Duzenle.cshtml`): Sağ taraftaki sabit ürün önizleme kartının hemen altına `Admin_UploadNewMainImage` butonu eklendi; admin kullanıcısı sekme değiştirmeden tek tıkla ana ürün görselini güncelleyebiliyor.
- [x] **Adım 243**: İndirim Sayacı ve Rozeti Canlı Kırmızı Tasarım Güncellemesi & Çift İki Nokta Onarımı (`Views/Urun/Detay.cshtml`, `Views/Shared/_CountdownPartial.cshtml`):
### Faz 37 (Video 6 — Toptan Ürün Grupları UX & İskonto Arayüz Yenilemesi — 26 Eylül 2026)
- [x] **Adım 244 (Bilgilendirme Bannerı & Toptan İskonto Mantığı Açıklığı)**:
  - `Admin/Toptanci/UrunGruplari.cshtml`: Sayfa başına toptan gruplarının amacını, sepette kademeli iskontoların nasıl işlediğini anlatan rehber info banner'ı eklendi.
  - 3 adet bilgilendirme rozeti (Kademeli indirim kuralları, sepette otomatik uygulama, tüm grup veya ürüne özel hedefleme) eklendi.
- [x] **Adım 245 (Kayıtlı Gruplar Tablosunda Doğrudan İskonto & Ürün Görünürlüğü)**:
  - İskontolar gizli collapse arkasından çıkarılarak doğrudan grup satırında şık, renkli etiketler (chips/badges) olarak gösterildi (Örn: `10+ %5 خصم`, `25+ ₪25 خصم`).
  - İskonto tanımlanmamış gruplarda açık ve şık gri bilgi mesajı (`لم يتم تحديد خصومات بعد`) gösterildi.
  - Her grup için ürün sayısı butonu (`X منتج`) ve ilk ürünlerin başlık önizlemesi eklendi.
- [x] **Adım 246 (Grup Ürünlerini Yönetme Modalı - Hızlı İşlem)**:
  - Grup satırındaki ürün butonuna veya işlem menüsündeki "منتجات المجموعة" linkine tıklandığında açılan `#grupUrunleriModal` eklendi.
  - AJAX ile gruptaki ürünler (resim, başlık, fiyat, toptan fiyat) anında yükleniyor; modal içinden tek tıkla ürün gruba atanabiliyor veya çıkarılabiliyor.
  - `ToptanciController.cs`: `GrubaUrunAta`, `GruptanUrunCikar` ve `GetGrupUrunleri` action'ları AJAX ve zengin ürün verisi dönecek şekilde güncellendi.
- [x] **Adım 247 (İskonto Ekle Formu UX & Client-side Validation)**:
  - Formun başına açıklayıcı Info Alert eklendi.
  - Kapsam seçimi ("تطبيق على جميع منتجات المجموعة" vs "تطبيق على منتج محدد فقط") toggle düğmeleriyle sadeleştirildi; varsayılan olarak ürün listesi gizlenerek kafa karışıklığı önlendi.
  - Hızlı adet butonları (`5+`, `10+`, `25+`, `50+`) eklendi.
  - İskonto tipi (% vs ₪) dinamik geçişi sağlandı.
  - `novalidate` + JavaScript client-side validasyonu ile grup seçilmeden veya adet girilmeden kaydetme girişiminde postback olmadan anında kırmızı çerçeve ve uyarı gösterilmesi sağlandı.
- [x] **Adım 248 (Lokalizasyon & Playwright E2E Canlı Doğrulama)**:
  - `SharedResource.ar.resx` ve `SharedResource.en.resx` dosyalarına 26 yeni anahtar eklendi; en.resx'teki eksik Arapça başlıklar (`Admin_Islem` -> Action, `Admin_Duzenle` -> Edit) düzeltildi.
  - Playwright MCP ile canlı sitede (`7anrps48.com`) admin girişi yapılarak AR ve EN dillerinde tüm formlar, modal açılışı, AJAX ürün listeleme, hızlı butonlar ve canlı iskonto rozetleri uçtan uca test edildi, ekran görüntüleriyle doğrulandı.
  - `dotnet build`: 0 Hata ile başarıyla derlendi.

### Faz 38 (Video 4 — Buton Metni Encoding Bozulması & Toptancı Fiyat Kademesi İnteraktif Seçici — 26 Eylül 2026)
- [x] **Adım 249 (Sepete Ekle Buton Metni Encoding Bozulması Düzeltmesi)**:
  - `Views/Urun/Detay.cshtml`: Razor içinde string interpolasyonu ile JS'e aktarılan metinlerin HTML entity encode (`&#X83A;&#X64A;&#X631; &#X62A;&#X648;&#X641;&#X631;`) edilerek butona basılması sorunu tespit edildi.
  - Razor'da `@Html.Raw(System.Text.Json.JsonSerializer.Serialize(...))` formatına geçilerek `TXT_OUT_OF_STOCK`, `TXT_ADD_TO_CART`, `TXT_STOCK_EXCEEDED` ve `TXT_UNIT_LABEL` JS sabitleri oluşturuldu.
  - Stokta olmayan varyasyon seçildiğinde veya stok sıfır olduğunda butonun temiz Arapça `"غير متوفر"` metnini göstermesi sağlandı.
- [x] **Adım 250 (Toptan Fiyat Kademeleri İnteraktif Seçici Dönüşümü)**:
  - Toptan kademeleri statik bilgi tablosundan tıpkı paketleme seçeneğinde olduğu gibi interaktif `<select id="wholesaleTierSelect">` menüsüne dönüştürüldü.
  - Kademe seçildiğinde:
    - Adet otomatik olarak kademe minimum adedine çekilir (`#productQuantity` & `#selectedQuantity`).
    - Birim fiyat vitrin fiyatında dinamik güncellenir (`#priceDisplay`).
    - Kademe belirli bir varyanta bağlıysa varyant otomatik olarak seçilir ve varyant butonları güncellenir.
    - Seçim özeti kutusu (`#selectionSummary`) otomatik açılır ve toplam tutarı kuruşu kuruşuna gösterir.
  - "السعر الأساسي للقطعة" (Standart Perakende Fiyatı) seçildiğinde temizce perakende moduna geri döner.
- [x] **Adım 251 (Playwright Canlı Site (7anrps48.com) Uçtan Uca Doğrulama)**:
  - Playwright MCP ile canlı sunucudaki `https://7anrps48.com/products/test-3-powder-canister-114` ürünü test edildi.
  - Kademe 4 (50+ adet @ 30.00 ₪) seçimi: Otomatik Varyant 155 (اسود) seçildi, adet 50 yapıldı, birim fiyat 30.00 ₪ oldu, özet kutusunda `1,500.00 ₪` başarıyla hesaplandı.
  - Kademe 5 (150+ adet @ 28.00 ₪) seçimi: Otomatik Varyant 119 (احمر) seçildi, adet 150 yapıldı, birim fiyat 28.00 ₪ oldu, özet kutusunda `4,200.00 ₪` başarıyla hesaplandı.
  - Perakendeye dönüş testi: 35.00 ₪ vitrin fiyatına başarıyla dönüldü.
  - Buton encoding testi: Stoksuz varyantta butonun HTML entity içermediği ve tam olarak `"غير متوفر"` yazdığı doğrulandı.
  - Seçim özetindeki `unitLabel` için de `TXT_UNIT_LABEL` (`للقطعة`) sanitizasyonu uygulandı.
  - `dotnet build`: 0 Hata, 0 Uyarı ile derlendi.

