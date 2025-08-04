using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_IK_Uygulamasi.Models
{
    public class Bordro
    {
        public int Id { get; set; }

        [ForeignKey("Personel")]
        public int PersonelId { get; set; }
        public Personel Personel { get; set; }

        [Display(Name = "Dönem (Ay/Yıl)")]
        public string Donem { get; set; }

        [Column(TypeName = "decimal(18, 2)")] // Veritabanında para birimi için uygun format
        [Display(Name = "Brüt Maaş")]
        public decimal BrutMaas { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Ek Ödemeler Toplamı")]
        public decimal EkOdemeler { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Toplam Kesintiler")]
        public decimal ToplamKesintiler { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Net Maaş")]
        public decimal NetMaas { get; set; }
    }
}