using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminLocalizationAndWholesaleRejectReason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "KurumsalSayfalar" ADD COLUMN IF NOT EXISTS "BaslikAr" text NOT NULL DEFAULT '';
                ALTER TABLE "KurumsalSayfalar" ADD COLUMN IF NOT EXISTS "BaslikEn" text NOT NULL DEFAULT '';
                ALTER TABLE "KurumsalSayfalar" ADD COLUMN IF NOT EXISTS "IcerikAr" text NOT NULL DEFAULT '';
                ALTER TABLE "KurumsalSayfalar" ADD COLUMN IF NOT EXISTS "IcerikEn" text NOT NULL DEFAULT '';
                ALTER TABLE "HomePageSections" ADD COLUMN IF NOT EXISTS "ButtonTextAr" text NOT NULL DEFAULT '';
                ALTER TABLE "HomePageSections" ADD COLUMN IF NOT EXISTS "ButtonTextEn" text NOT NULL DEFAULT '';
                ALTER TABLE "HomePageSections" ADD COLUMN IF NOT EXISTS "DescriptionAr" text NOT NULL DEFAULT '';
                ALTER TABLE "HomePageSections" ADD COLUMN IF NOT EXISTS "DescriptionEn" text NOT NULL DEFAULT '';
                ALTER TABLE "HomePageSections" ADD COLUMN IF NOT EXISTS "SubtitleAr" text NOT NULL DEFAULT '';
                ALTER TABLE "HomePageSections" ADD COLUMN IF NOT EXISTS "SubtitleEn" text NOT NULL DEFAULT '';
                ALTER TABLE "HomePageSections" ADD COLUMN IF NOT EXISTS "TitleAr" text NOT NULL DEFAULT '';
                ALTER TABLE "HomePageSections" ADD COLUMN IF NOT EXISTS "TitleEn" text NOT NULL DEFAULT '';
                ALTER TABLE "HomePageSections" ADD COLUMN IF NOT EXISTS "ViewAllTextAr" text NOT NULL DEFAULT '';
                ALTER TABLE "HomePageSections" ADD COLUMN IF NOT EXISTS "ViewAllTextEn" text NOT NULL DEFAULT '';
                ALTER TABLE "AspNetUsers" ADD COLUMN IF NOT EXISTS "ToptanciRedSebebi" character varying(1000) NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaslikAr",
                table: "KurumsalSayfalar");

            migrationBuilder.DropColumn(
                name: "BaslikEn",
                table: "KurumsalSayfalar");

            migrationBuilder.DropColumn(
                name: "IcerikAr",
                table: "KurumsalSayfalar");

            migrationBuilder.DropColumn(
                name: "IcerikEn",
                table: "KurumsalSayfalar");

            migrationBuilder.DropColumn(
                name: "ButtonTextAr",
                table: "HomePageSections");

            migrationBuilder.DropColumn(
                name: "ButtonTextEn",
                table: "HomePageSections");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "HomePageSections");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "HomePageSections");

            migrationBuilder.DropColumn(
                name: "SubtitleAr",
                table: "HomePageSections");

            migrationBuilder.DropColumn(
                name: "SubtitleEn",
                table: "HomePageSections");

            migrationBuilder.DropColumn(
                name: "TitleAr",
                table: "HomePageSections");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "HomePageSections");

            migrationBuilder.DropColumn(
                name: "ViewAllTextAr",
                table: "HomePageSections");

            migrationBuilder.DropColumn(
                name: "ViewAllTextEn",
                table: "HomePageSections");

            migrationBuilder.DropColumn(
                name: "ToptanciRedSebebi",
                table: "AspNetUsers");
        }
    }
}
