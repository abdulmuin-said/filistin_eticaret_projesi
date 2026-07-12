# MiniMax M3 — 7ANRPS48.com düzeltme oturumları

Bu promptları aşağıdaki sırayla, ayrı MiniMax oturumlarında kullan. Her yeni oturuma başlamadan önce bir önceki oturumun değişikliklerinin çalışma klasöründe bulunduğundan emin ol. Aynı repository üzerinde çalışılıyorsa promptları paralel çalıştırma; özellikle 2, 5, 6 ve 13 numaralı oturumlar aynı checkout dosyalarına dokunabilir.

Her oturumda MiniMax’a repository’nin tamamını, `AGENTS.md` dosyasını ve `AUDIT_RAPORU.md` dosyasını erişilebilir halde ver.

---

## Oturum 1 — Açık kimlik bilgileri, admin hesabı ve SMTP yapılandırması

```text
7ANRPS48.com repository’sinde production engelleyici kimlik bilgisi ve secret güvenliği sorunlarını düzelt.

Önce kök dizindeki AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. Bu oturumun kapsamı B0, B4 ve B30’dur:

- Giriş sayfasında admin@7anrps48.com / Admin123! bilgilerinin önceden doldurulması.
- Seed edilen admin hesabında tahmin edilebilir sabit parola bulunması.
- SMTP FromName/FromEmail ve fallback değerlerinde Canvasia markasının kalması.
- Gerçek SMTP anahtarlarının plaintext dosyalara bağlı olması.

Kurallar:

1. Önce gerçek kodu incele; rapordaki satır numaraları değişmiş olabilir.
2. Views/Hesap/GirisYap.cshtml ve ilgili ViewModel/controller içinde e-posta veya parola varsayılanı bırakma. Browser password manager davranışıyla view içindeki value değerini birbirine karıştırma.
3. Development seed davranışını güvenli hale getir:
   - Production’da sabit admin parolası üretme.
   - Gerçek parola veya secret’ı source code’a yazma.
   - Gerekirse environment variable üzerinden ilk admin kurulumunu destekle.
   - Eksik environment variable durumunda production için güvenli biçimde admin seed’ini atla ve açık log mesajı üret.
   - Development kolaylığı gerekiyorsa yalnız Development ortamında ve açık uyarıyla çalışsın.
4. SMTP FromName varsayılanı “7ANRPS48”, FromEmail ise configuration’dan gelsin. Sahte veya var olmayan gerçek e-posta adresi icat etme.
5. secrets.json dosyasındaki gerçek secret değerlerini başka dosyaya kopyalama. Git tracked dosyalarda secret kalmadığını ara. Eğer Brevo/API anahtarı repository dışındaki harici sistemde rotate edilmek zorundaysa bunu kodla yapmaya çalışma; finalde kullanıcıya manuel işlem olarak bildir.
6. Loglarda parola, API key veya SMTP credential gösterme.
7. İlgisiz dosyaları değiştirme, commit oluşturma.

Doğrulama:

- dotnet build FilistinProje.sln
- /Hesap/GirisYap sayfasını aç; e-posta ve parola alanlarının HTML value değerleri boş olmalı.
- Production environment varsayımıyla sabit Admin123! hesabı oluşmadığını statik olarak doğrula.
- Repository’de Admin123!, Canvasia FromName ve gerçek SMTP secret kalıntılarını ara; meşru tarihî dokümanları kod/config risklerinden ayrı raporla.

İş bitiminde değiştirilen dosyaları, yapılan güvenlik kararlarını, build sonucunu ve kullanıcının manuel olarak rotate etmesi gereken secret’ları yaz. Kanıtlamadığın şeyi tamamlandı sayma.
```

---

## Oturum 2 — Checkout fiyat bütünlüğü, atomik stok ve doğru sipariş toplamı

```text
7ANRPS48.com projesindeki en kritik sipariş bütünlüğü sorunlarını tek ve tutarlı bir değişiklik setiyle düzelt.

Önce AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. Kapsam: B2, B3, B13 ve B27.

Hedefler:

- Sipariş POST’unda SepetItem.Fiyat değerine güvenilmemesi.
- Ürün/varyasyon fiyatlarının server tarafında güncel DB verisinden yeniden hesaplanması.
- Sipariş oluşturulurken stokların atomik biçimde düşürülmesi.
- İki eşzamanlı checkout’un aynı son stoğu satamaması.
- Hediye paketi bedelinin yalnız bir kez ve doğru biçimde genel toplama katılması.
- Siparis entity’sini doğrudan bind etmek yerine checkout’a özel ViewModel/DTO kullanılması.

Uygulama ilkeleri:

1. Mevcut SepetService, SiparisController, Urun/UrunSecenek fiyat modeli, toptancı fiyatı, indirim, kupon, kargo, COD bedeli ve hediye paketi akışını baştan sona izle.
2. Fiyat kaynağı için tek server-side hesaplama yolu oluştur. Normal fiyat, indirimli fiyat, varyasyon fiyat farkı, toptancı fiyatı/iskontosu ve hediye paketi kurallarını kaybetme.
3. Client’tan veya sepet snapshot’ından gelen birim fiyatı siparişin nihai fiyatı olarak kabul etme. Fiyat değişmişse güvenli davranışı seç:
   - Sepeti güncel fiyatla yenile ve kullanıcıya fiyatın değiştiğini bildir; veya
   - Siparişi açık bir mesajla durdur.
   Sessizce farklı tutar tahsil etme.
4. Sipariş, detaylar, kupon kullanımı ve stok düşümü aynı DB transaction içinde olmalı.
5. Stok düşümünü read-then-write yarışıyla yapma. `StokAdedi >= istenenAdet` koşullu atomic update veya eşdeğer güvenli concurrency yaklaşımı kullan; affected row kontrol edilmezse işlem başarılı sayılmasın.
6. Her sepet satırını ve aynı varyantın birden fazla satırda bulunma ihtimalini normalize et.
7. Stok yetersizse transaction rollback olsun, sipariş/kupon/stok kısmi kalmasın ve kullanıcı anlaşılır hata görsün.
8. Hediye paketi fiyatının SepetService toplamına zaten dahil olup olmadığını önce kanıtla. Çifte ekleme yapma.
9. Checkout DTO yalnız kullanıcının göndermesi gereken alanları içersin. Toplam, indirim, kargo, user id, durum gibi server-owned alanları bind etme.
10. Yeni entity property veya şema değişikliği gerekirse AGENTS.md’deki dual migration kuralını uygula; gerekmiyorsa migration üretme.
11. İlgisiz büyük mimari refactor yapma. Önce güvenli çalışan akışı tamamla.

Doğrulama senaryoları:

- Normal ürün + varyasyon + hediye paketi: ara toplam ve genel toplam doğru.
- Sepetteki fiyat eski/manipüle edilmişken server DB fiyatını esas alıyor veya işlemi durduruyor.
- Stok 1 iken iki sipariş denemesinde yalnız biri başarılı; stok negatif olmuyor.
- Stoksuz varyant sipariş oluşturamıyor.
- Transaction ortasında hata olursa sipariş, detay, kupon ve stok rollback oluyor.
- Normal ve Wholesale kullanıcı fiyatları doğru.
- Kupon + kargo + ücretsiz kargo + COD bedeli sıralaması doğru.
- dotnet build FilistinProje.sln → 0 hata, mümkünse 0 uyarı.

Projedeki talimat gereği yeni test projesi oluşturma. Mevcut altyapı, kontrollü entegrasyon doğrulaması ve tarayıcı/DB kontrolleriyle kanıt üret. Finalde değişen dosyaları, fiyat formülünü, transaction davranışını ve test sonuçlarını açıkça yaz.
```

