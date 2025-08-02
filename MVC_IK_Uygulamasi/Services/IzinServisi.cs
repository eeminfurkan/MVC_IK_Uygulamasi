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


        // YENİ METOT: Sadece belirli bir personele ait izinleri getirir.
        public async Task<List<Izin>> PersoneleAitIzinleriGetirAsync(string personelIdentityId)
        {
            // Identity'den gelen kullanıcı ID'si ile bizim Personel modelimiz arasında bir bağlantı kurmamız gerekiyor.
            // Şimdilik bu bağlantıyı en basit haliyle, e-posta üzerinden yapacağız.
            // Daha gelişmiş sistemlerde, Personel modelinde Identity'nin UserId'sini tutan bir alan olur.
            var kullanici = await _dbContext.Users.FindAsync(personelIdentityId);

            if (kullanici != null)
            {
                // Kullanıcının e-postasına sahip personeli buluyoruz.
                var personel = await _dbContext.Personeller.FirstOrDefaultAsync(p => p.Eposta == kullanici.Email);
                if (personel != null)
                {
                    // O personele ait izinleri getiriyoruz.
                    return await _dbContext.Izinler
                        .Where(i => i.PersonelId == personel.Id)
                        .Include(i => i.Personel)
                        .ToListAsync();
                }
            }
            // Eğer eşleşen personel bulunamazsa boş liste döndür.
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