using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKargoBolgeSehirMultilingual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaglantiUrl",
                table: "Slaytlar",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButonMetni",
                table: "Slaytlar",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ButonMetniAr",
                table: "Slaytlar",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ButonMetniEn",
                table: "Slaytlar",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterAciklamasiAr",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterAciklamasiEn",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterAciklamasiTr",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroAltBaslikAr",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroAltBaslikEn",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroAltBaslikTr",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroBaslikAr",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroBaslikEn",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroBaslikTr",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroGorselUrl",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SehirAdiAr",
                table: "KargoBolgeSehirler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SehirAdiEn",
                table: "KargoBolgeSehirler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Fiyat",
                table: "KargoBolgeler",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaglantiUrl",
                table: "Slaytlar");

            migrationBuilder.DropColumn(
                name: "ButonMetni",
                table: "Slaytlar");

            migrationBuilder.DropColumn(
                name: "ButonMetniAr",
                table: "Slaytlar");

            migrationBuilder.DropColumn(
                name: "ButonMetniEn",
                table: "Slaytlar");

            migrationBuilder.DropColumn(
                name: "FooterAciklamasiAr",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "FooterAciklamasiEn",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "FooterAciklamasiTr",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "HeroAltBaslikAr",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "HeroAltBaslikEn",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "HeroAltBaslikTr",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "HeroBaslikAr",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "HeroBaslikEn",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "HeroBaslikTr",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "HeroGorselUrl",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "SehirAdiAr",
                table: "KargoBolgeSehirler");

            migrationBuilder.DropColumn(
                name: "SehirAdiEn",
                table: "KargoBolgeSehirler");

            migrationBuilder.DropColumn(
                name: "Fiyat",
                table: "KargoBolgeler");
        }
    }
}
