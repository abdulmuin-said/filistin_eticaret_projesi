using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddToptanciMinSiparisTutari : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "SiteAyarlari"
                ADD COLUMN IF NOT EXISTS "ToptanciMinSiparisTutari" numeric NOT NULL DEFAULT 0;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "SiteAyarlari"
                DROP COLUMN IF EXISTS "ToptanciMinSiparisTutari";
                """);
        }
    }
}
