using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKapidaOdemeBedeli : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "KapidaOdemeAktifMi",
                table: "SiteAyarlari",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "KapidaOdemeHizmetBedeli",
                table: "SiteAyarlari",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "KapidaOdemeHizmetBedeli",
                table: "Siparisler",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "OdemeYontemi",
                table: "Siparisler",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KapidaOdemeAktifMi",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "KapidaOdemeHizmetBedeli",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "KapidaOdemeHizmetBedeli",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "OdemeYontemi",
                table: "Siparisler");
        }
    }
}
