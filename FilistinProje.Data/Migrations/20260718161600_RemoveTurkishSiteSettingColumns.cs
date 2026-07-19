using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    [DbContext(typeof(KanvasDbContext))]
    [Migration("20260718161600_RemoveTurkishSiteSettingColumns")]
    public sealed class RemoveTurkishSiteSettingColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "FooterAciklamasiTr", table: "SiteAyarlari");
            migrationBuilder.DropColumn(name: "HeroAltBaslikTr", table: "SiteAyarlari");
            migrationBuilder.DropColumn(name: "HeroBaslikTr", table: "SiteAyarlari");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FooterAciklamasiTr",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroAltBaslikTr",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroBaslikTr",
                table: "SiteAyarlari",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
