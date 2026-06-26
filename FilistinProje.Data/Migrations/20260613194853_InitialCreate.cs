using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FilistinProje.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AdSoyad = table.Column<string>(type: "text", nullable: false),
                    Sehir = table.Column<string>(type: "text", nullable: false),
                    SifreSifirlamaToken = table.Column<string>(type: "text", nullable: true),
                    SifreSifirlamaGecerlilik = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BultenAbonelikleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    IpAdresi = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BultenAbonelikleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomePageSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SectionType = table.Column<int>(type: "integer", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Subtitle = table.Column<string>(type: "text", nullable: false),
                    ViewAllText = table.Column<string>(type: "text", nullable: true),
                    ViewAllUrl = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ButtonText = table.Column<string>(type: "text", nullable: true),
                    ButtonUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IletisimMesajlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdSoyad = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Konu = table.Column<string>(type: "text", nullable: false),
                    Mesaj = table.Column<string>(type: "text", nullable: false),
                    Tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IpAdresi = table.Column<string>(type: "text", nullable: true),
                    OkunduMu = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IletisimMesajlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KargoFirmalari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Kod = table.Column<string>(type: "text", nullable: false),
                    LogoUrl = table.Column<string>(type: "text", nullable: true),
                    Telefon = table.Column<string>(type: "text", nullable: true),
                    TakipUrl = table.Column<string>(type: "text", nullable: true),
                    GondericiUnvan = table.Column<string>(type: "text", nullable: false),
                    GondericiAdres = table.Column<string>(type: "text", nullable: false),
                    GondericiTelefon = table.Column<string>(type: "text", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    VarsayilanMi = table.Column<bool>(type: "boolean", nullable: false),
                    Fiyat = table.Column<decimal>(type: "numeric", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KargoFirmalari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kategoriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    KisaAciklama = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: true),
                    GorselUrl = table.Column<string>(type: "text", nullable: true),
                    BannerUrl = table.Column<string>(type: "text", nullable: true),
                    SeoTitle = table.Column<string>(type: "text", nullable: false),
                    SeoDescription = table.Column<string>(type: "text", nullable: false),
                    UstMetin = table.Column<string>(type: "text", nullable: false),
                    AltMetin = table.Column<string>(type: "text", nullable: false),
                    KampanyaEtiketi = table.Column<string>(type: "text", nullable: false),
                    UrunSiralamaTipi = table.Column<string>(type: "text", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    ParentKategoriId = table.Column<int>(type: "integer", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoriler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kategoriler_Kategoriler_ParentKategoriId",
                        column: x => x.ParentKategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kuponlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Kod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Tip = table.Column<int>(type: "integer", nullable: false),
                    Deger = table.Column<decimal>(type: "numeric", nullable: false),
                    MinSepetTutari = table.Column<decimal>(type: "numeric", nullable: false),
                    SonKullanmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KullanimLimiti = table.Column<int>(type: "integer", nullable: false),
                    KullanilanMiktar = table.Column<int>(type: "integer", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kuponlar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KurumsalSayfalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "text", nullable: false),
                    Icerik = table.Column<string>(type: "text", nullable: false),
                    UrlSlug = table.Column<string>(type: "text", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KurumsalSayfalar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SenTasarla",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KullaniciId = table.Column<string>(type: "text", nullable: true),
                    DosyaYolu = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Olcu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Fiyat = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Efekt = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CercveliMi = table.Column<bool>(type: "boolean", nullable: false),
                    ParcaSayisi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SepeteEklendi = table.Column<bool>(type: "boolean", nullable: false),
                    SessionId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenTasarla", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteAyarlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteAdi = table.Column<string>(type: "text", nullable: false),
                    MarkaAdi = table.Column<string>(type: "text", nullable: false),
                    SiteBasligi = table.Column<string>(type: "text", nullable: false),
                    SiteAciklamasi = table.Column<string>(type: "text", nullable: false),
                    SiteLogoUrl = table.Column<string>(type: "text", nullable: false),
                    FaviconUrl = table.Column<string>(type: "text", nullable: false),
                    BaseUrl = table.Column<string>(type: "text", nullable: false),
                    TemaRengi = table.Column<string>(type: "text", nullable: false),
                    UstBarMesaji = table.Column<string>(type: "text", nullable: false),
                    KampanyaMesaji = table.Column<string>(type: "text", nullable: false),
                    UstBarEtkin = table.Column<bool>(type: "boolean", nullable: false),
                    UstBarHizi = table.Column<double>(type: "double precision", nullable: false),
                    FooterAciklamasi = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Adres = table.Column<string>(type: "text", nullable: false),
                    WhatsappNumarasi = table.Column<string>(type: "text", nullable: false),
                    CalismaSaatleri = table.Column<string>(type: "text", nullable: false),
                    FacebookUrl = table.Column<string>(type: "text", nullable: false),
                    InstagramUrl = table.Column<string>(type: "text", nullable: false),
                    TwitterUrl = table.Column<string>(type: "text", nullable: false),
                    YoutubeUrl = table.Column<string>(type: "text", nullable: false),
                    TiktokUrl = table.Column<string>(type: "text", nullable: false),
                    PinterestUrl = table.Column<string>(type: "text", nullable: false),
                    ParaBirimi = table.Column<string>(type: "text", nullable: false),
                    KargoBedeli = table.Column<decimal>(type: "numeric", nullable: false),
                    UcretsizKargoLimiti = table.Column<decimal>(type: "numeric", nullable: false),
                    StokUyariLimiti = table.Column<int>(type: "integer", nullable: false),
                    StoktaYokSatisIzni = table.Column<bool>(type: "boolean", nullable: false),
                    PaytrAktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    PaytrTestModu = table.Column<bool>(type: "boolean", nullable: false),
                    PaytrMerchantId = table.Column<string>(type: "text", nullable: false),
                    PaytrMerchantKeyProtected = table.Column<string>(type: "text", nullable: false),
                    PaytrMerchantSaltProtected = table.Column<string>(type: "text", nullable: false),
                    PaytrCallbackUrl = table.Column<string>(type: "text", nullable: false),
                    PaytrBasariliDonusUrl = table.Column<string>(type: "text", nullable: false),
                    PaytrBasarisizDonusUrl = table.Column<string>(type: "text", nullable: false),
                    KargoFirmasi = table.Column<string>(type: "text", nullable: false),
                    KargoTakipUrl = table.Column<string>(type: "text", nullable: false),
                    SiparisTeslimSuresiGun = table.Column<int>(type: "integer", nullable: false),
                    IadeHakkiGun = table.Column<int>(type: "integer", nullable: false),
                    MetaTitle = table.Column<string>(type: "text", nullable: false),
                    MetaDescription = table.Column<string>(type: "text", nullable: false),
                    MetaKeywords = table.Column<string>(type: "text", nullable: false),
                    GoogleAnalyticsId = table.Column<string>(type: "text", nullable: false),
                    FacebookPixelId = table.Column<string>(type: "text", nullable: false),
                    VarsayilanSosyalPaylasimGorseliUrl = table.Column<string>(type: "text", nullable: false),
                    CookieMetni = table.Column<string>(type: "text", nullable: false),
                    YeniSiparisMailBildirimi = table.Column<bool>(type: "boolean", nullable: false),
                    StokUyarisiMailBildirimi = table.Column<bool>(type: "boolean", nullable: false),
                    IadeTalebiMailBildirimi = table.Column<bool>(type: "boolean", nullable: false),
                    BildirimAliciEmail = table.Column<string>(type: "text", nullable: false),
                    BakimModuAktif = table.Column<bool>(type: "boolean", nullable: false),
                    BakimModuMesaji = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteAyarlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Slaytlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "text", nullable: false),
                    AltBaslik = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: false),
                    ResimUrl = table.Column<string>(type: "text", nullable: true),
                    VideoUrl = table.Column<string>(type: "text", nullable: true),
                    Tur = table.Column<string>(type: "text", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slaytlar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrunOzellikTanimlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Kod = table.Column<string>(type: "text", nullable: false),
                    UrunTipi = table.Column<string>(type: "text", nullable: false),
                    AlanTipi = table.Column<string>(type: "text", nullable: false),
                    YardimMetni = table.Column<string>(type: "text", nullable: false),
                    Secenekler = table.Column<string>(type: "text", nullable: false),
                    FiltredeGoster = table.Column<bool>(type: "boolean", nullable: false),
                    DetaySayfasindaGoster = table.Column<bool>(type: "boolean", nullable: false),
                    TeknikTablodaGoster = table.Column<bool>(type: "boolean", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunOzellikTanimlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZiyaretciLoglari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IpAdresi = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    CihazBilgisi = table.Column<string>(type: "text", nullable: false),
                    ReferansUrl = table.Column<string>(type: "text", nullable: true),
                    KullaniciAdi = table.Column<string>(type: "text", nullable: true),
                    Metod = table.Column<string>(type: "text", nullable: false),
                    Sehir = table.Column<string>(type: "text", nullable: true),
                    Ulke = table.Column<string>(type: "text", nullable: true),
                    Tarayici = table.Column<string>(type: "text", nullable: true),
                    CihazModeli = table.Column<string>(type: "text", nullable: true),
                    IsletimSistemi = table.Column<string>(type: "text", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZiyaretciLoglari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Adresler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "text", nullable: false),
                    AdSoyad = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: false),
                    Sehir = table.Column<string>(type: "text", nullable: false),
                    Ilce = table.Column<string>(type: "text", nullable: false),
                    AcikAdres = table.Column<string>(type: "text", nullable: false),
                    AppUserId = table.Column<string>(type: "text", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adresler_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sepetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppUserId = table.Column<string>(type: "text", nullable: true),
                    SessionId = table.Column<string>(type: "text", nullable: true),
                    SonGuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TerkEdildi = table.Column<bool>(type: "boolean", nullable: false),
                    TerkEdilmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HatirlatmaGonderildi = table.Column<bool>(type: "boolean", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sepetler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sepetler_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Siparisler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppUserId = table.Column<string>(type: "text", nullable: true),
                    SiparisNo = table.Column<string>(type: "text", nullable: false),
                    KuponKodu = table.Column<string>(type: "text", nullable: true),
                    IndirimTutari = table.Column<decimal>(type: "numeric", nullable: false),
                    MusteriAdSoyad = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: false),
                    Eposta = table.Column<string>(type: "text", nullable: false),
                    Sehir = table.Column<string>(type: "text", nullable: false),
                    Ilce = table.Column<string>(type: "text", nullable: false),
                    AcikAdres = table.Column<string>(type: "text", nullable: false),
                    ToplamTutar = table.Column<decimal>(type: "numeric", nullable: false),
                    Durum = table.Column<int>(type: "integer", nullable: false),
                    KargoTakipNo = table.Column<string>(type: "text", nullable: true),
                    KargoFirmasiId = table.Column<int>(type: "integer", nullable: true),
                    KargoFirmasi = table.Column<string>(type: "text", nullable: true),
                    KargoUcreti = table.Column<decimal>(type: "numeric", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    EmailHashKodu = table.Column<string>(type: "text", nullable: true),
                    FaturaDosyaYolu = table.Column<string>(type: "text", nullable: true),
                    FaturaDosyaAdi = table.Column<string>(type: "text", nullable: true),
                    FaturaYuklendiMi = table.Column<bool>(type: "boolean", nullable: false),
                    FaturaYuklenmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FaturaMailGonderildiMi = table.Column<bool>(type: "boolean", nullable: false),
                    FaturaMailGonderimTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siparisler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Siparisler_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "text", nullable: false),
                    KisaAd = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: true),
                    UrlYolu = table.Column<string>(type: "text", nullable: false),
                    SKU = table.Column<string>(type: "text", nullable: false),
                    Barkod = table.Column<string>(type: "text", nullable: false),
                    Marka = table.Column<string>(type: "text", nullable: false),
                    UrunTipi = table.Column<string>(type: "text", nullable: false),
                    Etiketler = table.Column<string>(type: "text", nullable: false),
                    KisaAciklama = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: false),
                    TeknikOzellikler = table.Column<string>(type: "text", nullable: false),
                    MalzemeBilgisi = table.Column<string>(type: "text", nullable: false),
                    BakimTalimati = table.Column<string>(type: "text", nullable: false),
                    PaketlemeBilgisi = table.Column<string>(type: "text", nullable: false),
                    AnaGorselUrl = table.Column<string>(type: "text", nullable: false),
                    StokDurumu = table.Column<string>(type: "text", nullable: false),
                    Fiyat = table.Column<decimal>(type: "numeric", nullable: false),
                    IndirimliFiyat = table.Column<decimal>(type: "numeric", nullable: true),
                    Maliyet = table.Column<decimal>(type: "numeric", nullable: false),
                    KdvOrani = table.Column<decimal>(type: "numeric", nullable: false),
                    UretimSuresiGun = table.Column<int>(type: "integer", nullable: false),
                    KargoyaVerilisSuresiGun = table.Column<int>(type: "integer", nullable: false),
                    TahminiTeslimSuresiGun = table.Column<int>(type: "integer", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    OneCikanMi = table.Column<bool>(type: "boolean", nullable: false),
                    YeniUrunMu = table.Column<bool>(type: "boolean", nullable: false),
                    KampanyaliMi = table.Column<bool>(type: "boolean", nullable: false),
                    AnaSayfadaGoster = table.Column<bool>(type: "boolean", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    GoruntulenmeSayisi = table.Column<int>(type: "integer", nullable: false),
                    SatisSayisi = table.Column<int>(type: "integer", nullable: false),
                    FavoriSayisi = table.Column<int>(type: "integer", nullable: false),
                    MinSiparisAdedi = table.Column<int>(type: "integer", nullable: false),
                    MaxSiparisAdedi = table.Column<int>(type: "integer", nullable: true),
                    SeoTitle = table.Column<string>(type: "text", nullable: false),
                    SeoDescription = table.Column<string>(type: "text", nullable: false),
                    SeoKeywords = table.Column<string>(type: "text", nullable: false),
                    KategoriId = table.Column<int>(type: "integer", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Urunler_Kategoriler_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IadeTalepleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiparisId = table.Column<int>(type: "integer", nullable: false),
                    MusteriId = table.Column<string>(type: "text", nullable: false),
                    Neden = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    IBAN = table.Column<string>(type: "text", nullable: true),
                    Durum = table.Column<int>(type: "integer", nullable: false),
                    AdminNotu = table.Column<string>(type: "text", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IadeTalepleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IadeTalepleri_Siparisler_SiparisId",
                        column: x => x.SiparisId,
                        principalTable: "Siparisler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favoriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppUserId = table.Column<string>(type: "text", nullable: false),
                    UrunId = table.Column<int>(type: "integer", nullable: false),
                    FiyatDustugundaBildir = table.Column<bool>(type: "boolean", nullable: false),
                    EskiFiyat = table.Column<decimal>(type: "numeric", nullable: true),
                    SonBildirimTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favoriler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favoriler_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favoriler_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomePageSectionProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SectionId = table.Column<int>(type: "integer", nullable: false),
                    UrunId = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageSectionProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomePageSectionProducts_HomePageSections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "HomePageSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomePageSectionProducts_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UrunOzellikDegerleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UrunId = table.Column<int>(type: "integer", nullable: false),
                    UrunOzellikTanimiId = table.Column<int>(type: "integer", nullable: false),
                    Deger = table.Column<string>(type: "text", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunOzellikDegerleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunOzellikDegerleri_UrunOzellikTanimlari_UrunOzellikTanimi~",
                        column: x => x.UrunOzellikTanimiId,
                        principalTable: "UrunOzellikTanimlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrunOzellikDegerleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UrunResimleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResimYolu = table.Column<string>(type: "text", nullable: false),
                    Baslik = table.Column<string>(type: "text", nullable: false),
                    AltMetin = table.Column<string>(type: "text", nullable: false),
                    MedyaTipi = table.Column<string>(type: "text", nullable: false),
                    MedyaAlani = table.Column<string>(type: "text", nullable: false),
                    VideoUrl = table.Column<string>(type: "text", nullable: false),
                    ThumbnailYolu = table.Column<string>(type: "text", nullable: false),
                    MobilResimYolu = table.Column<string>(type: "text", nullable: false),
                    Etiketler = table.Column<string>(type: "text", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    VarsayilanMi = table.Column<bool>(type: "boolean", nullable: false),
                    UrunSecenekId = table.Column<int>(type: "integer", nullable: true),
                    UrunId = table.Column<int>(type: "integer", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunResimleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunResimleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UrunSecenekleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UrunId = table.Column<int>(type: "integer", nullable: false),
                    Olcu = table.Column<string>(type: "text", nullable: false),
                    CerceveTipi = table.Column<string>(type: "text", nullable: false),
                    CerceveRengi = table.Column<string>(type: "text", nullable: false),
                    CerceveKalinligi = table.Column<string>(type: "text", nullable: false),
                    MalzemeTuru = table.Column<string>(type: "text", nullable: false),
                    Yon = table.Column<string>(type: "text", nullable: false),
                    ParcaSayisi = table.Column<int>(type: "integer", nullable: false),
                    VaryantSku = table.Column<string>(type: "text", nullable: false),
                    KisilestirmeMetni = table.Column<string>(type: "text", nullable: false),
                    OzelTasarimNotu = table.Column<string>(type: "text", nullable: false),
                    FiyatFarki = table.Column<decimal>(type: "numeric", nullable: false),
                    SatisFiyati = table.Column<decimal>(type: "numeric", nullable: false),
                    MaliyetFiyati = table.Column<decimal>(type: "numeric", nullable: false),
                    StokAdedi = table.Column<int>(type: "integer", nullable: false),
                    UretimSuresiGun = table.Column<int>(type: "integer", nullable: false),
                    Desi = table.Column<decimal>(type: "numeric", nullable: false),
                    GorselUrl = table.Column<string>(type: "text", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    VarsayilanMi = table.Column<bool>(type: "boolean", nullable: false),
                    TukeninceGizle = table.Column<bool>(type: "boolean", nullable: false),
                    OnSipariseAcikMi = table.Column<bool>(type: "boolean", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunSecenekleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunSecenekleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Yorumlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UrunId = table.Column<int>(type: "integer", nullable: false),
                    AppUserId = table.Column<string>(type: "text", nullable: true),
                    AdSoyad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    YorumMetni = table.Column<string>(type: "text", nullable: false),
                    Puan = table.Column<int>(type: "integer", nullable: false),
                    OnayliMi = table.Column<bool>(type: "boolean", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yorumlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Yorumlar_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SepetItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SepetId = table.Column<int>(type: "integer", nullable: false),
                    UrunId = table.Column<int>(type: "integer", nullable: false),
                    UrunSecenekId = table.Column<int>(type: "integer", nullable: true),
                    Adet = table.Column<int>(type: "integer", nullable: false),
                    Fiyat = table.Column<decimal>(type: "numeric", nullable: false),
                    UrunBaslik = table.Column<string>(type: "text", nullable: false),
                    UrunResimUrl = table.Column<string>(type: "text", nullable: false),
                    CerceveModeli = table.Column<string>(type: "text", nullable: false),
                    SecenekAdi = table.Column<string>(type: "text", nullable: true),
                    MusteriNotu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SepetItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SepetItems_Sepetler_SepetId",
                        column: x => x.SepetId,
                        principalTable: "Sepetler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SepetItems_UrunSecenekleri_UrunSecenekId",
                        column: x => x.UrunSecenekId,
                        principalTable: "UrunSecenekleri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SepetItems_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiparisDetaylari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiparisId = table.Column<int>(type: "integer", nullable: false),
                    UrunSecenekId = table.Column<int>(type: "integer", nullable: true),
                    Adet = table.Column<int>(type: "integer", nullable: false),
                    BirimFiyat = table.Column<decimal>(type: "numeric", nullable: false),
                    UrunId = table.Column<int>(type: "integer", nullable: false),
                    CerceveModeli = table.Column<string>(type: "text", nullable: false),
                    MusteriNotu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SilindiMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisDetaylari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiparisDetaylari_Siparisler_SiparisId",
                        column: x => x.SiparisId,
                        principalTable: "Siparisler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiparisDetaylari_UrunSecenekleri_UrunSecenekId",
                        column: x => x.UrunSecenekId,
                        principalTable: "UrunSecenekleri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SiparisDetaylari_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adresler_AppUserId",
                table: "Adresler",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favoriler_AppUserId",
                table: "Favoriler",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favoriler_UrunId",
                table: "Favoriler",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_HomePageSectionProducts_SectionId",
                table: "HomePageSectionProducts",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_HomePageSectionProducts_UrunId",
                table: "HomePageSectionProducts",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_IadeTalepleri_SiparisId",
                table: "IadeTalepleri",
                column: "SiparisId");

            migrationBuilder.CreateIndex(
                name: "IX_KargoFirmalari_Kod",
                table: "KargoFirmalari",
                column: "Kod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kategoriler_ParentKategoriId",
                table: "Kategoriler",
                column: "ParentKategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Kategoriler_Slug",
                table: "Kategoriler",
                column: "Slug",
                filter: "\"Slug\" IS NOT NULL AND \"Slug\" <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_SepetItems_SepetId",
                table: "SepetItems",
                column: "SepetId");

            migrationBuilder.CreateIndex(
                name: "IX_SepetItems_UrunId",
                table: "SepetItems",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_SepetItems_UrunSecenekId",
                table: "SepetItems",
                column: "UrunSecenekId");

            migrationBuilder.CreateIndex(
                name: "IX_Sepetler_AppUserId",
                table: "Sepetler",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisDetaylari_SiparisId",
                table: "SiparisDetaylari",
                column: "SiparisId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisDetaylari_UrunId",
                table: "SiparisDetaylari",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisDetaylari_UrunSecenekId",
                table: "SiparisDetaylari",
                column: "UrunSecenekId");

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_AppUserId",
                table: "Siparisler",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_KategoriId",
                table: "Urunler",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_Slug",
                table: "Urunler",
                column: "Slug",
                filter: "\"Slug\" IS NOT NULL AND \"Slug\" <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_UrunOzellikDegerleri_UrunId",
                table: "UrunOzellikDegerleri",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunOzellikDegerleri_UrunOzellikTanimiId",
                table: "UrunOzellikDegerleri",
                column: "UrunOzellikTanimiId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunOzellikTanimlari_UrunTipi_Kod",
                table: "UrunOzellikTanimlari",
                columns: new[] { "UrunTipi", "Kod" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UrunResimleri_UrunId_Sira",
                table: "UrunResimleri",
                columns: new[] { "UrunId", "Sira" });

            migrationBuilder.CreateIndex(
                name: "IX_UrunSecenekleri_UrunId",
                table: "UrunSecenekleri",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_UrunId",
                table: "Yorumlar",
                column: "UrunId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adresler");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BultenAbonelikleri");

            migrationBuilder.DropTable(
                name: "Favoriler");

            migrationBuilder.DropTable(
                name: "HomePageSectionProducts");

            migrationBuilder.DropTable(
                name: "IadeTalepleri");

            migrationBuilder.DropTable(
                name: "IletisimMesajlari");

            migrationBuilder.DropTable(
                name: "KargoFirmalari");

            migrationBuilder.DropTable(
                name: "Kuponlar");

            migrationBuilder.DropTable(
                name: "KurumsalSayfalar");

            migrationBuilder.DropTable(
                name: "SenTasarla");

            migrationBuilder.DropTable(
                name: "SepetItems");

            migrationBuilder.DropTable(
                name: "SiparisDetaylari");

            migrationBuilder.DropTable(
                name: "SiteAyarlari");

            migrationBuilder.DropTable(
                name: "Slaytlar");

            migrationBuilder.DropTable(
                name: "UrunOzellikDegerleri");

            migrationBuilder.DropTable(
                name: "UrunResimleri");

            migrationBuilder.DropTable(
                name: "Yorumlar");

            migrationBuilder.DropTable(
                name: "ZiyaretciLoglari");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "HomePageSections");

            migrationBuilder.DropTable(
                name: "Sepetler");

            migrationBuilder.DropTable(
                name: "Siparisler");

            migrationBuilder.DropTable(
                name: "UrunSecenekleri");

            migrationBuilder.DropTable(
                name: "UrunOzellikTanimlari");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Urunler");

            migrationBuilder.DropTable(
                name: "Kategoriler");
        }
    }
}
