using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderDeliveryAndPrescriptionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceteDosyaYolu",
                table: "Siparisler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeslimatTipi",
                table: "Siparisler",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceteDosyaYolu",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "TeslimatTipi",
                table: "Siparisler");
        }
    }
}
