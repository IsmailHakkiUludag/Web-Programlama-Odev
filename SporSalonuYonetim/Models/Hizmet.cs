using System.ComponentModel.DataAnnotations;

namespace SporSalonuYonetim.Models
{
    public class Hizmet
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Hizmet Adı")]
        public string? Ad { get; set; }

        [Display(Name = "Süre (Dakika)")]
        public int Sure { get; set; }

        [Display(Name = "Ücret (₺)")]
        public decimal Ucret { get; set; }

        public int SalonId { get; set; }
        public Salon Salon { get; set; } = null!;

        // Çoka-Çok İlişki: Bu hizmeti veren antrenörler.
        public ICollection<Antrenor> Antrenorler { get; set; } = new List<Antrenor>();

        // YENİ EKLENEN: Bu hatayı çözer (Randevu Geçmişi)
        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }

}
