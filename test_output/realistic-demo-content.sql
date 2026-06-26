\set ON_ERROR_STOP on
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
('Geleneksel Filistin Ürünleri', 'Kefiye, tatreez ve kültürel tasarımlar', 'Filistin kültüründen ilham alan seçilmiş ürünler.', 'geleneksel-filistin-urunleri', '/img/products/demo-keffiyeh.svg', '/img/products/demo-keffiyeh.svg', 'Geleneksel Filistin Ürünleri - 7ANRPS48', 'Kefiye, tatreez ve kültürel Filistin ürünleri', '', '', 'Özel Koleksiyon', 'manual', 1, true, NULL, NOW(), false, 'منتجات فلسطينية تقليدية', 'Traditional Palestinian Products', 'الكوفية والتطريز وتصاميم ثقافية', 'Keffiyeh, tatreez and cultural designs', 'منتجات مختارة مستوحاة من الثقافة الفلسطينية.', 'Selected products inspired by Palestinian culture.', 'منتجات فلسطينية تقليدية - 7ANRPS48', 'Traditional Palestinian Products - 7ANRPS48', 'الكوفية والتطريز والمنتجات الثقافية الفلسطينية', 'Keffiyeh, tatreez and cultural Palestinian products', '', '', '', '', 'مجموعة خاصة', 'Special Collection', false),
('Ev ve Dekorasyon', 'Evinize anlam katan dekoratif ürünler', 'El yapımı görünümünde, zarif ve anlamlı dekoratif seçenekler.', 'ev-ve-dekorasyon', '/img/products/demo-olive-wood.svg', '/img/products/demo-olive-wood.svg', 'Ev ve Dekorasyon - 7ANRPS48', 'Filistin temalı ev dekorasyon ürünleri', '', '', 'Dekor', 'manual', 2, true, NULL, NOW(), false, 'المنزل والديكور', 'Home & Decoration', 'منتجات ديكور تضيف معنى لمنزلك', 'Decorative products with meaning', 'خيارات ديكور أنيقة وذات معنى بطابع فلسطيني.', 'Elegant and meaningful decoration options with a Palestinian touch.', 'المنزل والديكور - 7ANRPS48', 'Home & Decoration - 7ANRPS48', 'منتجات ديكور منزلية بطابع فلسطيني', 'Palestinian themed home decoration products', '', '', '', '', 'ديكور', 'Decor', false),
('Hediye ve Hatıra', 'Anlamlı ve kalıcı hediyeler', 'Sevdikleriniz için Filistin temalı hediyelik ürünler.', 'hediye-ve-hatira', '/img/products/demo-jerusalem-mug.svg', '/img/products/demo-jerusalem-mug.svg', 'Hediye ve Hatıra - 7ANRPS48', 'Filistin temalı hediyelik ürünler', '', '', 'Hediye', 'manual', 3, true, NULL, NOW(), false, 'هدايا وتذكارات', 'Gifts & Souvenirs', 'هدايا مميزة وذات معنى', 'Meaningful and memorable gifts', 'هدايا وتذكارات مستوحاة من فلسطين لمن تحبون.', 'Palestine inspired gifts and souvenirs for loved ones.', 'هدايا وتذكارات - 7ANRPS48', 'Gifts & Souvenirs - 7ANRPS48', 'هدايا وتذكارات فلسطينية', 'Palestinian themed gift products', '', '', '', '', 'هدية', 'Gift', false);

