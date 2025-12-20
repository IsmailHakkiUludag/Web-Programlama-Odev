using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SporSalonuYonetim.Models;

var builder = WebApplication.CreateBuilder(args);

// Veritabani Baglantisi
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(connectionString));

// --- KRÝTÝK DÜZELTME BURASI ---
// Rol  yerine IdentityRole kullanýyoruz.
builder.Services.AddIdentity<Kullanici, IdentityRole>(options =>
{
    // şifre Zorluk Ayarlari
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;

    // Giriş Ayarlari
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<IdentityContext>()
.AddDefaultTokenProviders();
// ------------------------------

// MVC Servisleri
builder.Services.AddControllersWithViews();

var app = builder.Build();

// HTTP Request Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik Doðrulama (Giriş)
app.UseAuthorization();  // Yetkilendirme (Rol)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
