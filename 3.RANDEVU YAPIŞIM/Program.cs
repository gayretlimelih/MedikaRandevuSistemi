using System;
using System.Text.Json.Serialization;
using _3.RANDEVU_YAPISIM.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ MVC
builder.Services.AddControllersWithViews();

// ✅ API + JSON (DÖNGÜSEL REFERANS ÇÖZÜMÜ)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// ✅ Swagger (Sade + ProblemDetails gizli)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Başlık “Medika” görünsün:
    c.SwaggerDoc("v1", new()
    {
        Title = "Medika API",
        Version = "v1",
        Description = "Medika Randevu Sistemi – Doktor, Hasta ve Randevu servisleri"
    });

    // Swagger Schemas altında ProblemDetails görünmesin
    c.MapType<ProblemDetails>(() => null!);
});

// ✅ Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Medika.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// ✅ Cookie Auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ✅ Hata / güvenlik
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ✅ Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Medika API Docs";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Medika API v1");
        c.RoutePrefix = "swagger";
        c.DisplayRequestDuration();
    });
}

// ✅ API
app.MapControllers();

// ✅ MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