INSERT INTO "Slaytlar" (
  "Baslik", "AltBaslik", "Aciklama", "ResimUrl", "VideoUrl", "Tur", "Sira", "AktifMi", "OlusturmaTarihi",
  "BaslikAr", "BaslikEn", "AltBaslikAr", "AltBaslikEn", "AciklamaAr", "AciklamaEn"
)
VALUES
('Filistin Kültüründen İlham Alan Ürünler', 'Kefiye, tatreez ve anlamlı hediyeler', 'Günlük kullanıma uygun, kültürel değeri olan seçilmiş ürünleri keşfedin.', '/img/products/demo-keffiyeh.svg', NULL, 'Görsel', 1, true, NOW(), 'منتجات مستوحاة من الثقافة الفلسطينية', 'Products Inspired by Palestinian Culture', 'الكوفية والتطريز وهدايا ذات معنى', 'Keffiyeh, tatreez and meaningful gifts', 'اكتشف منتجات مختارة تحمل قيمة ثقافية وتناسب الاستخدام اليومي.', 'Discover selected products with cultural value for everyday use.'),
('Evinize Filistin Esintisi Katın', 'Dekoratif ve zarif tasarımlar', 'Zeytin ağacı dokusu, Kudüs çizgileri ve sıcak renklerle hazırlanmış dekoratif seçenekler.', '/img/products/demo-olive-wood.svg', NULL, 'Görsel', 2, true, NOW(), 'أضف لمسة فلسطينية إلى منزلك', 'Add a Palestinian Touch to Your Home', 'تصاميم زخرفية وأنيقة', 'Decorative and elegant designs', 'خيارات ديكور مستوحاة من خشب الزيتون وخطوط القدس والألوان الدافئة.', 'Decorative options inspired by olive wood, Jerusalem lines and warm colors.'),
('Sevdikleriniz İçin Anlamlı Hediyeler', 'Hatırası olan özel parçalar', 'Kupa, sabun, çanta ve duvar dekoru gibi şık hediye seçeneklerini inceleyin.', '/img/products/demo-jerusalem-mug.svg', NULL, 'Görsel', 3, true, NOW(), 'هدايا ذات معنى لمن تحب', 'Meaningful Gifts for Loved Ones', 'قطع خاصة تبقى في الذاكرة', 'Memorable special pieces', 'اكتشف خيارات هدايا أنيقة مثل الأكواب والصابون والحقائب وديكور الحائط.', 'Explore elegant gift options such as mugs, soap, bags and wall decor.');

