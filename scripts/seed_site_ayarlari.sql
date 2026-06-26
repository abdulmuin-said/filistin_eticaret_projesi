-- ============================================================
-- 7ANRPS48 E-Ticaret - Site Ayarları Varsayılan Seed Verisi
-- ============================================================

INSERT INTO "SiteAyarlari" (
    "Id",
    "SiteAdi", "MarkaAdi", "SiteBasligi", "SiteAciklamasi",
    "SiteLogoUrl", "FaviconUrl", "BaseUrl",
    "TemaRengi", "UstBarMesaji", "KampanyaMesaji",
    "UstBarEtkin", "UstBarHizi",
    "FooterAciklamasi",
    "Telefon", "Email", "Adres", "WhatsappNumarasi", "CalismaSaatleri",
    "FacebookUrl", "InstagramUrl", "TwitterUrl", "YoutubeUrl", "TiktokUrl", "PinterestUrl",
    "ParaBirimi", "KargoBedeli", "UcretsizKargoLimiti", "StokUyariLimiti", "StoktaYokSatisIzni",
    "PaytrAktifMi", "PaytrTestModu",
    "PaytrMerchantId", "PaytrMerchantKeyProtected", "PaytrMerchantSaltProtected",
    "PaytrCallbackUrl", "PaytrBasariliDonusUrl", "PaytrBasarisizDonusUrl",
    "KargoFirmasi", "KargoTakipUrl", "SiparisTeslimSuresiGun", "IadeHakkiGun",
    "MetaTitle", "MetaDescription", "MetaKeywords",
    "GoogleAnalyticsId", "FacebookPixelId",
    "VarsayilanSosyalPaylasimGorseliUrl",
    "CookieMetni",
    "YeniSiparisMailBildirimi", "StokUyarisiMailBildirimi", "IadeTalebiMailBildirimi",
    "BildirimAliciEmail",
    "BakimModuAktif", "BakimModuMesaji"
) VALUES (
    1,
    '7ANRPS48',                                          -- SiteAdi
    '7ANRPS48',                                          -- MarkaAdi
    '7ANRPS48 - متجرك الإلكتروني',                        -- SiteBasligi (Online Mağazanız - Arapça)
    'منتجات متنوعة بأفضل الأسعار مع خدمة توصيل سريعة',    -- SiteAciklamasi (Çeşitli ürünler, en iyi fiyatlar, hızlı teslimat)
    '/logo_svg.svg',                                     -- SiteLogoUrl (ileride güncellenecek)
    '/favicon.ico',                                      -- FaviconUrl (ileride güncellenecek)
    'https://www.7anrps48.com',                          -- BaseUrl
    '#1a5632',                                           -- TemaRengi (Filistin yeşili)
    'توصيل مجاني للطلبات فوق 200 ₪',                     -- UstBarMesaji (200₪ üzeri ücretsiz kargo)
    'الدفع عند الاستلام متاح',                            -- KampanyaMesaji (Kapıda ödeme mevcuttur)
    true,                                                -- UstBarEtkin
    34,                                                  -- UstBarHizi
    'متجر إلكتروني فلسطيني يقدم منتجات متنوعة بأسعار منافسة وتوصيل سريع لجميع المدن', -- FooterAciklamasi
    '+970-599-000-000',                                  -- Telefon (placeholder)
    'info@7anrps48.com',                                 -- Email
    'فلسطين',                                            -- Adres (Filistin)
    '+970599000000',                                     -- WhatsappNumarasi (placeholder)
    'السبت - الخميس: 9:00 - 21:00',                      -- CalismaSaatleri (Cumartesi-Perşembe: 9-21)
    '',                                                  -- FacebookUrl
    '',                                                  -- InstagramUrl
    '',                                                  -- TwitterUrl
    '',                                                  -- YoutubeUrl
    '',                                                  -- TiktokUrl
    '',                                                  -- PinterestUrl
    '₪',                                                 -- ParaBirimi (ILS - Yeni İsrail Şekeli)
    15,                                                  -- KargoBedeli (15 ILS varsayılan)
    200,                                                 -- UcretsizKargoLimiti (200 ILS)
    5,                                                   -- StokUyariLimiti
    false,                                               -- StoktaYokSatisIzni
    false,                                               -- PaytrAktifMi (Online POS yok)
    false,                                               -- PaytrTestModu
    '',                                                  -- PaytrMerchantId
    '',                                                  -- PaytrMerchantKeyProtected
    '',                                                  -- PaytrMerchantSaltProtected
    '',                                                  -- PaytrCallbackUrl
    '',                                                  -- PaytrBasariliDonusUrl
    '',                                                  -- PaytrBasarisizDonusUrl
    'توصيل محلي',                                        -- KargoFirmasi (Yerel teslimat)
    '',                                                  -- KargoTakipUrl
    3,                                                   -- SiparisTeslimSuresiGun
    7,                                                   -- IadeHakkiGun
    '7ANRPS48 - متجر إلكتروني فلسطيني | تسوق أونلاين',  -- MetaTitle
    'تسوق أونلاين من 7ANRPS48. منتجات متنوعة بأفضل الأسعار مع توصيل سريع لجميع مدن فلسطين. الدفع عند الاستلام أو تحويل بنكي.', -- MetaDescription
    'تسوق أونلاين, فلسطين, متجر إلكتروني, توصيل, 7ANRPS48', -- MetaKeywords
    '',                                                  -- GoogleAnalyticsId
    '',                                                  -- FacebookPixelId
    '/logo_svg.svg',                                     -- VarsayilanSosyalPaylasimGorseliUrl
    'نستخدم ملفات تعريف الارتباط لتحسين تجربتك وتحليل حركة الموقع.', -- CookieMetni
    true,                                                -- YeniSiparisMailBildirimi
    true,                                                -- StokUyarisiMailBildirimi
    true,                                                -- IadeTalebiMailBildirimi
    'info@7anrps48.com',                                 -- BildirimAliciEmail
    false,                                               -- BakimModuAktif
    'نحن نعمل على تحسين الموقع لتقديم تجربة تسوق أفضل. سنعود قريباً!' -- BakimModuMesaji
);

-- Doğrulama
SELECT "Id", "SiteAdi", "MarkaAdi", "ParaBirimi", "BaseUrl" FROM "SiteAyarlari";
