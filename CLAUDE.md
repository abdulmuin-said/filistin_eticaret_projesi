# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

**7ANRPS48** (formerly *CANVASİA* / *MeteorGaleri*) — ASP.NET Core 8.0 MVC veterinary e-commerce site, backed by PostgreSQL.

- **Domain:** Production domain is configured through deployment environment variables.
- **Audience:** Palestinian veterinarians, pet owners, farmers
- **Currency:** ILS (₪)
- **Languages:** Arabic (default), English — RTL supported (`dir="rtl"` when `lang="ar"`). Turkish (`tr`) is still in `Program.cs` but is being phased out.
- **Shipping zones:** Interior 48, West Bank, Jerusalem
- **Deployment:** Docker Compose on Linux server (port 80 → container 8080)

Authoritative reference: `PROJE_DOKUMANTASYONU.txt` (Turkish) — DB credentials, SQL restore commands, URL patterns, known issues. Consult before guessing.

## Commands

### Local development (hybrid: DB in Docker, web on host)
```bash
docker compose up -d db                # PostgreSQL 16 Alpine on port 5434
cd FilistinProje.Web && dotnet watch run  # http://localhost:5002
```

### Full Docker stack (production-like)
```bash
docker compose build --no-cache
docker compose up -d                    # web on http://localhost:80 (→:8080)
```

### Restore the bundled SQL dump into the `db` container
```bash
# Full dump (schema + data) — preferred for fresh install:
Get-Content FilistinProje.Web/App_Data/filistindb_full.sql | docker exec -i filistinproje-db psql -U kanvasuser -d filistindb
```

### Build / migrations / tailwind
```bash
dotnet build FilistinProje.sln
dotnet ef migrations add <Name> --project FilistinProje.Data --startup-project FilistinProje.Web
dotnet ef database update --project FilistinProje.Data --startup-project FilistinProje.Web
cd FilistinProje.Web && npm run watch:storefront-css   # Tailwind → wwwroot/css/storefront.css
```

There is **no test project** in the solution — don't fabricate `dotnet test` instructions.

### Server update (after local changes pushed to GitHub)
```bash
# On server via SSH:
cd ~/filistin_projesi/filistin_eticaret_projesi
git pull origin main
docker compose build --no-cache
docker compose up -d
```
Detailed server update steps in `GUNCELLEME_ADIMLARI.md`.

## Architecture

Clean Architecture with four projects referenced top-down (Web → Service → Data → Core):

- **FilistinProje.Core** — Entities (`Varliklar/`), DTOs, interfaces, helpers. No framework dependencies beyond EF Core abstractions.
- **FilistinProje.Data** — `KanvasDbContext`, EF Core migrations (Npgsql), generic repository + UnitOfWork pattern.
- **FilistinProje.Service** — Business logic services, AutoMapper profiles, `SepetService` (DB-backed cart).
- **FilistinProje.Web** — MVC controllers, Razor views, Identity, an `Admin` Area, and startup pipeline in `Program.cs`.

### Key NuGet packages (Web layer)
EPPlus (Excel import/export), Hangfire (background jobs), QuestPDF (fatura PDF), Serilog (logging), ImageSharp (image processing), Newtonsoft.Json.

### Runtime pipeline highlights (`FilistinProje.Web/Program.cs`)
- Loads `secrets.json` as an additional config source before env vars.
- Probes the Postgres connection at startup; if unreachable, **Hangfire and the `AbandonedCartService` hosted service are disabled** instead of crashing. Preserve this degraded-start behavior when touching startup code.
- On startup it runs `context.Database.Migrate()` **and** `EnsureMissingMarch2026SchemaAsync` — a hand-rolled idempotent `DO $$ ... $$` block that adds columns/tables to cover drift between the migration history and the expected schema. **If you add columns to `Urunler`, `Kategoriler`, `UrunSecenekleri`, or `UrunResimleri`, mirror them in that SQL block** or it will diverge in prod.
- Identity uses `AppUser` + `IdentityRole`, Turkish error descriptions (`TurkceIdentityErrorDescriber`), 30-day sliding cookie, 5-try lockout.
- Admin-area auth redirects are intercepted to emit JSON 401/403 for `/api/admin/*` and log via `IAdminSecurityAuditService`.
- Rate limiter policies: `"auth"` (10/5min per IP) and `"general"` (100/min per IP).
- Global maintenance-mode middleware short-circuits non-admin/non-auth traffic based on `ISiteSettingsService`.
- HTTPS redirect is skipped when `DOTNET_RUNNING_IN_CONTAINER=true` (the reverse proxy handles TLS).
- Hangfire dashboard at `/admin/hangfire` with `LocalRequestsOnlyAuthorizationFilter`.
- Localization: supported cultures in `Program.cs` are `ar`, `en`, `tr` (line ~232–241). Default is `"ar"`. The `tr` culture is being phased out — target state is AR + EN only.