WITH k AS (
  SELECT "Id", "Slug" FROM "Kategoriler"
), products AS (
  SELECT * FROM (VALUES
    ('Filistin Kefiyesi', 'Palestinian Keffiyeh', 'الكوفية الفلسطينية', 'Kefiye', 'filistin-kefiyesi', '/Urun/Detay/filistin-kefiyesi', 'REAL-FIL-001', '8691000000011', 'Tekstil', 'kefiye,filistin,aksesuar', 'Siyah-beyaz geleneksel desenli Filistin kefiyesi.', 'A black-and-white Palestinian keffiyeh with a traditional pattern.', 'كوفية فلسطينية بنقشة تقليدية باللونين الأسود والأبيض.', 'Yumuşak dokulu, günlük kullanıma uygun ve kültürel anlam taşıyan geleneksel kefiye. Omuz şalı, atkı veya dekoratif aksesuar olarak kullanılabilir.', 'A soft, everyday keffiyeh carrying cultural meaning. It can be used as a scarf, shoulder wrap or decorative accessory.', 'كوفية ناعمة مناسبة للاستخدام اليومي وتحمل معنى ثقافياً، ويمكن استخدامها كوشاح أو شال أو قطعة زينة.', 'Pamuk karışımlı kumaş, geleneksel desen, yaklaşık 120x120 cm', 'Pamuk karışımı', '30°C hassas yıkama önerilir.', 'Kraft paket içinde gönderilir.', '/img/products/demo-keffiyeh.svg', 185::numeric, 149::numeric, 70::numeric, (SELECT "Id" FROM k WHERE "Slug"='geleneksel-filistin-urunleri'), true, true, true, true, 1, 'Filistin Kefiyesi - 7ANRPS48', 'Palestinian Keffiyeh - 7ANRPS48', 'الكوفية الفلسطينية - 7ANRPS48', 'Geleneksel desenli Filistin kefiyesi.', 'Traditional patterned Palestinian keffiyeh.', 'كوفية فلسطينية بنقشة تقليدية.', 'filistin,kefiye,kufiya,keffiyeh'),
    ('Tatreez İşlemeli Omuz Çantası', 'Tatreez Embroidered Shoulder Bag', 'حقيبة كتف مطرزة بالتطريز الفلسطيني', 'Tatreez Çanta', 'tatreez-islemeli-omuz-cantasi', '/Urun/Detay/tatreez-islemeli-omuz-cantasi', 'REAL-FIL-002', '8691000000028', 'Aksesuar', 'tatreez,çanta,işleme', 'Filistin tatreez desenlerinden ilham alan işlemeli omuz çantası.', 'A shoulder bag inspired by Palestinian tatreez embroidery patterns.', 'حقيبة كتف مستوحاة من نقوش التطريز الفلسطيني.', 'Günlük kullanıma uygun, geniş iç hacimli ve kültürel motiflerle zenginleştirilmiş şık omuz çantası.', 'A stylish shoulder bag for daily use with a spacious interior and cultural motifs.', 'حقيبة كتف أنيقة للاستخدام اليومي بمساحة داخلية جيدة وزخارف ثقافية.', 'Ayarlanabilir askı, iç cep, tatreez motifli yüzey', 'Kanvas kumaş ve suni deri detay', 'Nemli bezle siliniz.', 'Koruyucu bez torba ile gönderilir.', '/img/products/demo-tatreez-bag.svg', 320::numeric, 269::numeric, 130::numeric, (SELECT "Id" FROM k WHERE "Slug"='geleneksel-filistin-urunleri'), true, true, true, true, 2, 'Tatreez İşlemeli Omuz Çantası', 'Tatreez Embroidered Shoulder Bag', 'حقيبة كتف مطرزة بالتطريز الفلسطيني', 'Tatreez motifli omuz çantası.', 'Shoulder bag with tatreez inspired motifs.', 'حقيبة كتف بزخارف مستوحاة من التطريز الفلسطيني.', 'tatreez,çanta,filistin'),
    ('Zeytin Ağacı Dekoratif Tabak', 'Olive Wood Decorative Plate', 'طبق ديكور من خشب الزيتون', 'Zeytin Tabak', 'zeytin-agaci-dekoratif-tabak', '/Urun/Detay/zeytin-agaci-dekoratif-tabak', 'REAL-EV-001', '8691000000035', 'Dekor', 'zeytin,ağaç,dekor', 'Zeytin ağacı dokusundan ilham alan dekoratif tabak.', 'A decorative plate inspired by olive wood texture.', 'طبق ديكور مستوحى من ملمس خشب الزيتون.', 'Masa, vitrin veya duvar dekorunda sıcak ve doğal bir görünüm sunan dekoratif parça.', 'A decorative piece that adds a warm and natural look to tables, displays or wall decor.', 'قطعة ديكور تضيف مظهراً دافئاً وطبيعياً للطاولة أو الحائط أو الرفوف.', 'Dekoratif kullanım, doğal görünüm, 24 cm çap', 'Dekoratif kompozit yüzey', 'Kuru bezle temizleyiniz.', 'Kutulu ve korumalı gönderim.', '/img/products/demo-olive-wood.svg', 240::numeric, 199::numeric, 95::numeric, (SELECT "Id" FROM k WHERE "Slug"='ev-ve-dekorasyon'), true, true, false, true, 3, 'Zeytin Ağacı Dekoratif Tabak', 'Olive Wood Decorative Plate', 'طبق ديكور من خشب الزيتون', 'Zeytin ağacı görünümlü dekoratif tabak.', 'Olive wood inspired decorative plate.', 'طبق ديكور مستوحى من خشب الزيتون.', 'zeytin,dekor,tabak'),
    ('Nablus Zeytinyağı Sabunu Seti', 'Nablus Olive Oil Soap Set', 'مجموعة صابون نابلس بزيت الزيتون', 'Nablus Sabunu', 'nablus-zeytinyagi-sabunu-seti', '/Urun/Detay/nablus-zeytinyagi-sabunu-seti', 'REAL-HED-001', '8691000000042', 'Bakım', 'nablus,sabun,zeytinyağı', 'Zeytinyağı geleneğinden ilham alan doğal sabun seti.', 'A natural soap set inspired by the olive oil soap tradition.', 'مجموعة صابون طبيعية مستوحاة من تقليد صابون زيت الزيتون.', 'Günlük kullanım ve hediye için uygun, sade içerik hissi veren üçlü sabun seti.', 'A three-piece soap set suitable for daily use or gifting, inspired by simple olive oil care.', 'مجموعة من ثلاث قطع صابون مناسبة للاستخدام اليومي أو كهدية، مستوحاة من العناية بزيت الزيتون.', '3 adet sabun, zeytinyağı esintili, hediye paketi', 'Sabun', 'Kuru yerde saklayınız.', 'Hediye kutusunda gönderilir.', '/img/products/demo-nablus-soap.svg', 145::numeric, 119::numeric, 55::numeric, (SELECT "Id" FROM k WHERE "Slug"='hediye-ve-hatira'), true, true, true, true, 4, 'Nablus Zeytinyağı Sabunu Seti', 'Nablus Olive Oil Soap Set', 'مجموعة صابون نابلس بزيت الزيتون', 'Nablus esintili zeytinyağı sabunu seti.', 'Nablus inspired olive oil soap set.', 'مجموعة صابون مستوحاة من صابون نابلس بزيت الزيتون.', 'nablus,sabun,zeytinyağı'),
    ('Kudüs Silüetli Seramik Kupa', 'Jerusalem Skyline Ceramic Mug', 'كوب سيراميك بأفق القدس', 'Kudüs Kupa', 'kudus-siluetli-seramik-kupa', '/Urun/Detay/kudus-siluetli-seramik-kupa', 'REAL-HED-002', '8691000000059', 'Kupa', 'kudüs,kupa,seramik', 'Kudüs silüeti detaylı seramik kupa.', 'A ceramic mug with Jerusalem skyline details.', 'كوب سيراميك بتفاصيل أفق القدس.', 'Sıcak içecekler için ideal, günlük kullanımda anlamlı bir hatıra parçası.', 'Ideal for hot drinks, a meaningful keepsake for daily use.', 'مثالي للمشروبات الساخنة وقطعة تذكارية ذات معنى للاستخدام اليومي.', '330 ml, seramik, baskılı yüzey', 'Seramik', 'Elde yıkama önerilir.', 'Kutulu gönderim.', '/img/products/demo-jerusalem-mug.svg', 165::numeric, 139::numeric, 65::numeric, (SELECT "Id" FROM k WHERE "Slug"='hediye-ve-hatira'), true, true, false, true, 5, 'Kudüs Silüetli Seramik Kupa', 'Jerusalem Skyline Ceramic Mug', 'كوب سيراميك بأفق القدس', 'Kudüs temalı seramik kupa.', 'Jerusalem themed ceramic mug.', 'كوب سيراميك مستوحى من القدس.', 'kudüs,kupa,seramik'),
    ('Filistin Haritası Duvar Tablosu', 'Palestine Map Wall Art', 'لوحة حائط لخريطة فلسطين', 'Harita Tablo', 'filistin-haritasi-duvar-tablosu', '/Urun/Detay/filistin-haritasi-duvar-tablosu', 'REAL-EV-002', '8691000000066', 'Tablo', 'filistin,harita,tablo', 'Filistin haritası ve bayrak renklerinden ilham alan duvar tablosu.', 'Wall art inspired by the Palestine map and flag colors.', 'لوحة حائط مستوحاة من خريطة فلسطين وألوان العلم.', 'Ev, ofis ve çalışma alanları için güçlü anlam taşıyan modern duvar dekoru.', 'A modern wall decor piece with strong meaning for homes, offices and workspaces.', 'قطعة ديكور حائط حديثة وذات معنى للمنزل أو المكتب أو مساحة العمل.', 'Mat baskı, çerçeve opsiyonlu, 40x60 cm', 'Kanvas baskı', 'Direkt güneş ışığından koruyunuz.', 'Köşe korumalı ambalaj.', '/img/products/demo-palestine-map.svg', 280::numeric, 229::numeric, 110::numeric, (SELECT "Id" FROM k WHERE "Slug"='ev-ve-dekorasyon'), true, true, true, true, 6, 'Filistin Haritası Duvar Tablosu', 'Palestine Map Wall Art', 'لوحة حائط لخريطة فلسطين', 'Filistin haritası duvar tablosu.', 'Palestine map wall art.', 'لوحة حائط لخريطة فلسطين.', 'filistin,harita,tablo')
  ) AS v("Baslik", "BaslikEn", "BaslikAr", "KisaAd", "Slug", "UrlYolu", "SKU", "Barkod", "UrunTipi", "Etiketler", "KisaAciklama", "KisaAciklamaEn", "KisaAciklamaAr", "Aciklama", "AciklamaEn", "AciklamaAr", "TeknikOzellikler", "MalzemeBilgisi", "BakimTalimati", "PaketlemeBilgisi", "AnaGorselUrl", "Fiyat", "IndirimliFiyat", "Maliyet", "KategoriId", "YeniUrunMu", "OneCikanMi", "KampanyaliMi", "AnaSayfadaGoster", "Sira", "SeoTitle", "SeoTitleEn", "SeoTitleAr", "SeoDescription", "SeoDescriptionEn", "SeoDescriptionAr", "SeoKeywords")
), inserted AS (
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
  SELECT "Baslik", "BaslikEn", "BaslikAr", "KisaAd", "Slug", "UrlYolu", "SKU", "Barkod", '7ANRPS48', "UrunTipi", "Etiketler",
    "KisaAciklama", "KisaAciklamaEn", "KisaAciklamaAr", "Aciklama", "AciklamaEn", "AciklamaAr",
    "TeknikOzellikler", "MalzemeBilgisi", "BakimTalimati", "PaketlemeBilgisi", "AnaGorselUrl", 'Stokta',
    "Fiyat", "IndirimliFiyat", "IndirimliFiyat", "Maliyet", 20, "KategoriId", true, true, "YeniUrunMu", "OneCikanMi", "KampanyaliMi", "AnaSayfadaGoster",
    "Sira", 0, 0, 0, 1, NULL,
    2, 1, 4, NOW(), false,
    "SeoTitle", "SeoTitleEn", "SeoTitleAr", "SeoDescription", "SeoDescriptionEn", "SeoDescriptionAr", "SeoKeywords",
    15, true, true, false, NULL, NULL
  FROM products
  RETURNING "Id", "Baslik", "AnaGorselUrl", "IndirimliFiyat", "Fiyat", "Sira"
)
INSERT INTO "UrunSecenekleri" (
  "UrunId", "Olcu", "CerceveTipi", "CerceveRengi", "CerceveKalinligi", "MalzemeTuru", "Yon", "ParcaSayisi",
  "VaryantSku", "KisilestirmeMetni", "OzelTasarimNotu", "FiyatFarki", "SatisFiyati", "MaliyetFiyati", "StokAdedi",
  "UretimSuresiGun", "Desi", "GorselUrl", "AktifMi", "VarsayilanMi", "TukeninceGizle", "OnSipariseAcikMi", "Sira", "OlusturulmaTarihi", "SilindiMi"
)
SELECT "Id", 'Standart', 'Standart', 'Siyah', 'Standart', 'Standart', 'Dikey', 1,
       'VAR-' || "Id", '', '', 0, COALESCE("IndirimliFiyat", "Fiyat"), 0, 50,
       2, 1, "AnaGorselUrl", true, true, false, true, 1, NOW(), false
FROM inserted;

INSERT INTO "UrunResimleri" (
  "UrunId", "ResimYolu", "Baslik", "AltMetin", "MedyaTipi", "MedyaAlani", "VideoUrl", "ThumbnailYolu",
  "MobilResimYolu", "Etiketler", "Sira", "VarsayilanMi", "UrunSecenekId", "OlusturulmaTarihi", "SilindiMi"
)
SELECT "Id", "AnaGorselUrl", "Baslik", "Baslik", 'image', 'gallery', '', "AnaGorselUrl",
       "AnaGorselUrl", 'demo,realistic', 1, true, NULL, NOW(), false
FROM "Urunler";

COMMIT;

SELECT 'OK' AS sonuc,
  (SELECT COUNT(*) FROM "Kategoriler") AS kategori,
  (SELECT COUNT(*) FROM "Slaytlar") AS slayt,
  (SELECT COUNT(*) FROM "Urunler") AS urun,
  (SELECT COUNT(*) FROM "UrunSecenekleri") AS secenek,
  (SELECT COUNT(*) FROM "UrunResimleri") AS resim;
