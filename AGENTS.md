# AGENTS.md — 7ANRPS48.com (Filistin e-ticaret)

## Project overview

- **Stack**: ASP.NET Core 8.0 MVC + PostgreSQL (Docker) + TailwindCSS
- **Architecture**: Clean Architecture (4 projeler: Web → Service → Data → Core)
- **Brand**: 7ANRPS48.com (Filistin e-ticaret, eski Canvasia/MeteorGaleri fork)
- **Para birimi**: ₪ (ILS - Yeni İsrail Şekeli)
- **Diller**: Arapça (varsayılan, RTL), İngilizce (LTR), Türkçe (LTR) — `IStringLocalizer<SharedResource>` kullanılır
- **Entity property isimleri**: Türkçe PascalCase (`Urun.Baslik`, `Kategori.Ad`, `SiteAyarlari.GirisZorunluMu`)
- **Kimlik doğrulama**: ASP.NET Core Identity (`AppUser : IdentityUser`)

## Quick start

```bash
docker-compose up -d db
cd KanvasProje.Web && dotnet watch run   # http://localhost:5002

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
dotnet build KanvasProje.sln
dotnet ef migrations add <Name> --project KanvasProje.Data --startup-project KanvasProje.Web
dotnet ef database update --project KanvasProje.Data --startup-project KanvasProje.Web
cd KanvasProje.Web && npm run watch:storefront-css
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

### Program.cs (önemli satırlar)
- `~satır 784-1060` — `EnsureMissingMarch2026SchemaAsync`: hand-rolled SQL, tüm ek kolonları/tabloları kapsar (ReceteGerekliMi, WhatsappSiparisVarMi, FiyatGizliMi, ToptanciMinSiparisTutari, ToptanciUrunGrubuId, ToptanciUrunGruplari, ToptanciIskontoOranlari, BasvuruTarihi, KargoBolge.Ulke/Aciklama, Slayt dil alanları)
- `~satır 460-510` — Migration + Seed uygulama mantığı
- `~satır 700+` — Login middleware (GirisZorunluMu kontrolü)
- `~satır 40-45` — DB erişilebilirlik kontrolü

## Developer notları (AI için)

1. **WholesaleStatus** enum'u `KanvasProje.Core/Enums/` altında. Yeni enum'lar da aynı yere eklenmeli.
2. **View'da enum referansı**: `KanvasProje.Core.Enums.WholesaleStatus` — tam nitelikli kullan.
3. **DbSeeder** tüm rolleri `AdminSecurityRoles.AllRoles` listesinden seed eder. Yeni rol eklenirse bu listeye ekle.
4. **Wholesale** rolü admin rolü DEĞİLDİR. `AllAdminRoles`'a ekleme, `AllRoles`'a ekle.
5. **AdminBaseController** tüm admin controller'ların base class'ıdır. Attribute'ları (`[Authorize]`, `[Area]`) zaten içerir. Sadece `: AdminBaseController` yap yeter.
6. **Yeni bir admin controller eklerken**: (a) `: AdminBaseController` yap, (b) `AdminPermissionMatrix`'e controller adını ekle, (c) `AdminBaseController`'da ViewBag değişkenini set et, (d) `_AdminLayout.cshtml`'e link ekle.
7. **View'lar Türkçe** yazılır (admin paneli için localizer kullanılmıyor).
8. **Build'den önce** `dotnet build KanvasProje.sln` ile kontrol et. 0 hata 0 uyarı hedefi.
9. **StokBiteniGriGoster** (`SiteAyarlari`): Admin panelden yönetilir. true=stoğu biten varyasyonlar gri+tükendi rozeti gösterilir, false=tamamen gizlenir. `StoktaYokSatisIzni` true ise bu ayar devre dışı kalır (tüm varyasyonlar seçilebilir).
10. **HediyePaketi** akışı: `Urun.HediyePaketiVarMi` + `HediyePaketFiyati`. SepetItem ve SiparisDetay'da `HediyePaketi` (bool) ve `HediyePaketFiyati` (decimal) alanları.
11. **WhatsappSiparisVarMi** — Ürün bazında WhatsApp sipariş modu. `FiyatGizliMi` ile birlikte çalışır: true ise ürün fiyatı gizlenir, "WhatsApp ile Sipariş Ver" butonu gösterilir.
12. **ToptanciUrunGrubu** — Toptancı ürün grupları, `ToptanciIskontoOrani` ile adet bazlı iskonto yüzdesi tanımlanır. `ToptanciMinSiparisTutari` toptancı için minimum sipariş tutarıdır.
13. **ReceteGerekliMi** (Kategori) — Bu kategoriye ait ürünler için reçete yükleme zorunluluğu.
14. **Slayt dil alanları** — `BaslikEn/Ar`, `AltBaslikEn/Ar`, `AciklamaEn/Ar`. Slayt entity'sinde `LocalizedBaslik`, `LocalizedAltBaslik`, `LocalizedAciklama` computed property'leri mevcut.
15. **Tüm view'lar güncel** — `Detay.cshtml`, `Siparislerim.cshtml`, `Adreslerim.cshtml`, `Basarili.cshtml`, `Basarisiz.cshtml` modern, IStringLocalizer kullanır durumda.
16. **Admin URL'leri**: `/Admin/Slayt`, `/Admin/Kargo`, `/Admin/Rapor`, `/Admin/Iletisim`, `/Admin/Iade`, `/Admin/Kupon`, `/Admin/HomeSections`, `/Admin/Bulten`, `/Admin/Sayfa`, `/Admin/UrunOzellik`, `/Admin/TopluFiyatGuncelle`, `/Admin/SlugTool`, `/Admin/Yorum`, `/Admin/Ziyaretci`, `/Admin/XyzSecretMonitor`
