using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddToptanciUrunGrubu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ToptanciUrunGrubuId",
                table: "Urunler",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ToptanciUrunGruplari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToptanciUrunGruplari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToptanciIskontoOranlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ToptanciUrunGrubuId = table.Column<int>(type: "integer", nullable: false),
                    MinAdet = table.Column<int>(type: "integer", nullable: false),
                    IskontoYuzdesi = table.Column<decimal>(type: "numeric", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToptanciIskontoOranlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToptanciIskontoOranlari_ToptanciUrunGruplari_ToptanciUrunGr~",
                        column: x => x.ToptanciUrunGrubuId,
                        principalTable: "ToptanciUrunGruplari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_ToptanciUrunGrubuId",
                table: "Urunler",
                column: "ToptanciUrunGrubuId");

            migrationBuilder.CreateIndex(
                name: "IX_ToptanciIskontoOranlari_ToptanciUrunGrubuId",
                table: "ToptanciIskontoOranlari",
                column: "ToptanciUrunGrubuId");

            migrationBuilder.CreateIndex(
                name: "IX_ToptanciUrunGruplari_AktifMi",
                table: "ToptanciUrunGruplari",
                column: "AktifMi");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_ToptanciUrunGruplari_ToptanciUrunGrubuId",
                table: "Urunler",
                column: "ToptanciUrunGrubuId",
                principalTable: "ToptanciUrunGruplari",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_ToptanciUrunGruplari_ToptanciUrunGrubuId",
                table: "Urunler");

            migrationBuilder.DropTable(
                name: "ToptanciIskontoOranlari");

            migrationBuilder.DropTable(
                name: "ToptanciUrunGruplari");

            migrationBuilder.DropIndex(
                name: "IX_Urunler_ToptanciUrunGrubuId",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "ToptanciUrunGrubuId",
                table: "Urunler");
        }
    }
}
