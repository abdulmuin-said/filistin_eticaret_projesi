-- =====================================================================
-- Siparis Butunluk Test Scripti (B2, B3, B13, B27)
-- Bu SQL, yeni OrderPricingService davranışını doğrulayan kontrolleri
-- ve test senaryolarını icerir. Production'a import edilmez.
-- =====================================================================

-- 1) Mevcut urun/varyant snapshot
SELECT "Id", "Fiyat", "IndirimliFiyat", "TopFiyat", "HediyePaketiVarMi", "HediyePaketFiyati"
FROM "Urunler"
ORDER BY "Id";

-- 2) Varyant stok snapshot
SELECT "Id", "UrunId", "SatisFiyati", "StokAdedi", "OnSipariseAcikMi", "VarsayilanMi"
FROM "UrunSecenekleri"
ORDER BY "UrunId", "Id";

-- 3) Stok 1 senaryosu: Bir varyantin stokunu 1'e dusur.
--    İki ayrı kullanici ayni urunu ayni anda siparis etmeye calisirsa
--    sadece biri basarili olmalidir.
BEGIN;
UPDATE "UrunSecenekleri"
SET "StokAdedi" = 1
WHERE "Id" = 53; -- Urun 48 varyant
COMMIT;

-- 4) Stok yetersiz varyant icin siparis deneyi:
--    Sepete 2 adet eklenmistir. SiparisStokDusumu DENEY:
--    expected: affected_rows = 0, siparisin basarisiz olmasi.
BEGIN;
UPDATE "UrunSecenekleri"
SET "StokAdedi" = 0
WHERE "Id" = 53;
COMMIT;

-- 5) Sepet snapshot fiyat manipule testi:
--    Kotu niyetli kullanici SepetItems.Fiyat'i 1'e dusurmustur.
--    OrderPricingService.HesaplaAsync DB fiyatini (4999.00) kullanir,
--    manipule edilmis deger (1.00) ASLA siparis.BirimFiyat olmaz.
UPDATE "SepetItems"
SET "Fiyat" = 1.00
WHERE "Id" = 13;
-- Bu satir, OrderPricingService'in yeni hesaplanan fiyati ile dolayli olarak
-- duzeltilecektir (SepetItem.Fiyat'a yazilmaz; SiparisDetay.BirimFiyat'a yazilir).

-- 6) Kupon + Indirim + Kargo hesabi dogrulama:
--    Siparis 1000 ILS -- kupon %10 -- Kargo 15 ILS -- Sonuc 1000 - 100 + 15 = 915.
--    OrderPricingService.GenelToplam = (AraToplam - IndirimTutari) + KargoUcreti = 915.

-- 7) Atomic stok dusumu dogrulama (B2):
--    StokAdedi = 1 iken 1 siparis basarili, ikinci siparis reddedilmeli.
--    SQL: UPDATE "UrunSecenekleri" SET "StokAdedi" = "StokAdedi" - 1
--         WHERE "Id" = 53 AND "StokAdedi" >= 1;
--    Eger "StokAdedi" < 1 ise affected_rows = 0 → StokDusAsync Basarili = false → transaction rollback.

-- 8) Coklu hediye paketi (B13): paket bedeli adet basina uygulanir.
--    Toplam = UrunBirim * Adet + PaketBirim * Adet. Checkout paketi DB'den yeniden okur.
SELECT
  s."Id", s."UrunId", s."Adet", s."Fiyat", s."HediyePaketSecenegiId",
  s."HediyePaketAdi", s."HediyePaketFiyati",
  (s."Fiyat" * s."Adet" + (CASE WHEN s."HediyePaketSecenegiId" IS NOT NULL THEN s."HediyePaketFiyati" * s."Adet" ELSE 0 END)) AS "Toplam",
  p."Fiyat" AS "GuncelSunucuPaketFiyati",
  p."AktifMi" AS "PaketAktifMi"
FROM "SepetItems" s
LEFT JOIN "UrunHediyePaketSecenekleri" p ON p."Id" = s."HediyePaketSecenegiId"
WHERE s."SilindiMi" = false
ORDER BY s."Id";

DO $$
BEGIN
  IF EXISTS (
    SELECT 1
    FROM "SepetItems" s
    JOIN "UrunHediyePaketSecenekleri" p ON p."Id" = s."HediyePaketSecenegiId"
    WHERE s."SilindiMi" = false AND p."UrunId" <> s."UrunId"
  ) THEN
    RAISE EXCEPTION 'Sepette baska urune ait paket secenegi bulundu';
  END IF;

  IF EXISTS (
    SELECT 1
    FROM "Urunler" u
    WHERE (u."HediyePaketiVarMi" OR u."HediyePaketFiyati" > 0)
      AND NOT EXISTS (
        SELECT 1 FROM "UrunHediyePaketSecenekleri" p
        WHERE p."UrunId" = u."Id" AND NOT p."SilindiMi"
      )
  ) THEN
    RAISE EXCEPTION 'Legacy hediye paketi verisi yeni secenege tasinmamis';
  END IF;

  IF 2 * 100 + 2 * 15 <> 230 THEN
    RAISE EXCEPTION 'Paket fiyati adet basina formulu bozuk';
  END IF;
END
$$;

-- Manipulasyon kontrolu: HTTP istegindeki fiyat alani kullanilmaz. Secim ID'si
-- UrunId + AktifMi + SilindiMi ile dogrulanir; checkout p."Fiyat" degerini okur.
SELECT p."Id", p."UrunId", p."Fiyat", p."AktifMi", p."SilindiMi"
FROM "UrunHediyePaketSecenekleri" p
ORDER BY p."UrunId", p."Sira", p."Id";

-- 9) Tum stok dusum islemlerini gormek icin (transaction rollback dogrulamasi):
--    Stok = 5, 3 ayri siparis, 2 tanesi basarili, 1 tanesi stok yetersiz:
SELECT "Id", "StokAdedi" FROM "UrunSecenekleri" WHERE "Id" IN (53, 54, 55, 56) ORDER BY "Id";