---

## Oturum 3 — Kimlik/reçete dosyalarını özel depolama ve kamera güvenliği

```text
7ANRPS48.com projesinde kimlik fotoğrafı ve reçete yükleme güvenliğini düzelt; kamera özelliğini gerçekten çalışır hale getir.

Önce AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. Kapsam: B25 ve B9; ilgili dosyalar Program.cs, DosyaServisi, SiparisController, HesapController ve kimlik/reçete view’ları olabilir.

Yapılacaklar:

1. Kimlik ve reçete gibi hassas dosyaların wwwroot altında doğrudan statik URL ile sunulup sunulmadığını kanıtla.
2. Hassas dosyaları public web root dışındaki özel bir storage klasörüne taşıyacak güvenli tasarım uygula.
3. Yetkili görüntüleme/indirme endpoint’i oluştur:
   - Müşteri yalnız kendi belgesine erişebilsin.
   - Yetkili admin permission matrix’e uygun erişebilsin.
   - Path parametresi doğrudan fiziksel dosya yolu olmasın.
   - Content-Disposition, MIME ve cache header’ları güvenli olsun.
4. Mevcut kayıtların eski relative path formatıyla uyumluluğunu düşün. Veri kaybetmeden migration/compatibility planı uygula; fiziksel dosya yoksa kontrollü 404 üret.
5. Upload güvenliği:
   - Boyut sınırı.
   - İzinli uzantı + MIME kontrolü.
   - Dosya imzası/magic-byte doğrulaması.
   - Rastgele GUID dosya adı.
   - Path traversal engeli.
   - SVG/HTML/script gibi aktif içerikleri reddet.
   - Hata mesajlarında fiziksel path sızdırma.
6. `Permissions-Policy: camera=()` yüzünden kamera her yerde kapanıyorsa güvenli route bazlı politika uygula. Kamera yalnız gereken same-origin checkout/kayıt sayfalarında `camera=(self)` olsun; tüm site için gereksiz geniş izin verme.
7. WebRTC akışında izin reddi, kamera bulunmaması, mobil Safari/Chrome ve dosya yükleme fallback durumları için anlaşılır AR/EN mesajlar göster.
8. Kamera çıktısını normal upload validation hattından geçir; base64 veya blob’u doğrulamadan kaydetme.
9. Yeni admin endpoint/controller gerekiyorsa AdminBaseController, AdminPermissionMatrix, ViewBag ve sidebar kurallarını uygula.

Doğrulama:

- Eski `/uploads/kimlikler/...` ve `/uploads/receteler/...` URL’leri anonim kullanıcıya hassas dosya vermemeli.
- Yetkisiz müşteri başka kullanıcının belge id’sini deneyince 403/404 almalı.
- Yetkili kullanıcı belgeyi görebilmeli.
- Geçersiz uzantı, sahte MIME, büyük dosya ve path traversal reddedilmeli.
- Checkout kamera butonu desteklenen tarayıcıda izin istemeli; izin reddinde sayfa bozulmamalı.
- dotnet build FilistinProje.sln.

Gerçek kişisel belge içeriğini loglama veya final raporuna koyma. Finalde veri taşıma/production deployment adımlarını ayrıca belirt.
```

---

## Oturum 4 — Filistin telefon doğrulaması ve E.164 normalizasyonu

```text
7ANRPS48.com projesindeki Türkiye’ye özgü telefon doğrulamasını kaldır ve Filistin numaraları için tutarlı, merkezi bir çözüm uygula.

Önce AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. Ana bulgu B6’dır.

Gereksinimler:

1. Repository genelinde telefon doğrulaması, maxlength, placeholder, JavaScript maskesi, regex, DTO/DataAnnotation ve controller kontrollerini ara.
2. Tek bir server-side normalizasyon/validation yaklaşımı oluştur. En az şu kullanıcı girişlerini bilinçli şekilde değerlendir:
   - +970599123456
   - 0599123456
   - 970599123456
   - boşluk, tire ve parantez içeren eşdeğer gösterimler
3. Veritabanına mümkünse normalize edilmiş E.164 biçimi yaz. Mevcut kayıtlarla uyumluluğu bozma.
4. İsrail/48 bölgesi müşterileri için +972 numarası kabul edilip edilmeyeceği proje sahibince kesinleşmemişse:
   - Kodda kolay yapılandırılabilir ülke kodu listesi tasarla.
   - +970’ı destekle.
   - +972 hakkında varsayımı final raporunda açıkça belirt; gizlice reddetme veya kabul etme.
5. Sadece uzunluk kontrolü kullanma. Server ve client kuralları çelişmesin; güvenlik kararı server tarafında olsun.
6. Kayıt, profil/adres, checkout ve toptancı başvuru formlarını kapsa.
7. AR ve EN hata mesajlarını localizer üzerinden üret. `+90`, `TR`, `0 ile başlayan 11 hane` kalıntılarını temizle.
8. Entity şeması değişmiyorsa migration oluşturma.

Doğrulama:

- Geçerli Filistin numaraları kabul ve normalize edilir.
- Harfli, aşırı kısa/uzun ve anlamsız numaralar reddedilir.
- Kayıt ve checkout aynı sonucu verir.
- AR/EN hata mesajları doğru, form yeniden render edildiğinde değer kaybolmaz.
- dotnet build FilistinProje.sln.

Finalde kabul edilen formatları, saklanan canonical formatı ve +972 kararını açıkça yaz.
```

