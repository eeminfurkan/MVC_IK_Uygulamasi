using Microsoft.AspNetCore.Authorization; // Bu using ifadesini ekle
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_IK_Uygulamasi.Models;
using MVC_IK_Uygulamasi.Services;
using System.Security.Claims; // Bu using ifadesini ekle
using System.Threading.Tasks;

namespace MVC_IK_Uygulamasi.Controllers
{
    [Authorize] // BU ÖNEMLİ: Bu controller'daki tüm sayfalara erişmek için en azından GİRİŞ YAPMAK zorunlu.
    public class IzinlerController : Controller
    {
        private readonly IzinServisi _izinServisi;
        private readonly PersonelServisi _personelServisi;

        public IzinlerController(IzinServisi izinServisi, PersonelServisi personelServisi)
        {
            _izinServisi = izinServisi;
            _personelServisi = personelServisi;
        }

        public async Task<IActionResult> Index()
        {
            // Bu metodu daha önce yazmıştık, şimdi çalışacak.
            var izinler = new List<Izin>();
            if (User.IsInRole("Admin"))
            {
                izinler = await _izinServisi.TumIzinleriGetirAsync();
            }
            else
            {
                var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                izinler = await _izinServisi.PersoneleAitIzinleriGetirAsync(kullaniciId);
            }
            return View(izinler);
        }

        public async Task<IActionResult> Create()
        {
            // Normal bir personel, sadece kendisi için izin talep edebilmeli.
            // Admin ise herkes için talep oluşturabilmeli. Şimdilik basit tutalım.
            ViewBag.Personeller = new SelectList(await _personelServisi.TumPersonelleriGetirAsync(), "Id", "Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Izin yeniIzinTalebi)
        {
            yeniIzinTalebi.TalepTarihi = DateTime.Now;
            yeniIzinTalebi.OnayDurumu = "Beklemede";
            await _izinServisi.IzinTalebiEkleAsync(yeniIzinTalebi);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // SADECE Adminler bu metodu çalıştırabilir.
        public async Task<IActionResult> DurumGuncelle(int id, string durum)
        {
            await _izinServisi.IzinDurumGuncelleAsync(id, durum);
            return RedirectToAction(nameof(Index));
        }
    }
}