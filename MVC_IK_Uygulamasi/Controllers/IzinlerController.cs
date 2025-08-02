using Microsoft.AspNetCore.Mvc;
using MVC_IK_Uygulamasi.Services;
using System.Threading.Tasks;
using MVC_IK_Uygulamasi.Models; // Yeni ekledik
using Microsoft.AspNetCore.Mvc.Rendering; // SelectList için yeni ekledik

namespace MVC_IK_Uygulamasi.Controllers
{
    public class IzinlerController : Controller
    {
        private readonly IzinServisi _izinServisi;
        // Personel listesini çekmek için PersonelServisi'ne de ihtiyacımız var.
        private readonly PersonelServisi _personelServisi;

        public IzinlerController(IzinServisi izinServisi, PersonelServisi personelServisi)
        {
            _izinServisi = izinServisi;
            _personelServisi = personelServisi;
        }

        // GET: Izinler
        public async Task<IActionResult> Index()
        {
            var izinler = await _izinServisi.TumIzinleriGetirAsync();
            return View(izinler);
        }

        // GET: Izinler/Create
        // Boş izin talep formunu gösterir.
        public async Task<IActionResult> Create()
        {
            // Formdaki personel seçimi listesini doldurmak için tüm personelleri çekiyoruz.
            ViewBag.Personeller = new SelectList(await _personelServisi.TumPersonelleriGetirAsync(), "Id", "Ad");
            return View();
        }

        // POST: Izinler/Create
        // Dolu formdan gelen bilgileri alıp veritabanına kaydeder.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Izin yeniIzinTalebi)
        {
            // Otomatik olarak doldurulacak alanları ayarlıyoruz.
            yeniIzinTalebi.TalepTarihi = DateTime.Now;
            yeniIzinTalebi.OnayDurumu = "Beklemede";

            // ModelState.IsValid kontrolü, modeldeki kurallara uyulup uyulmadığını kontrol eder.
            // Biz henüz kural eklemediğimiz için bu kontrolü daha sonra detaylandıracağız.
            // Şimdilik temel haliyle devam edebiliriz.

            await _izinServisi.IzinTalebiEkleAsync(yeniIzinTalebi);
            return RedirectToAction(nameof(Index)); // Kayıttan sonra listeleme sayfasına yönlendir.
        }

        // Bu metodu Controller'ın içine, diğer metotların yanına ekle

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DurumGuncelle(int id, string durum)
        {
            await _izinServisi.IzinDurumGuncelleAsync(id, durum);
            return RedirectToAction(nameof(Index));
        }
    }
}