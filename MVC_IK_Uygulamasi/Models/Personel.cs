using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_IK_Uygulamasi.Models
{
    public class Personel
    {
        public int Id { get; set; }

        [Display(Name = "Sicil No")]
        public string? SicilNo { get; set; }

        [Display(Name = "T.C. Kimlik No")]
        public string? TCKimlikNo { get; set; }

        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Departman { get; set; }
        public string Pozisyon { get; set; }

        [Display(Name = "Maaş")]
        public decimal Maas { get; set; }

        [Display(Name = "İşe Giriş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime IseGirisTarihi { get; set; }

        [Display(Name = "E-posta Adresi")]
        [EmailAddress]
        public string Eposta { get; set; }

        [Display(Name = "Telefon Numarası")]
        public string? TelefonNumarasi { get; set; }

        // Bu alan, bir personelin o gün izinli olup olmadığını
        // arayüzde belirtmek için kullanılacak bir bayraktır.
        [NotMapped]
        public bool BugunIzinliMi { get; set; } = false;

        // YENİ İLİŞKİ ÖZELLİĞİ:
        // Bir personelin birden çok izni olabileceğini belirtir.
        public virtual ICollection<Izin> Izinler { get; set; }
    }
}