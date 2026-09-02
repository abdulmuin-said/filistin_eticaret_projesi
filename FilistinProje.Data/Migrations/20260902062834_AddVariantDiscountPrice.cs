using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVariantDiscountPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "IndirimliFiyat",
                table: "UrunSecenekleri",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndirimliFiyat",
                table: "UrunSecenekleri");
        }
    }
}
