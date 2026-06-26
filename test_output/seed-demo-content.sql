BEGIN;

DELETE FROM "SepetItems";
DELETE FROM "Sepetler";
DELETE FROM "UrunResimleri";
DELETE FROM "UrunSecenekleri";
DELETE FROM "Urunler";
DELETE FROM "Slaytlar";
DELETE FROM "Kategoriler";

INSERT INTO "Kategoriler" (
  "Ad", "KisaAciklama", "Aciklama", "Slug", "GorselUrl", "BannerUrl", "SeoTitle", "SeoDescription",
  "UstMetin", "AltMetin", "KampanyaEtiketi", "UrunSiralamaTipi", "Sira", "AktifMi", "ParentKategoriId",
  "OlusturulmaTarihi", "SilindiMi", "AdAr", "AdEn", "KisaAciklamaAr", "KisaAciklamaEn", "AciklamaAr", "AciklamaEn",
  "SeoTitleAr", "SeoTitleEn", "SeoDescriptionAr", "SeoDescriptionEn", "UstMetinAr", "UstMetinEn", "AltMetinAr", "AltMetinEn", "KampanyaEtiketiAr", "KampanyaEtiketiEn", "ReceteGerekliMi"
)
VALUES
('Filistin Koleksiyonu', 'Filistin temalı özel ürünler', 'Filistin ruhunu taşıyan özel tasarımlar.', 'filistin-koleksiyonu', '/img/products/demo-product.svg', '/img/products/demo-product.svg', 'Filistin Koleksiyonu - 7ANRPS48', 'Filistin temalı ürün koleksiyonu', '', '', 'Özel Koleksiyon', 'manual', 1, true, NULL, NOW(), false, 'مجموعة فلسطين', 'Palestine Collection', 'منتجات خاصة مستوحاة من فلسطين', 'Special Palestine inspired products', 'تصاميم تحمل روح فلسطين', 'Designs carrying the spirit of Palestine', 'مجموعة فلسطين - 7ANRPS48', 'Palestine Collection - 7ANRPS48', 'مجموعة منتجات مستوحاة من فلسطين', 'Palestine inspired product collection', '', '', '', '', 'مجموعة خاصة', 'Special Collection', false),
('Ev ve Dekorasyon', 'Eviniz için dekoratif ürünler', 'Yaşam alanlarını güzelleştiren ürünler.', 'ev-ve-dekorasyon', '/img/products/demo-product.svg', '/img/products/demo-product.svg', 'Ev ve Dekorasyon - 7ANRPS48', 'Ev dekorasyon ürünleri', '', '', 'Yeni', 'manual', 2, true, NULL, NOW(), false, 'المنزل والديكور', 'Home & Decoration', 'منتجات ديكور لمنزلك', 'Decorative products for your home', 'منتجات تجعل مساحة المعيشة أجمل', 'Products that beautify living spaces', 'المنزل والديكور - 7ANRPS48', 'Home & Decoration - 7ANRPS48', 'منتجات ديكور منزلية', 'Home decoration products', '', '', '', '', 'جديد', 'New', false),
('Hediyelik Ürünler', 'Anlamlı hediyeler', 'Sevdikleriniz için özel hediyelikler.', 'hediyelik-urunler', '/img/products/demo-product.svg', '/img/products/demo-product.svg', 'Hediyelik Ürünler - 7ANRPS48', 'Hediyelik ürünler', '', '', 'Hediye', 'manual', 3, true, NULL, NOW(), false, 'الهدايا', 'Gifts', 'هدايا مميزة', 'Meaningful gifts', 'هدايا خاصة لمن تحبون', 'Special gifts for loved ones', 'الهدايا - 7ANRPS48', 'Gifts - 7ANRPS48', 'منتجات هدايا مميزة', 'Gift products', '', '', '', '', 'هدية', 'Gift', false);