### Persistence conventions (PostgreSQL, quoted identifiers)

Tables and columns use **Turkish PascalCase** and require double quotes in raw SQL: `"Urunler"`, `"Kategoriler"`, `"UrunResimleri"`, `"SepetItems"`, `"AspNetUsers"`, etc.

Property names that are easy to get wrong:
- `Urun.Baslik` (product name, **not** `Ad`/`Name`), `Urun.Slug`, `Urun.Fiyat`, `Urun.IndirimliFiyat`, `Urun.EtkinFiyat`, `Urun.AnaGorselUrl`
- `UrunResim.ResimYolu` (image path, **not** `Url`/`ImageUrl`), `UrunResim.Sira`
- `Kategori.Ad`, `Kategori.Slug`
- `Slayt.LocalizedBaslik` / `.LocalizedAltBaslik` / `.LocalizedAciklama` — computed from current culture, reading `Baslik`/`BaslikEn`/`BaslikAr` fields

Multi-language entity pattern: each translatable entity has `Property`, `PropertyEn`, `PropertyAr` fields. A helper extension reads the right one based on `Thread.CurrentThread.CurrentCulture`.

### URL patterns (public site)
`/Urun` (list) · `/Urun/Detay/{slug}-{id}` · `/Urun?k={kategoriId}` · `/Urun?s={arama}` · `/Sepet` · `/Siparis/Odeme` · `/Hesap/GirisYap` · `/Hesap/KayitOl` · `/Profil/*` · `/Favori` · `/Kurumsal/Iletisim` · `/admin/*` (Area).

Turkish URL segments (Urun, Sepet, Siparis, etc.) are permanent — **never change URLs, only localize UI text**.

## Admin Area (`/Areas/Admin/`)

Full-featured admin panel with controllers for:
**Dashboard, Siparişler, Ürünler (CRUD + Excel import + variant/feature/media editors), Kategoriler, Slaytlar (hero slider), Kuponlar, İade Yönetimi, Kargo Bölgeleri, Kullanıcılar, Personel (yetki matrisi), Raporlar, Ayarlar, Bülten, Yorumlar, Toplu Fiyat Güncelleme, Toptancı Yönetimi, Çark Ödülü, Push Bildirim, Sayfalar, Slug Tool, URL İzleme.**

### Layout
- `_AdminLayout.cshtml` — sidebar navigation, separate from public layout
- `AdminBaseController` — base with auth checks, common view data

## Configuration & secrets

- `.env` / `.env.example` — docker-compose values: Postgres credentials and SMTP settings. **Never commit `.env`.**
- `FilistinProje.Web/secrets.json` — local dev DB connection string (gitignored; do not commit real values).
- `appsettings.json` — production defaults, EmailSettings (SMTP), Firebase project config.
- Docker container names: `filistinproje-db`, `filistinproje-web`.
- Docker volumes: `filistin_postgres_data`, `filistin_app_uploads` (`/app/wwwroot/img/products`), `filistin_app_media` (`/app/wwwroot/media/products`), `filistin_app_logs`, `filistin_app_data`.
- Default local connection: `Host=localhost;Port=5434;Database=filistindb;Username=kanvasuser;Password=changeme_in_production`.

## Payments

