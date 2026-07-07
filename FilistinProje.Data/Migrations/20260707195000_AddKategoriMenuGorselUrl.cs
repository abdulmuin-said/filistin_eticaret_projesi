using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    public partial class AddKategoriMenuGorselUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""Kategoriler"" ADD COLUMN IF NOT EXISTS ""MenuGorselUrl"" text NULL;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""Kategoriler"" DROP COLUMN IF EXISTS ""MenuGorselUrl"";");
        }
    }
}