---

## Oturum 5 — Filistin kargo bölgeleri, şehir kaynağı, firma ve banka hesabı akışı

```text
7ANRPS48.com projesindeki kargo/şehir/IBAN veri eksikliğini ve hardcoded fallback karmaşasını production-safe biçimde düzelt.

Önce AGENTS.md ile AUDIT_RAPORU.md dosyalarını oku. Kapsam: B1, G17, G18, G19 ve banka havalesi akışı.

Temel gerçekler:

- Müşteri kargo firması seçmez; yalnız bölge/şehir/adres seçer.
- Admin sipariş için uygun firmayı sonradan atayabilir.
- Proje sahibi 48 bölgesi, Batı Şeria ve Kudüs ayrımını istemiştir; Gazze kapsamı kesin değil.
- United Express örnek kargo firmasıdır.
- Gerçek kargo fiyatları ve gerçek IBAN bilgileri bilinmiyor; bunları icat etme.

Yapılacaklar:

1. KargoBolge, KargoBolgeSehir, KargoBolgeFiyat, KargoFirmasi ve BankaHesap entity/migration/Ensure SQL/admin CRUD yapılarını incele.
2. Şehir/bölge listesini tek server-side veri kaynağından getir. Odeme.cshtml, checkout partial’ları ve Profil/Adreslerim’deki tekrarlanan hardcoded array’leri kaldır.
3. UTF-8/mojibake şehir adlarını düzelt. Arapça ve İngilizce adlar için mevcut model yeterli değilse en küçük doğru şema değişikliğini yap ve dual migration kuralını uygula.
4. Güvenli seed yaklaşımı:
   - Bölge ve şehir adları seed edilebilir.
   - United Express inactive/default admin-configurable firma olarak seed edilebilir.
   - Gerçek fiyat, takip URL’si ve IBAN uydurma.
   - Production’da eksik fiyat veya aktif IBAN varsa “0/ücretsiz” varsayma.
5. Bir şehir için aktif kargo fiyatı yoksa checkout sipariş oluşturmayı engellesin ve kullanıcıya AR/EN anlaşılır mesaj, admin loguna yapılandırma uyarısı versin.
6. Mağazadan teslimde kargo 0 kalmalı.
7. Ücretsiz kargo barajı yalnız geçerli teslimat/kargo hesabından sonra uygulanmalı.
8. Banka havalesi seçili fakat aktif banka hesabı yoksa siparişi sessizce oluşturma; kullanıcıya yapılandırma mesajı göster.
9. Kargo firması müşteriye seçim olarak gösterilmesin. Admin sipariş detayında firma atama ve takip numarası akışını doğrula.
10. Admin CRUD permission matrix ve sidebar kurallarını koru.
11. Seed idempotent olsun; her startup’ta duplicate bölge/şehir/firma üretmesin.

Doğrulama:

- Checkout şehirleri DB’den gelir; view’larda hardcoded kopya kalmaz.
- Beytüllahim/Kudüs adlarında mojibake yoktur.
- Bilinen fiyatı olan şehir doğru ücret getirir.
- Fiyatı olmayan şehir ücretsiz kabul edilmez.
- Mağazadan teslim 0 kargo.
- Ücretsiz kargo limiti doğru.
- Aktif IBAN yokken banka havalesi güvenli biçimde engellenir.
- Admin firma/şehir/fiyat/banka hesabını yönetebilir.
- EF migration ve EnsureMissingMarch2026SchemaAsync uyumludur.
- dotnet build FilistinProje.sln.

Finalde gerçek fiyat, IBAN, Gazze kapsamı ve takip URL’si için proje sahibinden gereken verileri “manuel yapılandırma” listesi olarak yaz.
```

---

## Oturum 6 — AR/EN ürün lokalizasyonu, para formatı ve metin tutarlılığı

```text
7ANRPS48.com storefront’unda Arapça/İngilizce lokalizasyon ve ILS para gösterimi sorunlarını düzelt.

Önce AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. Kapsam: B8, B12, L1-L8, F3, F4 ve F8. Oturum 5 tamamlandıysa şehirleri yeniden hardcode etme.

Yapılacaklar:

1. Urun entity’sindeki Baslik, BaslikAr, BaslikEn, Aciklama alanları ve varsa Localized* property/helper’ları incele. Tek, tutarlı fallback sırası belirle:
   - AR kültüründe Arapça → mevcut güvenli fallback.
   - EN kültüründe İngilizce → mevcut güvenli fallback.
2. Ürün kartı, ürün detay, sepet, checkout özeti, sipariş geçmişi, e-posta ve PDF fatura dahil kullanıcıya görünen ürün adlarını doğru kültürden üret.
3. SepetItem içinde başlık snapshot’ı tutuluyorsa dil değiştirince neden Arapça kaldığını çöz. Sipariş tarihsel bütünlüğünü bozmadan storefront gösterimi için doğru stratejiyi uygula.
4. JavaScript’teki tüm `toLocaleString('tr-TR')` kalıntılarını merkezi bir `formatMoney`/Intl.NumberFormat helper’ına taşı:
   - AR için uygun Filistin/Arabic locale ve ILS.
   - EN için en-US veya uygun İngilizce locale ve ILS.
   - Para simgesi ve ondalık gösterimi sayfalar arasında tutarlı.
5. Server-side decimal/para formatlarıyla JS formatı çelişmesin.
6. Bozuk encoding karakterlerini ve görünmez kontrol karakterlerini ara. Kaynakları UTF-8 olarak düzelt.
7. Admin başlığındaki “Vendor Panel” ifadesini bağlama uygun “Admin Panel” AR/EN karşılığıyla düzelt.
8. Sepet badge’in toplam adet mi farklı ürün sayısı mı gösterdiğini belirle. Header, drawer ve “X ürün” metinlerinde aynı semantiği kullan.
9. Türkçe dil desteğini geri ekleme. Admin panelinin Türkçe kaynak metinleri ile public AR/EN localizer kararını AGENTS.md’ye göre koru.
10. Eksik localizer key varsa EN ve AR resource dosyalarına birlikte ekle.

Doğrulama:

- AR/EN ana sayfa, ürün listesi, detay, sepet, checkout ve sipariş geçmişini kontrol et.
- EN sepetinde Arapça ürün adı kalmamalı; veri gerçekten EN içermiyorsa kontrollü fallback açıkça belgelenmeli.
- Repository’de kullanıcı yüzüne çıkan `tr-TR`, TL, TRY ve bozuk şehir metni kalmamalı.
- 44,987 ILS örneği AR ve EN’de kültüre uygun ve matematiksel olarak aynı görünmeli.
- dir/lang doğru, 390 ve 1440 px’te taşma yok.
- dotnet build FilistinProje.sln.

Finalde fallback kararını ve dokunulan tüm kullanıcı yüzlerini listele.
```

