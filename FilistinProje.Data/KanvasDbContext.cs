using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FilistinProje.Core.Models;
using FilistinProje.Core.Varliklar;

namespace FilistinProje.Data
{
    public class KanvasDbContext : IdentityDbContext<AppUser>
    {
        public KanvasDbContext(DbContextOptions<KanvasDbContext> options) : base(options)
        {
        }

        public DbSet<SiteAyarlari> SiteAyarlari { get; set; }
        public DbSet<Slayt> Slaytlar { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<UrunSecenek> UrunSecenekleri { get; set; }
        public DbSet<UrunHediyePaketSecenegi> UrunHediyePaketSecenekleri { get; set; }
        public DbSet<UrunResim> UrunResimleri { get; set; }
        public DbSet<UrunOzellikTanimi> UrunOzellikTanimlari { get; set; }
        public DbSet<UrunOzellikDegeri> UrunOzellikDegerleri { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<SiparisDetay> SiparisDetaylari { get; set; }
        public DbSet<Sepet> Sepetler { get; set; }
        public DbSet<SepetItem> SepetItems { get; set; }
        public DbSet<Adres> Adresler { get; set; }
        public DbSet<Favori> Favoriler { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }
        public DbSet<SiteDegerlendirme> SiteDegerlendirmeleri { get; set; }
        public DbSet<IletisimMesaj> IletisimMesajlari { get; set; }
        public DbSet<BultenAboneligi> BultenAbonelikleri { get; set; }
        public DbSet<Kupon> Kuponlar { get; set; }
        public DbSet<IadeTalebi> IadeTalepleri { get; set; }
        public DbSet<KargoFirmasi> KargoFirmalari { get; set; }
        public DbSet<KargoBolge> KargoBolgeler { get; set; }
        public DbSet<KargoBolgeSehir> KargoBolgeSehirler { get; set; }
        public DbSet<KargoBolgeFiyat> KargoBolgeFiyatlari { get; set; }
        public DbSet<KurumsalSayfa> KurumsalSayfalar { get; set; }
        public DbSet<ZiyaretciLog> ZiyaretciLoglari { get; set; }
        public DbSet<SenTasarla> SenTasarla { get; set; }
        public DbSet<HomePageSection> HomePageSections { get; set; }
        public DbSet<HomePageSectionProduct> HomePageSectionProducts { get; set; }
        public DbSet<BankaHesap> BankaHesaplari { get; set; }
        public DbSet<ToptanciUrunGrubu> ToptanciUrunGruplari { get; set; }
        public DbSet<ToptanciIskontoOrani> ToptanciIskontoOranlari { get; set; }
        public DbSet<UrunToptanFiyatKademesi> UrunToptanFiyatKademeleri { get; set; }
        public DbSet<PushAbonelik> PushAbonelikleri { get; set; }
        public DbSet<StokBildirimLog> StokBildirimLoglari { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("citext");

            modelBuilder.Entity<SiteAyarlari>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Slayt>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Kategori>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ParentKategoriId);
                entity.HasIndex(e => e.Slug).HasFilter("\"Slug\" IS NOT NULL AND \"Slug\" <> ''");
                entity.HasOne(e => e.ParentKategori)
                    .WithMany(e => e.AltKategoriler)
                    .HasForeignKey(e => e.ParentKategoriId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Urun>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.KategoriId);
                entity.HasIndex(e => e.Slug).HasFilter("\"Slug\" IS NOT NULL AND \"Slug\" <> ''");
                entity.HasIndex(e => e.ToptanciUrunGrubuId);
                entity.Property(e => e.OneCikanEtiketRengi).HasDefaultValue("#D6AB5B");
                entity.Property(e => e.YeniUrunEtiketRengi).HasDefaultValue("#B33A3A");
                entity.Property(e => e.KampanyaEtiketRengi).HasDefaultValue("#31543B");
                entity.Property(e => e.IndirimEtiketRengi).HasDefaultValue("#B86A2F");
                entity.HasOne(e => e.Kategori)
                    .WithMany(e => e.Urunler)
                    .HasForeignKey(e => e.KategoriId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.ToptanciUrunGrubu)
                    .WithMany(e => e.Urunler)
                    .HasForeignKey(e => e.ToptanciUrunGrubuId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<UrunSecenek>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UrunId);
                entity.HasOne(e => e.Urun)
                    .WithMany(e => e.UrunSecenek)
                    .HasForeignKey(e => e.UrunId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UrunHediyePaketSecenegi>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Ad).HasMaxLength(150);
                entity.Property(e => e.AdEn).HasMaxLength(150);
                entity.Property(e => e.AdAr).HasMaxLength(150);
                entity.Property(e => e.Fiyat).HasPrecision(18, 2);
                entity.HasIndex(e => new { e.UrunId, e.Sira });
                entity.HasOne(e => e.Urun)
                    .WithMany(e => e.HediyePaketSecenekleri)
                    .HasForeignKey(e => e.UrunId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UrunResim>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UrunId, e.Sira });
                entity.HasOne(e => e.Urun)
                    .WithMany(e => e.UrunResimleri)
                    .HasForeignKey(e => e.UrunId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UrunOzellikTanimi>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UrunTipi, e.Kod }).IsUnique();
            });

            modelBuilder.Entity<UrunOzellikDegeri>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UrunId);
                entity.HasIndex(e => e.UrunOzellikTanimiId);
                entity.HasOne(e => e.Urun)
                    .WithMany(e => e.UrunOzellikleri)
                    .HasForeignKey(e => e.UrunId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.UrunOzellikTanimi)
                    .WithMany(e => e.UrunOzellikDegerleri)
                    .HasForeignKey(e => e.UrunOzellikTanimiId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Siparis>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.AppUserId);
                entity.HasIndex(e => e.SiparisNo)
                    .IsUnique()
                    .HasFilter("\"SiparisNo\" <> ''");
                entity.HasIndex(e => e.EmailHashKodu)
                    .IsUnique()
                    .HasFilter("\"EmailHashKodu\" IS NOT NULL AND \"EmailHashKodu\" <> ''");
                entity.HasOne(e => e.AppUser)
                    .WithMany()
                    .HasForeignKey(e => e.AppUserId);
            });

            modelBuilder.Entity<SiparisDetay>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.SiparisId);
                entity.HasIndex(e => e.UrunId);
                entity.HasIndex(e => e.UrunSecenekId);
                entity.HasOne(e => e.Siparis)
                    .WithMany(e => e.SiparisDetaylari)
                    .HasForeignKey(e => e.SiparisId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Urun)
                    .WithMany()
                    .HasForeignKey(e => e.UrunId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.UrunSecenek)
                    .WithMany()
                    .HasForeignKey(e => e.UrunSecenekId);
                entity.HasOne(e => e.HediyePaketSecenegi)
                    .WithMany()
                    .HasForeignKey(e => e.HediyePaketSecenegiId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_SiparisDetaylari_HediyePaketSecenekleri");
            });

            modelBuilder.Entity<Sepet>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.AppUserId)
                    .IsUnique()
                    .HasFilter("\"SilindiMi\" = false AND \"AppUserId\" IS NOT NULL");
                entity.HasIndex(e => e.SessionId)
                    .IsUnique()
                    .HasFilter("\"SilindiMi\" = false AND \"SessionId\" IS NOT NULL");
                entity.HasOne(e => e.AppUser)
                    .WithMany()
                    .HasForeignKey(e => e.AppUserId);
            });

            modelBuilder.Entity<SepetItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.SepetId);
                entity.HasIndex(e => e.UrunId);
                entity.HasIndex(e => e.UrunSecenekId);
                entity.HasOne(e => e.Sepet)
                    .WithMany(e => e.SepetItems)
                    .HasForeignKey(e => e.SepetId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Urun)
                    .WithMany()
                    .HasForeignKey(e => e.UrunId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.UrunSecenek)
                    .WithMany()
                    .HasForeignKey(e => e.UrunSecenekId);
                entity.HasOne(e => e.HediyePaketSecenegi)
                    .WithMany()
                    .HasForeignKey(e => e.HediyePaketSecenegiId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_SepetItems_HediyePaketSecenekleri");
            });

            modelBuilder.Entity<Adres>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.AppUser)
                    .WithMany()
                    .HasForeignKey(e => e.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity<Favori>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.AppUserId);
                entity.HasIndex(e => e.UrunId);
                entity.HasIndex(e => new { e.AppUserId, e.UrunId })
                    .IsUnique()
                    .HasFilter("\"SilindiMi\" = false");
                entity.HasOne(e => e.AppUser)
                    .WithMany()
                    .HasForeignKey(e => e.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
                entity.HasOne(e => e.Urun)
                    .WithMany()
                    .HasForeignKey(e => e.UrunId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity<Yorum>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UrunId);
                entity.HasOne(e => e.Urun)
                    .WithMany()
                    .HasForeignKey(e => e.UrunId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity<SiteDegerlendirme>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<IletisimMesaj>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<BultenAboneligi>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Kupon>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Kod).HasColumnType("citext");
                entity.HasIndex(e => e.Kod)
                    .IsUnique()
                    .HasFilter("\"SilindiMi\" = false");
            });

            modelBuilder.Entity<IadeTalebi>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.SiparisId);
                entity.HasOne(e => e.Siparis)
                    .WithMany()
                    .HasForeignKey(e => e.SiparisId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity<KargoFirmasi>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Kod).IsUnique();
            });

            modelBuilder.Entity<KargoBolge>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Ad).IsUnique();
                entity.Property(e => e.Ulke).HasMaxLength(100);
                entity.Property(e => e.Aciklama).HasMaxLength(500);
            });

            modelBuilder.Entity<KargoBolgeSehir>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.BolgeId, e.SehirAdi }).IsUnique();
                entity.HasOne(e => e.Bolge)
                    .WithMany(e => e.Sehirler)
                    .HasForeignKey(e => e.BolgeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<KargoBolgeFiyat>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.KargoFirmasiId, e.BolgeId }).IsUnique();
                entity.HasOne(e => e.KargoFirmasi)
                    .WithMany()
                    .HasForeignKey(e => e.KargoFirmasiId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Bolge)
                    .WithMany(e => e.Fiyatlar)
                    .HasForeignKey(e => e.BolgeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<KurumsalSayfa>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<ZiyaretciLog>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<SenTasarla>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<HomePageSection>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<HomePageSectionProduct>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.SectionId);
                entity.HasIndex(e => e.UrunId);
                entity.HasOne(e => e.Section)
                    .WithMany(e => e.SectionProducts)
                    .HasForeignKey(e => e.SectionId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Urun)
                    .WithMany()
                    .HasForeignKey(e => e.UrunId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ToptanciUrunGrubu>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.AktifMi);
            });

            modelBuilder.Entity<ToptanciIskontoOrani>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ToptanciUrunGrubuId);
                entity.HasOne(e => e.ToptanciUrunGrubu)
                    .WithMany(e => e.IskontoOranlari)
                    .HasForeignKey(e => e.ToptanciUrunGrubuId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UrunToptanFiyatKademesi>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BirimFiyat).HasPrecision(18, 2);
                entity.HasIndex(e => new { e.UrunId, e.UrunSecenekId, e.MinAdet });
                entity.HasOne(e => e.Urun)
                    .WithMany(e => e.ToptanFiyatKademeleri)
                    .HasForeignKey(e => e.UrunId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.UrunSecenek)
                    .WithMany(e => e.ToptanFiyatKademeleri)
                    .HasForeignKey(e => e.UrunSecenekId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PushAbonelik>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Token);
                entity.HasIndex(e => e.AppUserId);
                entity.HasOne(e => e.AppUser)
                    .WithMany()
                    .HasForeignKey(e => e.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<StokBildirimLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UrunId);
                entity.HasIndex(e => new { e.UrunId, e.UrunSecenekId, e.BildirimTipi });
                entity.HasOne(e => e.Urun)
                    .WithMany()
                    .HasForeignKey(e => e.UrunId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.UrunSecenek)
                    .WithMany()
                    .HasForeignKey(e => e.UrunSecenekId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

        }
    }
}