INSERT INTO "Slaytlar" (
  "Baslik", "AltBaslik", "Aciklama", "ResimUrl", "VideoUrl", "Tur", "Sira", "AktifMi", "OlusturmaTarihi",
  "BaslikAr", "BaslikEn", "AltBaslikAr", "AltBaslikEn", "AciklamaAr", "AciklamaEn"
)
VALUES
('Filistin İçin Özel Tasarımlar', '7ANRPS48 Koleksiyonu', 'Evinize anlam ve zarafet katacak ürünleri keşfedin.', '/img/products/demo-product.svg', NULL, 'Görsel', 1, true, NOW(), 'تصاميم خاصة لفلسطين', 'Special Designs for Palestine', 'مجموعة 7ANRPS48', '7ANRPS48 Collection', 'اكتشف منتجات تضيف معنى وأناقة إلى منزلك.', 'Discover products that add meaning and elegance to your home.'),
('Evinize Zarafet Katın', 'Dekorasyon Ürünleri', 'Modern ve anlamlı dekoratif seçenekler.', '/img/products/demo-product.svg', NULL, 'Görsel', 2, true, NOW(), 'أضف الأناقة إلى منزلك', 'Add Elegance to Your Home', 'منتجات الديكور', 'Decoration Products', 'خيارات ديكور حديثة وذات معنى.', 'Modern and meaningful decorative options.'),
('Sevdiklerinize Anlamlı Hediyeler', 'Özel Hediye Seçenekleri', 'Hatırası olan hediyelik ürünleri inceleyin.', '/img/products/demo-product.svg', NULL, 'Görsel', 3, true, NOW(), 'هدايا ذات معنى لمن تحب', 'Meaningful Gifts for Loved Ones', 'خيارات هدايا خاصة', 'Special Gift Options', 'اكتشف الهدايا التي تحمل ذكرى جميلة.', 'Explore memorable gift products.');