İyzico integration (`IyzicoPaymentService`) in **sandbox mode** — `ApiKey` in `appsettings.json` is a placeholder until production keys land.
Payment flow: Sepet → `/Siparis/Odeme` (address + confirm) → İyzico redirect → `/Siparis/Basarili` or `/Siparis/Basarisiz`.

## Frontend

### Tailwind CSS & Brand Tokens
- **Tailwind CSS v3.4** — built locally via `npm run build:storefront-css` → `wwwroot/css/storefront.css`. Config in `tailwind.config.js`.
- **`site.css` is removed** from `_Layout.cshtml` — do not re-add it.
- All CSS must use brand tokens from `tailwind.config.js`, **never** bare `bg-[]` / `text-[]` / `hover:bg-[]` empty Tailwind values.

**Brand color tokens** (`brand.*` prefix in Tailwind):
| Token | Hex | Use |
|-------|-----|-----|
| `brand-primary` | `#1a2b1b` | — |
| `brand-olive` | `#313511` | Primary text, buttons |
| `brand-gold` | `#b58735` | Accents, prices |
| `brand-cream` | `#fcf9f3` | Page backgrounds, breadcrumbs |
| `brand-warmcream` | `#f7f1e6` | Hover states, avatar bg, card accents |
| `brand-lightgold` | `#c6ca99` | Subtle accents |
| `brand-darktext` | `#1c1c18` | Body text |
| `brand-mutedtext` | `#47473d` | Secondary text, placeholders |
| `brand-border` | `#e5e2dc` | Default borders |
| `brand-darkborder` | `#d8c9aa` | Stronger borders |

Standard hover pattern for secondary buttons: `hover:bg-brand-warmcream`
Primary button dark hover: `hover:bg-[#1c2001]` (acceptable inline hex for olive darkened)

### Localization resources
Three `.resx` files in `FilistinProje.Web/Resources/`:
- `SharedResource.ar.resx` — Arabic (primary)
- `SharedResource.en.resx` — English
- `SharedResource.tr.resx` — Turkish (being phased out)

All UI text in views **must** use `@Localizer["Key"]`. Hardcoded Turkish strings in `.cshtml` files are a known debt to eliminate.

### Fonts & Icons
- Font: **Cairo** (headings + body, Google Fonts) — defined in `tailwind.config.js` under `fontFamily.heading/body`. Also `IBM Plex Sans Arabic` in `fontFamily.sans`.
- Icons: **Font Awesome 6.4.0**

### Main layout
`Views/Shared/_Layout.cshtml` — sticky header, mega-dropdown category nav (DB-driven), cart count via `SepetService`, AR/EN/TR language switcher, WhatsApp floating button. This file is ~1400 lines and a candidate for partial extraction.

## Active Development Plan

Current priorities (in order):

### Faz 2: View'ları Partial'lara Böl
Split large view files into `@await Html.PartialAsync(...)` partials:
- `_Layout.cshtml` → `_Header.cshtml` + `_MobileNav.cshtml` + `_Footer.cshtml`
- `Views/Urun/Detay.cshtml` (1600 lines) → `_ProductGallery.cshtml` + `_ProductInfo.cshtml` + `_Reviews.cshtml`
- `Views/Siparis/Odeme.cshtml` (1200 lines) → `_AddressForm.cshtml` + `_PaymentOptions.cshtml` + `_IdentityVerification.cshtml` + `_OrderSummary.cshtml`

### Faz 3: Remove Turkish, AR+EN only
- `Program.cs`: remove `"tr"` from `supportedCultures` array (line ~236), keep `ar` as default
- `_Layout.cshtml`: remove Turkish option from language switcher (both desktop popover and mobile)
- `SharedResource.tr.resx`: can be retained but unused
- All hardcoded Turkish UI text in `.cshtml` files → `@Localizer["Key"]` with AR+EN `.resx` entries

### Faz 4: UX improvements (pending)
- AJAX cart operations (quantity update, delete without page reload)
- Loading skeletons for product list
- Grid/List view toggle on `/Urun`
- Sticky filter sidebar
- Improved empty states (cart, favorites)
