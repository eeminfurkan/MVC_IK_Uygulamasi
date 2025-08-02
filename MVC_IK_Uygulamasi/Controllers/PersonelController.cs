using Microsoft.AspNetCore.Mvc;
using MVC_IK_Uygulamasi.Services; // Servisimizi kullanmak için ekledik.
using System.Threading.Tasks;

namespace MVC_IK_Uygulamasi.Controllers
{
    public class PersonelController : Controller
    {
        private readonly PersonelServisi _personelServisi;

        // Constructor üzerinden PersonelServisi'ni istiyoruz (Dependency Injection).
        public PersonelController(PersonelServisi personelServisi)
        {
            _personelServisi = personelServisi;
        }

        // Bu metot, /Personel veya /Personel/Index adresine gidildiğinde çalışacak.
        public async Task<IActionResult> Index()
        {
            // Servisimizden tüm personellerin listesini istiyoruz.
            var personelListesi = await _personelServisi.TumPersonelleriGetirAsync();

            // Gelen listeyi View'a (görünüme) gönderiyoruz.
            return View(personelListesi);
        }
    }
}