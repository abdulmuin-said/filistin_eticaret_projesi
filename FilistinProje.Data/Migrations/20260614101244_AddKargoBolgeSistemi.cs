using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKargoBolgeSistemi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KargoBolgeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KargoBolgeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KargoBolgeFiyatlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KargoFirmasiId = table.Column<int>(type: "integer", nullable: false),
                    BolgeId = table.Column<int>(type: "integer", nullable: false),
                    Fiyat = table.Column<decimal>(type: "numeric", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KargoBolgeFiyatlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KargoBolgeFiyatlari_KargoBolgeler_BolgeId",
                        column: x => x.BolgeId,
                        principalTable: "KargoBolgeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KargoBolgeFiyatlari_KargoFirmalari_KargoFirmasiId",
                        column: x => x.KargoFirmasiId,
                        principalTable: "KargoFirmalari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KargoBolgeSehirler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BolgeId = table.Column<int>(type: "integer", nullable: false),
                    SehirAdi = table.Column<string>(type: "text", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KargoBolgeSehirler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KargoBolgeSehirler_KargoBolgeler_BolgeId",
                        column: x => x.BolgeId,
                        principalTable: "KargoBolgeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KargoBolgeFiyatlari_BolgeId",
                table: "KargoBolgeFiyatlari",
                column: "BolgeId");

            migrationBuilder.CreateIndex(
                name: "IX_KargoBolgeFiyatlari_KargoFirmasiId_BolgeId",
                table: "KargoBolgeFiyatlari",
                columns: new[] { "KargoFirmasiId", "BolgeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KargoBolgeSehirler_BolgeId_SehirAdi",
                table: "KargoBolgeSehirler",
                columns: new[] { "BolgeId", "SehirAdi" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KargoBolgeFiyatlari");

            migrationBuilder.DropTable(
                name: "KargoBolgeSehirler");

            migrationBuilder.DropTable(
                name: "KargoBolgeler");
        }
    }
}
