UPDATE "SiteAyarlari"
SET "ParaBirimi" = '₪'
WHERE "Id" = 1;

SELECT "Id", "ParaBirimi"
FROM "SiteAyarlari"
ORDER BY "Id";
