using Microsoft.EntityFrameworkCore;
using MVC_IK_Uygulamasi.Data;
using MVC_IK_Uygulamasi.Models;

namespace MVC_IK_Uygulamasi.Services
{
    public class BordroServisi
    {
        private readonly UygulamaDbContext _dbContext;

        public BordroServisi(UygulamaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Gelen bordro listesini veritabanına kaydeder.
        public async Task TopluBordroEkleAsync(List<Bordro> bordrolar)
        {
            foreach (var bordro in bordrolar)
            {
                // Net maaşı burada, son kez hesaplıyoruz.
                bordro.NetMaas = bordro.BrutMaas + bordro.EkOdemeler - bordro.ToplamKesintiler;
                _dbContext.Bordrolar.Add(bordro);
            }
            await _dbContext.SaveChangesAsync();
        }

        // Bu metodu sınıfın içine, diğer metotların yanına ekle
        public async Task<List<Bordro>> TumBordrolariGetirAsync()
        {
            // Bordroları listelerken, hangi personele ait olduğunu görebilmek için
            // .Include() ile Personel bilgilerini de çekiyoruz.
            return await _dbContext.Bordrolar.Include(b => b.Personel).OrderByDescending(b => b.Donem).ToListAsync();
        }
    }
}