# 7ANRPS48 — Frontend Dönüşüm Implementasyon Planı

Mevcut ASP.NET Core MVC Razor frontend'in Next.js (App Router) ile yeniden yazılması. Backend iş mantığı, entity yapısı ve veritabanı korunacak. Backend'e JSON API endpoint'leri eklenecek, mevcut Razor view'lar silinmeyecek.

---

## User Review Required

> [!IMPORTANT]
> **Kimlik Doğrulama Stratejisi**: API için JWT Bearer Token öneriyorum. Mevcut Identity cookie sistemi admin panel (Razor) için korunacak. Next.js tarafı JWT ile çalışacak. Login endpoint JWT döndürecek, Next.js `httpOnly cookie` olarak saklayacak.

> [!IMPORTANT]
> **Faz Bazlı Yaklaşım**: Proje çok büyük olduğu için 5 faza bölünmüştür. Her faz sonunda çalışan, test edilebilir bir çıktı olacak. Onayınızla faz faz ilerleyeceğiz.

> [!WARNING]
> **Anonymous Cart (Anonim Sepet)**: Mevcut sistemde `Session.Id` ile çalışıyor. API'de bunu `X-Cart-Token` header'ı ile (UUID) yöneteceğiz. Login olunca `MergeSepetlerAsync` ile birleştirilecek.

> [!WARNING]
> **PascalCase JSON**: ASP.NET varsayılan olarak PascalCase JSON döner. Next.js tarafında TypeScript interface'leri PascalCase olacak (API ile 1:1 eşleşme). Frontend'de camelCase mapping yapılmayacak, doğrudan PascalCase kullanılacak.

---

## Open Questions

> [!IMPORTANT]
> 1. **Tailwind CSS Versiyonu**: Tailwind v3 mü v4 mü kullanılsın? v3 daha stabil ve shadcn/ui ile daha uyumlu. **v3 öneriyorum.**

> [!IMPORTANT]
> 2. **Next.js Deployment Portu**: Docker'da Next.js container'ı hangi portta çalışsın? Öneri: `3000` (internal), Nginx/Cloudflare üzerinden yönlendirme.

