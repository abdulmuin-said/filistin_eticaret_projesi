\set ON_ERROR_STOP on
BEGIN;

-- Demo operasyon verilerini ve vitrin verilerini temizle
DELETE FROM "IadeTalepleri";
DELETE FROM "SiparisDetaylari";
DELETE FROM "Siparisler";
DELETE FROM "SepetItems";
DELETE FROM "Sepetler";
DELETE FROM "Favoriler";
DELETE FROM "HomePageSectionProducts";
DELETE FROM "Yorumlar";
DELETE FROM "UrunOzellikDegerleri";
DELETE FROM "UrunResimleri";
DELETE FROM "UrunSecenekleri";
DELETE FROM "Urunler";
DELETE FROM "Slaytlar";
DELETE FROM "Kategoriler";

-- Eski slider tasarım dosyalarını geri bağla
INSERT INTO "Slaytlar" (
  "Baslik", "AltBaslik", "Aciklama", "ResimUrl", "VideoUrl", "Tur", "Sira", "AktifMi", "OlusturmaTarihi",
  "BaslikAr", "BaslikEn", "AltBaslikAr", "AltBaslikEn", "AciklamaAr", "AciklamaEn"
)
VALUES
('Veteriner Klinik Ürünleri', 'Klinik, çiftlik ve evcil hayvan bakımında güvenilir ürünler', 'Muayenehane, pet shop ve hayvan sahipleri için seçilmiş veteriner ürünleri.', '/slider1.png', NULL, 'Görsel', 1, true, NOW(), 'منتجات بيطرية موثوقة', 'Veterinary Clinic Products', 'منتجات موثوقة للعيادات والمزارع والحيوانات الأليفة', 'Reliable products for clinics, farms and pets', 'منتجات بيطرية مختارة للعيادات ومتاجر الحيوانات وأصحاب الحيوانات الأليفة.', 'Selected veterinary products for clinics, pet shops and animal owners.'),
('Evcil Hayvan Sağlığı ve Bakımı', 'Vitamin, bakım ve hijyen ürünleri', 'Kedi ve köpeklerin günlük bakımını destekleyen pratik ürünleri keşfedin.', '/slider2.png', NULL, 'Görsel', 2, true, NOW(), 'صحة ورعاية الحيوانات الأليفة', 'Pet Health & Care', 'فيتامينات ومنتجات عناية ونظافة', 'Vitamins, care and hygiene products', 'اكتشف منتجات عملية تدعم العناية اليومية بالقطط والكلاب.', 'Discover practical products supporting daily care for cats and dogs.'),
('Çiftlik Hayvanları İçin Destek Ürünleri', 'Mineral, bakım ve saha kullanımına uygun çözümler', 'Büyükbaş, küçükbaş ve kanatlı işletmeleri için seçilmiş destek ürünleri.', '/slider3.png', NULL, 'Görsel', 3, true, NOW(), 'منتجات دعم لحيوانات المزرعة', 'Support Products for Farm Animals', 'معادن وعناية وحلول مناسبة للاستخدام الميداني', 'Minerals, care and field-use solutions', 'منتجات دعم مختارة للأبقار والأغنام والدواجن.', 'Selected support products for cattle, sheep, goats and poultry.'),
('Klinik Sarf ve Bakım Setleri', 'Günlük kullanım için hazır çözümler', 'Muayene, bakım ve hijyen süreçlerinde kullanılabilecek tamamlayıcı ürünler.', '/slider4.png', NULL, 'Görsel', 4, true, NOW(), 'مستلزمات العيادة ومجموعات العناية', 'Clinic Consumables & Care Sets', 'حلول جاهزة للاستخدام اليومي', 'Ready solutions for daily use', 'منتجات مساعدة للفحص والعناية والنظافة.', 'Complementary products for examination, care and hygiene workflows.'),
('Hızlı Teslim Veteriner Ürünleri', 'Filistin genelinde güvenli teslimat', 'Stoklu ürünlerde hızlı hazırlık ve güvenli paketleme.', '/slider5.png', NULL, 'Görsel', 5, true, NOW(), 'منتجات بيطرية بتوصيل سريع', 'Fast Delivery Veterinary Products', 'توصيل آمن في جميع مناطق فلسطين', 'Secure delivery across Palestine', 'تحضير سريع وتغليف آمن للمنتجات المتوفرة في المخزون.', 'Fast preparation and secure packaging for in-stock products.');

