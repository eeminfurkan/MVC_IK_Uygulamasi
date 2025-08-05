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
            return await _dbContext.Izinler.Include(i => i.Personel).ToListAsync();
        }


        public async Task<List<Izin>> PersoneleAitIzinleriGetirAsync(string personelIdentityId)
        {
            // --- ESKİ KODU SİLİP YENİSİNİ EKLİYORUZ ---

            // Doğrudan bize gelen kullanıcı ID'si (personelIdentityId) ile eşleşen personeli buluyoruz.
            // Bu, en doğru ve en güvenilir yöntemdir.
            var personel = await _dbContext.Personeller
                                           .FirstOrDefaultAsync(p => p.UserId == personelIdentityId);

            if (personel != null)
            {
                // O personele ait izinleri getiriyoruz.
                return await _dbContext.Izinler
                    .Where(i => i.PersonelId == personel.Id)
                    .Include(i => i.Personel) // Personel bilgilerini de dahil ediyoruz.
                    .OrderByDescending(i => i.TalepTarihi) // İzinleri en yeniden eskiye doğru sıralayabiliriz.
                    .ToListAsync();
            }

            // Eğer eşleşen personel bulunamazsa (normalde olmamalı), boş liste döndür.
            return new List<Izin>();
        }

        public async Task IzinTalebiEkleAsync(Izin yeniIzinTalebi)
        {
            _dbContext.Izinler.Add(yeniIzinTalebi);
            await _dbContext.SaveChangesAsync();
        }

        // Bu metodu sınıfın içine, diğer metotların yanına ekle
        public async Task IzinDurumGuncelleAsync(int id, string yeniDurum)
        {
            var guncellenecekIzin = await _dbContext.Izinler.FindAsync(id);
            if (guncellenecekIzin != null)
            {
                guncellenecekIzin.OnayDurumu = yeniDurum;
                _dbContext.Update(guncellenecekIzin);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}