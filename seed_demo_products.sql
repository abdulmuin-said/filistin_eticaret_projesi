WITH product_data AS (
    SELECT * FROM (VALUES
        (1, 'Filistin Motifli Duvar Tablosu', 'Palestinian Pattern Wall Art', 'لوحة جدارية بنقوش فلسطينية', 'Filistin motifleriyle hazırlanmış dekoratif duvar tablosu.', 'Decorative wall art inspired by Palestinian patterns.', 'لوحة جدارية مزخرفة مستوحاة من النقوش الفلسطينية.', 95.00, 1, true),
        (2, 'Zeytin Dalı Dekoratif Set', 'Olive Branch Decor Set', 'طقم ديكور غصن الزيتون', 'Ev dekorasyonu için zarif zeytin dalı temalı set.', 'Elegant olive branch themed set for home decoration.', 'طقم أنيق مستوحى من غصن الزيتون لديكور المنزل.', 120.00, 2, true),
        (3, 'Filistin Hatıra Hediye Kutusu', 'Palestine Keepsake Gift Box', 'صندوق هدايا تذكاري فلسطيني', 'Özel günler için Filistin temalı hediye kutusu.', 'Palestine themed gift box for special occasions.', 'صندوق هدايا بطابع فلسطيني للمناسبات الخاصة.', 75.00, 3, true),
        (4, 'Kudüs Silüet Tablosu', 'Jerusalem Skyline Art', 'لوحة أفق القدس', 'Kudüs silüetinden ilham alan premium tablo.', 'Premium art inspired by the Jerusalem skyline.', 'لوحة فاخرة مستوحاة من أفق مدينة القدس.', 110.00, 4, false),
        (5, 'Gazze Dayanışma Posteri', 'Gaza Solidarity Poster', 'ملصق تضامن غزة', 'Dayanışma mesajı taşıyan modern poster.', 'Modern poster carrying a message of solidarity.', 'ملصق عصري يحمل رسالة تضامن.', 45.00, 5, false),
        (6, 'Filistin Harita & Bayrak Seti', 'Palestine Map & Flag Set', 'طقم خريطة وعلم فلسطين', 'Harita ve bayrak detaylı dekoratif set.', 'Decorative set with map and flag details.', 'طقم ديكوري بتفاصيل الخريطة والعلم.', 65.00, 6, false),
        (7, 'Salon İçin Kudüs Dekoru', 'Jerusalem Living Room Decor', 'ديكور القدس لغرفة المعيشة', 'Salonlara sıcaklık katan Kudüs temalı dekor.', 'Jerusalem themed decor that adds warmth to living rooms.', 'ديكور بطابع القدس يضيف دفئاً لغرفة المعيشة.', 135.00, 7, false),
        (8, 'Altın Çerçeveli Duvar Dekoru', 'Gold Framed Wall Decor', 'ديكور جداري بإطار ذهبي', 'Altın çerçeveli sade ve şık duvar dekoru.', 'Simple and elegant wall decor with a gold frame.', 'ديكور جداري بسيط وأنيق بإطار ذهبي.', 145.00, 8, false),
        (9, 'Filistin Sofra Sunum Seti', 'Palestine Table Serving Set', 'طقم تقديم مائدة فلسطيني', 'Mutfak ve sofra için dekoratif sunum seti.', 'Decorative serving set for kitchen and table.', 'طقم تقديم ديكوري للمطبخ والمائدة.', 88.00, 9, false),
        (10, 'İsme Özel Filistin Hediyesi', 'Personalized Palestine Gift', 'هدية فلسطينية مخصصة بالاسم', 'İsim eklenebilen anlamlı kişiye özel hediye.', 'Meaningful personalized gift with custom name option.', 'هدية مخصصة ومعبرة مع إمكانية إضافة الاسم.', 55.00, 10, false),
        (11, 'Kudüs Anahtarlık & Magnet Seti', 'Jerusalem Keychain & Magnet Set', 'طقم ميدالية ومغناطيس القدس', 'Kudüs temalı anahtarlık ve magnet seti.', 'Jerusalem themed keychain and magnet set.', 'طقم ميدالية ومغناطيس بطابع القدس.', 28.00, 11, false),
        (12, 'Nakışlı Filistin Bez Çanta', 'Embroidered Palestine Tote Bag', 'حقيبة قماش فلسطينية مطرزة', 'Günlük kullanım için nakış detaylı bez çanta.', 'Embroidered tote bag for daily use.', 'حقيبة قماش مطرزة للاستخدام اليومي.', 70.00, 12, false)
    ) AS v(id, baslik_tr, baslik_en, baslik_ar, kisa_tr, kisa_en, kisa_ar, fiyat, kategori_id, featured)
), inserted_products AS (
    INSERT INTO "Urunler" (
        "Baslik", "BaslikEn", "BaslikAr", "KisaAd", "Slug", "UrlYolu", "SKU", "Barkod", "Marka", "UrunTipi", "Etiketler",
        "KisaAciklama", "KisaAciklamaEn", "KisaAciklamaAr", "Aciklama", "AciklamaEn", "AciklamaAr",
        "TeknikOzellikler", "MalzemeBilgisi", "BakimTalimati", "PaketlemeBilgisi", "AnaGorselUrl", "StokDurumu",
        "Fiyat", "IndirimliFiyat", "Maliyet", "KdvOrani", "UretimSuresiGun", "KargoyaVerilisSuresiGun", "TahminiTeslimSuresiGun",
        "AktifMi", "OneCikanMi", "YeniUrunMu", "KampanyaliMi", "AnaSayfadaGoster", "Sira", "GoruntulenmeSayisi", "SatisSayisi", "FavoriSayisi",
        "MinSiparisAdedi", "MaxSiparisAdedi", "SeoTitle", "SeoTitleEn", "SeoTitleAr", "SeoDescription", "SeoDescriptionEn", "SeoDescriptionAr", "SeoKeywords",
        "KategoriId", "OlusturulmaTarihi", "SilindiMi", "TopFiyat", "HediyePaketFiyati", "HediyePaketiVarMi", "WhatsappSiparisVarMi", "FiyatGizliMi", "ToptanciUrunGrubuId"
    )
    SELECT
        baslik_tr, baslik_en, baslik_ar, baslik_tr,
        lower(regexp_replace(translate(baslik_en, '&', 'and'), '[^a-zA-Z0-9]+', '-', 'g')),
        '', 'DEMO-' || lpad(id::text, 3, '0'), '', '7ANRPS48', 'Dekoratif Ürün', 'palestine, gift, decor',
        kisa_tr, kisa_en, kisa_ar,
        '<p>' || kisa_tr || ' Ürün demo amaçlıdır.</p>',
        '<p>' || kisa_en || ' This product is for demo purposes.</p>',
        '<p>' || kisa_ar || ' هذا المنتج لغرض العرض التجريبي.</p>',
        'Boyut: Standart\nTema: Filistin\nÜretim: Demo', 'Kaliteli baskı ve dekoratif malzeme', 'Kuru bezle temizleyiniz', 'Koruyucu paketleme', '/img/products/demo-product.svg', 'Stokta',
        fiyat, CASE WHEN id IN (2,4,8) THEN fiyat - 10 ELSE NULL END, round(fiyat * 0.55, 2), 17, 2, 1, 4,
        true, featured, true, id IN (5,11), featured, id, 0, 0, 0,
        1, NULL, baslik_tr, baslik_en, baslik_ar, kisa_tr, kisa_en, kisa_ar, 'palestine,7anrps48,decor,gift',
        kategori_id, now(), false, round(fiyat * 0.85, 2), 8.00, true, true, false, NULL
    FROM product_data
    RETURNING "Id", "Fiyat", "Maliyet", "SKU", "Baslik"
), inserted_options AS (
    INSERT INTO "UrunSecenekleri" (
        "UrunId", "Olcu", "CerceveTipi", "CerceveRengi", "CerceveKalinligi", "MalzemeTuru", "Yon", "ParcaSayisi", "VaryantSku",
        "KisilestirmeMetni", "OzelTasarimNotu", "FiyatFarki", "SatisFiyati", "MaliyetFiyati", "StokAdedi", "UretimSuresiGun", "Desi", "GorselUrl",
        "AktifMi", "VarsayilanMi", "TukeninceGizle", "OnSipariseAcikMi", "Sira", "OlusturulmaTarihi", "SilindiMi"
    )
    SELECT "Id", 'Standart', 'Standart', 'Doğal', 'Standart', 'Premium', 'Dikey', 1, "SKU" || '-STD', '', '', 0, "Fiyat", "Maliyet", 25, 2, 1.20, '/img/products/demo-product.svg', true, true, false, false, 1, now(), false
    FROM inserted_products
    RETURNING "UrunId"
)
INSERT INTO "UrunResimleri" (
    "ResimYolu", "Baslik", "AltMetin", "MedyaTipi", "MedyaAlani", "VideoUrl", "ThumbnailYolu", "MobilResimYolu", "Etiketler", "Sira", "VarsayilanMi", "UrunSecenekId", "UrunId", "OlusturulmaTarihi", "SilindiMi"
)
SELECT '/img/products/demo-product.svg', "Baslik", "Baslik", 'Gorsel', 'Galeri', '', '', '', 'demo,palestine', 1, true, NULL, "Id", now(), false
FROM inserted_products;
