using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPushAbonelik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PushAbonelikleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AppUserId = table.Column<string>(type: "text", nullable: true),
                    Tarayici = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Platform = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    SonGorulmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Tercihler = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushAbonelikleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PushAbonelikleri_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PushAbonelikleri_AppUserId",
                table: "PushAbonelikleri",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PushAbonelikleri_Token",
                table: "PushAbonelikleri",
                column: "Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PushAbonelikleri");

            migrationBuilder.CreateTable(
                name: "HaberBandi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    BgRenk = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LinkMetniAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LinkMetniEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LinkMetniTr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MetinAr = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    MetinEn = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    MetinRengi = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    MetinTr = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    YeniSekme = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HaberBandi", x => x.Id);
                });
        }
    }
}
