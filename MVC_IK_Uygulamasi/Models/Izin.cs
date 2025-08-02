using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_IK_Uygulamasi.Models
{
    public class Izin
    {
        public int Id { get; set; }

        [Display(Name = "Personel")]
        [ForeignKey("Personel")]
        public int PersonelId { get; set; }
        public Personel Personel { get; set; }

        [Display(Name = "İzin Türü")]
        public string IzinTuru { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        [DataType(DataType.Date)]
        public DateTime BaslangicTarihi { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime BitisTarihi { get; set; }

        [Display(Name = "Talep Tarihi")]
        [DataType(DataType.Date)]
        public DateTime TalepTarihi { get; set; }

        [Display(Name = "Onay Durumu")]
        public string OnayDurumu { get; set; }
    }
}