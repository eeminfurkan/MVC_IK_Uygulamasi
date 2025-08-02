using Microsoft.EntityFrameworkCore;
using MVC_IK_Uygulamasi.Data;
using MVC_IK_Uygulamasi.Models;

namespace MVC_IK_Uygulamasi.Services
{
    public class IzinServisi
    {
        private readonly UygulamaDbContext _dbContext;

        public IzinServisi(UygulamaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Izin>> TumIzinleriGetirAsync()
        {
            // Controller'da yaptığımızın aynısını buraya taşıyoruz.
            // İzinleri listelerken, ilgili personelin Ad ve Soyadını da göstermek için
            // .Include() kullanarak Personel verilerini de çekiyoruz.
            return await _dbContext.Izinler.Include(i => i.Personel).ToListAsync();
        }

        // Yeni izin talebi oluşturmak için metot. Bunu birazdan kullanacağız.
        public async Task IzinTalebiEkleAsync(Izin yeniIzinTalebi)
        {
            _dbContext.Izinler.Add(yeniIzinTalebi);
            await _dbContext.SaveChangesAsync();
        }

        // Bu iki metodu sınıfın içine, diğer metotların yanına ekle

        public async Task<Izin> IzinBulAsync(int id)
        {
            return await _dbContext.Izinler.FindAsync(id);
        }

        public async Task IzinDurumGuncelleAsync(int id, string yeniDurum)
        {
            var guncellenecekIzin = await IzinBulAsync(id);
            if (guncellenecekIzin != null)
            {
                guncellenecekIzin.OnayDurumu = yeniDurum;
                _dbContext.Update(guncellenecekIzin);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}