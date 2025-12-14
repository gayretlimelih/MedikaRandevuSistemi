using Microsoft.AspNetCore.Mvc;
using _3.RANDEVU_YAPISIM.Data;
using System.Linq;

namespace _3.RANDEVU_YAPISIM.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Admin kontrol helper
        private bool IsAdmin()
        {
            var role = (HttpContext.Session.GetString("Role") ?? "").Trim().ToLower();
            return role == "admin";
        }

        // ✅ Admin değilse Login’e at
        private IActionResult? AdminGirisYoksaAt()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            return null;
        }

        // 🔹 Admin Ana Sayfa
        public IActionResult Index()
        {
            var guard = AdminGirisYoksaAt();
            if (guard != null) return guard;

            return View();
        }

        // 🔹 Doktor Yönetimi
        public IActionResult Doktorlar()
        {
            var guard = AdminGirisYoksaAt();
            if (guard != null) return guard;

            var doktorlar = _context.Doktorlar.OrderByDescending(d => d.Id).ToList();
            return View(doktorlar);
        }

        // 🔹 Hasta Yönetimi (Aktif + Pasif hepsi)
        public IActionResult Hastalar()
        {
            var guard = AdminGirisYoksaAt();
            if (guard != null) return guard;

            var hastalar = _context.Hastalar.OrderByDescending(h => h.Id).ToList();
            return View(hastalar);
        }

        // ✅ Hasta Durum Değiştir (Aktif/Pasif)
        [HttpPost]
        public IActionResult HastaDurumDegistir(int id, bool aktifMi)
        {
            var guard = AdminGirisYoksaAt();
            if (guard != null) return guard;

            var hasta = _context.Hastalar.FirstOrDefault(x => x.Id == id);
            if (hasta == null) return NotFound();

            hasta.Aktif = aktifMi;
            _context.SaveChanges();

            return RedirectToAction("Hastalar");
        }

        // 🔹 Randevu Yönetimi
        public IActionResult Randevular()
        {
            var guard = AdminGirisYoksaAt();
            if (guard != null) return guard;

            var liste = (from r in _context.Randevular
                         join d in _context.Doktorlar on r.DoktorId equals d.Id
                         join h in _context.Hastalar on r.HastaId equals h.Id
                         orderby r.Id descending
                         select new _3.RANDEVU_YAPISIM.Models.AdminRandevuVM
                         {
                             Id = r.Id,
                             DoktorAdSoyad = d.AdSoyad,
                             Brans = d.Brans,
                             HastaAdSoyad = h.AdSoyad,
                             HastaEmail = h.Email,
                             Tarih = r.Tarih,
                             Saat = r.Saat,
                             Durum = r.Durum
                         }).ToList();

            return View(liste);
        }
    }
}