---

## Oturum 7 — Eski marka, domain, e-posta ve Türkiye kargo kalıntılarının temizliği

```text
7ANRPS48.com repository’sinde production davranışını etkileyen eski Canvasia/MeteorGaleri/Türkiye marka kalıntılarını temizle.

Önce AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. Kapsam: B7 ve L9-L14. Oturum 1’de yapılan secret güvenliği değişikliklerini koru.

Hedef marka:
- 7ANRPS48.com
- Filistin pazarı
- ₪ / ILS
- AR varsayılan, EN ikinci dil
- Yönetilebilir Filistin kargo firmaları

Yapılacaklar:

1. Case-insensitive olarak Canvasia, MeteorGaleri, meteorgaleri, Türkiye, Turkey, İstanbul, +90, TR00, TRY, TL, Aras, MNG, kastamonuesnaf ve eski domain/e-posta/logo referanslarını ara.
2. Her eşleşmeyi sınıflandır:
   - Runtime/production etkili
   - Configuration/deployment
   - Kullanıcıya görünen içerik
   - Tarihî migration veya arşiv dokümanı
3. Runtime etkili kalıntıları düzelt:
   - SiteSettingsService fallback logo/e-posta/site adı
   - SmtpEmailService kargo takip linkleri
   - E-posta şablonları ve Content-ID
   - SEO/meta/sitemap/base URL fallback
   - PDF başlık/logo
   - package metadata
4. Kargo takip URL’sini hardcoded United Express URL’si uydurarak değiştirme. KargoFirmasi/admin configuration’dan gelen güvenli URL template’i kullan; yoksa link gösterme.
5. Final domain henüz kesin yapılandırılmamışsa `filistin.kastamonuesnaf.com.tr` yerine kod içine sahte domain yazma. Base URL’yi environment/config zorunlu hale getir ve eksik production config’i açık bildir.
6. Eski SQL dump/migration snapshot’larını körlemesine değiştirme; migration checksum/tarihsel güvenilirliği bozma. Runtime’a yanlışlıkla import edilebilen dump’ları açık biçimde legacy olarak ayır veya belgele.
7. KanvasDbContext gibi geniş rename’in migration namespace/snapshot etkisini değerlendir. Sadece estetik için riskli rename yapma; gerekiyorsa ayrı teknik borç olarak bırak.
8. Siyah/altın marka brief’i için yalnız temel site ayarı fallback/seed rengini düzelt. Bu oturumda geniş UI redesign yapma.
9. Gerçek secret veya gerçek kullanıcı e-postası commit etme.

Doğrulama:

- Runtime dosyalarında eski marka ve Türkiye kargo linki kalmadığını göster.
- AR/EN e-posta ön izlemesinde 7ANRPS48 adı/logo kaynağı doğru.
- Tracking URL yalnız admin yapılandırılmışsa görünür.
- dotnet build FilistinProje.sln.

Finalde bilinçli olarak bırakılan tarihî referansları ve proje sahibinin sağlaması gereken final domain/e-posta/logo değerlerini ayrı listele.
```

---

## Oturum 8 — Ürün görselleri, galeri/video, düşük stok ve ilgili ürünler

```text
7ANRPS48.com ürün listeleme ve ürün detay deneyimindeki medya/ürün sunumu eksiklerini düzelt.

Önce AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. Kapsam: B5, F1, G5, G7 ve ilgili ürünler bölümü. Uygulamayı çalıştırıp mevcut UI’ı görmeden tasarım yapma.

Yapılacaklar:

1. UrunResim.ResimYolu ve MedyaTipi modelini, admin upload akışını, ürün kartlarını ve ürün detay view/partial’larını incele.
2. `/img/products/placeholder.webp` 404 sorununu çöz. Küçük, optimize, markaya uyumlu ve gerçekten mevcut bir fallback asset kullan. Üretimde olmayan gerçek ürün fotoğrafları uydurma.
3. Ürün görseli yoksa:
   - Kırık img/404 olmamalı.
   - Alt text kültüre uygun ürün adı olmalı.
   - Kart ve detay layout’u bozulmamalı.
4. Birden fazla görsel için erişilebilir ana galeri + thumbnail yapısı kur/onar. RTL/LTR yönleri, klavye, touch swipe ve loading davranışını düşün.
5. MedyaTipi=Video destekleniyorsa güvenli video render et:
   - autoplay zorunlu olmasın.
   - poster/fallback olsun.
   - dış URL veya upload güvenlik sınırlarını koru.
6. Varyasyon seçimi varsa seçilen varyasyona ait medya/fiyat/stok güncellemesini bozma.
7. Stok 1–4 arası için “Son X adet” AR/EN uyarısı göster. Stok 0 ve StoktaYokSatisIzni/StokBiteniGriGoster kurallarına saygı duy.
8. Ürün detayında kategori/marka bazlı “İlgili ürünler” bölümü ekle veya mevcutsa düzelt. Silinmiş, pasif, fiyatı gizli veya stok politikasına aykırı ürün gösterme.
9. Görsellerde width/height veya aspect-ratio, lazy loading ve uygun responsive boyutlar kullan; CLS/LCP’yi kötüleştirme.
10. Demo seed’e telif belirsiz internet görselleri ekleme. Gerekirse yalnız placeholder ve mevcut repository asset’lerini kullan.

Doğrulama:

- Ürün görselli ve görselsiz iki örneği test et.
- Network/console’da placeholder 404 yok.
- 390, 768 ve 1440 px; AR RTL ve EN LTR.
- Galeri klavye/touch ile çalışır.
- Düşük stok doğru varyanta göre değişir.
- İlgili ürün linkleri doğru slug/id route’una gider.
- dotnet build FilistinProje.sln.

Finalde ekran/route bazlı doğrulama sonuçlarını ve gerçek katalog görseli için kullanıcıdan gereken asset listesini yaz.
```

