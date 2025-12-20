using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonuYonetim.Models
{
    public enum RandevuDurum
    {
        Bekliyor,
        Onaylandi,
        Rededildi
    }

    public class Randevu
    {
        [Key]
        public int Id { get; set; }
        public DateTime RandevuTarihi { get; set; }
        public TimeSpan RandevuSaati { get; set; }

        public int HizmetId { get; set; }
        [ForeignKey(nameof(HizmetId))]
        public Hizmet Hizmet { get; set; } = null!;

        public int AntrenorId { get; set; }
        [ForeignKey(nameof(AntrenorId))]
        public Antrenor Antrenor { get; set; } = null!;

        public string? MusteriAdi { get; set; }
        public string? MusteriTelefon { get; set; }

        public bool Onay { get; set; }
        public RandevuDurum Durum { get; set; } = RandevuDurum.Bekliyor;
    }
}