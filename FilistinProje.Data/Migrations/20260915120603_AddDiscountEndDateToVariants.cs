using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountEndDateToVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Yorumlar"" ADD COLUMN IF NOT EXISTS ""DuzenlenmeTarihi"" timestamp with time zone NULL;
                ALTER TABLE ""Yorumlar"" ADD COLUMN IF NOT EXISTS ""DuzenleyenKullaniciId"" text NULL;
                ALTER TABLE ""Yorumlar"" ADD COLUMN IF NOT EXISTS ""GizlemeTarihi"" timestamp with time zone NULL;
                ALTER TABLE ""Yorumlar"" ADD COLUMN IF NOT EXISTS ""GizleyenKullaniciId"" text NULL;
                ALTER TABLE ""UrunSecenekleri"" ADD COLUMN IF NOT EXISTS ""IndirimBitisTarihi"" timestamp with time zone NULL;
                ALTER TABLE ""ToptanciIskontoOranlari"" ADD COLUMN IF NOT EXISTS ""IskontoTipi"" text NOT NULL DEFAULT '';
                ALTER TABLE ""ToptanciIskontoOranlari"" ADD COLUMN IF NOT EXISTS ""IskontoTutari"" numeric NOT NULL DEFAULT 0;
                ALTER TABLE ""ToptanciIskontoOranlari"" ADD COLUMN IF NOT EXISTS ""UrunId"" integer NULL;
                ALTER TABLE ""SiteAyarlari"" ADD COLUMN IF NOT EXISTS ""AdreseTeslimAktifMi"" boolean NOT NULL DEFAULT false;
                ALTER TABLE ""SiteAyarlari"" ADD COLUMN IF NOT EXISTS ""BankaHavalesiAktifMi"" boolean NOT NULL DEFAULT false;
                ALTER TABLE ""SiteAyarlari"" ADD COLUMN IF NOT EXISTS ""MagazadanTeslimAktifMi"" boolean NOT NULL DEFAULT false;
                ALTER TABLE ""Siparisler"" ADD COLUMN IF NOT EXISTS ""OdemeDekontYolu"" text NULL;
                CREATE INDEX IF NOT EXISTS ""IX_ToptanciIskontoOranlari_UrunId"" ON ""ToptanciIskontoOranlari"" (""UrunId"");
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM pg_constraint WHERE conname = 'FK_ToptanciIskontoOranlari_Urunler_UrunId'
                    ) THEN
                        ALTER TABLE ""ToptanciIskontoOranlari""
                        ADD CONSTRAINT ""FK_ToptanciIskontoOranlari_Urunler_UrunId""
                        FOREIGN KEY (""UrunId"") REFERENCES ""Urunler"" (""Id"") ON DELETE SET NULL;
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""UrunSecenekleri"" DROP COLUMN IF EXISTS ""IndirimBitisTarihi"";
            ");
        }
    }
}
