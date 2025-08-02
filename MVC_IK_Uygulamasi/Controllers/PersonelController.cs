using Microsoft.AspNetCore.Authorization; // Yetkilendirme için eklendi
using Microsoft.AspNetCore.Mvc;
using MVC_IK_Uygulamasi.Models;
using MVC_IK_Uygulamasi.Services;
using System.Threading.Tasks;

namespace MVC_IK_Uygulamasi.Controllers
{
    [Authorize(Roles = "Admin")] // SADECE "Admin" ROLÜNDEKİLER GİREBİLİR
    public class PersonelController : Controller
    {
        private readonly PersonelServisi _personelServisi;

        public PersonelController(PersonelServisi personelServisi)
        {
            _personelServisi = personelServisi;
        }

        // GET: /Personel
        public async Task<IActionResult> Index()
        {
            var personelListesi = await _personelServisi.TumPersonelleriGetirAsync();
            return View(personelListesi);
        }

        // GET: /Personel/Create
        // Boş personel ekleme formunu gösterir.
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Personel/Create
        // Dolu formdan gelen bilgileri alıp veritabanına kaydeder.
        [HttpPost]
        [ValidateAntiForgeryToken] // Güvenlik için önemli bir ekleme
        public async Task<IActionResult> Create(Personel yeniPersonel)
        {
            if (ModelState.IsValid) // Formun doğru bir şekilde doldurulduğunu kontrol eder
            {
                await _personelServisi.PersonelEkleAsync(yeniPersonel);
                return RedirectToAction(nameof(Index)); // Kayıttan sonra listeleme sayfasına yönlendir.
            }
            return View(yeniPersonel); // Eğer formda hata varsa, kullanıcıya hatalarla birlikte formu geri göster.
        }

        // Bu iki metodu Controller'ın içine, diğer metotların yanına ekle

        // GET: /Personel/Delete/5
        // Id'si 5 olan personelin silme onay sayfasını gösterir.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Eğer Id gelmemişse "Bulunamadı" hatası ver.
            }

            var personel = await _personelServisi.PersonelBulAsync(id.Value);
            if (personel == null)
            {
                return NotFound(); // Eğer o Id'de bir personel yoksa "Bulunamadı" hatası ver.
            }

            return View(personel); // Personel bilgilerini onay sayfasına gönder.
        }

        // POST: /Personel/Delete/5
        // Onay sayfasından gelen isteği işler ve personeli siler.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _personelServisi.PersonelSilAsync(id);
            return RedirectToAction(nameof(Index)); // Silme işleminden sonra listeleme sayfasına yönlendir.
        }

        // Bu iki metodu Controller'ın içine, diğer metotların yanına ekle

        // GET: /Personel/Edit/5
        // Düzenlenecek personelin bilgilerini bulup formda gösterir.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personel = await _personelServisi.PersonelBulAsync(id.Value);
            if (personel == null)
            {
                return NotFound();
            }
            return View(personel); // Personel bilgilerini düzenleme formuna gönder.
        }

        // POST: /Personel/Edit/5
        // Formdan gelen güncel bilgileri alıp veritabanına kaydeder.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Personel personel)
        {
            if (id != personel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _personelServisi.PersonelGuncelleAsync(personel);
                return RedirectToAction(nameof(Index)); // İşlemden sonra listeye yönlendir.
            }
            return View(personel); // Hata varsa formu geri göster.
        }

        // Bu metodu Controller'ın içine, diğer metotların yanına ekle

        // GET: /Personel/Details/5
        // Id'si 5 olan personelin detaylarını gösterir.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // PersonelServisi'nden personeli bulmasını istiyoruz.
            // İleride izinleri de göstermek için .Include() ekleyebiliriz ama şimdilik bu yeterli.
            var personel = await _personelServisi.PersonelBulAsync(id.Value);

            if (personel == null)
            {
                return NotFound();
            }

            return View(personel); // Personel bilgilerini detay sayfasına gönder.
        }
    }
}