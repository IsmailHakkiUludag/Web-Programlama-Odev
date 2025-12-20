using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetim.Models;

namespace SporSalonuYonetim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HizmetApiController : ControllerBase
    {
        private readonly IdentityContext _context;

        public HizmetApiController(IdentityContext context)
        {
            _context = context;
        }

        // Tüm hizmetleri JSON olarak döndürür
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hizmet>>> GetHizmetler()
        {
            return await _context.Hizmetler.Include(h => h.Salon).ToListAsync();
        }

        // ID'ye göre hizmet getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<Hizmet>> GetHizmet(int id)
        {
            var hizmet = await _context.Hizmetler.FindAsync(id);
            if (hizmet == null) return NotFound();
            return hizmet;
        }

        // Ödev gereksinimi: API üzerinden Antrenörleri filtreleme/listeleme
        [HttpGet("antrenorler")]
        public async Task<ActionResult<IEnumerable<Antrenor>>> GetAntrenorler()
        {
            return await _context.Antrenorler.ToListAsync();
        }
    }
}