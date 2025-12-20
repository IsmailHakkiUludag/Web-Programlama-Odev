using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporSalonuYonetim.Models;
using SporSalonuYonetim.ViewModels;

namespace SporSalonuYonetim.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Kullanici> _userManager;
        private readonly SignInManager<Kullanici> _signInManager;

        
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<Kullanici> userManager,
                                 SignInManager<Kullanici> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GİRİŞ YAP SAYFASI (GET METHOD)
        public IActionResult Login()
        {
            return View();
        }

        // GİRİŞ YAP İŞLEMİ (POST METHOD)
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Bu e-posta ile kayıtlı kullanıcı bulunamadı.");
                return View(model);
            }

            // Şifre kontrolü ve giriş kısmı
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "E-posta veya şifre hatalı.");
            return View(model);
        }

        // KAYIT OL SAYFASI (GET METHOD)
        public IActionResult Register()
        {
            return View();
        }

        // KAYIT OL İŞLEMİ (POST METHOD) - ROL ATAMA BURADA YAPILIYOR
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new Kullanici
            {
                UserName = model.Email, // Kullanıcı adı olarak E-posta kullanıyoruz.
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // --- OTOMATİK ROL ATAMA SİSTEMİ ---
                // 1. Eğer sistemde "uye" rolü hiç yoksa (ilk kez çalışıyorsa), oluştur.
                if (!await _roleManager.RoleExistsAsync("uye"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("uye"));
                }

                // 2. Yeni kayıt olan kullanıcıya "uye" rolünü ver.
                await _userManager.AddToRoleAsync(user, "uye");
                // ----------------------------------

                // Kayıt bitince otomatik giriş yap.
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // Eğer hata varsa (Şifre basitse vs.) hataları ekrana yaz.
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // ÇIKIŞ YAP
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // YETKİSİZ GİRİŞ SAYFASI
        public IActionResult AccessDenied()
        {
            return View();
        }
    }

}
