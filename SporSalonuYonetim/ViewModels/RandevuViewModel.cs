using SporSalonuYonetim.Models;

namespace SporSalonuYonetim.ViewModels
{
    public class RandevuViewModel
    {
        public DateTime RandevuTarihi { get; set; }
        public TimeSpan RandevuSaati { get; set; }
        public int HizmetId { get; set; }
        public int AntrenorId { get; set; }

        // Dropdown (açılır liste) içini doldurmak için gerekli listeler
        public List<Hizmet>? Hizmetler { get; set; }
        public List<Antrenor>? Antrenorler { get; set; }
    }
}