# MiniMax M3 — 7ANRPS48.com kapsamlı analiz promptu

Aşağıdaki görevi kıdemli bir yazılım mimarı, e-ticaret uzmanı, güvenlik denetçisi ve senior UI/UX mühendisi gibi yürüt.

## Proje ve amaç

Elindeki repository, Filistin pazarı için geliştirilen **7ANRPS48.com** adlı e-ticaret sitesidir.

- Teknoloji: ASP.NET Core 8 MVC, Entity Framework Core, PostgreSQL, Docker, TailwindCSS
- Mimari: Clean Architecture — Web → Service → Data → Core
- Varsayılan dil: Arapça ve RTL
- İkinci dil: İngilizce ve LTR
- Para birimi: ILS / ₪
- Kimlik doğrulama: ASP.NET Core Identity
- Hedef cihazlar: Öncelikle mobil, sonra tablet ve masaüstü

Projenin “tamamlandığı” söyleniyor; fakat bunu doğru kabul etme. `AGENTS.md` içindeki tamamlandı işaretleri yalnızca geliştirici notudur, kanıt değildir. Her iddiayı gerçek kod, çalışan akış, veritabanı modeli, UI ve mümkünse tarayıcı testiyle doğrula.

Bu aşamada **hiçbir dosyayı değiştirme, kod yazma, migration üretme veya otomatik düzeltme yapma**. Yalnızca inceleme, doğrulama, risk analizi ve uygulanabilir iyileştirme planı hazırla. Bir şeyi çalıştıramazsan tahmin yürütme; hangi nedenle doğrulayamadığını açıkça yaz.

## İncelenecek kaynaklar

1. Repository’nin tamamı
2. Kök dizindeki `AGENTS.md`
3. Proje sahibinin taleplerini içeren:
   - `Belge.docx`
   - `bilgiler.docx`
   - WhatsApp sohbet arşivi ve içindeki yazılı mesajlar/görseller
4. Mevcut migration’lar
5. `Program.cs` içindeki elle yazılmış `EnsureMissingMarch2026SchemaAsync` şema tamamlama kodu
6. Public ve Admin controller/view/service/entity dosyaları
7. Lokalizasyon `.resx` dosyaları, CSS/JS, statik asset’ler ve e-posta şablonları

Önce proje sahibinin isteklerini tekilleştirerek bir “gereksinim envanteri” çıkar. Aynı isteğin farklı cümlelerle tekrarlandığı yerleri tek maddede birleştir. Ses kaydı veya görsel içeriği okuyamıyorsan bunu kapsam sınırlaması olarak belirt.

## Proje sahibinin temel beklentileri

Belgelerdeki ayrıntıları esas al; aşağıdaki özet yalnızca kontrol listesi görevi görür:

- Arapça varsayılan RTL arayüz ve İngilizce LTR arayüz
- Mobil öncelikli, hafif, gözü yormayan, profesyonel siyah/altın ağırlıklı tasarım
- Ana sayfa slider’ı, kategori/ürün grupları, marka ve ürün bölümleri
- Arama, mümkünse canlı arama, filtreleme, fiyat slider’ı ve sıralama
- Ürün galerisi, video, marka, kategori, renk/boyut/hacim/ağırlık varyasyonları
- Varyasyon bazlı fiyat farkı ve stok; tükenen seçeneğin pasif/gri görünmesi
- Düşük stokta “son X adet” uyarısı
- Ürün ve site değerlendirmeleri, yıldız ve yorumlar
- İndirim, kampanya, kupon, ücretsiz kargo barajı ve kampanya gösterimleri
- İlgili ürünler, favoriler, sayaçlı kampanya ve çark/ödül gibi pazarlama özellikleri
- WhatsApp üzerinden sipariş veya fiyat gizleme gerektiren pahalı/özel ürünler
- Hediye paketi ve fiyatlı ek hizmetler
- Reçete gerektiren kategorilerde reçete/kimlik yükleme
- Üyelikte ayrıntılı kimlik, doğum tarihi, telefon, bölge, şehir, adres ve kimlik fotoğrafı
- Kameradan kimlik/fotoğraf çekebilme
- Adrese teslim veya mağazadan teslim seçenekleri
- Filistin bölgelerine göre yönetilebilir kargo ücreti; müşteri kargo firması seçmez
- Bölgeler: iç bölge/48, Batı Şeria ve Kudüs gibi yönetilebilir alanlar
- Yönetilebilir kargo firması; örnek olarak United Express
- Kapıda ödeme ve elektronik ödeme seçenekleri; görünürlük kontrolleri
- Yüksek tutarlı siparişlerde kapıda ödeme sınırı
- Sipariş notu ve şartlar/politikalar onayı
- Sipariş takibi ve belirli koşullarda iptal
- Toptancı hesabı, onay süreci, özel fiyat/iskonto ve minimum sipariş tutarı
- Admin çalışanları ve ayrıntılı yetkilendirme
- Ürün, kategori, stok, sipariş, kullanıcı, kupon, slider, kargo, iade, yorum, sayfa ve rapor yönetimi
- PDF fatura, stok uyarısı, satış/bölge raporları
- İletişim, teknik destek, hakkımızda, gizlilik, iade/iptal/kargo politikaları
- SEO, doğrudan kategori/marka bağlantıları ve sosyal medya/WhatsApp bağlantıları
- Marka adı `7ANRPS48.com`, para birimi `₪ / ILS`; eski Canvasia/MeteorGaleri/Türkiye kalıntıları bulunmamalı

## Zorunlu çalışma yöntemi

### 1. Repository ve mimari haritası

- Solution ve dört projenin sorumluluklarını çıkar.
- Controller → service → repository/DbContext → entity → view akışlarını haritala.
- Katman ihlallerini, aşırı bağımlılıkları, tekrarları, ölü kodu ve yüksek karmaşıklıklı dosyaları bul.
- DI kayıtlarını, middleware sırasını, background job’ları, cache/session kullanımını ve hata yönetimini incele.
- “Clean Architecture kullanılıyor” iddiasının gerçekte ne kadar doğru olduğunu değerlendir.

### 2. Çalıştırma ve temel doğrulama

Mümkünse şu kontrolleri çalıştır:

```powershell
dotnet build FilistinProje.sln
docker compose up -d db
dotnet ef migrations list --project FilistinProje.Data --startup-project FilistinProje.Web
```

Uygulamayı çalıştırabilirsen public ve admin akışlarını tarayıcıdan incele. Kullanılan komutları ve sonuçlarını rapora yaz. Başarısız komutlarda gerçek hata çıktısını özetle; başarısız testi geçmiş gibi gösterme.

### 3. Gereksinim izlenebilirlik matrisi

Her gereksinim için şu alanları üret:

| ID | Gereksinim | Durum | Kod kanıtı | UI/akış kanıtı | Eksik/risk | Önerilen iş |
|---|---|---|---|---|---|---|

Durum yalnızca şu değerlerden biri olsun:

- Tam ve doğrulandı
- Kısmen uygulanmış
- Kodda var fakat kullanıcı akışı kırık/doğrulanmadı
- Yalnızca UI var, backend yok
- Yalnızca backend var, UI yok
- Eksik
- Çakışmalı/yanlış uygulanmış
- İncelenemedi

Dosya adı ve mümkünse satır numarası vermeden “var” deme.

### 4. Frontend ve UI/UX denetimi — en yüksek öncelik

Frontend incelemesi yüzeysel bir CSS yorumu olmasın. Uygulamayı tarayıcıda gerçek sayfalar üzerinden test et ve mümkünse ekran görüntüsü kanıtı üret.

En az şu sayfaları incele:

- Ana sayfa
- Ürün listeleme/kategori
- Arama sonuçları
- Ürün detay
- Sepet
- Checkout/ödeme
- Giriş/kayıt/şifre sıfırlama
- Profil, adresler ve siparişler
- Başarılı/başarısız ödeme
- Temel admin dashboard ve yoğun kullanılan admin CRUD sayfaları

Her kritik sayfayı en az şu genişliklerde değerlendir:

- 360 px mobil
- 390/412 px mobil
- 768 px tablet
- 1024 px küçük masaüstü
- 1440 px masaüstü

