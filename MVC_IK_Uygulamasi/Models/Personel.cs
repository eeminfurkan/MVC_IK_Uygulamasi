using System.ComponentModel.DataAnnotations; // Bu satırı ekliyoruz

namespace MVC_IK_Uygulamasi.Models
{
    public class Personel
    {
        public int Id { get; set; }
        
        [Display(Name = "Sicil No")] // BU SATIRI EKLE
        public string? SicilNo { get; set; } // BU SATIRI EKLE

        [Display(Name = "T.C. Kimlik No")]
        public string? TCKimlikNo { get; set; }

        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Departman { get; set; }
        public string Pozisyon { get; set; }

        [Display(Name = "Maaş")]
        public decimal Maas { get; set; }

        [Display(Name = "İşe Giriş Tarihi")]
        [DataType(DataType.Date)] // Bu, formlarda sadece tarih seçilmesini sağlar
        public DateTime IseGirisTarihi { get; set; }

        [Display(Name = "E-posta Adresi")]
        [EmailAddress] // Bu, e-posta formatı doğrulaması yapar
        public string Eposta { get; set; }

        [Display(Name = "Telefon Numarası")]
        public string? TelefonNumarasi { get; set; }


    }
}