using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUrunSecenekIdToToptanciIskontoOrani : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UrunSecenekId",
                table: "ToptanciIskontoOranlari",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToptanciIskontoOranlari_UrunSecenekId",
                table: "ToptanciIskontoOranlari",
                column: "UrunSecenekId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToptanciIskontoOranlari_UrunSecenekleri_UrunSecenekId",
                table: "ToptanciIskontoOranlari",
                column: "UrunSecenekId",
                principalTable: "UrunSecenekleri",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToptanciIskontoOranlari_UrunSecenekleri_UrunSecenekId",
                table: "ToptanciIskontoOranlari");

            migrationBuilder.DropIndex(
                name: "IX_ToptanciIskontoOranlari_UrunSecenekId",
                table: "ToptanciIskontoOranlari");

            migrationBuilder.DropColumn(
                name: "UrunSecenekId",
                table: "ToptanciIskontoOranlari");
        }
    }
}