---

## Oturum 9 — Canlı arama, marka/özellik filtreleri ve mobil navigasyon

```text
7ANRPS48.com ürün keşif deneyimini tamamla: canlı arama, marka/özellik filtreleri ve mobil drawer düzeltmeleri.

Önce AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. Kapsam: G3, G4, F5 ve F6. Mevcut UrunController filtre query parametrelerini ve BuildOzellikFilterUrl helper’ını koru/yeniden kullan.

Gereksinimler:

1. Mevcut kategori, fiyat slider’ı, sıralama, pagination ve query-string davranışını haritala.
2. Marka filtresini gerçek ürün verisinden üret; boş/null markaları göstermeme ve sayıları doğru hesaplama.
3. Dinamik UrunOzellikTanimi/UrunOzellikDegeri filtrelerini ekle. Birden çok filtrenin birlikte uygulanması, temizlenmesi ve URL ile paylaşılabilmesi gerekir.
4. Filtre linkleri mevcut kategori, arama, min/max fiyat ve sıralama parametrelerini yanlışlıkla kaybetmesin.
5. Mobile 360–390 px’te sidebar’ı sıkıştırma; erişilebilir filter drawer/bottom sheet veya uygun mevcut tasarım kullan.
6. Canlı arama:
   - En az 2 karakterden sonra debounce.
   - AR/EN ürün adı, marka ve kategori araması.
   - Sonuçlarda optimize görsel, localized başlık ve gerekiyorsa fiyat.
   - Klavye okları, Enter, Escape ve dışarı tıklama.
   - Loading/empty/error durumları.
   - XSS-safe render.
   - Kısa sorgu ve abuse için server limit/pagination/rate davranışı.
7. SQL tarafında parametrik LINQ kullan; tüm kataloğu memory’ye çekme.
8. Mobil drawer’a sepet bağlantısını ve tutarlı badge’i ekle. Focus trap, Escape ve aria-expanded davranışını kontrol et.
9. Ana sayfa slider’ını bu oturumda baştan tasarlama; yalnız arama/navigation entegrasyonu gerekiyorsa dokun.

Doğrulama:

- Marka + özellik + fiyat + kategori + sıralama kombinasyonu.
- Filtre temizleme ve browser back/forward.
- AR/EN arama ve RTL/LTR dropdown.
- 360, 390, 768, 1440 px.
- Arama özel karakterleri XSS veya server hatası üretmez.
- Büyük veri için sorguda pagination/limit bulunur.
- dotnet build FilistinProje.sln.

Finalde eklenen endpointleri, query parametre sözleşmesini ve test edilen kombinasyonları yaz.
```

---

## Oturum 10 — CSRF, rate limit, XSS, IDOR, session ve ziyaretçi gizliliği

```text
7ANRPS48.com projesinde rapordaki doğrulanmamış güvenlik hipotezlerini önce kanıtla, gerçek olanları düzelt.

Önce AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. Kapsam: B14, B22, B23, B24, B28, B29 ve B31. “Hipotez” olan bir bulguyu incelemeden açık kabul etme.

Çalışma planı:

1. Tüm state-changing POST/PUT/DELETE endpoint’lerini çıkar:
   - Public controllers
   - Admin area
   - Sepet/favori/yorum/kupon/profil/hesap/sipariş
2. Cookie tabanlı isteklerde antiforgery kapsamını doğrula. Eksik endpoint’lere token ekle; AJAX çağrılarında header/form token akışını düzelt. API gibi farklı auth modeli varsa körlemesine MVC token ekleme.
3. Rate limiter:
   - Login, kayıt, şifre sıfırlama, e-posta doğrulama ve kupon deneme gibi abuse-sensitive endpoint’lerde “auth” veya amaca uygun policy uygula.
   - Genel sayfalarda mevcut “general” politikasını bozma.
   - IP arkasında reverse proxy kullanımı varsa ForwardedHeaders güvenlik sınırını incele.
4. Stored XSS:
   - `Html.Raw`, açıklama, yorum, kurumsal sayfa, slider ve admin rich text alanlarını ara.
   - Rich HTML gerçekten gerekiyorsa allowlist sanitizer kullan.
   - Script, event handler, javascript: URL, iframe gibi aktif içeriği engelle.
   - Sadece tüm HTML’i encode ederek gerekli içerik editörünü kırma.
5. IDOR/BOLA:
   - Guest sepet session id istemciden değiştirilebilir mi?
   - Profil sipariş/adres/belge endpoint’leri user id ownership kontrolü yapıyor mu?
   - Admin permission matrix controller/action bazında gerçekten uygulanıyor mu?
   Gerçek açık varsa düzelt; session id’yi query/form’dan kabul etme.
6. Session fixation:
   - Login sonrası ASP.NET Core Identity cookie yenilenmesini ve session kullanımını incele.
   - Gerekliyse sepeti kaybetmeden güvenli session yenileme/guest cart merge tasarla.
7. Ziyaretçi logları:
   - IP/User-Agent/path retention ve hassas query-string riskini azalt.
   - Parola/token/reset code gibi query değerlerini asla loglama.
   - Yapılandırılabilir retention/anonymization uygula; gereksiz tam IP saklama.
8. Open redirect olasılığını returnUrl kullanan tüm login/redirect kodlarında doğrula; yalnız local URL kabul et.
9. Güvenlik düzeltmelerinde kullanıcı akışlarını ve admin yetkilerini bozmamaya dikkat et.

Doğrulama:

- Token’sız state-changing istek reddedilir.
- Geçerli UI formları çalışır.
- Login/kupon brute-force rate limit alır.
- Script payload storefront’ta çalışmaz.
- Müşteri başka kullanıcının sipariş/adres/belgesini göremez.
- Harici returnUrl yönlendirilmez.
- Loglarda token/parola/tam hassas query bulunmaz.
- dotnet build FilistinProje.sln.

Finalde her B numarası için “doğrulandı ve düzeltildi / yanlış alarm / kalan risk” tablosu üret.
```

