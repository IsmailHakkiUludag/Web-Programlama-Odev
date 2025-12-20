using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetim.Models;

namespace SporSalonuYonetim.Models
{
    public class IdentityContext : IdentityDbContext<Kullanici>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Antrenor> Antrenorler { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Ücret alanı hassasiyet ayarı.
            builder.Entity<Hizmet>()
                .Property(h => h.Ucret)
                .HasColumnType("decimal(18,2)");

            // 2. Antrenör - Hizmet Arasındaki Çakışmayı Önleme işlemi
            builder.Entity<Antrenor>()
                .HasMany(a => a.Hizmetler)
                .WithMany(h => h.Antrenorler)
                .UsingEntity<Dictionary<string, object>>(
                    "AntrenorHizmet",
                    j => j
                        .HasOne<Hizmet>()
                        .WithMany()
                        .HasForeignKey("HizmetlerId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<Antrenor>()
                        .WithMany()
                        .HasForeignKey("AntrenorlerId")
                        .OnDelete(DeleteBehavior.Cascade));

            // 3. YENİ EKLENEN: Randevu Tablosundaki Çakışmayı Önleme işlemi
            // Antrenör silinirse Randevuyu silme, hata ver (Restrict)
            builder.Entity<Randevu>()
                .HasOne(r => r.Antrenor)
                .WithMany(a => a.Randevular)
                .HasForeignKey(r => r.AntrenorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Hizmet silinirse Randevuyu silme, hata ver (Restrict)
            builder.Entity<Randevu>()
                .HasOne(r => r.Hizmet)
                .WithMany(h => h.Randevular)
                .HasForeignKey(r => r.HizmetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
