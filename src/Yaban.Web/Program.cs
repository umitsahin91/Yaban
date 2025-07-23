using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yaban.Web.Application;
using Yaban.Web.Domain.Entities;
using Yaban.Web.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();

// Add services to the container.
builder.Services.AddControllersWithViews();

//==================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequireDigit = false;
    // Diğer şifre kuralları...
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// COOKIE AYARLARI
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// AUTHENTICATION VE AUTHORIZATION'I AKTİF ETME
// Bu iki satır UseRouting'den sonra ve MapControllerRoute'dan önce olmalı.
app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


// Veritabanını başlangıç verileriyle tohumlamak için bir alan (scope) oluşturuyoruz.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

        logger.LogInformation("Veritabanı tohumlama işlemi başlıyor.");

        // 1. "Admin" ROLÜNÜ OLUŞTURMA
        //---------------------------------
        // "Admin" adında bir rol var mı diye kontrol et
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            // Eğer yoksa, yeni bir "Admin" rolü oluştur.
            await roleManager.CreateAsync(new AppRole { Name = "Admin" });
            logger.LogInformation("Admin rolü oluşturuldu.");
        }

        // 2. İLK ADMİN KULLANICISINI OLUŞTURMA
        //---------------------------------
        // Belirtilen e-posta adresine sahip bir kullanıcı var mı diye kontrol et
        if (await userManager.FindByEmailAsync("admin@yaban.com") == null)
        {
            // Eğer yoksa, yeni bir admin kullanıcısı oluştur.
            var adminUser = new AppUser
            {
                UserName = "admin@yaban.com",
                Email = "admin@yaban.com",
                EmailConfirmed = true // E-posta doğrulaması gerektirmemesi için true yapıyoruz.
            };

            // Kullanıcıyı belirtilen şifreyle oluştur.
            // DİKKAT: Bu şifreyi canlı ortamda çok daha güçlü bir şeyle değiştirin!
            var result = await userManager.CreateAsync(adminUser, "Sifre123!");

            if (result.Succeeded)
            {
                logger.LogInformation("Admin kullanıcısı oluşturuldu.");
                // Eğer kullanıcı başarıyla oluşturulduysa, onu "Admin" rolüne ata.
                await userManager.AddToRoleAsync(adminUser, "Admin");
                logger.LogInformation("Admin kullanıcısı Admin rolüne atandı.");
            }
        }
    }
    catch (Exception ex)
    {
        // Tohumlama sırasında bir hata olursa logla.
        logger.LogError(ex, "Veritabanı tohumlama sırasında bir hata oluştu.");
    }
}

app.Run();