---

## Oturum 11 — Migration hataları, readiness health check ve production operasyonu

```text
7ANRPS48.com uygulamasının migration ve production başlangıç davranışını güvenli hale getir.

Önce AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. Kapsam: B17, B21 ve operasyon bölümündeki DB readiness/DataProtection riskleri.

Yapılacaklar:

1. Program.cs içindeki EF Migrate + seed + EnsureMissingMarch2026SchemaAsync sırasını ayrıntılı incele.
2. Migration hatasının catch edilip uygulamanın eski şemayla trafik kabul etmesi riskini düzelt:
   - Production’da kritik migration/şema hatası varsa readiness false olmalı ve uygulama normal trafik kabul etmemeli veya startup fail-fast olmalı.
   - Development’ta tanı koymayı kolaylaştır.
   - DB geçici olarak erişilemiyorsa mevcut “app crash etmez, background jobs disabled” kararını bilinçli şekilde koru veya gerekçeli iyileştir; migration failure ile DB unavailable durumunu birbirine karıştırma.
3. `/health` liveness ve `/health/ready` readiness ayrımı yap. Readiness DB bağlantısını ve gerekiyorsa migration durumunu kontrol et.
4. Health endpoint’lerinde connection string, exception stack veya secret sızdırma.
5. Dual migration sistemini kaldırma veya büyük rewrite yapma. Mevcut proje kuralına göre:
   - EF migration
   - EnsureMissingMarch2026SchemaAsync
   uyumunu denetle ve yeni fark varsa düzelt.
6. Elle `__EFMigrationsHistory` yazan kodları dikkatle incele; uygulanmamış migration’ı uygulanmış gibi işaretleme riskini azalt.
7. DataProtection key’lerinin Docker/container restart sonrası kalıcılığı için configuration ve docker-compose volume yaklaşımını uygula veya net deployment örneği ekle. Secret key’i repo’ya koyma.
8. Production migration/rollback/backup adımlarını kısa ve gerçek komutlarla dokümante et. Linux komutları yerine projenin Windows/PowerShell ve Docker bağlamını gözet.
9. Tarihî migration dosyalarını silme veya yeniden yazma.

Doğrulama:

- Normal DB ile app ready.
- DB kapalıyken liveness/readiness beklenen farklı sonuçları verir.
- Migration hatası simülasyonunda normal trafik yanlışlıkla hazır görünmez.
- Health response secret içermez.
- DataProtection volume/config açıkça tanımlı.
- dotnet build FilistinProje.sln.

Finalde seçilen startup politikasını, production deployment sırasını ve rollback koşullarını yaz.
```

---

## Oturum 12 — N+1, cache, pagination ve asset performansı

```text
7ANRPS48.com projesinde ölçülebilir performans iyileştirmeleri yap; kanıtsız mikro-optimizasyon yapma.

Önce AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. Kapsam: B10, B19 ve performans bölümündeki pagination/2.3 MB logo/asset riskleri.

Çalışma yöntemi:

1. Uygulamayı gerçekçi veriyle veya mevcut DB ile çalıştır. Ana sayfa, /Urun, arama, kategori, sepet ve sık kullanılan admin listelerinde SQL sayısı/süre ve network asset boyutlarını ölç.
2. N+1 iddiasını log/profil kanıtı olmadan kabul etme. Hangi route’un kaç sorgu ürettiğini raporla.
3. N+1 varsa projection, Include/ThenInclude veya toplu sorguyla düzelt. Devasa graph Include ederek cartesian explosion üretme; AsSplitQuery/projection kararını gerekçelendir.
4. Büyük listelerde server-side pagination uygula/onar. Filtre ve sıralama query parametrelerini koru. Admin listelerini de kontrol et.
5. Site settings, kategori ağacı ve ana sayfa bölümleri için uygun IMemoryCache kullan:
   - Net cache key.
   - Kısa/uygun TTL.
   - Admin güncellemesinden sonra invalidation.
   - Kullanıcıya özel/sepet/fiyat verisini yanlışlıkla global cache’leme.
   - Çok instance production için memory cache sınırlamasını belgele.
6. 2.3 MB civarı logo/hero SVG veya ağır asset’i analiz et. Görsel kaliteyi koruyarak optimize et; responsive asset, preload ve lazy-loading kararlarını doğru kullan.
7. Render-blocking CDN/CSS/JS, duplicate DOM topbar ve gereksiz scriptleri incele.
8. İş kurallarını veya görünümü değiştirme; performans değişikliği sonrası regression yap.

Doğrulama:

- Önce/sonra route bazlı sorgu sayısı ve süre.
- Ürün sayfasında pagination.
- Admin ayarı değişince cache eski değer göstermiyor.
- Logo/hero transfer boyutu anlamlı şekilde azalıyor.
- AR/EN, 390/1440 görünüm bozulmuyor.
- dotnet build FilistinProje.sln.

Finalde yalnız ölçülmüş kazançları yaz; ölçülemeyenleri tahmin olarak etiketle.
```

---

## Oturum 13 — Sipariş mimarisini service katmanına taşıma

