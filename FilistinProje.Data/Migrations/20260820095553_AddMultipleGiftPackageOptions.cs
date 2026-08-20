using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMultipleGiftPackageOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HediyePaketAdi",
                table: "SiparisDetaylari",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HediyePaketAdiAr",
                table: "SiparisDetaylari",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HediyePaketAdiEn",
                table: "SiparisDetaylari",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HediyePaketSecenegiId",
                table: "SiparisDetaylari",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HediyePaketAdi",
                table: "SepetItems",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HediyePaketAdiAr",
                table: "SepetItems",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HediyePaketAdiEn",
                table: "SepetItems",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HediyePaketSecenegiId",
                table: "SepetItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UrunHediyePaketSecenekleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UrunId = table.Column<int>(type: "integer", nullable: false),
                    Ad = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AdEn = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AdAr = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Fiyat = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunHediyePaketSecenekleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunHediyePaketSecenekleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("""
                INSERT INTO "UrunHediyePaketSecenekleri"
                    ("UrunId", "Ad", "AdEn", "AdAr", "Fiyat", "AktifMi", "Sira", "OlusturulmaTarihi", "SilindiMi")
                SELECT u."Id", 'Standart paket', 'Standard package', 'تغليف قياسي',
                       GREATEST(0, u."HediyePaketFiyati"), true, 1, NOW(), false
                FROM "Urunler" u
                WHERE (u."HediyePaketiVarMi" OR u."HediyePaketFiyati" > 0)
                  AND NOT EXISTS (
                      SELECT 1 FROM "UrunHediyePaketSecenekleri" p WHERE p."UrunId" = u."Id"
                  );

                WITH first_package AS (
                    SELECT DISTINCT ON ("UrunId") "Id", "UrunId", "Ad", "AdEn", "AdAr", "Fiyat"
                    FROM "UrunHediyePaketSecenekleri"
                    WHERE NOT "SilindiMi"
                    ORDER BY "UrunId", "Sira", "Id"
                )
                UPDATE "SepetItems" s
                SET "HediyePaketSecenegiId" = p."Id",
                    "HediyePaketAdi" = p."Ad",
                    "HediyePaketAdiEn" = p."AdEn",
                    "HediyePaketAdiAr" = p."AdAr",
                    "HediyePaketFiyati" = p."Fiyat"
                FROM first_package p
                WHERE s."UrunId" = p."UrunId"
                  AND s."HediyePaketi"
                  AND s."HediyePaketSecenegiId" IS NULL;

                WITH first_package AS (
                    SELECT DISTINCT ON ("UrunId") "Id", "UrunId", "Ad", "AdEn", "AdAr", "Fiyat"
                    FROM "UrunHediyePaketSecenekleri"
                    WHERE NOT "SilindiMi"
                    ORDER BY "UrunId", "Sira", "Id"
                )
                UPDATE "SiparisDetaylari" s
                SET "HediyePaketSecenegiId" = p."Id",
                    "HediyePaketAdi" = p."Ad",
                    "HediyePaketAdiEn" = p."AdEn",
                    "HediyePaketAdiAr" = p."AdAr"
                FROM first_package p
                WHERE s."UrunId" = p."UrunId"
                  AND s."HediyePaketi"
                  AND s."HediyePaketSecenegiId" IS NULL;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_SiparisDetaylari_HediyePaketSecenegiId",
                table: "SiparisDetaylari",
                column: "HediyePaketSecenegiId");

            migrationBuilder.CreateIndex(
                name: "IX_SepetItems_HediyePaketSecenegiId",
                table: "SepetItems",
                column: "HediyePaketSecenegiId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunHediyePaketSecenekleri_UrunId_Sira",
                table: "UrunHediyePaketSecenekleri",
                columns: new[] { "UrunId", "Sira" });

            migrationBuilder.AddForeignKey(
                name: "FK_SepetItems_HediyePaketSecenekleri",
                table: "SepetItems",
                column: "HediyePaketSecenegiId",
                principalTable: "UrunHediyePaketSecenekleri",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SiparisDetaylari_HediyePaketSecenekleri",
                table: "SiparisDetaylari",
                column: "HediyePaketSecenegiId",
                principalTable: "UrunHediyePaketSecenekleri",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SepetItems_HediyePaketSecenekleri",
                table: "SepetItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SiparisDetaylari_HediyePaketSecenekleri",
                table: "SiparisDetaylari");

            migrationBuilder.DropTable(
                name: "UrunHediyePaketSecenekleri");

            migrationBuilder.DropIndex(
                name: "IX_SiparisDetaylari_HediyePaketSecenegiId",
                table: "SiparisDetaylari");

            migrationBuilder.DropIndex(
                name: "IX_SepetItems_HediyePaketSecenegiId",
                table: "SepetItems");

            migrationBuilder.DropColumn(
                name: "HediyePaketAdi",
                table: "SiparisDetaylari");

            migrationBuilder.DropColumn(
                name: "HediyePaketAdiAr",
                table: "SiparisDetaylari");

            migrationBuilder.DropColumn(
                name: "HediyePaketAdiEn",
                table: "SiparisDetaylari");

            migrationBuilder.DropColumn(
                name: "HediyePaketSecenegiId",
                table: "SiparisDetaylari");

            migrationBuilder.DropColumn(
                name: "HediyePaketAdi",
                table: "SepetItems");

            migrationBuilder.DropColumn(
                name: "HediyePaketAdiAr",
                table: "SepetItems");

            migrationBuilder.DropColumn(
                name: "HediyePaketAdiEn",
                table: "SepetItems");

            migrationBuilder.DropColumn(
                name: "HediyePaketSecenegiId",
                table: "SepetItems");
        }
    }
}
