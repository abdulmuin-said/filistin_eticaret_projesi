-- ============================================================
-- 7ANRPS48 E-Ticaret - Veritabanı Veri Temizleme Betiği
-- Bu betik tüm iş verilerini siler, şema yapısını korur.
-- Kullanım: docker exec filistinproje-db psql -U kanvasuser -d filistindb -f /tmp/cleanup_data.sql
-- ============================================================

BEGIN;

-- 1. Önce bağımlı (child) tabloları temizle
TRUNCATE TABLE "SepetItems" CASCADE;
TRUNCATE TABLE "SiparisDetaylari" CASCADE;
TRUNCATE TABLE "HomePageSectionProducts" CASCADE;
TRUNCATE TABLE "UrunResimleri" CASCADE;
TRUNCATE TABLE "UrunSecenekleri" CASCADE;
TRUNCATE TABLE "UrunOzellikDegerleri" CASCADE;
TRUNCATE TABLE "Favoriler" CASCADE;
TRUNCATE TABLE "Yorumlar" CASCADE;
TRUNCATE TABLE "IadeTalepleri" CASCADE;
TRUNCATE TABLE "Adresler" CASCADE;

-- 2. Ana tabloları temizle
TRUNCATE TABLE "Sepetler" CASCADE;
TRUNCATE TABLE "Siparisler" CASCADE;
TRUNCATE TABLE "Urunler" CASCADE;
TRUNCATE TABLE "UrunOzellikTanimlari" CASCADE;
TRUNCATE TABLE "Kategoriler" CASCADE;
TRUNCATE TABLE "Kuponlar" CASCADE;
TRUNCATE TABLE "HomePageSections" CASCADE;
TRUNCATE TABLE "Slaytlar" CASCADE;
TRUNCATE TABLE "SenTasarla" CASCADE;
TRUNCATE TABLE "KargoFirmalari" CASCADE;

-- 3. İletişim ve log tabloları
TRUNCATE TABLE "IletisimMesajlari" CASCADE;
TRUNCATE TABLE "BultenAbonelikleri" CASCADE;
TRUNCATE TABLE "ZiyaretciLoglari" CASCADE;
TRUNCATE TABLE "KurumsalSayfalar" CASCADE;

-- 4. Site ayarlarını temizle (yeniden girilecek)
TRUNCATE TABLE "SiteAyarlari" CASCADE;

-- NOT: AspNetUsers, AspNetRoles ve ilişkili Identity tabloları
-- burada bilinçli olarak TEMİZLENMEZ. Admin hesabı korunmalıdır.
-- Eğer kullanıcıları da silmek isterseniz aşağıdaki satırları aktif edin:
-- TRUNCATE TABLE "AspNetUserTokens" CASCADE;
-- TRUNCATE TABLE "AspNetUserRoles" CASCADE;
-- TRUNCATE TABLE "AspNetUserLogins" CASCADE;
-- TRUNCATE TABLE "AspNetUserClaims" CASCADE;
-- TRUNCATE TABLE "AspNetRoleClaims" CASCADE;
-- TRUNCATE TABLE "AspNetUsers" CASCADE;
-- TRUNCATE TABLE "AspNetRoles" CASCADE;

COMMIT;

-- Doğrulama: Tüm tabloların kayıt sayısını göster
SELECT 'Urunler' AS tablo, COUNT(*) FROM "Urunler"
UNION ALL SELECT 'Kategoriler', COUNT(*) FROM "Kategoriler"
UNION ALL SELECT 'Siparisler', COUNT(*) FROM "Siparisler"
UNION ALL SELECT 'SiparisDetaylari', COUNT(*) FROM "SiparisDetaylari"
UNION ALL SELECT 'Sepetler', COUNT(*) FROM "Sepetler"
UNION ALL SELECT 'SepetItems', COUNT(*) FROM "SepetItems"
UNION ALL SELECT 'Yorumlar', COUNT(*) FROM "Yorumlar"
UNION ALL SELECT 'Kuponlar', COUNT(*) FROM "Kuponlar"
UNION ALL SELECT 'Favoriler', COUNT(*) FROM "Favoriler"
UNION ALL SELECT 'SiteAyarlari', COUNT(*) FROM "SiteAyarlari"
UNION ALL SELECT 'AspNetUsers', COUNT(*) FROM "AspNetUsers"
ORDER BY tablo;
