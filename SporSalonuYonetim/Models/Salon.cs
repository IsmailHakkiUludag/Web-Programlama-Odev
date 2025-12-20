using System.ComponentModel.DataAnnotations;

namespace SporSalonuYonetim.Models
{
    public class Salon
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Salon Adı")]
        public string? Ad { get; set; }
        public string? Adres { get; set; }
        public string? TelefonNumarasi { get; set; }
        public string? Image { get; set; } // Resim yolu

        // Saat Ayarları
        [Display(Name = "Açılış Saati")]
        [DataType(DataType.Time)]
        public TimeSpan AcilisSaati { get; set; }

        [Display(Name = "Kapanış Saati")]
        [DataType(DataType.Time)]
        public TimeSpan KapanisSaati { get; set; }

        public ICollection<Hizmet> Hizmetler { get; set; } = new List<Hizmet>();
    }
}