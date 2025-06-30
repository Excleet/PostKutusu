using Microsoft.EntityFrameworkCore;
using Otel.Entity.Entities;

namespace Otel.DAL.Data
{
    public class OtelDbContext : DbContext
    {
        public OtelDbContext(DbContextOptions<OtelDbContext> options) : base(options)
        {
        }

        public DbSet<Rezervasyon> Rezervasyonlar { get; set; }
        public DbSet<Admin> Adminler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Rezervasyon entity konfigürasyonları
            modelBuilder.Entity<Rezervasyon>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AdSoyad).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Telefon).IsRequired().HasMaxLength(20);
                entity.Property(e => e.OdaTipi).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Mesaj).HasMaxLength(500);
                entity.Property(e => e.OlusturmaTarihi).HasDefaultValueSql("GETDATE()");
            });

            // Admin entity konfigürasyonları
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.KullaniciAdi).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Sifre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.AdSoyad).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.OlusturmaTarihi).HasDefaultValueSql("GETDATE()");
                
                // Unique constraint for KullaniciAdi
                entity.HasIndex(e => e.KullaniciAdi).IsUnique();
            });

            // Seed data - Varsayılan admin kullanıcısı
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    KullaniciAdi = "admin",
                    Sifre = "123456", // Gerçek projede hash'lenmiş olmalı
                    AdSoyad = "Sistem Yöneticisi",
                    Email = "admin@valeriaantique.com",
                    Aktif = true,
                    OlusturmaTarihi = DateTime.Now
                }
            );
        }
    }
}