Arapça RTL ve İngilizce LTR modlarını ayrı ayrı kontrol et:

- Taşma, üst üste binme, kesilen metin ve yatay scroll
- Header, mobil menü, arama, sepet ve dil değiştirici
- Grid/kart tutarlılığı, görsel oranları ve bozuk resimler
- Tipografi, kontrast, boşluk, hiyerarşi ve dokunma hedefleri
- Form hata mesajları, disabled/loading/empty/error/success durumları
- Çok uzun Arapça/İngilizce metinler
- Para, tarih, sayı ve ILS gösterimi
- RTL’de ikon, ok, breadcrumb, slider ve form yönleri
- Klavye erişimi, focus görünürlüğü, label/ARIA, alt text ve renk kontrastı
- CLS/LCP oluşturabilecek görseller, gereksiz JS/CSS ve render-blocking kaynaklar
- Tutarsız component stilleri ve dağınık inline CSS/JS
- Tasarımın markaya uygunluğu ve “hafif, gözü yormayan siyah/altın” hedefi

Her frontend problemi için sayfa, viewport, dil, tekrar üretme adımları, beklenen/gerçek davranış ve önerilen çözümü yaz. Sadece “tasarım geliştirilmeli” gibi soyut cümleler kullanma.

### 5. İş kuralları ve uçtan uca akışlar

Şu akışları hem olumlu hem olumsuz senaryolarla izle:

- Ürün/varyasyon seçme → stok → fiyat → sepete ekleme
- Hediye paketi/ek hizmet → sepet → sipariş toplamı
- Kupon → indirim → kargo → kapıda ödeme bedeli → genel toplam
- Bölge/şehir → kargo fiyatının otomatik hesaplanması
- Ücretsiz kargo barajı
- 2000 ILS veya ayardaki limit üstünde kapıda ödemenin engellenmesi
- Reçeteli kategori → reçete/kimlik zorunluluğu
- WhatsApp/fiyat gizli ürün → normal satın alma yolunun kapanması
- Toptancı kullanıcı → onay → özel fiyat/iskonto → minimum sipariş
- Stok azalması ve eşzamanlı iki siparişte overselling riski
- Sipariş oluşturma, ödeme başarısı/başarısızlığı, iptal ve iade
- Admin yetkisi olmayan kullanıcının admin endpoint’lerine erişimi

Toplam hesaplarının yalnızca JavaScript’e güvenip güvenmediğini özellikle kontrol et. Kritik fiyat ve yetki kararları server tarafında yeniden doğrulanmalı.

### 6. Backend, veri ve migration denetimi

- Entity, DTO/ViewModel, validation ve mapping tutarlılığını incele.
- EF migration’larıyla `EnsureMissingMarch2026SchemaAsync` SQL bloğunu karşılaştır.
- Her yeni kolon/tablonun iki sistemde de bulunduğunu ve aynı tip/default/nullability/index/FK davranışına sahip olduğunu doğrula.
- Raw SQL içindeki PostgreSQL çift tırnak ve Türkçe PascalCase tablo/kolon adlarını kontrol et.
- N+1 sorguları, gereksiz `Include`, tracking, pagination eksikleri ve büyük veri sorunlarını bul.
- Para alanlarında doğru `decimal` hassasiyeti, tarih/saat ve Filistin saat dilimi kullanımını incele.
- Transaction sınırları, stok bütünlüğü, idempotency, concurrency ve silme davranışlarını kontrol et.
- Seed verileri, roller ve permission matrix’in gerçek controller’larla uyumunu doğrula.

### 7. Güvenlik ve gizlilik

OWASP odaklı inceleme yap:

- Authentication/authorization ve rol yükseltme
- IDOR/BOLA
- CSRF/antiforgery
- XSS ve HTML/raw content
- SQL injection
- Dosya yükleme: uzantı, MIME, içerik, boyut, path traversal, yeniden adlandırma ve web root riski
- Kimlik fotoğrafı/reçete gibi hassas belgelerin erişim kontrolü
- Secret/connection string/log sızıntısı
- Rate limiter kapsamı ve bypass ihtimalleri
- Open redirect
- Güvensiz model binding/overposting
- Session/cookie güvenliği
- Admin ve bakım modu bypass’ları
- Kullanıcı girdilerinin loglarda kişisel veri sızdırması

