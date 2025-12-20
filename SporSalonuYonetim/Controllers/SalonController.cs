using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetim.Models;

namespace SporSalonuYonetim.Controllers
{
    public class SalonController : Controller
    {
        private readonly IdentityContext _context;

        public SalonController(IdentityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var salonlar = await _context.Salonlar.ToListAsync();
            return View(salonlar);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Salon model, IFormFile? imageFile)
        {
            if (imageFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                model.Image = "/img/" + fileName;
            }

            _context.Salonlar.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var salon = await _context.Salonlar.FindAsync(id);
            if (salon == null) return NotFound();
            return View(salon);
        }

        // --- DÜZELTİLEN KISIM BURASI ---
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, Salon model, IFormFile? imageFile)
        {
            var salon = await _context.Salonlar.FindAsync(id);
            if (salon == null) return NotFound();

            salon.Ad = model.Ad;
            salon.Adres = model.Adres;
            salon.TelefonNumarasi = model.TelefonNumarasi;

            // ESKİ HATA VEREN KOD: salon.CalismaSaatleri = model.CalismaSaatleri;
            // YENİ DOĞRU KOD:
            salon.AcilisSaati = model.AcilisSaati;
            salon.KapanisSaati = model.KapanisSaati;

            if (imageFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                salon.Image = "/img/" + fileName;
            }

            _context.Salonlar.Update(salon);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // --------------------------------

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var salon = await _context.Salonlar.FindAsync(id);
            if (salon != null)
            {
                _context.Salonlar.Remove(salon);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}