-- Kategoriler
INSERT INTO "Kategoriler" (
  "Ad", "KisaAciklama", "Aciklama", "Slug", "GorselUrl", "BannerUrl", "SeoTitle", "SeoDescription",
  "UstMetin", "AltMetin", "KampanyaEtiketi", "UrunSiralamaTipi", "Sira", "AktifMi", "ParentKategoriId",
  "OlusturulmaTarihi", "SilindiMi", "AdAr", "AdEn", "KisaAciklamaAr", "KisaAciklamaEn", "AciklamaAr", "AciklamaEn",
  "SeoTitleAr", "SeoTitleEn", "SeoDescriptionAr", "SeoDescriptionEn", "UstMetinAr", "UstMetinEn", "AltMetinAr", "AltMetinEn", "KampanyaEtiketiAr", "KampanyaEtiketiEn", "ReceteGerekliMi"
)
VALUES
('Kedi ve Köpek Bakımı', 'Evcil hayvanlar için vitamin, hijyen ve bakım ürünleri', 'Kedi ve köpeklerin günlük bakımını destekleyen veteriner ürünleri.', 'kedi-ve-kopek-bakimi', '/img/products/vet-real-pet-vitamin.jpg', '/slider2.png', 'Kedi ve Köpek Bakımı - 7ANRPS48', 'Evcil hayvan bakım ve destek ürünleri', '', '', 'Evcil Bakım', 'manual', 1, true, NULL, NOW(), false, 'رعاية القطط والكلاب', 'Cat & Dog Care', 'فيتامينات ومنتجات نظافة وعناية للحيوانات الأليفة', 'Vitamins, hygiene and care products for pets', 'منتجات بيطرية تدعم العناية اليومية بالقطط والكلاب.', 'Veterinary products supporting daily cat and dog care.', 'رعاية القطط والكلاب - 7ANRPS48', 'Cat & Dog Care - 7ANRPS48', 'منتجات رعاية ودعم للحيوانات الأليفة', 'Pet care and support products', '', '', '', '', 'رعاية الحيوانات', 'Pet Care', false),
('Klinik Sarf ve Hijyen', 'Veteriner klinikleri için pratik sarf ürünleri', 'Muayene, bakım ve hijyen süreçlerinde kullanılan tamamlayıcı ürünler.', 'klinik-sarf-ve-hijyen', '/img/products/vet-real-wound-spray.jpg', '/slider4.png', 'Klinik Sarf ve Hijyen - 7ANRPS48', 'Veteriner klinik sarf ve hijyen ürünleri', '', '', 'Klinik', 'manual', 2, true, NULL, NOW(), false, 'مستلزمات ونظافة العيادة', 'Clinic Consumables & Hygiene', 'مستلزمات عملية للعيادات البيطرية', 'Practical consumables for veterinary clinics', 'منتجات مساعدة للفحص والعناية والنظافة في العيادات.', 'Complementary products for examination, care and hygiene in clinics.', 'مستلزمات ونظافة العيادة - 7ANRPS48', 'Clinic Consumables & Hygiene - 7ANRPS48', 'مستلزمات ونظافة للعيادات البيطرية', 'Veterinary clinic consumables and hygiene products', '', '', '', '', 'عيادة', 'Clinic', false),
('Çiftlik Hayvanı Destek', 'Büyükbaş, küçükbaş ve kanatlılar için destek ürünleri', 'Çiftlik hayvanlarının bakımını destekleyen saha kullanımına uygun ürünler.', 'ciftlik-hayvani-destek', '/img/products/vet-real-mineral-block.jpg', '/slider3.png', 'Çiftlik Hayvanı Destek - 7ANRPS48', 'Çiftlik hayvanları için destek ürünleri', '', '', 'Çiftlik', 'manual', 3, true, NULL, NOW(), false, 'دعم حيوانات المزرعة', 'Farm Animal Support', 'منتجات دعم للأبقار والأغنام والدواجن', 'Support products for cattle, sheep, goats and poultry', 'منتجات مناسبة للاستخدام الميداني لدعم رعاية حيوانات المزرعة.', 'Field-use products supporting farm animal care.', 'دعم حيوانات المزرعة - 7ANRPS48', 'Farm Animal Support - 7ANRPS48', 'منتجات دعم لحيوانات المزرعة', 'Support products for farm animals', '', '', '', '', 'مزرعة', 'Farm', false);

