using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    public partial class AddSliderMultilingual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""Slaytlar"" ADD COLUMN IF NOT EXISTS ""BaslikEn"" text NOT NULL DEFAULT '';");
            migrationBuilder.Sql(@"ALTER TABLE ""Slaytlar"" ADD COLUMN IF NOT EXISTS ""BaslikAr"" text NOT NULL DEFAULT '';");
            migrationBuilder.Sql(@"ALTER TABLE ""Slaytlar"" ADD COLUMN IF NOT EXISTS ""AltBaslikEn"" text NOT NULL DEFAULT '';");
            migrationBuilder.Sql(@"ALTER TABLE ""Slaytlar"" ADD COLUMN IF NOT EXISTS ""AltBaslikAr"" text NOT NULL DEFAULT '';");
            migrationBuilder.Sql(@"ALTER TABLE ""Slaytlar"" ADD COLUMN IF NOT EXISTS ""AciklamaEn"" text NOT NULL DEFAULT '';");
            migrationBuilder.Sql(@"ALTER TABLE ""Slaytlar"" ADD COLUMN IF NOT EXISTS ""AciklamaAr"" text NOT NULL DEFAULT '';");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""Slaytlar"" DROP COLUMN IF EXISTS ""AciklamaAr"";");
            migrationBuilder.Sql(@"ALTER TABLE ""Slaytlar"" DROP COLUMN IF EXISTS ""AciklamaEn"";");
            migrationBuilder.Sql(@"ALTER TABLE ""Slaytlar"" DROP COLUMN IF EXISTS ""AltBaslikAr"";");
            migrationBuilder.Sql(@"ALTER TABLE ""Slaytlar"" DROP COLUMN IF EXISTS ""AltBaslikEn"";");
            migrationBuilder.Sql(@"ALTER TABLE ""Slaytlar"" DROP COLUMN IF EXISTS ""BaslikAr"";");
            migrationBuilder.Sql(@"ALTER TABLE ""Slaytlar"" DROP COLUMN IF EXISTS ""BaslikEn"";");
        }
    }
}
