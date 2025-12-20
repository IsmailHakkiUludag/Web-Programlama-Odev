using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetim.Models;

namespace SporSalonuYonetim.Controllers
{
    public class AntrenorController : Controller
    {
        private readonly IdentityContext _context;

        public AntrenorController(IdentityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var antrenorler = await _context.Antrenorler
                .Include(x => x.Salon)
                .Include(x => x.Hizmetler) // İlişkili hizmetleri çek
                .ToListAsync();
            return View(antrenorler);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Salonlar = new SelectList(await _context.Salonlar.ToListAsync(), "Id", "Ad");
            ViewBag.Hizmetler = await _context.Hizmetler.ToListAsync();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Antrenor model, int[] selectedHizmetler)
        {
            ModelState.Remove("Salon");
            ModelState.Remove("Hizmetler");

            var secilenSalon = await _context.Salonlar.FindAsync(model.SalonId);
            if (secilenSalon != null)
            {
                if (model.BaslangicSaati < secilenSalon.AcilisSaati)
                    ModelState.AddModelError("BaslangicSaati", $"Salon {secilenSalon.AcilisSaati:hh\\:mm}'da açılıyor.");
                if (model.BitisSaati > secilenSalon.KapanisSaati)
                    ModelState.AddModelError("BitisSaati", $"Salon {secilenSalon.KapanisSaati:hh\\:mm}'da kapanıyor.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Salonlar = new SelectList(await _context.Salonlar.ToListAsync(), "Id", "Ad", model.SalonId);
                ViewBag.Hizmetler = await _context.Hizmetler.ToListAsync();
                return View(model);
            }

            // HİZMETLERİ EKLE
            if (selectedHizmetler != null)
            {
                foreach (var hizmetId in selectedHizmetler)
                {
                    var hizmet = await _context.Hizmetler.FindAsync(hizmetId);
                    if (hizmet != null) model.Hizmetler.Add(hizmet);
                }
            }

            _context.Antrenorler.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var antrenor = await _context.Antrenorler
                .Include(a => a.Hizmetler) // Mevcut hizmetlerini getir ki seçili gözüksün
                .FirstOrDefaultAsync(a => a.Id == id);

            if (antrenor == null) return NotFound();

            ViewBag.Salonlar = new SelectList(await _context.Salonlar.ToListAsync(), "Id", "Ad", antrenor.SalonId);
            ViewBag.Hizmetler = await _context.Hizmetler.ToListAsync();

            return View(antrenor);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, Antrenor model, int[] selectedHizmetler)
        {
            if (id != model.Id) return NotFound();

            ModelState.Remove("Salon");
            ModelState.Remove("Hizmetler");

            // Veritabanındaki asıl kaydı hizmetleriyle birlikte çekiyoruz
            var dbAntrenor = await _context.Antrenorler
                .Include(a => a.Hizmetler)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (dbAntrenor == null) return NotFound();

            // Saat Kontrolü
            var secilenSalon = await _context.Salonlar.FindAsync(model.SalonId);
            if (secilenSalon != null)
            {
                if (model.BaslangicSaati < secilenSalon.AcilisSaati)
                    ModelState.AddModelError("BaslangicSaati", "Mesai salon açılışından önce olamaz.");
                if (model.BitisSaati > secilenSalon.KapanisSaati)
                    ModelState.AddModelError("BitisSaati", "Mesai salon kapanışından sonra olamaz.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Salonlar = new SelectList(await _context.Salonlar.ToListAsync(), "Id", "Ad", model.SalonId);
                ViewBag.Hizmetler = await _context.Hizmetler.ToListAsync();
                return View(model);
            }

            // 1. Bilgileri Güncelle
            dbAntrenor.Ad = model.Ad;
            dbAntrenor.Soyad = model.Soyad;
            dbAntrenor.Telefon = model.Telefon;
            dbAntrenor.BaslangicSaati = model.BaslangicSaati;
            dbAntrenor.BitisSaati = model.BitisSaati;
            dbAntrenor.SalonId = model.SalonId;

            // 2. Hizmet İlişkilerini Güncelle (En Kritik Kısım)
            dbAntrenor.Hizmetler.Clear(); // Önce eskileri temizle
            if (selectedHizmetler != null)
            {
                foreach (var hizmetId in selectedHizmetler)
                {
                    var hizmet = await _context.Hizmetler.FindAsync(hizmetId);
                    if (hizmet != null) dbAntrenor.Hizmetler.Add(hizmet);
                }
            }

            _context.Update(dbAntrenor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var antrenor = await _context.Antrenorler
                .Include(x => x.Salon)
                .Include(x => x.Hizmetler)
                .FirstOrDefaultAsync(x => x.Id == id);
            return View(antrenor);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var antrenor = await _context.Antrenorler.FindAsync(id);
            if (antrenor != null)
            {
                _context.Antrenorler.Remove(antrenor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}