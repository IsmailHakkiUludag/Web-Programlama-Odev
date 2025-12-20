using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonuYonetim.Models
{
    public class Antrenor
    {
        [Key]
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }

        [Display(Name = "Ad Soyad")]
        public string AdSoyad => $"{Ad} {Soyad}";

        public string? Telefon { get; set; }

        // ESKİ "UzmanlikAlani" string'i kalktı.
        // YENİ: Antrenörün verebildiği hizmetler listesi
        public ICollection<Hizmet> Hizmetler { get; set; } = new List<Hizmet>();

        [Display(Name = "Mesai Başlangıç")]
        [DataType(DataType.Time)]
        public TimeSpan BaslangicSaati { get; set; }

        [Display(Name = "Mesai Bitiş")]
        [DataType(DataType.Time)]
        public TimeSpan BitisSaati { get; set; }

        public int SalonId { get; set; }
        [ForeignKey(nameof(SalonId))]
        public Salon Salon { get; set; } = null!;

        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }
}