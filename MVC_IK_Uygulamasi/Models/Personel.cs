using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MVC_IK_Uygulamasi.Models;

namespace MVC_IK_Uygulamasi.Models
{
    public class Personel
    {
        public int Id { get; set; }

        [Display(Name = "Sicil No")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Sicil no 6 haneli olmalıdır.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Sicil no 6 haneli olmalıdır.")]
        public string? SicilNo { get; set; }

        [Display(Name = "T.C. Kimlik No")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "T.C. Kimlik No 11 haneli olmalıdır.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "T.C. Kimlik No 11 haneli olmalıdır.")]
        public string? TCKimlikNo { get; set; }

        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Departman { get; set; }
        public string Pozisyon { get; set; }

        [Display(Name = "Brüt Maaş")]
        public decimal BrutMaas { get; set; }

        [Display(Name = "İşe Giriş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime IseGirisTarihi { get; set; }

        [Display(Name = "E-posta Adresi")]
        [EmailAddress]
        public string Eposta { get; set; }

        [Display(Name = "Telefon Numarası")]
        public string? TelefonNumarasi { get; set; }

        [NotMapped]
        public bool BugunIzinliMi { get; set; } = false;

        // YENİ İLİŞKİ ÖZELLİĞİ:
        // Bir personelin birden çok izni olabileceğini belirtir.
        public virtual ICollection<Izin> Izinler { get; set; }
        public Personel()
        {
            // Izinler koleksiyonunu burada başlatıyoruz.
            // Böylece null olma ihtimalini ortadan kaldırıyoruz.
            Izinler = new List<Izin>();
        }
        public string? UserId { get; set; }

    }
}