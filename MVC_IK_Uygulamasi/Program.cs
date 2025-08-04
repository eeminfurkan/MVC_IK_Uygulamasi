using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_IK_Uygulamasi.Data;
using MVC_IK_Uygulamasi.Services;
using MVC_IK_Uygulamasi.Data.Seed;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// PersonelServisi'ni kaydediyoruz. BU SATIR KALIYOR.
builder.Services.AddScoped<PersonelServisi>();
// IzinServisi'ni kaydediyoruz.
builder.Services.AddScoped<IzinServisi>(); // BU SATIRI EKLE
// ...
builder.Services.AddScoped<BordroServisi>(); // BU SATIRI EKLE


// --- EKLENTÝLERÝN SONU ---

// Identity sistemini, bizim düzenlediðimiz UygulamaDbContext'i kullanacak þekilde ayarlýyoruz.
// YENÝ HALÝ:
// YENÝSÝ:
// YENÝ VE DOÐRU HALÝ:
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UygulamaDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();


// ... app.MapRazorPages(); ...

// YENÝ EKLEDÝÐÝMÝZ BÖLÜM
// Uygulama çalýþmaya baþlamadan önce rolleri ve admin'i tohumluyoruz.
await AppSeeder.Seeder(app);


// Uygulama her baþladýðýnda bekleyen Migration'larý otomatik olarak uygula.
// Bu, "Update-Database" komutunu koddan çalýþtýrmak gibidir.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UygulamaDbContext>();
    await dbContext.Database.MigrateAsync();
}
// --- KODUN SONU ---

app.Run();
