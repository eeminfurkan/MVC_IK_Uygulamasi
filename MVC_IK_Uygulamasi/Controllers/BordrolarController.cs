using Microsoft.AspNetCore.Mvc;
using MVC_IK_Uygulamasi.Services;
using System.Threading.Tasks;
using System.Collections.Generic; // List için eklendi
using MVC_IK_Uygulamasi.Models; // Bordro modeli için eklendi

namespace MVC_IK_Uygulamasi.Controllers
{
    public class BordrolarController : Controller
    {
        private readonly BordroServisi _bordroServisi;
        private readonly PersonelServisi _personelServisi;

        public BordrolarController(BordroServisi bordroServisi, PersonelServisi personelServisi)
        {
            _bordroServisi = bordroServisi;
            _personelServisi = personelServisi;
        }

        // GET: Bordrolar
        // Tüm bordroları listeler.
        public async Task<IActionResult> Index()
        {
            var bordrolar = await _bordroServisi.TumBordrolariGetirAsync();
            return View(bordrolar);
        }

        // GET: Bordrolar/Olustur
        // Bordro oluşturma sayfasının ilk halini gösterir.
        public async Task<IActionResult> Olustur()
        {
            // Sayfayı ilk açtığımızda personel listesini alıp View'a gönderiyoruz.
            var personeller = await _personelServisi.TumPersonelleriGetirAsync();
            return View(personeller);
        }

        // POST: Bordrolar/Olustur
        // Kullanıcının doldurduğu bordro listesini alıp kaydeder.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Olustur(List<Bordro> bordrolar)
        {
            await _bordroServisi.TopluBordroEkleAsync(bordrolar);
            // Şimdilik ana sayfaya yönlendirelim. Sonra bordro listesine yönlendiririz.
            return RedirectToAction("Index", "Home");
        }
    }
}