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
            return await _dbContext.Personeller.ToListAsync();
        }
        public async Task PersonelEkleAsync(Personel yeniPersonel)
        {
            _dbContext.Personeller.Add(yeniPersonel);
            await _dbContext.SaveChangesAsync();
        }

        // Bu iki metodu sınıfın içine, diğer metotların yanına ekle

        public async Task<Personel> PersonelBulAsync(int id)
        {
            // FindAsync, verilen Id'ye sahip personeli bulur. Bulamazsa null döner.
            return await _dbContext.Personeller.FindAsync(id);
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

    }
}