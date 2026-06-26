using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGiftWrapFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HediyePaketFiyati",
                table: "Urunler",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "HediyePaketiVarMi",
                table: "Urunler",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "HediyePaketFiyati",
                table: "SiparisDetaylari",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "HediyePaketi",
                table: "SiparisDetaylari",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "HediyePaketFiyati",
                table: "SepetItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "HediyePaketi",
                table: "SepetItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HediyePaketFiyati",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "HediyePaketiVarMi",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "HediyePaketFiyati",
                table: "SiparisDetaylari");

            migrationBuilder.DropColumn(
                name: "HediyePaketi",
                table: "SiparisDetaylari");

            migrationBuilder.DropColumn(
                name: "HediyePaketFiyati",
                table: "SepetItems");

            migrationBuilder.DropColumn(
                name: "HediyePaketi",
                table: "SepetItems");
        }
    }
}
