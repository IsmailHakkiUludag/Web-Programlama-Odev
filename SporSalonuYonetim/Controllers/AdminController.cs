using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetim.Models;
using SporSalonuYonetim.ViewModels;

namespace SporSalonuYonetim.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IdentityContext _context;
        private readonly UserManager<Kullanici> _userManager;

        public AdminController(IdentityContext context, UserManager<Kullanici> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ==========================================
        // DASHBOARD (ANA SAYFA)
        // ==========================================
        public async Task<IActionResult> Index()
        {
            // DÜZELTME 1: Liste yerine sayısını (.Count) alıyoruz.
            var uyeListesi = await _userManager.GetUsersInRoleAsync("uye");
            ViewBag.ToplamUye = uyeListesi.Count;

            ViewBag.ToplamAntrenor = await _context.Antrenorler.CountAsync();
            ViewBag.ToplamRandevu = await _context.Randevular.CountAsync();

            // Toplam Ciro (Sadece Onaylı Randevular)
            var ciro = await _context.Randevular
                .Include(r => r.Hizmet)
                .Where(r => r.Durum == RandevuDurum.Onaylandi)
                .SumAsync(r => r.Hizmet.Ucret);

            ViewBag.ToplamKazanc = ciro;

            return View();
        }

        // ==========================================
        // 1. SAYFA: ANTRENÖR BAZLI KAZANÇ RAPORU
        // ==========================================
        public IActionResult Kazanc()
        {
            // Toplam Ciro
            var toplamCiro = _context.Randevular
                .Include(r => r.Hizmet)
                .Where(r => r.Durum == RandevuDurum.Onaylandi)
                .Sum(r => r.Hizmet.Ucret);

            ViewBag.ToplamCiro = toplamCiro;

            // Antrenör Raporu
            var antrenorBazliKazanc = _context.Randevular
                .Include(r => r.Hizmet)
                .Include(r => r.Antrenor)
                .Where(r => r.Durum == RandevuDurum.Onaylandi)
                .AsEnumerable()
                .GroupBy(r => r.Antrenor)
                .Select(g => new
                {
                    AntrenorAdi = g.Key.AdSoyad,
                    ToplamTutar = g.Sum(r => r.Hizmet.Ucret),
                    RandevuSayisi = g.Count()
                })
                .OrderByDescending(x => x.ToplamTutar)
                .ToList();

            ViewBag.AntrenorRaporu = antrenorBazliKazanc;
            return View();
        }

        // ==========================================
        // 2. SAYFA: GÜNLÜK KAZANÇ RAPORU
        // ==========================================
        public IActionResult GunlukKazanc()
        {
            var gunlukRapor = _context.Randevular
                .Include(r => r.Hizmet)
                .Where(r => r.Durum == RandevuDurum.Onaylandi)
                .AsEnumerable()
                .GroupBy(r => r.RandevuTarihi.Date)
                .Select(g => new
                {
                    Tarih = g.Key,
                    GunlukCiro = g.Sum(r => r.Hizmet.Ucret),
                    RandevuAdedi = g.Count()
                })
                .OrderByDescending(x => x.Tarih)
                .ToList();

            ViewBag.GunlukRapor = gunlukRapor;
            return View();
        }

        // ==========================================
        // RANDEVU YÖNETİMİ
        // ==========================================
        public async Task<IActionResult> Randevular(DateTime? tarih)
        {
            // DÜZELTME 2: Burada 'Include(r => r.Kullanici)' satırı hataya sebep oluyordu, kaldırıldı.
            // Artık senin orijinal kodundaki gibi çalışacak.
            var randevular = _context.Randevular
                .Include(r => r.Hizmet)
                .Include(r => r.Antrenor)
                .AsQueryable();

            if (tarih.HasValue)
            {
                randevular = randevular.Where(r => r.RandevuTarihi.Date == tarih.Value.Date);
                ViewBag.SeciliTarih = tarih.Value;
            }
            else
            {
                randevular = randevular.Where(r => r.RandevuTarihi.Date == DateTime.Today);
                ViewBag.SeciliTarih = DateTime.Today;
            }

            return View(await randevular.OrderBy(r => r.RandevuSaati).ToListAsync());
        }

        public async Task<IActionResult> Onayla(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu != null)
            {
                randevu.Durum = RandevuDurum.Onaylandi;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Randevular");
        }

        public async Task<IActionResult> RedEt(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu != null)
            {
                randevu.Durum = RandevuDurum.Rededildi;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Randevular");
        }
    }
}