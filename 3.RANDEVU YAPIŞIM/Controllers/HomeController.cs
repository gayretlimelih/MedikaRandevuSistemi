using Microsoft.AspNetCore.Mvc;
using _3.RANDEVU_YAPISIM.Data;
using _3.RANDEVU_YAPISIM.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3.RANDEVU_YAPISIM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 Ana Sayfa
        public IActionResult Index()
        {
            return View();
        }

        // 🔹 Randevu alma sayfası (GET)
        [HttpGet]
        public IActionResult RandevuAl(DateTime? tarih, int? doktorId)
        {
            // 🔒 Giriş yapılmamışsa giriş sayfasına yönlendir
            var hastaId = HttpContext.Session.GetInt32("HastaId");
            if (hastaId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // ✅ TempData'daki mesaj ViewBag'e aktarılıyor (her yenilemede görünsün)
            if (TempData["Mesaj"] != null)
                ViewBag.Mesaj = TempData["Mesaj"].ToString();

            // ✅ Dolu saat listesi (sadece tarih ve doktor seçildiyse)
            var doluSaatler = new List<string>();
            if (tarih.HasValue && doktorId.HasValue)
            {
                doluSaatler = _context.Randevular
                    .Where(r => r.Tarih.Date == tarih.Value.Date && r.DoktorId == doktorId.Value)
                    .Select(r => r.Saat)
                    .ToList();
            }

            ViewBag.DoluSaatler = doluSaatler;
            return View();
        }

        // 🔹 AJAX ile branşa göre doktor getirir
        [HttpGet]
        public JsonResult GetDoktorlarByBrans(string brans)
        {
            if (string.IsNullOrEmpty(brans))
                return Json(new { success = false, message = "Branş belirtilmedi." });

            var doktorlar = _context.Doktorlar
                .Where(d => d.Brans == brans)
                .Select(d => new
                {
                    id = d.Id,
                    adSoyad = d.AdSoyad
                })
                .ToList();

            if (doktorlar.Count == 0)
                return Json(new { success = false, message = "Bu branşa ait doktor bulunamadı." });

            return Json(new { success = true, data = doktorlar });
        }

        // 🔹 Randevu kaydetme işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RandevuAl(Randevu model)
        {
            var hastaId = HttpContext.Session.GetInt32("HastaId");
            if (hastaId == null)
            {
                TempData["Mesaj"] = "⚠️ Lütfen önce giriş yapınız.";
                return RedirectToAction("Login", "Account");
            }

            model.HastaId = hastaId.Value;

            if (ModelState.IsValid)
            {
                // ✅ Aynı doktor, tarih ve saatte randevu var mı kontrol et
                bool saatDolu = _context.Randevular.Any(r =>
                    r.DoktorId == model.DoktorId &&
                    r.Tarih.Date == model.Tarih.Date &&
                    r.Saat == model.Saat
                );

                if (saatDolu)
                {
                    TempData["Mesaj"] = "⚠️ Bu saat zaten dolu, lütfen başka bir saat seçiniz.";
                }
                else
                {
                    // ✅ Yeni randevuyu kaydet
                    _context.Randevular.Add(model);
                    _context.SaveChanges();

                    TempData["Mesaj"] = "✅ Randevunuz başarıyla oluşturuldu!";
                }

                // ✅ Yeniden aynı sayfaya yönlendir (mesaj ve doluluk için)
                return RedirectToAction("RandevuAl", new { tarih = model.Tarih, doktorId = model.DoktorId });
            }

            // ✅ Model geçersizse dolu saatleri yeniden yükle
            ViewBag.DoluSaatler = _context.Randevular
                .Where(r => r.Tarih.Date == model.Tarih.Date && r.DoktorId == model.DoktorId)
                .Select(r => r.Saat)
                .ToList();

            return View(model);
        }
    }
}