Her bulguya saldırı senaryosu, etki, kanıt ve düzeltme önerisi ekle. Kanıtsız kritik açık ilan etme.

### 8. Lokalizasyon, marka ve içerik

- Arapça/İngilizce bütün kullanıcı metinlerini ve eksik localizer key’lerini kontrol et.
- Hard-coded Türkçe metin, `tr-TR`, Türkiye şehirleri, `+90`, `TR00`, TRY/TL, Canvasia, MeteorGaleri ve eski domain/logo/e-posta kalıntılarını ara.
- Arapçayı gerçek varsayılan dil yapan fallback, cookie ve URL davranışını doğrula.
- Admin panelinin Türkçe olmasının bilinçli proje kararı olduğunu kullanıcı arayüzü lokalizasyon hatasıyla karıştırma.
- E-posta, PDF/fatura, SEO metadata, hata sayfaları ve validation mesajlarını da denetle.

### 9. Performans, güvenilirlik ve operasyon

- Sayfalama yapılmayan listeler
- Büyük resim/video yükleri
- Cache eksikleri veya hatalı cache invalidation
- DB yokken startup davranışı
- Hangfire ve background servislerinin hata davranışı
- Loglama, health check, retry/timeout ve gözlemlenebilirlik
- Docker ve production configuration
- Backup/restore, migration deployment ve rollback riskleri
- Eksik testler ve en yüksek değerli otomasyon testleri

## Önceliklendirme

Her bulguyu şu şekilde puanla:

- P0: Güvenlik, veri kaybı, yanlış ücretlendirme veya siparişin tamamlanamaması
- P1: Kritik iş gereksinimi eksik/kırık, ciddi mobil/RTL problemi
- P2: Önemli UX, performans, bakım veya operasyon problemi
- P3: Görsel tutarlılık, küçük iyileştirme veya teknik borç

Her maddeye ayrıca:

- Etki: 1–5
- Olasılık: 1–5
- Efor: XS / S / M / L / XL
- Bağımlılıklar
- Önerilen çözüm sırası

ekle. Önceliği yalnızca kaç dosya değişeceğine göre verme; müşteri ve gelir etkisini esas al.

## Nihai çıktı biçimi

Raporu Türkçe yaz ve şu sırayı koru:

1. Yönetici özeti — en önemli 10 sonuç
2. İnceleme kapsamı, çalıştırılan komutlar ve doğrulanamayan alanlar
3. Mimari harita
4. Gereksinim izlenebilirlik matrisi
5. Frontend/UI/UX raporu
6. Backend ve iş kuralları raporu
7. Veritabanı/migration raporu
8. Güvenlik ve gizlilik raporu
9. Lokalizasyon/marka raporu
10. Performans/operasyon raporu
11. Dosya ve satır kanıtlı tüm bulgular listesi
12. Uygulama yol haritası:
    - Faz 0: acil P0 düzeltmeleri
    - Faz 1: kritik iş akışları
    - Faz 2: frontend/RTL/mobil iyileştirmeleri
    - Faz 3: performans, test ve operasyon
13. İlk 2 haftalık uygulanabilir sprint planı
14. Regression test kontrol listesi
15. Proje sahibine sorulması gereken açık sorular

Son bölümde ayrıca üç kısa liste ver:

- **Şu anda production’a çıkmayı engelleyenler**
- **Production sonrası yapılabilecekler**
- **Kodda tamamlandı görünen fakat gerçek kullanıcı akışında doğrulanmayanlar**

Raporun dili net, kanıta dayalı ve uygulanabilir olsun. Genel tavsiye vermek yerine gerçek dosyaya, metoda, route’a, sorguya veya görünür UI problemine bağlan. Bir bulgu için kanıt yoksa bunu “hipotez” olarak etiketle.

şuan site localhost 5002 portunda çalışıyor site incelemesi için playwright mcp sini kullanarak chrome açarak gerçekleştir bakalım.