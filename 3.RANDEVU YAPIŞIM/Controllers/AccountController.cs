using Microsoft.AspNetCore.Mvc;
using _3.RANDEVU_YAPISIM.Data;
using _3.RANDEVU_YAPISIM.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace _3.RANDEVU_YAPISIM.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 Giriş Sayfası (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 🔹 Giriş İşlemi (POST)
        [HttpPost]
        public IActionResult Login(string Email, string Sifre)
        {
            var hasta = _context.Hastalar.FirstOrDefault(x => x.Email == Email && x.Sifre == Sifre);

            if (hasta != null)
            {
                HttpContext.Session.SetInt32("HastaId", hasta.Id);
                HttpContext.Session.SetString("KullaniciAdi", hasta.AdSoyad);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Hata = "E-posta veya şifre hatalı!";
                return View();
            }
        }


        // 🔹 Kayıt Sayfası (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 🔹 Kayıt İşlemi (POST) — E-Posta Kontrolü Dahil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Hasta model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "⚠️ Lütfen tüm zorunlu alanları doldurun.";
                return View(model);
            }

            // Aynı e-posta zaten var mı kontrolü
            var mevcutHasta = _context.Hastalar.FirstOrDefault(x => x.Email == model.Email);

            if (mevcutHasta != null)
            {
                ViewBag.Message = "⚠️ Bu e-posta adresiyle zaten kayıtlı bir kullanıcı var!";
                return View(model);
            }

            // Yeni hasta kaydı
            _context.Hastalar.Add(model);
            _context.SaveChanges();

            TempData["KayitMesaji"] = "✅ Kayıt başarılı! Şimdi giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        // 🔹 Çıkış işlemi
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
