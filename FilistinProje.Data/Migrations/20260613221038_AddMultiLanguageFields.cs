using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiLanguageFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AciklamaAr",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AciklamaEn",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BaslikAr",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BaslikEn",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KisaAciklamaAr",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KisaAciklamaEn",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoDescriptionAr",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoDescriptionEn",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoTitleAr",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoTitleEn",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AciklamaAr",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AciklamaEn",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdAr",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdEn",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AltMetinAr",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AltMetinEn",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KampanyaEtiketiAr",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KampanyaEtiketiEn",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KisaAciklamaAr",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KisaAciklamaEn",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoDescriptionAr",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoDescriptionEn",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoTitleAr",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoTitleEn",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UstMetinAr",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UstMetinEn",
                table: "Kategoriler",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AciklamaAr",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "AciklamaEn",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "BaslikAr",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "BaslikEn",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "KisaAciklamaAr",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "KisaAciklamaEn",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "SeoDescriptionAr",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "SeoDescriptionEn",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "SeoTitleAr",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "SeoTitleEn",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "AciklamaAr",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "AciklamaEn",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "AdAr",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "AdEn",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "AltMetinAr",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "AltMetinEn",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "KampanyaEtiketiAr",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "KampanyaEtiketiEn",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "KisaAciklamaAr",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "KisaAciklamaEn",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "SeoDescriptionAr",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "SeoDescriptionEn",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "SeoTitleAr",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "SeoTitleEn",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "UstMetinAr",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "UstMetinEn",
                table: "Kategoriler");
        }
    }
}
