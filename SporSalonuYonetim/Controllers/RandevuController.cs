using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetim.Models;
using SporSalonuYonetim.ViewModels;

namespace SporSalonuYonetim.Controllers
{
    [Authorize]
    public class RandevuController : Controller
    {
        private readonly UserManager<Kullanici> _userManager;
        private readonly IdentityContext _context;

        public RandevuController(UserManager<Kullanici> userManager, IdentityContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Randevu Al Sayfası
        [HttpGet]
        public IActionResult RandevuAl(int? hizmetId)
        {
            var hizmetler = _context.Hizmetler.ToList();

            var antrenorQuery = _context.Antrenorler
                .Include(a => a.Hizmetler)
                .AsQueryable();

            if (hizmetId.HasValue && hizmetId.Value > 0)
            {
                antrenorQuery = antrenorQuery.Where(a => a.Hizmetler.Any(h => h.Id == hizmetId.Value));
            }

            var viewModel = new RandevuViewModel
            {
                Hizmetler = hizmetler,
                Antrenorler = antrenorQuery.ToList(),
                RandevuTarihi = DateTime.Today,
                HizmetId = hizmetId ?? 0
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RandevuAl(RandevuViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var secilenHizmet = await _context.Hizmetler.FindAsync(model.HizmetId);
            if (secilenHizmet == null)
            {
                ModelState.AddModelError("", "Hizmet seçimi geçersiz.");
                return YenidenYukle(model);
            }

            // Yeni randevunun zaman aralığını hesapla
            TimeSpan yeniBaslangic = model.RandevuSaati;
            TimeSpan yeniBitis = model.RandevuSaati.Add(TimeSpan.FromMinutes(secilenHizmet.Sure));

            // -----------------------------------------------------------------------
            // 1. KONTROL: ANTRENÖR MESAİSİ VE ÇAKIŞMASI
            // -----------------------------------------------------------------------

            // A) Mesai Kontrolü
            var secilenAntrenor = await _context.Antrenorler.FindAsync(model.AntrenorId);
            if (secilenAntrenor != null)
            {
                if (yeniBaslangic < secilenAntrenor.BaslangicSaati || yeniBitis > secilenAntrenor.BitisSaati)
                {
                    ModelState.AddModelError("", $"Hizmet süresi ({secilenHizmet.Sure} dk) mesai saatlerini aşıyor. ({secilenAntrenor.BitisSaati:hh\\:mm} kapanış)");
                    return YenidenYukle(model);
                }
            }

            // B) Antrenör Doluluk Kontrolü
            var antrenorRandevulari = await _context.Randevular
                .Include(r => r.Hizmet)
                .Where(r => r.AntrenorId == model.AntrenorId && r.RandevuTarihi.Date == model.RandevuTarihi.Date)
                .ToListAsync();

            foreach (var randevu in antrenorRandevulari)
            {
                TimeSpan mevcutBaslangic = randevu.RandevuSaati;
                TimeSpan mevcutBitis = randevu.RandevuSaati.Add(TimeSpan.FromMinutes(randevu.Hizmet.Sure));

                if (yeniBaslangic < mevcutBitis && yeniBitis > mevcutBaslangic)
                {
                    ModelState.AddModelError("", $"Seçilen saatlerde antrenör dolu. ({randevu.RandevuSaati:hh\\:mm} - {mevcutBitis:hh\\:mm} arası dolu)");
                    return YenidenYukle(model);
                }
            }

            // -----------------------------------------------------------------------
            // 2. KONTROL (YENİ): MÜŞTERİNİN KENDİ ÇAKIŞMASI
            // Müşteri aynı saatte başka bir derste mi?
            // -----------------------------------------------------------------------

            var musteriRandevulari = await _context.Randevular
                .Include(r => r.Hizmet)
                .Where(r => r.MusteriAdi == user.FullName && r.RandevuTarihi.Date == model.RandevuTarihi.Date)
                .ToListAsync();

            foreach (var randevu in musteriRandevulari)
            {
                TimeSpan mevcutBaslangic = randevu.RandevuSaati;
                TimeSpan mevcutBitis = randevu.RandevuSaati.Add(TimeSpan.FromMinutes(randevu.Hizmet.Sure));

                // Çakışma Mantığı: (Yeni Başlangıç < Eski Bitiş) VE (Yeni Bitiş > Eski Başlangıç)
                if (yeniBaslangic < mevcutBitis && yeniBitis > mevcutBaslangic)
                {
                    ModelState.AddModelError("", $"Dikkat! Bu saat aralığında ({randevu.RandevuSaati:hh\\:mm} - {mevcutBitis:hh\\:mm}) zaten '{randevu.Hizmet.Ad}' randevunuz var.");
                    return YenidenYukle(model);
                }
            }

            // -----------------------------------------------------------------------

            // Her şey temizse kaydet
            var yeniRandevu = new Randevu
            {
                RandevuTarihi = model.RandevuTarihi,
                RandevuSaati = model.RandevuSaati,
                HizmetId = model.HizmetId,
                AntrenorId = model.AntrenorId,
                MusteriAdi = user.FullName,
                MusteriTelefon = user.PhoneNumber ?? "Belirtilmemiş",
                Durum = RandevuDurum.Bekliyor
            };

            _context.Randevular.Add(yeniRandevu);
            await _context.SaveChangesAsync();
            return RedirectToAction("Randevularim");
        }

        private IActionResult YenidenYukle(RandevuViewModel model)
        {
            model.Hizmetler = _context.Hizmetler.ToList();
            model.Antrenorler = _context.Antrenorler.Include(a => a.Hizmetler).ToList();
            return View(model);
        }

        public async Task<IActionResult> Randevularim()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var randevular = await _context.Randevular
                .Include(r => r.Hizmet)
                .Include(r => r.Antrenor)
                .Where(r => r.MusteriAdi == user.FullName)
                .OrderByDescending(r => r.RandevuTarihi)
                .ToListAsync();

            return View(randevular);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu != null)
            {
                _context.Randevular.Remove(randevu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Randevularim");
        }
    }
}