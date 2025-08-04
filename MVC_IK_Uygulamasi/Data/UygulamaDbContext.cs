using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Bu satırı ekliyoruz
using Microsoft.EntityFrameworkCore;
using MVC_IK_Uygulamasi.Models;

namespace MVC_IK_Uygulamasi.Data
{
    // Artık DbContext yerine IdentityDbContext'ten miras alıyoruz.
    // Bu, bu sınıfın hem Identity tablolarını hem de bizim tablolarımızı yönetebileceği anlamına gelir.
    public class UygulamaDbContext : IdentityDbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options)
        {
        }

        // Kendi tablomuzu da buraya ekliyoruz.
        public DbSet<Personel> Personeller { get; set; }
        public DbSet<Izin> Izinler { get; set; }
        public DbSet<Bordro> Bordrolar { get; set; } // BU SATIRI EKLE

    }
}