-- 7ANRPS48 Özellik Doğrulama Test Verisi
-- Çalıştırma: docker exec -i filistinproje-db psql -U kanvasuser -d filistindb

-- ========== Özellik 2: WhatsApp + FiyatGizliMi ==========
-- Ürün Id=50 (Fistanu Shehre) → WhatsApp siparişi + fiyat gizli
UPDATE "Urunler"
SET "WhatsappSiparisVarMi" = true, "FiyatGizliMi" = true
WHERE "Id" = 50;

-- ========== Özellik 3: Hediye Paketi ==========
-- Ürün Id=49 (Saat Zekiyye) → Hediye paketi aktif, 25 ILS fiyat
UPDATE "Urunler"
SET "HediyePaketiVarMi" = true, "HediyePaketFiyati" = 25.00
WHERE "Id" = 49;

-- ========== Özellik 4: Reçeteli Kategori ==========
-- Kategori Id=79 (Elektronik) → Reçete zorunlu
UPDATE "Kategoriler"
SET "ReceteGerekliMi" = true
WHERE "Id" = 79;

-- ========== Özellik 5: Toptancı Grubu + İskonto ==========
INSERT INTO "ToptanciUrunGruplari" ("Ad", "Aciklama", "AktifMi", "Sira", "OlusturulmaTarihi", "SilindiMi")
VALUES ('مجموعة الجملة الأساسية', 'Basic wholesale group', true, 1, NOW(), false)
ON CONFLICT DO NOTHING;

-- İlk grup için iskonto oranları
INSERT INTO "ToptanciIskontoOranlari" ("ToptanciUrunGrubuId", "MinAdet", "IskontoYuzdesi", "AktifMi", "OlusturulmaTarihi", "SilindiMi")
SELECT g."Id", 5, 10.0, true, NOW(), false
FROM "ToptanciUrunGruplari" g
WHERE g."Ad" = 'مجموعة الجملة الأساسية' AND NOT EXISTS (
    SELECT 1 FROM "ToptanciIskontoOranlari" i WHERE i."ToptanciUrunGrubuId" = g."Id" AND i."MinAdet" = 5
);

INSERT INTO "ToptanciIskontoOranlari" ("ToptanciUrunGrubuId", "MinAdet", "IskontoYuzdesi", "AktifMi", "OlusturulmaTarihi", "SilindiMi")
SELECT g."Id", 10, 20.0, true, NOW(), false
FROM "ToptanciUrunGruplari" g
WHERE g."Ad" = 'مجموعة الجملة الأساسية' AND NOT EXISTS (
    SELECT 1 FROM "ToptanciIskontoOranlari" i WHERE i."ToptanciUrunGrubuId" = g."Id" AND i."MinAdet" = 10
);

-- Ürün Id=48 (Koltuk) → Toptancı grubuna bağla, toptan fiyat 3500
UPDATE "Urunler"
SET "ToptanciUrunGrubuId" = (SELECT "Id" FROM "ToptanciUrunGruplari" WHERE "Ad" = 'مجموعة الجملة الأساسية' LIMIT 1),
    "TopFiyat" = 3500.00
WHERE "Id" = 48;

-- testuser2026 → Wholesale onaylı (WholesaleStatus=1 = Approved, enum: Pending=0,Approved=1,Rejected=2)
UPDATE "AspNetUsers"
SET "WholesaleStatus" = 1
WHERE "Email" = 'testuser2026@example.com';
-- Wholesale rolü ata
INSERT INTO "AspNetUserRoles" ("UserId","RoleId")
SELECT u."Id", r."Id"
FROM "AspNetUsers" u, "AspNetRoles" r
WHERE u."Email" = 'testuser2026@example.com' AND r."Name" = 'Wholesale'
ON CONFLICT DO NOTHING;

-- ========== Özellik 6: COD Aktif Et ==========
UPDATE "SiteAyarlari"
SET "KapidaOdemeAktifMi" = true,
    "KapidaOdemeHizmetBedeli" = 15.00,
    "KapidaOdemeLimiti" = 2000.00;

-- ========== Özellik 9: Kupon (çark testi için) ==========
INSERT INTO "Kuponlar" ("Kod", "Tip", "Deger", "MinSepetTutari", "SonKullanmaTarihi", "KullanimLimiti", "KullanilanMiktar", "AktifMi", "OlusturulmaTarihi", "SilindiMi")
VALUES ('TEST-DISCOUNT-10', 0, 10, 0, NOW() + INTERVAL '30 days', 100, 0, true, NOW(), false)
ON CONFLICT DO NOTHING;

INSERT INTO "Kuponlar" ("Kod", "Tip", "Deger", "MinSepetTutari", "SonKullanmaTarihi", "KullanimLimiti", "KullanilanMiktar", "AktifMi", "OlusturulmaTarihi", "SilindiMi")
VALUES ('FREESHIP-TEST', 1, 50, 100, NOW() + INTERVAL '30 days', 50, 0, true, NOW(), false)
ON CONFLICT DO NOTHING;

-- Doğrulama sorguları
SELECT 'WhatsApp ürünü' as kontrol, COUNT(*) FROM "Urunler" WHERE "WhatsappSiparisVarMi"=true AND "FiyatGizliMi"=true;
SELECT 'Hediye paketi ürünü' as kontrol, COUNT(*) FROM "Urunler" WHERE "HediyePaketiVarMi"=true;
SELECT 'ReceteGerekliMi kategori' as kontrol, COUNT(*) FROM "Kategoriler" WHERE "ReceteGerekliMi"=true;
SELECT 'Toptancı grubu' as kontrol, COUNT(*) FROM "ToptanciUrunGruplari";
SELECT 'İskonto oranı' as kontrol, COUNT(*) FROM "ToptanciIskontoOranlari";
SELECT 'COD aktif' as kontrol, "KapidaOdemeAktifMi" FROM "SiteAyarlari" LIMIT 1;
SELECT 'Wholesale user' as kontrol, "Email","WholesaleStatus" FROM "AspNetUsers" WHERE "WholesaleStatus"=2;
SELECT 'Kupon sayısı' as kontrol, COUNT(*) FROM "Kuponlar" WHERE "AktifMi"=true;