-- Ürün görselleri SVG olarak daha önce dosya yoksa demo-product kullanılır; birazdan dosyalar oluşturulacak.
WITH k AS (
  SELECT "Id", "Slug" FROM "Kategoriler"
), products AS (
  SELECT * FROM (VALUES
    ('VetPlus Multivitamin Kedi & Köpek Damlası 50 ml', 'VetPlus Multivitamin Drops for Cats & Dogs 50 ml', 'قطرات فيتامين متعددة للقطط والكلاب 50 مل', 'Multivitamin Damla', 'vetplus-multivitamin-kedi-kopek-damlasi-50ml', '/Urun/Detay/vetplus-multivitamin-kedi-kopek-damlasi-50ml', 'VET-PET-001', '8692000000018', 'Pet Destek', 'vitamin,kedi,köpek,damla', 'Kedi ve köpeklerin günlük vitamin desteği için formüle edilmiş tamamlayıcı damla.', 'A complementary multivitamin drop formulated for daily cat and dog support.', 'قطرات مكملة متعددة الفيتامينات لدعم القطط والكلاب يومياً.', 'Günlük beslenmeyi desteklemek amacıyla geliştirilmiş, mama ile kolayca karıştırılabilen multivitamin damla. Hastalık tedavisi yerine geçmez; veteriner hekimin önerisiyle kullanılması tavsiye edilir.', 'A multivitamin drop developed to support daily nutrition and easily mixed with food. It does not replace treatment; use with veterinary advice is recommended.', 'قطرات فيتامينات لدعم التغذية اليومية ويمكن خلطها بسهولة مع الطعام. لا تغني عن العلاج وينصح باستخدامها بتوجيه الطبيب البيطري.', '50 ml, damlalıklı şişe, kedi ve köpek kullanımına uygun', 'Likit destek ürünü', 'Serin ve kuru yerde saklayınız. Çocuklardan uzak tutunuz.', 'Emniyet kapaklı şişe ve koruyucu kutu', '/img/products/vet-real-pet-vitamin.jpg', 185::numeric, 149::numeric, 70::numeric, (SELECT "Id" FROM k WHERE "Slug"='kedi-ve-kopek-bakimi'), true, true, true, true, 1, 'VetPlus Multivitamin Kedi & Köpek Damlası', 'VetPlus Multivitamin Drops for Cats & Dogs', 'قطرات فيتامين متعددة للقطط والكلاب', 'Kedi ve köpekler için günlük multivitamin desteği.', 'Daily multivitamin support for cats and dogs.', 'دعم يومي متعدد الفيتامينات للقطط والكلاب.', 'kedi,köpek,vitamin,damla'),
    ('Dermacare Hassas Deri Pet Şampuanı 250 ml', 'Dermacare Sensitive Skin Pet Shampoo 250 ml', 'شامبو للحيوانات الأليفة للبشرة الحساسة 250 مل', 'Pet Şampuanı', 'dermacare-hassas-deri-pet-sampuani-250ml', '/Urun/Detay/dermacare-hassas-deri-pet-sampuani-250ml', 'VET-PET-002', '8692000000025', 'Pet Bakım', 'şampuan,kedi,köpek,hijyen', 'Hassas derili kedi ve köpekler için nazik temizleme şampuanı.', 'A gentle cleansing shampoo for cats and dogs with sensitive skin.', 'شامبو تنظيف لطيف للقطط والكلاب ذات البشرة الحساسة.', 'Düzenli bakımda tüylerin kolay taranmasına yardımcı olur. Paraben içermez, ferah ve hafif kokuludur.', 'Helps make the coat easier to comb during regular care. Paraben-free with a light fresh scent.', 'يساعد على تمشيط الفراء بسهولة أثناء العناية المنتظمة. خالٍ من البارابين وبرائحة خفيفة.', '250 ml, hassas deri, kedi ve köpek kullanımına uygun', 'Şampuan', 'Gözle temasından kaçınınız. İyice durulayınız.', 'Sızdırmaz kapaklı şişe', '/img/products/vet-real-pet-shampoo.jpg', 125::numeric, 99::numeric, 45::numeric, (SELECT "Id" FROM k WHERE "Slug"='kedi-ve-kopek-bakimi'), false, true, true, true, 2, 'Dermacare Hassas Deri Pet Şampuanı', 'Dermacare Sensitive Skin Pet Shampoo', 'شامبو للحيوانات الأليفة للبشرة الحساسة', 'Hassas derili kedi ve köpekler için pet şampuanı.', 'Pet shampoo for cats and dogs with sensitive skin.', 'شامبو للقطط والكلاب ذات البشرة الحساسة.', 'pet,şampuan,kedi,köpek'),
    ('ProBioVet Sindirim Destek Tozu 100 g', 'ProBioVet Digestive Support Powder 100 g', 'مسحوق دعم الهضم بروبيوفيت 100 غ', 'Sindirim Destek', 'probiovet-sindirim-destek-tozu-100g', '/Urun/Detay/probiovet-sindirim-destek-tozu-100g', 'VET-PET-003', '8692000000032', 'Pet Destek', 'probiyotik,sindirim,kedi,köpek', 'Kedi ve köpeklerde günlük sindirim desteği için tamamlayıcı toz.', 'A complementary powder for daily digestive support in cats and dogs.', 'مسحوق مكمل لدعم الهضم اليومي لدى القطط والكلاب.', 'Mama üzerine serpilerek kullanılabilen pratik sindirim destek ürünüdür. Veteriner hekimin önerdiği kullanım miktarına uyunuz.', 'A practical digestive support product that can be sprinkled over food. Follow the amount recommended by your veterinarian.', 'منتج عملي لدعم الهضم يمكن رشه فوق الطعام. يرجى اتباع الكمية التي يوصي بها الطبيب البيطري.', '100 g, ölçü kaşıklı, mama üstüne kullanım', 'Toz destek ürünü', 'Kuru ve serin yerde saklayınız.', 'Kilitli kapaklı kutu', '/img/products/vet-real-probiotic.jpg', 210::numeric, 179::numeric, 82::numeric, (SELECT "Id" FROM k WHERE "Slug"='kedi-ve-kopek-bakimi'), true, false, true, true, 3, 'ProBioVet Sindirim Destek Tozu', 'ProBioVet Digestive Support Powder', 'مسحوق دعم الهضم بروبيوفيت', 'Kedi ve köpekler için sindirim destek tozu.', 'Digestive support powder for cats and dogs.', 'مسحوق دعم الهضم للقطط والكلاب.', 'probiyotik,sindirim,toz'),
    ('Klinik Yara Bakım Spreyi 100 ml', 'Clinic Wound Care Spray 100 ml', 'بخاخ عناية بالجروح للعيادة 100 مل', 'Yara Bakım Spreyi', 'klinik-yara-bakim-spreyi-100ml', '/Urun/Detay/klinik-yara-bakim-spreyi-100ml', 'VET-CLN-001', '8692000000049', 'Klinik Sarf', 'yara,bakım,sprey,klinik', 'Veteriner kliniklerinde bakım süreçlerini desteklemek için pratik sprey.', 'A practical spray supporting care workflows in veterinary clinics.', 'بخاخ عملي يدعم إجراءات العناية في العيادات البيطرية.', 'Temizlik ve bakım süreçlerinde yardımcı amaçla kullanılan, kolay uygulanabilir sprey formunda klinik sarf ürünüdür. Tedavi ürünü değildir.', 'An easy-to-apply spray form clinic consumable used as an aid in cleaning and care processes. Not a treatment product.', 'منتج عيادي بشكل بخاخ سهل الاستخدام للمساعدة في التنظيف والعناية. ليس منتجاً علاجياً.', '100 ml, sprey başlık, saha ve klinik kullanımına uygun', 'Bakım spreyi', 'Harici kullanım içindir. Gözle temas ettirmeyiniz.', 'Sprey şişe', '/img/products/vet-real-wound-spray.jpg', 160::numeric, 135::numeric, 60::numeric, (SELECT "Id" FROM k WHERE "Slug"='klinik-sarf-ve-hijyen'), false, true, true, true, 4, 'Klinik Yara Bakım Spreyi', 'Clinic Wound Care Spray', 'بخاخ عناية بالجروح للعيادة', 'Veteriner klinikleri için bakım spreyi.', 'Care spray for veterinary clinics.', 'بخاخ عناية للعيادات البيطرية.', 'yara,bakım,sprey,klinik'),
    ('OtoClean Kulak Temizleme Solüsyonu 120 ml', 'OtoClean Ear Cleaning Solution 120 ml', 'محلول تنظيف الأذن أوتوكلين 120 مل', 'Kulak Temizleme', 'otoclean-kulak-temizleme-solusyonu-120ml', '/Urun/Detay/otoclean-kulak-temizleme-solusyonu-120ml', 'VET-CLN-002', '8692000000056', 'Klinik Hijyen', 'kulak,temizlik,kedi,köpek', 'Kedi ve köpeklerde kulak çevresi hijyeni için temizleme solüsyonu.', 'A cleaning solution for ear area hygiene in cats and dogs.', 'محلول تنظيف لنظافة منطقة الأذن لدى القطط والكلاب.', 'Kulak çevresindeki kirlerin yumuşatılmasına yardımcı olur. Kulak hastalıklarında veteriner hekime danışılmalıdır.', 'Helps soften dirt around the ear area. Consult a veterinarian for ear diseases.', 'يساعد على تليين الأوساخ حول الأذن. يجب استشارة الطبيب البيطري في حالات أمراض الأذن.', '120 ml, damlalıklı şişe, kedi ve köpek için', 'Temizleme solüsyonu', 'Kulak kanalına derin uygulamayınız.', 'Damlalıklı şişe', '/img/products/vet-real-ear-cleaner.jpg', 145::numeric, 119::numeric, 52::numeric, (SELECT "Id" FROM k WHERE "Slug"='klinik-sarf-ve-hijyen'), false, false, true, true, 5, 'OtoClean Kulak Temizleme Solüsyonu', 'OtoClean Ear Cleaning Solution', 'محلول تنظيف الأذن أوتوكلين', 'Kedi ve köpekler için kulak temizleme solüsyonu.', 'Ear cleaning solution for cats and dogs.', 'محلول تنظيف الأذن للقطط والكلاب.', 'kulak,temizlik,solüsyon'),
    ('MineralBlock Büyükbaş & Küçükbaş Mineral Bloğu 2 kg', 'MineralBlock Cattle & Small Ruminant Mineral Block 2 kg', 'قالب معادن للأبقار والأغنام 2 كغ', 'Mineral Blok', 'mineralblock-buyukbas-kucukbas-mineral-blogu-2kg', '/Urun/Detay/mineralblock-buyukbas-kucukbas-mineral-blogu-2kg', 'VET-FRM-001', '8692000000063', 'Çiftlik Destek', 'mineral,büyükbaş,küçükbaş,çiftlik', 'Büyükbaş ve küçükbaş hayvanlar için mineral destek bloğu.', 'A mineral support block for cattle, sheep and goats.', 'قالب دعم معدني للأبقار والأغنام والماعز.', 'Ahır ve mera kullanımına uygun, günlük mineral alımını desteklemek amacıyla kullanılan pratik blok formu.', 'A practical block form suitable for barn and pasture use to support daily mineral intake.', 'قالب عملي مناسب للاستخدام في الحظائر والمراعي لدعم المدخول اليومي من المعادن.', '2 kg, asma delikli, saha kullanımına uygun', 'Mineral blok', 'Kuru zeminde muhafaza ediniz.', 'Dayanıklı paketleme', '/img/products/vet-real-mineral-block.jpg', 260::numeric, 219::numeric, 105::numeric, (SELECT "Id" FROM k WHERE "Slug"='ciftlik-hayvani-destek'), true, true, true, true, 6, 'MineralBlock Büyükbaş & Küçükbaş Mineral Bloğu', 'MineralBlock Cattle & Small Ruminant Mineral Block', 'قالب معادن للأبقار والأغنام', 'Çiftlik hayvanları için mineral destek bloğu.', 'Mineral support block for farm animals.', 'قالب دعم معدني لحيوانات المزرعة.', 'mineral,büyükbaş,küçükbaş'),
    ('PoultryCare Kanatlı Vitamin Premiksi 500 g', 'PoultryCare Poultry Vitamin Premix 500 g', 'بريمكس فيتامين للدواجن 500 غ', 'Kanatlı Premiks', 'poultrycare-kanatli-vitamin-premiksi-500g', '/Urun/Detay/poultrycare-kanatli-vitamin-premiksi-500g', 'VET-FRM-002', '8692000000070', 'Kanatlı Destek', 'kanatlı,vitamin,premiks', 'Kanatlı işletmeleri için vitamin destek premiksi.', 'A vitamin support premix for poultry operations.', 'بريمكس دعم فيتاميني لمزارع الدواجن.', 'Yem karışımlarında destek amacıyla kullanılan, saha kullanımına uygun premiks formu. Kullanım oranı için uzman önerisi alınmalıdır.', 'A field-use premix form used as support in feed mixtures. Expert advice should be obtained for usage rate.', 'بريمكس مناسب للاستخدام الميداني في خلطات العلف. يجب أخذ رأي المختص لتحديد نسبة الاستخدام.', '500 g, toz premiks, kanatlı yem desteği', 'Toz premiks', 'Nemden uzak tutunuz.', 'Kilitli ambalaj', '/img/products/vet-real-poultry-premix.jpg', 190::numeric, 159::numeric, 75::numeric, (SELECT "Id" FROM k WHERE "Slug"='ciftlik-hayvani-destek'), false, true, true, true, 7, 'PoultryCare Kanatlı Vitamin Premiksi', 'PoultryCare Poultry Vitamin Premix', 'بريمكس فيتامين للدواجن', 'Kanatlılar için vitamin destek premiksi.', 'Vitamin support premix for poultry.', 'بريمكس دعم فيتاميني للدواجن.', 'kanatlı,vitamin,premiks'),
    ('CalfStart Buzağı Başlangıç Destek Paketi', 'CalfStart Calf Starter Support Pack', 'حزمة دعم بداية للعجول', 'Buzağı Destek', 'calfstart-buzagi-baslangic-destek-paketi', '/Urun/Detay/calfstart-buzagi-baslangic-destek-paketi', 'VET-FRM-003', '8692000000087', 'Buzağı Destek', 'buzağı,destek,çiftlik', 'Buzağı bakım döneminde kullanılan başlangıç destek paketi.', 'A starter support pack used during calf care periods.', 'حزمة دعم تستخدم خلال فترة رعاية العجول.', 'Yeni doğan ve genç buzağıların bakım programlarını desteklemek amacıyla hazırlanmış pratik paket.', 'A practical pack prepared to support care programs for newborn and young calves.', 'حزمة عملية لدعم برامج رعاية العجول حديثة الولادة والصغيرة.', '3 parçalı destek seti, saha kullanımına uygun', 'Destek seti', 'Veteriner önerisiyle kullanınız.', 'Kutulu set', '/img/products/vet-real-calf-start.jpg', 340::numeric, 289::numeric, 145::numeric, (SELECT "Id" FROM k WHERE "Slug"='ciftlik-hayvani-destek'), true, false, true, true, 8, 'CalfStart Buzağı Başlangıç Destek Paketi', 'CalfStart Calf Starter Support Pack', 'حزمة دعم بداية للعجول', 'Buzağı bakımına destek paketi.', 'Support pack for calf care.', 'حزمة دعم لرعاية العجول.', 'buzağı,destek,çiftlik')
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
  SELECT "Baslik", "BaslikEn", "BaslikAr", "KisaAd", "Slug", "UrlYolu", "SKU", "Barkod", 'VetCare Palestine', "UrunTipi", "Etiketler",
    "KisaAciklama", "KisaAciklamaEn", "KisaAciklamaAr", "Aciklama", "AciklamaEn", "AciklamaAr",
    "TeknikOzellikler", "MalzemeBilgisi", "BakimTalimati", "PaketlemeBilgisi", "AnaGorselUrl", 'Stokta',
    "Fiyat", "IndirimliFiyat", "IndirimliFiyat", "Maliyet", 20, "KategoriId", true, true, "YeniUrunMu", "OneCikanMi", "KampanyaliMi", "AnaSayfadaGoster",
    "Sira", 0, 0, 0, 1, NULL,
    2, 1, 4, NOW(), false,
    "SeoTitle", "SeoTitleEn", "SeoTitleAr", "SeoDescription", "SeoDescriptionEn", "SeoDescriptionAr", "SeoKeywords",
    15, true, true, false, NULL, NULL
  FROM products
  RETURNING "Id", "Baslik", "Slug", "AnaGorselUrl", "IndirimliFiyat", "Fiyat", "Sira"
)
INSERT INTO "UrunSecenekleri" (
  "UrunId", "Olcu", "CerceveTipi", "CerceveRengi", "CerceveKalinligi", "MalzemeTuru", "Yon", "ParcaSayisi",
  "VaryantSku", "KisilestirmeMetni", "OzelTasarimNotu", "FiyatFarki", "SatisFiyati", "MaliyetFiyati", "StokAdedi",
  "UretimSuresiGun", "Desi", "GorselUrl", "AktifMi", "VarsayilanMi", "TukeninceGizle", "OnSipariseAcikMi", "Sira", "OlusturulmaTarihi", "SilindiMi"
)
SELECT "Id", 'Standart', 'Standart', 'Standart', 'Standart', 'Standart', 'Standart', 1,
       'VET-VAR-' || "Id", '', '', 0, COALESCE("IndirimliFiyat", "Fiyat"), 0, 50,
       2, 1, "AnaGorselUrl", true, true, false, true, 1, NOW(), false
FROM inserted;

INSERT INTO "UrunResimleri" (
  "UrunId", "ResimYolu", "Baslik", "AltMetin", "MedyaTipi", "MedyaAlani", "VideoUrl", "ThumbnailYolu",
  "MobilResimYolu", "Etiketler", "Sira", "VarsayilanMi", "UrunSecenekId", "OlusturulmaTarihi", "SilindiMi"
)
SELECT "Id", "AnaGorselUrl", "Baslik", "Baslik", 'image', 'gallery', '', "AnaGorselUrl",
       "AnaGorselUrl", 'veteriner,demo', 1, true, NULL, NOW(), false
FROM "Urunler";

-- Sipariş / satış / iade demo verileri
WITH u AS (
  SELECT "Id", "Baslik", "IndirimliFiyat", "Fiyat", "Sira" FROM "Urunler"
), v AS (
  SELECT DISTINCT ON ("UrunId") "Id" AS "SecenekId", "UrunId" FROM "UrunSecenekleri" ORDER BY "UrunId", "Id"
), orders AS (
  INSERT INTO "Siparisler" (
    "AppUserId", "SiparisNo", "KuponKodu", "IndirimTutari", "MusteriAdSoyad", "Telefon", "Eposta", "Sehir", "Ilce", "AcikAdres",
    "ToplamTutar", "Durum", "KargoTakipNo", "KargoFirmasiId", "KargoFirmasi", "KargoUcreti", "Aciklama", "EmailHashKodu",
    "FaturaDosyaYolu", "FaturaDosyaAdi", "FaturaYuklendiMi", "FaturaYuklenmeTarihi", "FaturaMailGonderildiMi", "FaturaMailGonderimTarihi",
    "OlusturulmaTarihi", "SilindiMi", "ReceteDosyaYolu", "TeslimatTipi", "KimlikFotoYolu", "KapidaOdemeHizmetBedeli", "OdemeYontemi", "ReceteOnayDurumu", "ReceteRedSebebi"
  )
  VALUES
  (NULL, 'VET-202606-1001', NULL, 0, 'Meryem Khalil', '+970599111223', 'meryem.khalil@example.com', 'Ramallah', 'El-Bireh', 'Al-Bireh merkez, VetCare teslimat adresi', 447, 3, 'TRK-VET-1001', NULL, 'Filistin Kargo', 20, 'Teslim edilmiş klinik bakım siparişi', 'hash1001', '/uploads/invoices/VET-202606-1001.pdf', 'VET-202606-1001.pdf', true, NOW() - INTERVAL '5 days', true, NOW() - INTERVAL '5 days', NOW() - INTERVAL '12 days', false, NULL, 'AdreseTeslim', NULL, 0, 'BankaHavalesi', 1, NULL),
  (NULL, 'VET-202606-1002', 'VET10', 24, 'Ahmad Nasser', '+970599222334', 'ahmad.nasser@example.com', 'Nablus', 'Merkez', 'Nablus veteriner kliniği yanı', 454, 2, 'TRK-VET-1002', NULL, 'Filistin Kargo', 25, 'Kargoya verilmiş çiftlik destek siparişi', 'hash1002', NULL, NULL, false, NULL, false, NULL, NOW() - INTERVAL '7 days', false, NULL, 'AdreseTeslim', NULL, 0, 'KapidaOdeme', 1, NULL),
  (NULL, 'VET-202606-1003', NULL, 0, 'Sara Haddad', '+970599333445', 'sara.haddad@example.com', 'Gazze', 'Merkez', 'Gazze şehir merkezi', 338, 1, NULL, NULL, NULL, 30, 'Hazırlık aşamasında evcil hayvan bakım siparişi', 'hash1003', NULL, NULL, false, NULL, false, NULL, NOW() - INTERVAL '2 days', false, NULL, 'AdreseTeslim', NULL, 0, 'BankaHavalesi', 0, NULL),
  (NULL, 'VET-202606-1004', NULL, 0, 'Omar Darwish', '+970599444556', 'omar.darwish@example.com', 'El Halil', 'Merkez', 'El Halil çiftlik yolu', 518, 5, 'TRK-VET-1004', NULL, 'Filistin Kargo', 35, 'İade talebi alınmış sipariş', 'hash1004', NULL, NULL, false, NULL, false, NULL, NOW() - INTERVAL '16 days', false, NULL, 'AdreseTeslim', NULL, 0, 'BankaHavalesi', 1, NULL),
  (NULL, 'VET-202606-1005', NULL, 0, 'Layla Mansour', '+970599555667', 'layla.mansour@example.com', 'Kudüs', 'Doğu Kudüs', 'Klinik deposu teslimat', 288, 4, NULL, NULL, NULL, 0, 'Müşteri isteğiyle iptal edildi', 'hash1005', NULL, NULL, false, NULL, false, NULL, NOW() - INTERVAL '9 days', false, NULL, 'AdreseTeslim', NULL, 0, 'BankaHavalesi', 0, NULL)
  RETURNING "Id", "SiparisNo"
)
INSERT INTO "SiparisDetaylari" ("SiparisId", "UrunSecenekId", "Adet", "BirimFiyat", "UrunId", "CerceveModeli", "MusteriNotu", "OlusturulmaTarihi", "SilindiMi", "HediyePaketFiyati", "HediyePaketi")
SELECT o."Id", v."SecenekId", x.adet, x.fiyat, u."Id", 'Standart', x.notu, NOW(), false, 0, false
FROM orders o
JOIN LATERAL (
  VALUES
    ('VET-202606-1001', 1, 2, 149::numeric, 'Klinik stok için'),
    ('VET-202606-1001', 4, 1, 135::numeric, 'Sprey başlık kontrol edilsin'),
    ('VET-202606-1002', 6, 2, 219::numeric, 'Çiftlik teslimatı'),
    ('VET-202606-1003', 2, 1, 99::numeric, 'Hassas deri için'),
    ('VET-202606-1003', 3, 1, 179::numeric, 'Mama ile kullanım'),
    ('VET-202606-1004', 8, 1, 289::numeric, 'Buzağı destek paketi'),
    ('VET-202606-1004', 7, 1, 159::numeric, 'Kanatlı premiks'),
    ('VET-202606-1005', 5, 2, 119::numeric, 'İptal edilen sipariş')
) AS x(sipno, urun_sira, adet, fiyat, notu) ON x.sipno = o."SiparisNo"
JOIN u ON u."Sira" = x.urun_sira
JOIN v ON v."UrunId" = u."Id";

INSERT INTO "IadeTalepleri" ("SiparisId", "MusteriId", "Neden", "Aciklama", "IBAN", "Durum", "AdminNotu", "OlusturulmaTarihi", "SilindiMi")
SELECT "Id", 'guest-omar-darwish', 'Ürün paketinde ezilme var', 'CalfStart paketinin dış kutusu taşıma sırasında ezilmiş görünüyor. Ürünün değişimini talep ediyorum.', 'PS92PALS000000000123456789', 1, 'Talep onaylandı, ürün depoya ulaştığında iade tamamlanacak.', NOW() - INTERVAL '1 day', false
FROM "Siparisler" WHERE "SiparisNo" = 'VET-202606-1004';

COMMIT;

SELECT 'OK' AS sonuc,
  (SELECT COUNT(*) FROM "Kategoriler") AS kategori,
  (SELECT COUNT(*) FROM "Slaytlar") AS slayt,
  (SELECT COUNT(*) FROM "Urunler") AS urun,
  (SELECT COUNT(*) FROM "Siparisler") AS siparis,
  (SELECT COUNT(*) FROM "SiparisDetaylari") AS siparis_detay,
  (SELECT COUNT(*) FROM "IadeTalepleri") AS iade;
