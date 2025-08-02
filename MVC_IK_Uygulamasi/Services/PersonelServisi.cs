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
    }
}