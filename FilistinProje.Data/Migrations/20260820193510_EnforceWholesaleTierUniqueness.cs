using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnforceWholesaleTierUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM "UrunToptanFiyatKademeleri" duplicate
                USING "UrunToptanFiyatKademeleri" keeper
                WHERE duplicate."SilindiMi" = false
                  AND keeper."SilindiMi" = false
                  AND duplicate."UrunId" = keeper."UrunId"
                  AND duplicate."UrunSecenekId" IS NOT DISTINCT FROM keeper."UrunSecenekId"
                  AND duplicate."MinAdet" = keeper."MinAdet"
                  AND duplicate."Id" > keeper."Id";
                """);

            migrationBuilder.DropIndex(
                name: "IX_UrunToptanFiyatKademeleri_UrunId_UrunSecenekId_MinAdet",
                table: "UrunToptanFiyatKademeleri");

            migrationBuilder.CreateIndex(
                name: "UX_UrunToptanFiyatKademeleri_Urun_MinAdet",
                table: "UrunToptanFiyatKademeleri",
                columns: new[] { "UrunId", "MinAdet" },
                unique: true,
                filter: "\"UrunSecenekId\" IS NULL AND \"SilindiMi\" = false");

            migrationBuilder.CreateIndex(
                name: "UX_UrunToptanFiyatKademeleri_Varyant_MinAdet",
                table: "UrunToptanFiyatKademeleri",
                columns: new[] { "UrunId", "UrunSecenekId", "MinAdet" },
                unique: true,
                filter: "\"UrunSecenekId\" IS NOT NULL AND \"SilindiMi\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_UrunToptanFiyatKademeleri_Urun_MinAdet",
                table: "UrunToptanFiyatKademeleri");

            migrationBuilder.DropIndex(
                name: "UX_UrunToptanFiyatKademeleri_Varyant_MinAdet",
                table: "UrunToptanFiyatKademeleri");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToptanFiyatKademeleri_UrunId_UrunSecenekId_MinAdet",
                table: "UrunToptanFiyatKademeleri",
                columns: new[] { "UrunId", "UrunSecenekId", "MinAdet" });
        }
    }
}
