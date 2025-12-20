using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetim.Models;

namespace SporSalonuYonetim.Controllers
{
    public class HizmetController : Controller
    {
        private readonly IdentityContext _context;

        public HizmetController(IdentityContext context)
        {
            _context = context;
        }

        // LİSTELEME
        public async Task<IActionResult> Index()
        {
            var hizmetler = await _context.Hizmetler.Include(h => h.Salon).ToListAsync();
            return View(hizmetler);
        }

        // EKLEME SAYFASI (GET) - SORUN BURADA ÇÖZÜLÜYOR
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            // Salonları çekip Dropdown için hazırlıyoruz
            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad");
            return View();
        }

        // EKLEME İŞLEMİ (POST)
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hizmet hizmet)
        {
            // Validasyon hatası olsa bile salon verisi kaybolmasın diye siliyoruz (Opsiyonel)
            ModelState.Remove("Salon");
            ModelState.Remove("Antrenorler");
            ModelState.Remove("Randevular");

            if (ModelState.IsValid)
            {
                _context.Add(hizmet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa sayfayı yenilerken listeyi tekrar doldur
            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", hizmet.SalonId);
            return View(hizmet);
        }

        // DÜZENLEME SAYFASI (GET)
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var hizmet = await _context.Hizmetler.FindAsync(id);
            if (hizmet == null) return NotFound();

            // Mevcut salon seçili gelsin diye son parametreye hizmet.SalonId ekledik
            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", hizmet.SalonId);
            return View(hizmet);
        }

        // DÜZENLEME İŞLEMİ (POST)
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hizmet hizmet)
        {
            if (id != hizmet.Id) return NotFound();

            ModelState.Remove("Salon");
            ModelState.Remove("Antrenorler");
            ModelState.Remove("Randevular");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hizmet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Hizmetler.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", hizmet.SalonId);
            return View(hizmet);
        }

        // SİLME SAYFASI (GET)
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var hizmet = await _context.Hizmetler
                .Include(h => h.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hizmet == null) return NotFound();

            return View(hizmet);
        }

        // SİLME ONAYI (POST)
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hizmet = await _context.Hizmetler.FindAsync(id);
            if (hizmet != null)
            {
                _context.Hizmetler.Remove(hizmet);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}