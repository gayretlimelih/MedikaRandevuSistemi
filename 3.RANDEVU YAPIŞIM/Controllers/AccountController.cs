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

        // ✅ Giriş Sayfası
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ✅ Giriş İşlemi
        [HttpPost]
        public IActionResult Login(string Email, string Sifre)
        {
            // Kullanıcı var mı?
            var hasta = _context.Hastalar.FirstOrDefault(x => x.Email == Email && x.Sifre == Sifre);

            if (hasta == null)
            {
                ViewBag.Hata = "E-posta veya şifre hatalı!";
                return View();
            }

            // ✅ Role (normalize)
            var roleLower = (hasta.Role ?? "Hasta").Trim().ToLower();

            // ✅ PASİF KULLANICI GİREMEZ (ADMIN HARİÇ)
            if (!hasta.Aktif && roleLower != "admin")
            {
                ViewBag.Hata = "Hesabınız pasife alınmıştır. Lütfen yönetici ile iletişime geçin.";
                return View();
            }

            // ✅ Session
            HttpContext.Session.SetInt32("HastaId", hasta.Id);
            HttpContext.Session.SetString("KullaniciAdi", hasta.AdSoyad);
            HttpContext.Session.SetString("Role", (hasta.Role ?? "Hasta").Trim());

            // ✅ Admin giriş yaptıysa admin paneline
            if (roleLower == "admin")
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Home");
        }

        // ✅ Kayıt Sayfası
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // ✅ Kayıt İşlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Hasta model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "⚠️ Lütfen tüm zorunlu alanları doldurun.";
                return View(model);
            }

            // ✅ E-Posta kontrol
            var mevcutHasta = _context.Hastalar.FirstOrDefault(x => x.Email == model.Email);
            if (mevcutHasta != null)
            {
                ViewBag.Message = "⚠️ Bu e-posta adresiyle zaten kayıtlı bir kullanıcı var!";
                return View(model);
            }

            // ✅ Varsayılan rol ve durum (PRO)
            model.Role = "Hasta";
            model.Aktif = true;

            _context.Hastalar.Add(model);
            _context.SaveChanges();

            TempData["KayitMesaji"] = "✅ Kayıt başarılı! Şimdi giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        // ✅ Çıkış
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
