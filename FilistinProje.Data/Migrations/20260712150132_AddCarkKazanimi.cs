using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCarkKazanimi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarkKazanimlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppUserId = table.Column<string>(type: "text", nullable: false),
                    CarkOdulId = table.Column<int>(type: "integer", nullable: false),
                    KuponId = table.Column<int>(type: "integer", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarkKazanimlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarkKazanimlari_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarkKazanimlari_CarkOdulleri_CarkOdulId",
                        column: x => x.CarkOdulId,
                        principalTable: "CarkOdulleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarkKazanimlari_Kuponlar_KuponId",
                        column: x => x.KuponId,
                        principalTable: "Kuponlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarkKazanimlari_AppUserId",
                table: "CarkKazanimlari",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarkKazanimlari_CarkOdulId",
                table: "CarkKazanimlari",
                column: "CarkOdulId");

            migrationBuilder.CreateIndex(
                name: "IX_CarkKazanimlari_KuponId",
                table: "CarkKazanimlari",
                column: "KuponId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarkKazanimlari");
        }
    }
}
