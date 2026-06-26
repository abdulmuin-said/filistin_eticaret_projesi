using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCarkOdulleri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarkOdulleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LabelTr = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LabelEn = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LabelAr = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Tip = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Deger = table.Column<int>(type: "integer", nullable: false),
                    Renk = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    MesajTr = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MesajEn = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MesajAr = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarkOdulleri", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarkOdulleri");
        }
    }
}