> [!IMPORTANT]
> 3. **Admin Panel**: Admin panel şimdilik Razor olarak kalacak mı? (Planda Razor olarak bırakıyorum, sonra ayrı bir fazda Next.js'e taşınabilir.)

> [!NOTE]
> 4. **Mevcut "canvasia" renk token'ları**: Prompt'taki renk paleti ile mevcut tailwind.config.js'teki `canvasia-*` token'ları biraz farklı. Prompt'taki paleti esas alıyorum, mevcut `canvasia-*` isimlerini `brand-*` olarak yeniden adlandıracağım.

---

## Faz Yapısı

| Faz | Kapsam | Tahmini Dosya Sayısı |
|-----|--------|---------------------|
| **Faz 1** | Backend API + Next.js Altyapı + Auth | ~45 dosya |
| **Faz 2** | Ana Sayfa + Ürün Sayfaları | ~25 dosya |
| **Faz 3** | Sepet + Checkout + Sipariş | ~20 dosya |
| **Faz 4** | Kullanıcı Profili + Favoriler | ~15 dosya |
| **Faz 5** | Statik Sayfalar + SEO + Polish | ~15 dosya |

---

## Faz 1: Backend API Katmanı + Next.js Altyapı

### Backend — API Endpoint'leri

Backend'e `Api/` area'sı yerine **`/api/` prefix'li controller'lar** ekliyorum. ASP.NET Core'da `[Route("api/[controller]")]` ile doğrudan kullanılacak. Mevcut Razor controller'lar dokunulmayacak.

---

#### [NEW] [ApiUrunlerController.cs](file:///e:/Projeler/filistin_eticaret_projesi/FilistinProje.Web/Controllers/Api/ApiUrunlerController.cs)

REST API for products. Mevcut `UrunController` iş mantığını JSON olarak döndürür.

**Endpoint'ler:**
- `GET /api/urunler` — Sayfalı ürün listesi (query params: `kategoriId`, `s`, `sort`, `min`, `max`, `ozellik[]`, `marka`, `page`, `pageSize`)
- `GET /api/urunler/{id}` — Ürün detay (includes: resimleri, seçenekleri, yorumları, benzer ürünler)
- `GET /api/urunler/populer` — Popüler ürünler (top 8 by SatisSayisi)
- `GET /api/urunler/yeni` — Yeni ürünler (top 8, YeniUrunMu)
- `GET /api/urunler/indirimli` — İndirimli ürünler (top 8, KampanyaliMi or IndirimliFiyat)
- `GET /api/urunler/canli-ara?q=` — Live search (top 8)

**Response DTO'lar:**
- `UrunListeDto` — Liste görünümü: Id, Baslik/En/Ar, Slug, AnaGorselUrl, Fiyat, IndirimliFiyat, EtkinFiyat, IndirimVarMi, IndirimYuzdesi, YeniUrunMu, StoktaVarMi, KategoriAd
- `UrunDetayDto` — Detay: Tüm alanlar + Resimleri, Secenekleri, Yorumlar, BenzerUrunler, OrtalamaPuan
- `UrunAramaDto` — Arama: Id, Slug, Baslik, AnaGorselUrl, EtkinFiyat
- `PagedResult<T>` — Generic: Items, TotalCount, Page, PageSize, TotalPages

---

#### [NEW] [ApiKategorilerController.cs](file:///e:/Projeler/filistin_eticaret_projesi/FilistinProje.Web/Controllers/Api/ApiKategorilerController.cs)

- `GET /api/kategoriler` — Hiyerarşik kategori ağacı (root + children nested)
- `GET /api/kategoriler/{slug}` — Kategori detay (with product count)

---

#### [NEW] [ApiSlaytlarController.cs](file:///e:/Projeler/filistin_eticaret_projesi/FilistinProje.Web/Controllers/Api/ApiSlaytlarController.cs)

- `GET /api/slaytlar` — Aktif slider'lar (ordered by Sira)

---

#### [NEW] [ApiAuthController.cs](file:///e:/Projeler/filistin_eticaret_projesi/FilistinProje.Web/Controllers/Api/ApiAuthController.cs)

JWT tabanlı kimlik doğrulama.

- `POST /api/auth/login` — Email + şifre → JWT token (access + refresh)
- `POST /api/auth/register` — Kayıt (AdSoyad, Email, Sifre, KimlikNo, DogumTarihi, Telefon, Adres, Sehir, ToptanciMi, KimlikFoto)
- `POST /api/auth/logout` — Token invalidation
- `POST /api/auth/refresh` — Refresh token
- `POST /api/auth/sifremi-unuttum` — Şifre sıfırlama emaili gönder
- `POST /api/auth/sifre-sifirla` — Yeni şifre set et
- `GET /api/auth/eposta-dogrula` — Email doğrulama

**JWT Config**: `appsettings.json`'a `JwtSettings` section eklenir (Secret, Issuer, Audience, AccessTokenExpirationMinutes=60, RefreshTokenExpirationDays=30).

---

#### [NEW] [ApiSepetController.cs](file:///e:/Projeler/filistin_eticaret_projesi/FilistinProje.Web/Controllers/Api/ApiSepetController.cs)

- `GET /api/sepet` — Sepet içeriği (auth: userId, anon: `X-Cart-Token` header)
- `POST /api/sepet/ekle` — Sepete ekle
- `PUT /api/sepet/guncelle/{id}` — Adet güncelle
- `PUT /api/sepet/not-guncelle/{id}` — Müşteri notu güncelle
- `DELETE /api/sepet/sil/{id}` — Sepetten sil
- `DELETE /api/sepet/temizle` — Sepeti boşalt
- `POST /api/sepet/kupon-uygula` — Kupon uygula
- `DELETE /api/sepet/kupon-kaldir` — Kupon kaldır
- `GET /api/sepet/sayac` — Sepet ürün sayısı (navbar badge için)

**Session → Stateless**: Kupon kodu ve seçili kargo sepet nesnesine eklenir (yeni alanlar: `Sepet.KuponKodu`, `Sepet.SecilenKargoId`). Session bağımlılığı kaldırılır.

---

#### [NEW] [ApiSiparisController.cs](file:///e:/Projeler/filistin_eticaret_projesi/FilistinProje.Web/Controllers/Api/ApiSiparisController.cs)

- `POST /api/siparis/olustur` — Sipariş oluştur (checkout flow)
- `GET /api/siparis/kargo-hesapla?sehir=` — Kargo ücreti hesapla
- `POST /api/siparis/yukle-kimlik` — Kimlik fotoğrafı yükle (multipart)
- `POST /api/siparis/yukle-recete` — Reçete yükle (multipart)

---

#### [NEW] [ApiFavorilerController.cs](file:///e:/Projeler/filistin_eticaret_projesi/FilistinProje.Web/Controllers/Api/ApiFavorilerController.cs)

- `GET /api/favoriler` — Favori listesi [Authorize]
- `POST /api/favoriler/toggle/{urunId}` — Favori ekle/çıkar
- `POST /api/favoriler/fiyat-bildirimi/{urunId}` — Fiyat düşüş bildirimi toggle

---

#### [NEW] [ApiProfilController.cs](file:///e:/Projeler/filistin_eticaret_projesi/FilistinProje.Web/Controllers/Api/ApiProfilController.cs)

- `GET /api/profil` — Kullanıcı profili [Authorize]
- `PUT /api/profil/guncelle` — Profil güncelle
- `GET /api/profil/siparislerim` — Sipariş listesi
- `GET /api/profil/siparislerim/{id}` — Sipariş detay
- `POST /api/profil/siparis-iptal/{id}` — Sipariş iptal
- `POST /api/profil/iade-olustur` — İade talebi oluştur
- `GET /api/profil/adreslerim` — Adres listesi
- `POST /api/profil/adres-ekle` — Adres ekle
- `DELETE /api/profil/adres-sil/{id}` — Adres sil
- `POST /api/profil/hesabi-sil` — Hesap sil

---

#### [NEW] [ApiSiteController.cs](file:///e:/Projeler/filistin_eticaret_projesi/FilistinProje.Web/Controllers/Api/ApiSiteController.cs)

- `GET /api/site-ayarlari` — Site ayarları (logo, para birimi, footer bilgileri, WhatsApp no, sosyal medya linkleri)
- `POST /api/bulten/kayit` — Bülten kaydı
- `POST /api/iletisim/gonder` — İletişim formu
- `GET /api/kargo-bolgeler` — Kargo bölgeleri + fiyatlar
- `GET /api/banka-hesaplari` — Banka hesapları
- `GET /api/kurumsal/{slug}` — Kurumsal sayfa içeriği
- `GET /api/ana-sayfa` — Ana sayfa bölümleri (HomePageSections + slider + ürün koleksiyonları)

---

#### [MODIFY] [Program.cs](file:///e:/Projeler/filistin_eticaret_projesi/FilistinProje.Web/Program.cs)

- JWT Authentication eklenir (mevcut Cookie auth'un yanına)
- `AddAuthentication().AddJwtBearer()` konfigürasyonu
- CORS policy eklenir (Next.js origin: `http://localhost:3000`, production domain)
- `appsettings.json`'a `JwtSettings` section
- API controller'lar için route mapping: `app.MapControllers()` zaten var, `[ApiController]` attribute yeterli
- Swagger/OpenAPI opsiyonel (development için)

---

#### [NEW] API DTO'lar

Mevcut entity'lerden frontend'e dönecek verileri şekillendiren DTO sınıfları:

#### [NEW] [ApiDtos/](file:///e:/Projeler/filistin_eticaret_projesi/FilistinProje.Core/DTOs/Api/)

```
ApiDtos/
├── UrunListeDto.cs
├── UrunDetayDto.cs
├── UrunAramaDto.cs
├── UrunResimDto.cs
├── UrunSecenekDto.cs
├── KategoriDto.cs
├── KategoriAgaciDto.cs
├── SlaytDto.cs
├── SepetDto.cs
├── SepetItemDto.cs
├── SiparisOlusturDto.cs
├── SiparisDto.cs
├── SiparisDetayDto.cs
├── FavoriDto.cs
├── AdresDto.cs
├── ProfilDto.cs
├── SiteAyarlariDto.cs
├── YorumDto.cs
├── BankaHesapDto.cs
├── KargoBolgeDto.cs
├── KurumsalSayfaDto.cs
├── HomePageDto.cs
├── AuthResponseDto.cs
├── LoginRequestDto.cs
├── RegisterRequestDto.cs
├── PagedResult.cs
└── ApiResponse.cs          # Generic wrapper: { success, data, message, errors }
```

---

### Frontend — Next.js Altyapı

#### [NEW] Next.js Projesi: `/frontend`

```bash
npx -y create-next-app@latest ./frontend --typescript --tailwind --eslint --app --src-dir --import-alias "@/*" --no-turbopack
```

---

#### [NEW] [frontend/package.json](file:///e:/Projeler/filistin_eticaret_projesi/frontend/package.json) — Ek bağımlılıklar

```
Dependencies:
  axios, @tanstack/react-query, zustand, react-hook-form, @hookform/resolvers, zod,
  next-intl, framer-motion, embla-carousel-react, embla-carousel-autoplay,
  sonner, lucide-react, clsx, tailwind-merge, class-variance-authority,
  @radix-ui/react-dialog, @radix-ui/react-dropdown-menu, @radix-ui/react-select,
  @radix-ui/react-tabs, @radix-ui/react-accordion, @radix-ui/react-tooltip,
  @radix-ui/react-popover, @radix-ui/react-separator, @radix-ui/react-slot

DevDependencies:
  @types/node, @types/react, tailwindcss, postcss, autoprefixer
```

---

#### [NEW] [frontend/tailwind.config.ts](file:///e:/Projeler/filistin_eticaret_projesi/frontend/tailwind.config.ts)

Prompt'taki renk paleti + shadcn/ui tema entegrasyonu:

```typescript
colors: {
  brand: {
    primary: '#1a2b1b',        // koyu orman yeşili
    'primary-light': '#313511', // zeytin yeşili
    accent: '#b58735',          // altın
    'accent-light': '#c6ca99',  // açık altın/yeşil
  },
  surface: {
    DEFAULT: '#fcf9f3',         // krem beyaz (body bg)
    alt: '#f7f1e6',             // sıcak krem
    card: '#ffffff',            // beyaz kart
  },
  content: {
    DEFAULT: '#1c1c18',         // ana metin
    muted: '#6f6a5e',           // sıcak gri
  },
  line: {
    DEFAULT: '#e5e2dc',         // açık bej border
    dark: '#d8c9aa',            // altınımsı bej
  },
  status: {
    danger: '#c0392b',
    success: '#27ae60',
  }
}
```

---

#### [NEW] [frontend/src/i18n/](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/i18n/)

next-intl konfigürasyonu:

```
i18n/
├── request.ts        # next-intl getRequestConfig
├── routing.ts        # locales: ['ar', 'en', 'tr'], defaultLocale: 'ar'
└── navigation.ts     # createSharedPathnamesNavigation
```

---

#### [NEW] [frontend/messages/](file:///e:/Projeler/filistin_eticaret_projesi/frontend/messages/)

```
messages/
├── ar.json    # Arapça (varsayılan) — ~300 key
├── en.json    # İngilizce
└── tr.json    # Türkçe
```

**Kategoriler:**
```json
{
  "common": { "search", "cart", "login", "register", "logout", "language", ... },
  "nav": { "home", "products", "categories", "favorites", "profile", "orders", ... },
  "product": { "addToCart", "outOfStock", "newBadge", "discountBadge", "price", ... },
  "cart": { "title", "empty", "total", "checkout", "coupon", "applyCoupon", ... },
  "checkout": { "title", "address", "shipping", "payment", "placeOrder", ... },
  "auth": { "login", "register", "email", "password", "forgotPassword", ... },
  "profile": { "myOrders", "myAddresses", "myInfo", "deleteAccount", ... },
  "footer": { "about", "quickLinks", "corporate", "contact", "newsletter", ... },
  "errors": { "notFound", "serverError", "networkError", ... }
}
```

---

#### [NEW] [frontend/src/lib/api.ts](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/lib/api.ts)

Axios instance:
- `baseURL`: env `NEXT_PUBLIC_API_URL` (default: `http://localhost:5002`)
- Request interceptor: JWT token from cookie → `Authorization: Bearer` header
- Request interceptor: `X-Cart-Token` for anonymous cart
- Response interceptor: 401 → refresh token flow
- `Accept-Language` header from current locale

---

#### [NEW] [frontend/src/lib/queries.ts](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/lib/queries.ts)

TanStack Query hooks:
- `useProducts(params)` — Product listing with filters
- `useProduct(id)` — Product detail
- `useCategories()` — Category tree
- `useSliders()` — Homepage sliders
- `useHomePage()` — Homepage sections
- `useCart()` — Cart data
- `useCartMutations()` — Add/update/remove cart items
- `useFavorites()` — Favorites list
- `useProfile()` — User profile
- `useOrders()` — Order history
- `useAddresses()` — Address list
- `useSiteSettings()` — Site settings (cached long-term)
- `useShippingRegions()` — Shipping regions

---

#### [NEW] [frontend/src/store/](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/store/)

Zustand stores:
- `authStore.ts` — `{ user, token, isAuthenticated, login, logout, setUser }`
- `cartStore.ts` — `{ cartToken, itemCount, setCartToken, setItemCount }` (sadece UI state, data TanStack Query'de)
- `uiStore.ts` — `{ isMobileMenuOpen, isSearchOpen, isCartDrawerOpen, toggleX }`

---

#### [NEW] [frontend/src/app/[locale]/layout.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/layout.tsx)

Root layout:
- `<html lang={locale} dir={locale === 'ar' ? 'rtl' : 'ltr'}>`
- Google Fonts: `IBM Plex Sans Arabic` + `Inter`
- `NextIntlClientProvider`
- `QueryClientProvider` (TanStack)
- `Toaster` (Sonner)
- `<Navbar />` + `<Footer />`

---

#### [NEW] shadcn/ui Setup

```bash
npx shadcn@latest init    # inside /frontend
```

Kullanılacak shadcn/ui component'leri:
- `button`, `input`, `select`, `textarea`, `checkbox`, `radio-group`
- `dialog`, `sheet` (drawer), `popover`, `dropdown-menu`
- `tabs`, `accordion`, `separator`, `badge`, `skeleton`
- `card`, `table`, `pagination`
- `form` (react-hook-form integration)
- `toast` → Sonner ile değiştirilecek

---

#### [NEW] [frontend/Dockerfile](file:///e:/Projeler/filistin_eticaret_projesi/frontend/Dockerfile)

```dockerfile
FROM node:20-alpine AS deps
WORKDIR /app
COPY package*.json ./
RUN npm ci

FROM node:20-alpine AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .
RUN npm run build

FROM node:20-alpine AS runner
WORKDIR /app
ENV NODE_ENV=production
COPY --from=builder /app/.next/standalone ./
COPY --from=builder /app/.next/static ./.next/static
COPY --from=builder /app/public ./public
EXPOSE 3000
CMD ["node", "server.js"]
```

---

#### [MODIFY] [docker-compose.yml](file:///e:/Projeler/filistin_eticaret_projesi/docker-compose.yml)

Yeni `frontend` service eklenir:

```yaml
frontend:
  build:
    context: ./frontend
    dockerfile: Dockerfile
  ports:
    - "3000:3000"
  environment:
    - NEXT_PUBLIC_API_URL=http://web:8080
    - NEXT_PUBLIC_SITE_URL=https://7anrps48.com
  depends_on:
    web:
      condition: service_healthy
```

---

### Faz 1 Dosya Listesi (Backend API)

| # | Dosya | Tür |
|---|-------|-----|
| 1 | `Controllers/Api/ApiUrunlerController.cs` | [NEW] |
| 2 | `Controllers/Api/ApiKategorilerController.cs` | [NEW] |
| 3 | `Controllers/Api/ApiSlaytlarController.cs` | [NEW] |
| 4 | `Controllers/Api/ApiAuthController.cs` | [NEW] |
| 5 | `Controllers/Api/ApiSepetController.cs` | [NEW] |
| 6 | `Controllers/Api/ApiSiparisController.cs` | [NEW] |
| 7 | `Controllers/Api/ApiFavorilerController.cs` | [NEW] |
| 8 | `Controllers/Api/ApiProfilController.cs` | [NEW] |
| 9 | `Controllers/Api/ApiSiteController.cs` | [NEW] |
| 10-34 | `Core/DTOs/Api/*.cs` (25 DTO dosyası) | [NEW] |
| 35 | `Program.cs` (JWT + CORS) | [MODIFY] |
| 36 | `appsettings.json` (JwtSettings) | [MODIFY] |

### Faz 1 Dosya Listesi (Frontend Altyapı)

| # | Dosya | Tür |
|---|-------|-----|
| 1 | `frontend/` — Next.js project init | [NEW] |
| 2 | `frontend/tailwind.config.ts` | [NEW] |
| 3 | `frontend/src/lib/api.ts` | [NEW] |
| 4 | `frontend/src/lib/queries.ts` | [NEW] |
| 5 | `frontend/src/lib/utils.ts` | [NEW] |
| 6 | `frontend/src/store/authStore.ts` | [NEW] |
| 7 | `frontend/src/store/cartStore.ts` | [NEW] |
| 8 | `frontend/src/store/uiStore.ts` | [NEW] |
| 9 | `frontend/src/i18n/request.ts` | [NEW] |
| 10 | `frontend/src/i18n/routing.ts` | [NEW] |
| 11 | `frontend/src/i18n/navigation.ts` | [NEW] |
| 12 | `frontend/messages/ar.json` | [NEW] |
| 13 | `frontend/messages/en.json` | [NEW] |
| 14 | `frontend/messages/tr.json` | [NEW] |
| 15 | `frontend/src/types/index.ts` | [NEW] |
| 16 | `frontend/src/app/[locale]/layout.tsx` | [NEW] |
| 17 | `frontend/Dockerfile` | [NEW] |
| 18 | `docker-compose.yml` | [MODIFY] |

---

## Faz 2: Ana Sayfa + Ürün Sayfaları + Layout Component'leri

### Layout Component'leri

#### [NEW] [frontend/src/components/layout/Navbar.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/layout/Navbar.tsx)

Premium sticky header:
- **Üst bar**: Güven mesajı scrolling ticker (gold arka plan `#b58735`)
- **Ana bar**: Logo (ortada), arama çubuğu, dil seçici, hesap/favori/sepet ikonları
- **Kategori nav**: Mega menu ile hover/click açılan kategori ağacı
- **Mobil**: Hamburger → Sheet (slide-in drawer), full-width arama overlay
- **Sticky**: Scroll'da küçülen header, backdrop-blur efekt
- RTL: Tüm ikonlar ve layout otomatik flip

#### [NEW] [frontend/src/components/layout/MegaMenu.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/layout/MegaMenu.tsx)

- Kategori ağacı (2-3 seviye)
- Desktop: Hover ile açılan panel (max-w-5xl, grid layout)
- Mobil: Accordion ile alt kategoriler
- Framer Motion ile fade/slide animasyon

#### [NEW] [frontend/src/components/layout/Footer.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/layout/Footer.tsx)

- Trust bar: 3 kart (kargo, banka havalesi, kapıda ödeme) — icon + metin
- 4 sütunlu grid: Marka, Hızlı linkler, Kurumsal, İletişim + Bülten + Sosyal medya
- Newsletter formu (email input + button)
- Alt bar: Copyright + politika linkleri
- Responsive: 1 → 2 → 4 sütun

#### [NEW] [frontend/src/components/layout/SearchBar.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/layout/SearchBar.tsx)

- Desktop: Input + icon, focus'ta genişleme
- Canlı arama: 2+ karakter sonrası debounced API call
- Sonuç dropdown: Ürün görseli, başlık, fiyat (max 8 sonuç)
- Keyboard navigation (ArrowUp/Down, Enter, Escape)
- Mobil: Full-screen overlay

#### [NEW] [frontend/src/components/layout/LanguageSwitcher.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/layout/LanguageSwitcher.tsx)

- 3 dil: AR 🇵🇸 / EN 🇬🇧 / TR 🇹🇷
- Dropdown menu (Radix)
- Sayfa yenilenmeden dil değişimi (next-intl router)

#### [NEW] [frontend/src/components/layout/CartDrawer.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/layout/CartDrawer.tsx)

- Sheet (slide-in sağdan) — sepet mini önizleme
- Ürün listesi, adet kontrol, toplam tutar
- "Sepete Git" ve "Ödemeye Geç" CTA butonları

#### [NEW] [frontend/src/components/layout/FloatingButtons.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/layout/FloatingButtons.tsx)

- WhatsApp butonu (sağ alt, yeşil)
- Sepet butonu (sol alt, badge ile adet sayacı)
- Scroll-to-top butonu (scroll > 300px'de görünür)

---

### Shared Component'ler

#### [NEW] [frontend/src/components/shared/Breadcrumb.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/shared/Breadcrumb.tsx)
#### [NEW] [frontend/src/components/shared/Pagination.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/shared/Pagination.tsx)
#### [NEW] [frontend/src/components/shared/ProductCardSkeleton.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/shared/ProductCardSkeleton.tsx)
#### [NEW] [frontend/src/components/shared/EmptyState.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/shared/EmptyState.tsx)
#### [NEW] [frontend/src/components/shared/ErrorState.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/shared/ErrorState.tsx)
#### [NEW] [frontend/src/components/shared/PriceDisplay.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/shared/PriceDisplay.tsx)

Fiyat gösterimi: İndirimli/normal, üstü çizili eski fiyat, ₪ sembolü, RTL uyumlu

---

### Ürün Component'leri

#### [NEW] [frontend/src/components/product/ProductCard.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/product/ProductCard.tsx)

Premium ürün kartı:
- Resim (next/image, hover'da zoom efekt)
- İndirim rozeti (yüzde, kırmızı)
- "Yeni" rozeti (yeşil)
- "Tükendi" rozeti (gri, stokta yoksa)
- Favori butonu (kalp ikonu, toggle animasyonu)
- Başlık, fiyat (indirimli/normal)
- Hover'da "Sepete Ekle" butonu slide-up
- Framer Motion: hover scale, badge pulse

#### [NEW] [frontend/src/components/product/ProductGallery.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/product/ProductGallery.tsx)

- Ana görsel (büyük, zoom on hover/pinch)
- Thumbnail strip (alt kısım, aktif thumbnail vurgusu)
- Lightbox modal (tam ekran, arrow navigation)
- Video desteği (VideoMu kontrolü)
- Mobil: Swipe carousel (Embla)

#### [NEW] [frontend/src/components/product/VariantSelector.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/product/VariantSelector.tsx)

- Seçenek kartları (renk/boyut/tip bazlı gruplandırma)
- Seçili varyant vurgusu (border + arka plan)
- Stokta olmayan varyantlar gri (StokBiteniGriGoster ayarına göre)
- Fiyat farkı gösterimi (+₪X)
- Stok uyarısı ("Son 5 adet!")

#### [NEW] [frontend/src/components/product/ReviewSection.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/product/ReviewSection.tsx)

- Yıldız puanlama (ortalama + toplam yorum sayısı)
- Yorum listesi (avatar, ad, tarih, puan, metin)
- Yorum yazma formu (React Hook Form + Zod)

#### [NEW] [frontend/src/components/product/FilterSidebar.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/product/FilterSidebar.tsx)

- Kategori ağacı (accordion)
- Fiyat aralığı (dual range slider)
- Marka listesi (checkbox)
- Dinamik özellik filtreleri (checkbox grupları)
- Aktif filtre tag'leri (çıkarılabilir)
- Mobil: Sheet olarak açılır

---

### Sayfalar

#### [NEW] [frontend/src/app/[locale]/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/page.tsx) — Ana Sayfa

- **Hero Slider**: Embla Carousel + autoplay, fade geçiş, overlay metin animasyonu (Framer Motion)
- **Kategori Vitrin**: Yatay scroll/grid, kategori görselleri
- **Popüler Ürünler**: 4-sütun grid, "Tümünü Gör" linki
- **Banner**: Full-width CTA banner
- **Yeni Ürünler**: Carousel
- **İndirimli Ürünler**: Grid
- **Bülten**: Email kayıt formu
- **Schema.org**: WebSite + Organization JSON-LD
- **SSR**: Server Component, API'den data fetch

#### [NEW] [frontend/src/app/[locale]/urun/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/urun/page.tsx) — Ürün Listeleme

- Filter sidebar (desktop: sol taraf, mobil: Sheet)
- Ürün grid (responsive: 1 → 2 → 3 → 4 sütun)
- Sıralama dropdown (yeni, fiyat artan/azalan, popüler)
- Pagination (sayfa numaraları + "Daha Fazla Yükle")
- Aktif filtre tag'leri
- URL-based filtering (search params)
- Loading skeletons

#### [NEW] [frontend/src/app/[locale]/urun/[slug]/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/urun/[slug]/page.tsx) — Ürün Detay

- Breadcrumb
- 2-sütun layout: Galeri (sol) + Bilgi (sağ)
- Başlık, fiyat, stok durumu
- Varyant seçimi
- Adet seçici
- "Sepete Ekle" CTA butonu (animasyonlu)
- Hediye paketi toggle
- Müşteri notu textarea
- Trust badges (kargo, iade, güven)
- Teknik özellikler tablosu
- Açıklama (tab veya accordion)
- Yorumlar
- Benzer ürünler carousel
- Schema.org Product JSON-LD

#### [NEW] [frontend/src/app/[locale]/kategori/[slug]/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/kategori/[slug]/page.tsx) — Kategori Sayfası

- Kategori başlığı + açıklama
- Alt kategoriler grid'i
- Ürün listeleme (reuses FilterSidebar + ProductCard)

#### [NEW] [frontend/src/app/[locale]/ara/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/ara/page.tsx) — Arama

- Arama sonuçları grid
- "X sonuç bulundu" bilgisi
- Filtreler
- Boş sonuç durumu

---

## Faz 3: Sepet + Checkout + Sipariş

#### [NEW] [frontend/src/app/[locale]/sepet/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/sepet/page.tsx) — Sepet

- Ürün listesi tablosu (görsel, başlık, varyant, fiyat, adet, toplam)
- Adet +/- kontrolleri
- Müşteri notu (inline edit)
- Ürün silme (swipe veya X butonu)
- Sepeti temizle butonu
- Kupon kodu input + uygula
- Sipariş özeti: Ara toplam, kupon indirimi, kargo, genel toplam
- Ücretsiz kargo barı (progress bar)
- Tamamlayıcı ürünler carousel
- Boş sepet durumu (EmptyState)
- "Ödemeye Geç" CTA butonu

#### [NEW] [frontend/src/components/cart/CartItem.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/cart/CartItem.tsx)
#### [NEW] [frontend/src/components/cart/CartSummary.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/cart/CartSummary.tsx)
#### [NEW] [frontend/src/components/cart/CouponInput.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/cart/CouponInput.tsx)
#### [NEW] [frontend/src/components/cart/FreeShippingBar.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/components/cart/FreeShippingBar.tsx)

#### [NEW] [frontend/src/app/[locale]/odeme/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/odeme/page.tsx) — Checkout

Multi-step checkout:
1. **Teslimat Bilgileri**: Ad, email, telefon, adres, şehir (dropdown from kargo bölgeleri)
2. **Teslimat Tipi**: Adrese teslim / Mağazadan teslim (radio)
3. **Ödeme Yöntemi**: Banka havalesi (IBAN listesi göster) / Kapıda ödeme (hizmet bedeli göster, limit kontrolü)
4. **Sipariş Özeti**: Ürünler, kargo, indirim, toplam
5. **Sipariş Notu**: Textarea
6. **Kullanım Şartları**: Checkbox (zorunlu)
7. **Siparişi Onayla** butonu

- Reçete yükleme (ReceteGerekliMi kategorideki ürünler varsa)
- Kimlik fotoğrafı yükleme (WebRTC kamera veya dosya)
- Kayıtlı adres seçimi (auth ise)
- Dinamik kargo hesaplama (şehir değişince)
- Form validation (Zod schema)

#### [NEW] [frontend/src/app/[locale]/siparis/beklemede/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/siparis/beklemede/page.tsx)
#### [NEW] [frontend/src/app/[locale]/siparis/basarili/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/siparis/basarili/page.tsx)
#### [NEW] [frontend/src/app/[locale]/siparis/basarisiz/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/siparis/basarisiz/page.tsx)

---

## Faz 4: Kullanıcı Profili + Auth + Favoriler

#### [NEW] [frontend/src/app/[locale]/giris/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/giris/page.tsx) — Giriş

- Email + Şifre input
- "Beni Hatırla" checkbox
- "Şifremi Unuttum" linki
- "Kayıt Ol" linki
- Form validation (Zod)
- JWT token → httpOnly cookie

#### [NEW] [frontend/src/app/[locale]/kayit/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/kayit/page.tsx) — Kayıt

- Ad, email, telefon, kimlik no, doğum tarihi, şifre, şehir, adres
- Kimlik fotoğrafı yükleme
- Toptancı başvurusu toggle
- Form validation (Zod)

#### [NEW] [frontend/src/app/[locale]/profil/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/profil/page.tsx) — Profil Dashboard
#### [NEW] [frontend/src/app/[locale]/profil/siparislerim/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/profil/siparislerim/page.tsx)
#### [NEW] [frontend/src/app/[locale]/profil/siparislerim/[id]/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/profil/siparislerim/[id]/page.tsx)
#### [NEW] [frontend/src/app/[locale]/profil/adreslerim/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/profil/adreslerim/page.tsx)
#### [NEW] [frontend/src/app/[locale]/profil/layout.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/profil/layout.tsx) — Sidebar menu layout
#### [NEW] [frontend/src/app/[locale]/favoriler/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/favoriler/page.tsx)
#### [NEW] [frontend/src/app/[locale]/sifremi-unuttum/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/sifremi-unuttum/page.tsx)
#### [NEW] [frontend/src/app/[locale]/sifre-sifirla/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/sifre-sifirla/page.tsx)

---

## Faz 5: Statik Sayfalar + SEO + Polish

#### [NEW] [frontend/src/app/[locale]/iletisim/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/iletisim/page.tsx) — İletişim
#### [NEW] [frontend/src/app/[locale]/hakkimizda/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/hakkimizda/page.tsx) — Hakkımızda
#### [NEW] [frontend/src/app/[locale]/kurumsal/[slug]/page.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/[locale]/kurumsal/[slug]/page.tsx) — Kurumsal Sayfalar
#### [NEW] [frontend/src/app/not-found.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/not-found.tsx) — 404
#### [NEW] [frontend/src/app/error.tsx](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/error.tsx) — 500

### SEO
#### [NEW] [frontend/src/app/sitemap.ts](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/sitemap.ts) — Dynamic sitemap
#### [NEW] [frontend/src/app/robots.ts](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/app/robots.ts) — robots.txt
#### [NEW] [frontend/src/lib/structured-data.ts](file:///e:/Projeler/filistin_eticaret_projesi/frontend/src/lib/structured-data.ts) — Schema.org helpers

### Animasyonlar & Polish
- Page transitions (Framer Motion `AnimatePresence`)
- Scroll reveal animasyonları
- Micro-interactions (button hover, card hover, skeleton loading)
- `prefers-reduced-motion` desteği

---

## Verification Plan

### Build & Lint
```bash
# Backend
dotnet build FilistinProje.sln

# Frontend
cd frontend && npm run build
cd frontend && npm run lint
```

### API Testing
- Her endpoint için Postman/Thunder Client ile manuel test
- JWT auth flow testi (login → token → protected endpoint)
- Anonymous cart flow testi

### Frontend Testing
- Her sayfa için visual inspection (3 dil × 2 direction)
- Responsive test (mobil, tablet, desktop)
- Lighthouse audit (Performance, Accessibility, SEO, Best Practices)
- RTL layout doğrulama (Arapça)

### Integration Testing
- Full e2e flow: Ürün listele → Detay → Sepete ekle → Checkout → Sipariş başarılı
- Auth flow: Kayıt → Email doğrulama → Giriş → Profil
- Favori toggle flow
- Kupon uygulama flow
- Dil değiştirme flow

### Docker Testing
```bash
docker-compose up -d --build
# http://localhost:3000 (frontend)
# http://localhost:8080 (backend API + admin panel)
```