```text
7ANRPS48.com checkout akışındaki mimari borcu, çalışan ve güvenliği doğrulanmış iş kurallarını bozmadan azalt.

Bu oturumu ancak fiyat/stok bütünlüğü oturumu tamamlandıktan sonra yap. Önce AGENTS.md, AUDIT_RAPORU.md ve mevcut git diff’i oku. Kapsam: B11 ve SiparisController’ın aşırı sorumluluğu.

Hedef:

- SiparisController’daki doğrudan KanvasDbContext sipariş oluşturma mantığını Service katmanına taşımak.
- Controller’ı HTTP/model state/orchestration seviyesinde tutmak.
- Fiyat, stok, kupon, kargo, hediye paketi, COD ve toptancı kurallarını tek transaction’lı application service içinde toplamak.

Kurallar:

1. Önce mevcut son davranışı belgeleyen akış haritası çıkar. Önceki oturumun atomic stok ve server-side fiyat düzeltmesini kaybetme.
2. Core/Data/Service/Web bağımlılık yönüne uy.
3. Uygun request/result modelleri oluştur:
   - Başarılı sipariş id/numarası.
   - Fiyat değişti.
   - Stok yetersiz.
   - Kargo yapılandırılmamış.
   - Kupon geçersiz.
   - Validation/business error.
4. Service HTTP, ViewBag, TempData veya HttpContext’e bağımlı olmasın. Kullanıcı/rol/session için gereken sade verileri controller sağlasın.
5. Transaction sınırı service içinde açık ve tek olsun.
6. E-posta/PDF/background side-effect’lerinin transaction commit’inden önce gönderilmediğini doğrula. Commit sonrası hata siparişi rollback olmuş gibi göstermesin.
7. Repository/UoW ile doğrudan DbContext arasında projede iki paralel stil var. Bu oturumda tüm solution’ı rewrite etme; checkout için tutarlı en küçük mimariyi seç.
8. Public davranış, route, form alanları ve localizer mesajları geriye uyumlu kalsın.
9. Yeni test projesi oluşturma; mevcut talimata uy.

Doğrulama:

- Oturum 2’deki tüm fiyat/stok/hediye/kupon/kargo/COD senaryolarını yeniden çalıştır.
- Controller’da doğrudan Siparis/SiparisDetay/Stok persistence kalmadığını doğrula.
- Transaction rollback ve commit sonrası bildirim davranışını kontrol et.
- dotnet build FilistinProje.sln.

Finalde önce/sonra sorumluluk dağılımını, service API’sini ve regression sonuçlarını yaz.
```

---

## Oturum 14 — Tamamlandı denilen fakat doğrulanmamış ticari akışlar

```text
7ANRPS48.com projesinde raporda “kodda var fakat uçtan uca doğrulanmadı” olarak işaretlenen ticari özellikleri tek tek test et ve yalnız gerçekten kırık olanları düzelt.

Önce AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. Kapsam:

- Ürün/site yorumu ve admin onayı
- Ürün bazlı WhatsAppSiparisVarMi + FiyatGizliMi
- Hediye paketi seçim UI’ı
- Reçeteli kategori ve reçete zorunluluğu
- Toptancı başvuru/onay/özel fiyat/iskonto/minimum sipariş
- COD limit altı ve üstü
- Sipariş iptal ve iade
- PDF fatura
- Çark/ödül kullanıcı akışı
- Favoriler ve fiyat düşüş bildirimi

Yöntem:

1. Her özellik için önce entity → migration/Ensure SQL → admin ayarı → service/controller → public UI → sonuç/veri zincirini çıkar.
2. Özelliğin çalıştığını sadece dosyanın varlığıyla kabul etme. Uygulamayı çalıştır, uygun test verisi oluştur ve gerçek kullanıcı akışını tamamla.
3. Her özellik için şu sonucu ver:
   - Çalışıyor, değişiklik gerekmedi.
   - Kısmen çalışıyor, düzeltildi.
   - Kırık, düzeltildi.
   - Harici veri/servis eksikliği nedeniyle blokeli.
4. Düzeltmelerde önceki oturumların checkout, upload, kargo ve lokalizasyon kararlarını bozma.
5. WhatsApp ürününde normal sepete ekleme/checkout yolu gerçekten kapanmalı; fiyat gizliyse HTML/structured data/API üzerinden yanlışlıkla sızmamalı.
6. Reçeteli ürün başka normal ürünlerle aynı sepetteyken belge zorunluluğu korunmalı.
7. Toptancı fiyatı kart, detay, sepet ve server-side checkout’ta aynı olmalı.
8. PDF faturada doğru 7ANRPS48 markası, localized ürün adı, ILS toplamları, kargo/indirim/hediye/COD satırları bulunmalı.
9. Bildirim/e-posta için gerçek müşteriye veya gerçek SMTP’ye mesaj gönderme. Güvenli development sink/mock/preview kullan.
10. Çark ödülünde kupon üretimi abuse edilemiyorsa doğrula; tek kullanıcı/oturum sınırlarını kontrol et.

Doğrulama:

- Her özellik için kullanıcı rolü, veri önkoşulu, adımlar, beklenen/gerçek sonuç.
- AR/EN ve en az mobil 390 + desktop 1440.
- DB’de oluşan sonuçların kontrolü.
- dotnet build FilistinProje.sln.

Bu oturum geniştir: önce doğrula, sonra yalnız kırık olan yere küçük düzeltme yap. Finalde özellik bazlı kanıt tablosu üret.
```

---

## Oturum 15 — Admin CRUD, izin matrisi ve mobil/RTL kalite turu

