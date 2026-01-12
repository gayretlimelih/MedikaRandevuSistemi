using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using _3.RANDEVU_YAPISIM.Data;
using _3.RANDEVU_YAPISIM.Models;
using System.Linq;
using System.Threading.Tasks;

namespace _3.RANDEVU_YAPISIM.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= ADMIN KONTROL =================
        private bool IsAdmin()
        {
            var role = (HttpContext.Session.GetString("Role") ?? "")
                .Trim()
                .ToLower();

            return role == "admin";
        }

        private IActionResult? AdminGirisYoksaAt()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            return null;
        }

        // ================= ADMIN ANA SAYFA =================
        public IActionResult Index()
        {
            var guard = AdminGirisYoksaAt();
            if (guard != null) return guard;

            return View();
        }

        // ================= DOKTOR YÖNETİMİ =================
        public IActionResult Doktorlar()
        {
            var guard = AdminGirisYoksaAt();
            if (guard != null) return guard;

            var doktorlar = _context.Doktorlar
                .Where(d => !d.Arsivli)
                .OrderByDescending(d => d.Id)
                .ToList();

            return View(doktorlar);
        }

        public IActionResult DoktorArsiv()
        {
            var guard = AdminGirisYoksaAt();
            if (guard != null) return guard;

            var arsiv = _context.Doktorlar
                .Where(d => d.Arsivli)
                .OrderByDescending(d => d.Id)
                .ToList();

            return View(arsiv);
        }

        // ================= DOKTOR SİL (ARŞİVE AT) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DoktorSil(int id)
        {
            var guard = AdminGirisYoksaAt();
            if (guard != null) return guard;

            var doktor = _context.Doktorlar.FirstOrDefault(d => d.Id == id);
            if (doktor == null) return NotFound();

            doktor.Arsivli = true;
            _context.SaveChanges();

            return RedirectToAction("Doktorlar");
        }

        // ================= ARŞİVDEN GERİ AL =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DoktorGeriAl(int id)
        {
            var guard = AdminGirisYoksaAt();
            if (guard != null) return guard;

            var doktor = _context.Doktorlar.FirstOrDefault(d => d.Id == id);
            if (doktor == null) return NotFound();

            doktor.Arsivli = false;
            _context.SaveChanges();

            return RedirectToAction("DoktorArsiv");
        }

        // ================= HASTA YÖNETİMİ =================
        public IActionResult Hastalar()
        {
            var guard = AdminGirisYoksaAt();
            if (guard != null) return guard;

            var hastalar = _context.Hastalar
                .OrderByDescending(h => h.Id)
                .ToList();

            return View(hastalar);
        }

        // ================= RANDEVU YÖNETİMİ =================
        public async Task<IActionResult> Randevular()
        {
            var guard = AdminGirisYoksaAt();
            if (guard != null) return guard;

            var randevular = await _context.Randevular
                .Include(r => r.Doktor)
                .Include(r => r.Hasta)
                .OrderByDescending(r => r.Id)
                .Select(r => new AdminRandevuVM
                {
                    Id = r.Id,
                    DoktorAdSoyad = r.Doktor.AdSoyad ?? "",
                    Brans = r.Doktor.Brans ?? "",
                    HastaAdSoyad = r.Hasta.AdSoyad ?? "",
                    HastaEmail = r.Hasta.Email ?? "",
                    Tarih = r.Tarih,
                    Saat = r.Saat ?? "",
                    Durum = r.Durum ?? "Aktif"
                })
                .ToListAsync();

            return View(randevular);
        }
    }
}
