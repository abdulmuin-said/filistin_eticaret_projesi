using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStokBildirimLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StokBildirimLoglari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UrunId = table.Column<int>(type: "integer", nullable: false),
                    UrunSecenekId = table.Column<int>(type: "integer", nullable: true),
                    KalanStok = table.Column<int>(type: "integer", nullable: false),
                    StokEsigi = table.Column<int>(type: "integer", nullable: false),
                    BildirimTipi = table.Column<string>(type: "text", nullable: false),
                    GonderildiMi = table.Column<bool>(type: "boolean", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StokBildirimLoglari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StokBildirimLoglari_UrunSecenekleri_UrunSecenekId",
                        column: x => x.UrunSecenekId,
                        principalTable: "UrunSecenekleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StokBildirimLoglari_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StokBildirimLoglari_UrunId",
                table: "StokBildirimLoglari",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_StokBildirimLoglari_UrunId_UrunSecenekId_BildirimTipi",
                table: "StokBildirimLoglari",
                columns: new[] { "UrunId", "UrunSecenekId", "BildirimTipi" });

            migrationBuilder.CreateIndex(
                name: "IX_StokBildirimLoglari_UrunSecenekId",
                table: "StokBildirimLoglari",
                column: "UrunSecenekId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StokBildirimLoglari");
        }
    }
}
