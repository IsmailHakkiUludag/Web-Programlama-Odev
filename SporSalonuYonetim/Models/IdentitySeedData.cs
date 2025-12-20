using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SporSalonuYonetim.Models
{
    public static class IdentitySeedData
    {
        // Ödevde istenen sabit şifre
        private const string adminPassword = "sau";

        // Öğrenci e posta adresi
        private const string adminEmail = "g231210049@sakarya.edu.tr";

        public static async Task IdentityTestUser(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();

            // Veritabanı yoksa oluştur (Migrationları uygula)
            if (context.Database.GetAppliedMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Kullanici>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Rol>>();

            // 1. Admin Rolü var mı? Yoksa oluştur.
            var roleExist = await roleManager.RoleExistsAsync("admin");
            if (!roleExist)
            {
                await roleManager.CreateAsync(new Rol { Name = "admin" });
            }

            // 2. Üye Rolü var mı? Yoksa oluştur.
            var memberRoleExist = await roleManager.RoleExistsAsync("uye");
            if (!memberRoleExist)
            {
                await roleManager.CreateAsync(new Rol { Name = "uye" });
            }

            // 3. Admin kullanıcısı var mı?
            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                user = new Kullanici
                {
                    FullName = "Admin Öğrenci",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                }
            }
        }
    }
}