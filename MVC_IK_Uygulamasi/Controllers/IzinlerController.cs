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

        // GET: /Izinler/Create
        public async Task<IActionResult> Create()
        {
            // Adminler bu sayfaya erişememeli, talep oluşturamamalı.
            if (User.IsInRole("Admin"))
            {
                return Forbid();
            }

            // Artık tüm personelleri göndermeye gerek yok.
            // Sadece view'ı döndürüyoruz.
            return View(new Izin());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Izin yeniIzinTalebi)
        {
            if (User.IsInRole("Admin"))
            {
                return Forbid();
            }

            // 1. Giriş yapmış kullanıcının ID'sini al.
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // Kullanıcı bulunamadıysa yetkisiz hatası ver.
            }

            // 2. Bu UserId ile ilişkili personeli veritabanından bul.
            var personel = await _personelServisi.PersonelBulByUserIdAsync(userId); // Bu metodu servise ekleyeceğiz.
            if (personel == null)
            {
                // Eğer kullanıcının bir personel kaydı yoksa hata göster.
                ModelState.AddModelError(string.Empty, "Sistemde adınıza kayıtlı bir personel bulunamadı. Lütfen yöneticinizle iletişime geçin.");
                return View(yeniIzinTalebi);
            }

            // 3. Formdan gelen PersonelId'yi yok sayıp, bulunan personelin Id'sini ata.
            yeniIzinTalebi.PersonelId = personel.Id;
            yeniIzinTalebi.TalepTarihi = DateTime.Now;
            yeniIzinTalebi.OnayDurumu = "Beklemede";

            // ModelState.Remove("Personel"); // "Personel" nesnesiyle ilgili doğrulama hatalarını yok say
            if (ModelState.IsValid)
            {
                await _izinServisi.IzinTalebiEkleAsync(yeniIzinTalebi);
                return RedirectToAction(nameof(Index));
            }

            return View(yeniIzinTalebi);
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