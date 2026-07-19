using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    public partial class SecurityIntegrityHardening : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sepetler_AppUserId",
                table: "Sepetler");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Kod",
                table: "Kuponlar",
                type: "citext",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<DateTime>(
                name: "BaslangicTarihi",
                table: "Kuponlar",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_EmailHashKodu",
                table: "Siparisler",
                column: "EmailHashKodu",
                unique: true,
                filter: "\"EmailHashKodu\" IS NOT NULL AND \"EmailHashKodu\" <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_SiparisNo",
                table: "Siparisler",
                column: "SiparisNo",
                unique: true,
                filter: "\"SiparisNo\" <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_Sepetler_AppUserId",
                table: "Sepetler",
                column: "AppUserId",
                unique: true,
                filter: "\"SilindiMi\" = false AND \"AppUserId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sepetler_SessionId",
                table: "Sepetler",
                column: "SessionId",
                unique: true,
                filter: "\"SilindiMi\" = false AND \"SessionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kuponlar_Kod",
                table: "Kuponlar",
                column: "Kod",
                unique: true,
                filter: "\"SilindiMi\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_KargoBolgeler_Ad",
                table: "KargoBolgeler",
                column: "Ad",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favoriler_AppUserId_UrunId",
                table: "Favoriler",
                columns: new[] { "AppUserId", "UrunId" },
                unique: true,
                filter: "\"SilindiMi\" = false");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Siparisler_EmailHashKodu", table: "Siparisler");
            migrationBuilder.DropIndex(name: "IX_Siparisler_SiparisNo", table: "Siparisler");
            migrationBuilder.DropIndex(name: "IX_Sepetler_AppUserId", table: "Sepetler");
            migrationBuilder.DropIndex(name: "IX_Sepetler_SessionId", table: "Sepetler");
            migrationBuilder.DropIndex(name: "IX_Kuponlar_Kod", table: "Kuponlar");
            migrationBuilder.DropIndex(name: "IX_KargoBolgeler_Ad", table: "KargoBolgeler");
            migrationBuilder.DropIndex(name: "IX_Favoriler_AppUserId_UrunId", table: "Favoriler");

            migrationBuilder.DropColumn(
                name: "BaslangicTarihi",
                table: "Kuponlar");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Kod",
                table: "Kuponlar",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "IX_Sepetler_AppUserId",
                table: "Sepetler",
                column: "AppUserId");
        }
    }
}