WITH k AS (
  SELECT "Id", "Slug" FROM "Kategoriler"
), inserted_products AS (
  INSERT INTO "Urunler" (
    "Baslik", "BaslikEn", "BaslikAr", "KisaAd", "Slug", "UrlYolu", "SKU", "Barkod", "Marka", "UrunTipi", "Etiketler",
    "KisaAciklama", "KisaAciklamaEn", "KisaAciklamaAr", "Aciklama", "AciklamaEn", "AciklamaAr",
    "TeknikOzellikler", "MalzemeBilgisi", "BakimTalimati", "PaketlemeBilgisi", "AnaGorselUrl", "StokDurumu",
    "Fiyat", "IndirimliFiyat", "TopFiyat", "Maliyet", "KdvOrani", "KategoriId", "AktifMi", "YayindaMi", "YeniUrunMu", "OneCikanMi", "KampanyaliMi", "AnaSayfadaGoster",
    "Sira", "GoruntulenmeSayisi", "SatisSayisi", "FavoriSayisi", "MinSiparisAdedi", "MaxSiparisAdedi",
    "UretimSuresiGun", "KargoyaVerilisSuresiGun", "TahminiTeslimSuresiGun", "OlusturulmaTarihi", "SilindiMi",
    "SeoTitle", "SeoTitleEn", "SeoTitleAr", "SeoDescription", "SeoDescriptionEn", "SeoDescriptionAr", "SeoKeywords",
    "HediyePaketFiyati", "HediyePaketiVarMi", "WhatsappSiparisVarMi", "FiyatGizliMi", "ToptanciUrunGrubuId", "KampanyaBitisTarihi"
  )
  SELECT * FROM (
    SELECT 'Filistin Motifli Duvar Tablosu', 'Palestine Pattern Wall Art', 'لوحة حائط بزخارف فلسطينية', 'Filistin Tablo', 'filistin-motifli-duvar-tablosu', '/Urun/Detay/filistin-motifli-duvar-tablosu', 'DEMO-FIL-001', '869000100001', '7ANRPS48', 'Kanvas', 'filistin,tablo,dekor', 'Filistin motifli dekoratif tablo.', 'Decorative wall art with Palestine patterns.', 'لوحة ديكور بزخارف فلسطينية.', 'Salon ve ofisler için anlamlı dekoratif tablo.', 'Meaningful decorative wall art for homes and offices.', 'لوحة ديكور ذات معنى للمنزل والمكتب.', 'Yüksek kalite baskı', 'Kanvas', 'Kuru bezle temizleyiniz', 'Koruyucu ambalaj', '/img/products/demo-product.svg', 'Stokta', 250::numeric, 199::numeric, 199::numeric, 90::numeric, 20::numeric, (SELECT "Id" FROM k WHERE "Slug"='filistin-koleksiyonu'), true, true, true, true, true, true, 1, 0, 0, 0, 1, NULL::integer, 2, 1, 4, NOW(), false, 'Filistin Motifli Duvar Tablosu', 'Palestine Pattern Wall Art', 'لوحة حائط بزخارف فلسطينية', 'Filistin motifli tablo', 'Palestine pattern wall art', 'لوحة بزخارف فلسطينية', 'filistin,kanvas,tablo', 15::numeric, true, true, false, NULL::integer, NULL::timestamp with time zone
    UNION ALL SELECT 'Kudüs Silüeti Kanvas', 'Jerusalem Skyline Canvas', 'كانفاس أفق القدس', 'Kudüs Kanvas', 'kudus-silueti-kanvas', '/Urun/Detay/kudus-silueti-kanvas', 'DEMO-FIL-002', '869000100002', '7ANRPS48', 'Kanvas', 'kudüs,kanvas,dekor', 'Kudüs silüetli özel kanvas.', 'Special canvas with Jerusalem skyline.', 'كانفاس خاص بأفق القدس.', 'Kudüs atmosferini evinize taşıyan tasarım.', 'A design bringing Jerusalem atmosphere to your home.', 'تصميم يجلب أجواء القدس إلى منزلك.', 'Yüksek kalite baskı', 'Kanvas', 'Kuru bezle temizleyiniz', 'Koruyucu ambalaj', '/img/products/demo-product.svg', 'Stokta', 320::numeric, 269::numeric, 269::numeric, 120::numeric, 20::numeric, (SELECT "Id" FROM k WHERE "Slug"='filistin-koleksiyonu'), true, true, true, true, true, true, 2, 0, 0, 0, 1, NULL::integer, 3, 1, 5, NOW(), false, 'Kudüs Silüeti Kanvas', 'Jerusalem Skyline Canvas', 'كانفاس أفق القدس', 'Kudüs temalı kanvas', 'Jerusalem themed canvas', 'كانفاس مستوحى من القدس', 'kudüs,kanvas', 15::numeric, true, true, false, NULL::integer, NULL::timestamp with time zone
    UNION ALL SELECT 'Zeytin Dalı Dekoratif Set', 'Olive Branch Decor Set', 'طقم ديكور غصن الزيتون', 'Zeytin Set', 'zeytin-dali-dekoratif-set', '/Urun/Detay/zeytin-dali-dekoratif-set', 'DEMO-EV-001', '869000100003', '7ANRPS48', 'Dekor', 'zeytin,dekor,ev', 'Zeytin dalı temalı dekor seti.', 'Olive branch themed decor set.', 'طقم ديكور مستوحى من غصن الزيتون.', 'Doğal ve zarif bir atmosfer oluşturur.', 'Creates a natural and elegant atmosphere.', 'يخلق جوا طبيعيا وأنيقا.', 'El işçiliği görünümü', 'Dekoratif malzeme', 'Kuru bezle temizleyiniz', 'Koruyucu ambalaj', '/img/products/demo-product.svg', 'Stokta', 180::numeric, 149::numeric, 149::numeric, 70::numeric, 20::numeric, (SELECT "Id" FROM k WHERE "Slug"='ev-ve-dekorasyon'), true, true, false, true, true, true, 3, 0, 0, 0, 1, NULL::integer, 2, 1, 4, NOW(), false, 'Zeytin Dalı Dekoratif Set', 'Olive Branch Decor Set', 'طقم ديكور غصن الزيتون', 'Zeytin dalı dekor', 'Olive branch decor', 'ديكور غصن الزيتون', 'zeytin,dekor', 15::numeric, true, true, false, NULL::integer, NULL::timestamp with time zone
    UNION ALL SELECT 'Filistin Hatıra Hediye Kutusu', 'Palestine Memory Gift Box', 'صندوق هدايا تذكاري فلسطيني', 'Hediye Kutusu', 'filistin-hatira-hediye-kutusu', '/Urun/Detay/filistin-hatira-hediye-kutusu', 'DEMO-HED-001', '869000100005', '7ANRPS48', 'Hediye', 'hediye,filistin', 'Anlamlı hediye kutusu.', 'Meaningful gift box.', 'صندوق هدايا ذو معنى.', 'Özel günler için anlamlı hediye seçeneği.', 'Meaningful gift option for special days.', 'خيار هدية مميز للمناسبات الخاصة.', 'Özel kutu', 'Karma malzeme', 'Kuru yerde saklayınız', 'Hediye kutusu', '/img/products/demo-product.svg', 'Stokta', 150::numeric, 129::numeric, 129::numeric, 60::numeric, 20::numeric, (SELECT "Id" FROM k WHERE "Slug"='hediyelik-urunler'), true, true, true, true, true, true, 4, 0, 0, 0, 1, NULL::integer, 1, 1, 3, NOW(), false, 'Filistin Hatıra Hediye Kutusu', 'Palestine Memory Gift Box', 'صندوق هدايا تذكاري فلسطيني', 'Hatıra hediye kutusu', 'Memory gift box', 'صندوق هدايا تذكاري', 'hediye,filistin', 15::numeric, true, true, false, NULL::integer, NULL::timestamp with time zone
  ) AS p
  RETURNING "Id", "Baslik", "IndirimliFiyat", "Fiyat"
)
INSERT INTO "UrunSecenekleri" (
  "UrunId", "Olcu", "CerceveTipi", "CerceveRengi", "CerceveKalinligi", "MalzemeTuru", "Yon", "ParcaSayisi",
  "VaryantSku", "KisilestirmeMetni", "OzelTasarimNotu", "FiyatFarki", "SatisFiyati", "MaliyetFiyati", "StokAdedi",
  "UretimSuresiGun", "Desi", "GorselUrl", "AktifMi", "VarsayilanMi", "TukeninceGizle", "OnSipariseAcikMi", "Sira", "OlusturulmaTarihi", "SilindiMi"
)
SELECT "Id", 'Standart', 'Standart', 'Siyah', 'Standart', 'Kanvas', 'Dikey', 1,
       'VAR-' || "Id", '', '', 0, COALESCE("IndirimliFiyat", "Fiyat"), 0, 50,
       2, 1, '/img/products/demo-product.svg', true, true, false, true, 1, NOW(), false
FROM inserted_products;

INSERT INTO "UrunResimleri" (
  "UrunId", "ResimYolu", "Baslik", "AltMetin", "MedyaTipi", "MedyaAlani", "VideoUrl", "ThumbnailYolu",
  "MobilResimYolu", "Etiketler", "Sira", "VarsayilanMi", "UrunSecenekId", "OlusturulmaTarihi", "SilindiMi"
)
SELECT "Id", '/img/products/demo-product.svg', "Baslik", "Baslik", 'image', 'gallery', '', '/img/products/demo-product.svg',
       '/img/products/demo-product.svg', 'demo', 1, true, NULL, NOW(), false
FROM "Urunler";

COMMIT;

SELECT 'Demo içerik eklendi' AS sonuc,
  (SELECT COUNT(*) FROM "Kategoriler") AS kategori,
  (SELECT COUNT(*) FROM "Slaytlar") AS slayt,
  (SELECT COUNT(*) FROM "Urunler") AS urun,
  (SELECT COUNT(*) FROM "UrunSecenekleri") AS secenek,
  (SELECT COUNT(*) FROM "UrunResimleri") AS resim;
