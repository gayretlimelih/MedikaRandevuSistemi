using System;
using Microsoft.EntityFrameworkCore;
using _3.RANDEVU_YAPISIM.Data;
using Microsoft.AspNetCore.Authentication.Cookies; // 🔹 Kimlik doğrulama için eklendi

var builder = WebApplication.CreateBuilder(args);

// 📦 Veritabanı bağlantısı
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 📄 MVC servisi
builder.Services.AddControllersWithViews();

// 💾 Session (oturum) ayarları
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Medika.Session"; // Cookie ismi
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Oturum süresi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS güvenliği
});

// 🔐 Cookie Authentication (giriş yapan kullanıcıyı hatırlamak için)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";   // Giriş sayfası
        options.LogoutPath = "/Account/Logout"; // Çıkış sayfası
        options.AccessDeniedPath = "/Account/Login"; // Yetkisiz erişimde yönlendirme
    });

// HttpContext erişimi (kullanıcı bilgisine ulaşmak için)
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// 🚨 Hata yönetimi ve güvenlik
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 🌐 Temel middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🧠 Oturum başlatma
app.UseSession();

// 🔑 Authentication & Authorization (sırası çok önemli!)
app.UseAuthentication();
app.UseAuthorization();

// 🗺️ Varsayılan yönlendirme
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 🚀 Uygulamayı çalıştır
app.Run();
