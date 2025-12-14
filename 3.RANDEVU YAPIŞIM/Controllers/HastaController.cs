using Microsoft.AspNetCore.Mvc;
using _3.RANDEVU_YAPISIM.Data;
using System;

namespace _3.RANDEVU_YAPISIM.Controllers
{
    public class HastaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HastaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Login kontrolü
        private int? GetHastaId() => HttpContext.Session.GetInt32("HastaId");

        private IActionResult? GirisYoksaAt()
        {
            if (GetHastaId() == null)
                return RedirectToAction("Login", "Account");
            return null;
        }

        // PROFİL (GET)
        [HttpGet]
        public IActionResult Profil()
        {
            var guard = GirisYoksaAt();
            if (guard != null) return guard;

            var hastaId = GetHastaId()!.Value;
            var hasta = _context.Hastalar.Find(hastaId);
            if (hasta == null) return RedirectToAction("Logout", "Account");

            return View(hasta);
        }

        // PROFİL (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profil(string? TcKimlik, string? Adres, DateTime? DogumTarihi)
        {
            var guard = GirisYoksaAt();
            if (guard != null) return guard;

            var hastaId = GetHastaId()!.Value;
            var hasta = _context.Hastalar.Find(hastaId);
            if (hasta == null) return RedirectToAction("Logout", "Account");

            hasta.TcKimlik = string.IsNullOrWhiteSpace(TcKimlik) ? null : TcKimlik.Trim();
            hasta.Adres = string.IsNullOrWhiteSpace(Adres) ? null : Adres.Trim();
            hasta.DogumTarihi = DogumTarihi;

            _context.SaveChanges();

            TempData["ProfilMesaji"] = "✅ Profil güncellendi.";
            return RedirectToAction("Profil");
        }
    }
}
