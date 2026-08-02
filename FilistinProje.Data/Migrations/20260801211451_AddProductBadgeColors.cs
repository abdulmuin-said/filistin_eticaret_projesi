using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductBadgeColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IndirimEtiketRengi",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "#B86A2F");

            migrationBuilder.AddColumn<string>(
                name: "KampanyaEtiketRengi",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "#31543B");

            migrationBuilder.AddColumn<string>(
                name: "OneCikanEtiketRengi",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "#D6AB5B");

            migrationBuilder.AddColumn<string>(
                name: "YeniUrunEtiketRengi",
                table: "Urunler",
                type: "text",
                nullable: false,
                defaultValue: "#B33A3A");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndirimEtiketRengi",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "KampanyaEtiketRengi",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "OneCikanEtiketRengi",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "YeniUrunEtiketRengi",
                table: "Urunler");
        }
    }
}
