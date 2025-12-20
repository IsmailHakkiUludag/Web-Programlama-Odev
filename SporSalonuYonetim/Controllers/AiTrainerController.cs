using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SporSalonuYonetim.Services;

namespace SporSalonuYonetim.Controllers
{
    [Authorize] // Sadece üyeler girebilsin.
    public class AiTrainerController : Controller
    {
        private readonly GeminiService _geminiService;
        private readonly ImageAiService _imageAiService;

        public AiTrainerController()
        {
            _geminiService = new GeminiService();
            _imageAiService = new ImageAiService();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Olustur(string Cinsiyet, int Yas, double Kilo, double Boy, string Hedef)
        {
            // İki yapay zekayı aynı anda işe koşuyoruz (Parallel Task)
            // Böylece toplam bekleme süresi azalır.
            var programTask = _geminiService.ProgramOlustur(Cinsiyet, Yas, Kilo, Boy, Hedef);
            var resimTask = _imageAiService.ResimCiz(Cinsiyet, Hedef);

            await Task.WhenAll(programTask, resimTask);

            string program = programTask.Result;
            string resimUrl = resimTask.Result;

            // Eğer yoğunluktan dolayı resim çizilemediyse, boş kalmasın diye stok foto gösterelim.
            if (string.IsNullOrEmpty(resimUrl))
            {
                resimUrl = Cinsiyet == "Kadın"
                    ? "[https://images.unsplash.com/photo-1518611012118-696072aa579a?w=600&q=80](https://images.unsplash.com/photo-1518611012118-696072aa579a?w=600&q=80)"
                    : "[https://images.unsplash.com/photo-1581009146145-b5ef050c2e1e?w=600&q=80](https://images.unsplash.com/photo-1581009146145-b5ef050c2e1e?w=600&q=80)";
            }

            ViewBag.Program = program;
            ViewBag.ResimUrl = resimUrl;
            ViewBag.Hedef = Hedef;

            return View("Sonuc");
        }
    }

}
