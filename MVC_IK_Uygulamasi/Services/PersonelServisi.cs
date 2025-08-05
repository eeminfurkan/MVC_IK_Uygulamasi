using Microsoft.EntityFrameworkCore;
using MVC_IK_Uygulamasi.Data;
using MVC_IK_Uygulamasi.Models;

namespace MVC_IK_Uygulamasi.Services
{
    public class PersonelServisi
    {
        private readonly UygulamaDbContext _dbContext;
        public PersonelServisi(UygulamaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Personel>> TumPersonelleriGetirAsync()
        {
            var bugun = DateTime.Today;

            var bugununIzinleri = await _dbContext.Izinler
                .Where(i => i.OnayDurumu == "Onaylandı" &&
                            i.BaslangicTarihi <= bugun &&
                            i.BitisTarihi >= bugun)
                .Select(i => i.PersonelId)
                .ToListAsync();

            var tumPersoneller = await _dbContext.Personeller.ToListAsync();

            foreach (var personel in tumPersoneller)
            {
                if (bugununIzinleri.Contains(personel.Id))
                {
                    personel.BugunIzinliMi = true;
                }
            }

            return tumPersoneller;
        }
        // ... PersonelBulAsync, PersonelEkleAsync, GuncelleAsync, SilAsync gibi diğer metotların da bu dosyada olduğundan emin ol.
        public async Task PersonelEkleAsync(Personel yeniPersonel)
        {
            _dbContext.Personeller.Add(yeniPersonel);
            await _dbContext.SaveChangesAsync();
        }

        // Bu iki metodu sınıfın içine, diğer metotların yanına ekle

        public async Task<Personel> PersonelBulAsync(int id)
        {
            // Personeli bulurken, ona bağlı olan İzinler listesini de getirmesini söylüyoruz.
            return await _dbContext.Personeller
                                   .Include(p => p.Izinler)
                                   .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Personel> PersonelBulByUserIdAsync(string userId)
        {
            return await _dbContext.Personeller
                                   .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task PersonelSilAsync(int id)
        {
            var silinecekPersonel = await _dbContext.Personeller.FindAsync(id);
            if (silinecekPersonel != null)
            {
                _dbContext.Personeller.Remove(silinecekPersonel);
                await _dbContext.SaveChangesAsync();
            }
        }
        // Bu metodu sınıfın içine, diğer metotların yanına ekle
        public async Task PersonelGuncelleAsync(Personel guncellenecekPersonel)
        {
            // EF Core, nesneyi zaten takip ettiği için sadece durumunu "Değiştirildi" olarak işaretliyoruz.
            _dbContext.Update(guncellenecekPersonel);
            await _dbContext.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet.
        }

    }
}