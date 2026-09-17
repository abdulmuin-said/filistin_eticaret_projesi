using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNoExchangeFieldsToUrunler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExchangeable",
                table: "Urunler",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NoExchangeBadgeColor",
                table: "Urunler",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExchangeable",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "NoExchangeBadgeColor",
                table: "Urunler");
        }
    }
}
