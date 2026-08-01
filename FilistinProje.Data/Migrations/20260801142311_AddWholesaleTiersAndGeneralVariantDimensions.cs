using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWholesaleTiersAndGeneralVariantDimensions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Beden",
                table: "UrunSecenekleri",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OlcuBirimi",
                table: "UrunSecenekleri",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Renk",
                table: "UrunSecenekleri",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RenkKodu",
                table: "UrunSecenekleri",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UrunToptanFiyatKademeleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UrunId = table.Column<int>(type: "integer", nullable: false),
                    UrunSecenekId = table.Column<int>(type: "integer", nullable: true),
                    MinAdet = table.Column<int>(type: "integer", nullable: false),
                    BirimFiyat = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunToptanFiyatKademeleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunToptanFiyatKademeleri_UrunSecenekleri_UrunSecenekId",
                        column: x => x.UrunSecenekId,
                        principalTable: "UrunSecenekleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrunToptanFiyatKademeleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrunToptanFiyatKademeleri_UrunId_UrunSecenekId_MinAdet",
                table: "UrunToptanFiyatKademeleri",
                columns: new[] { "UrunId", "UrunSecenekId", "MinAdet" });

            migrationBuilder.CreateIndex(
                name: "IX_UrunToptanFiyatKademeleri_UrunSecenekId",
                table: "UrunToptanFiyatKademeleri",
                column: "UrunSecenekId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrunToptanFiyatKademeleri");

            migrationBuilder.DropColumn(
                name: "Beden",
                table: "UrunSecenekleri");

            migrationBuilder.DropColumn(
                name: "OlcuBirimi",
                table: "UrunSecenekleri");

            migrationBuilder.DropColumn(
                name: "Renk",
                table: "UrunSecenekleri");

            migrationBuilder.DropColumn(
                name: "RenkKodu",
                table: "UrunSecenekleri");
        }
    }
}