```text
7ANRPS48.com Admin alanındaki tüm controller/view akışlarını sistematik biçimde test et ve kırıkları düzelt.

Önce AGENTS.md ve AUDIT_RAPORU.md dosyalarını oku. AdminBaseController, AdminPermissionMatrix, AdminSecurityRoles, ViewBag yetkileri ve _AdminLayout kuralları bağlayıcıdır.

Kapsam:

- Home, AnaSayfa
- Urun, Kategori, UrunOzellik, UrunImport, TopluFiyatGuncelle, SlugTool
- Siparis, Kullanici, Toptanci
- Bankalar, Kargo, Ayarlar, Slayt
- Rapor, Iletisim, Iade, Kupon, HomeSections, Bulten, Sayfa
- Search, Yorum, Ziyaretci, XyzSecretMonitor

Yapılacaklar:

1. Her controller için Index/Create/Edit/Delete/Detail ve özel POST action’ları çıkar.
2. SuperAdmin ve en az iki sınırlı rolle erişim test et:
   - İzinli GET çalışmalı.
   - İzinli POST çalışmalı.
   - Görüntüleyici/sınırlı rol yetkisiz POST yapamamalı.
   - Sidebar görünürlüğü gerçek permission ile uyumlu olmalı.
3. Eksik antiforgery, validation, overposting, not-found ve concurrency davranışını düzelt.
4. Delete işlemlerinde foreign-key/veri kaybı etkisini kontrol et; güvenli hata veya soft delete kuralı uygula.
5. 390/768/1440 px’te drawer, tablo overflow, form alanı, modal ve action butonlarını kontrol et.
6. Admin panelindeki dil kararı AGENTS.md’ye uygun kalsın. AR/EN localizer kullanılan mevcut bölümleri bozma; Türkçe destek dilini geri ekleme.
7. Tablo/liste sayfalarında pagination ve arama yoksa büyük veri riski olanlara server-side pagination ekle.
8. Export/import işlemlerinde dosya boyutu, içerik doğrulama ve formül injection risklerini kontrol et.
9. XyzSecretMonitor veya benzeri hassas sayfalarda secret değerlerini maskesiz gösterme.
10. Yeni admin controller/permission gerekiyorsa dört zorunlu noktayı güncelle: base class, matrix, ViewBag, sidebar.

Doğrulama:

- Controller/action/rol matrisi halinde sonuç.
- CRUD sırasında ModelState hata senaryosu.
- 390 px’te yatay sayfa taşması olmamalı; geniş tablolar kendi container’ında scroll olabilir.
- Console/server log’da beklenmeyen exception yok.
- dotnet build FilistinProje.sln.

Finalde değiştirilmemiş fakat doğrulanmış sayfaları da listele; yalnız problem çıkan dosyaları değiştir.
```

---

## Oturum 16 — Nihai regression, yeniden denetim ve production kararı

```text
7ANRPS48.com üzerinde final regression ve bağımsız yeniden denetim yap. Bu oturumun amacı yeni özellik eklemek değil; önceki tüm düzeltmelerin birlikte çalıştığını kanıtlamak ve kalan production engellerini bulmaktır.

Önce AGENTS.md, AUDIT_RAPORU.md ve çalışma klasöründeki güncel değişiklikleri oku. Eski audit sonucunu doğru kabul etme; bütün kritik bulguları mevcut kodda yeniden doğrula.

Zorunlu kontroller:

1. `dotnet build FilistinProje.sln` — hedef 0 hata, 0 uyarı.
2. DB/migration/Ensure SQL uyumu ve uygulama başlangıcı.
3. Secret taraması:
   - Admin123!
   - gerçek SMTP/API key
   - Canvasia/MeteorGaleri runtime kalıntıları
4. AR varsayılan RTL ve EN LTR.
5. Viewport: 360, 390/412, 768, 1024, 1440.
6. Sayfalar:
   - Home
   - Urun liste/arama/filtre
   - Urun detay
   - Sepet
   - Checkout
   - Login/kayıt/reset
   - Profil/adres/sipariş
   - Temel admin CRUD
7. Kritik ticari senaryolar:
   - Server-side fiyat
   - Atomic stok
   - Hediye paketi
   - Kupon
   - Kargo ve ücretsiz kargo
   - COD limit altı/üstü
   - Banka hesabı
   - Reçete/kimlik
   - WhatsApp/fiyat gizleme
   - Toptancı
8. Güvenlik:
   - Hassas upload anonim erişim
   - CSRF
   - XSS
   - IDOR
   - rate limit
   - open redirect
   - admin permission
9. Lokalizasyon:
   - Ürün adı
   - ILS formatı
   - Şehir adları
   - e-posta/PDF
   - tr-TR/+90/TR00/TL kalıntıları
10. Operasyon:
   - liveness/readiness
   - DB unavailable
   - migration failure
   - DataProtection persistence
11. Console’da 404, JS error, failed request; server log’da exception.

Kurallar:

- Önce test et ve kanıt topla.
- Küçük regression bulursan düzelt ve yeniden test et.
- Büyük veya tasarım kararı gerektiren yeni sorun bulursan kapsamı gizlice genişletme; açık blocker olarak raporla.
- Gerçek ödeme, gerçek SMTP, gerçek müşteri belgesi veya production sistemine dış etki oluşturma.
- İlgisiz refactor yapma ve commit oluşturma.

Nihai çıktı:

1. PASS/FAIL regression matrisi.
2. Eski B0–B31 bulgularının son durumu.
3. Kalan P0/P1/P2/P3 listesi.
4. Manuel production configuration listesi: domain, SMTP, secret rotation, IBAN, kargo fiyatları, firma takip URL’si, gerçek ürün görselleri.
5. “Production’a hazır / koşullu hazır / hazır değil” kararı ve somut gerekçesi.
6. Değiştirilen dosyalar ve son build sonucu.
```

---

## Önerilen sıra

1. Oturum 1 — Kimlik bilgileri ve secret
2. Oturum 2 — Fiyat, stok, checkout toplamı
3. Oturum 3 — Hassas upload ve kamera
4. Oturum 4 — Telefon
5. Oturum 5 — Kargo/şehir/IBAN
6. Oturum 6 — Lokalizasyon ve para
7. Oturum 7 — Marka temizliği
8. Oturum 8 — Ürün medya ve detay UX
9. Oturum 9 — Arama/filtre/mobil navigasyon
10. Oturum 10 — Güvenlik hardening
11. Oturum 11 — Migration/health/operasyon
12. Oturum 12 — Performans
13. Oturum 13 — Sipariş mimari refactor
14. Oturum 14 — Doğrulanmamış ticari özellikler
15. Oturum 15 — Admin tam tur
16. Oturum 16 — Final regression ve production kararı

Oturum 2 tamamlanmadan siteyi gerçek siparişe açma. Oturum 16 PASS vermeden production’a hazır kabul etme.
