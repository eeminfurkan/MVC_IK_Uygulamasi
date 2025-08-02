using Microsoft.AspNetCore.Identity;

namespace MVC_IK_Uygulamasi.Data.Seed
{
    public static class AppSeeder
    {
        public static async Task Seeder(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                // Roller için RoleManager'ı alıyoruz
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // "Admin" rolü yoksa oluşturuyoruz
                if (!await roleManager.RoleExistsAsync("Admin"))
                    await roleManager.CreateAsync(new IdentityRole("Admin"));

                // "Personel" rolü yoksa oluşturuyoruz
                if (!await roleManager.RoleExistsAsync("Personel"))
                    await roleManager.CreateAsync(new IdentityRole("Personel"));

                // Kullanıcılar için UserManager'ı alıyoruz
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                string adminEmail = "admin@ik.com";

                // Bu e-posta ile bir kullanıcı var mı diye bakıyoruz
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    // Eğer yoksa, yeni bir kullanıcı oluşturuyoruz
                    var yeniAdminUser = new IdentityUser()
                    {
                        UserName = "adminuser",
                        Email = adminEmail,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(yeniAdminUser, "IkMODULU01*"); // Şifreyi de basitleştirelim: "admin"
                    if (!result.Succeeded)
                    {
                        throw new Exception("Admin kullanıcısı oluşturulurken hata oluştu: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                    
                    result = await userManager.AddToRoleAsync(yeniAdminUser, "Admin");
                    if (!result.Succeeded)
                    {
                        throw new Exception("Admin kullanıcısı role atanırken hata oluştu: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }
    }
}