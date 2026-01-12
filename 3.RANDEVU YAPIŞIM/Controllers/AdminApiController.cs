using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using _3.RANDEVU_YAPISIM.Data;
using _3.RANDEVU_YAPISIM.Models;
using System.Linq;

namespace _3.RANDEVU_YAPISIM.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            var role = (HttpContext.Session.GetString("Role") ?? "").Trim().ToLower();
            return role == "admin";
        }

        // ===================== DOKTOR DTO =====================
        public class DoktorEkleDto
        {
            public string AdSoyad { get; set; } = "";
            public string Brans { get; set; } = "";
        }

        // ===================== DOKTOR EKLE =====================
        // POST: /api/admin/doktorlar
        [HttpPost("doktorlar")]
        public IActionResult DoktorEkle([FromBody] DoktorEkleDto dto)
        {
            if (!IsAdmin()) return Unauthorized("Yetkisiz");

            var adSoyad = (dto.AdSoyad ?? "").Trim();
            var brans = (dto.Brans ?? "").Trim();

            if (string.IsNullOrWhiteSpace(adSoyad) || string.IsNullOrWhiteSpace(brans))
                return BadRequest("Ad Soyad ve Branş boş olamaz.");

            // Aynı ad+branş (aktiflerde) varsa engelle
            var varMi = _context.Doktorlar.Any(d =>
                !d.Arsivli &&
                d.AdSoyad.ToLower() == adSoyad.ToLower() &&
                d.Brans.ToLower() == brans.ToLower());

            if (varMi)
                return BadRequest("Bu doktor zaten mevcut (aynı Ad Soyad + Branş).");

            var doktor = new Doktor
            {
                AdSoyad = adSoyad,
                Brans = brans,
                Arsivli = false
            };

            _context.Doktorlar.Add(doktor);
            _context.SaveChanges();

            return Ok(new { id = doktor.Id });
        }

        // ===================== DOKTOR SİL (ARŞİVE AT) =====================
        // DELETE: /api/admin/doktorlar/5
        [HttpDelete("doktorlar/{id:int}")]
        public IActionResult DoktorSil(int id)
        {
            if (!IsAdmin()) return Unauthorized("Yetkisiz");

            var doktor = _context.Doktorlar.FirstOrDefault(d => d.Id == id);
            if (doktor == null) return NotFound("Doktor bulunamadı.");

            doktor.Arsivli = true;
            _context.SaveChanges();

            return Ok();
        }

        // ===================== RANDEVU İPTAL =====================
        [HttpPost("randevular/{id:int}/iptal")]
        public IActionResult RandevuIptal(int id)
        {
            if (!IsAdmin()) return Unauthorized("Yetkisiz");

            var r = _context.Randevular.FirstOrDefault(x => x.Id == id);
            if (r == null) return NotFound("Randevu bulunamadı");

            r.Durum = "İptal";
            _context.SaveChanges();
            return Ok();
        }

        // ===================== RANDEVU AKTİF =====================
        [HttpPost("randevular/{id:int}/aktif")]
        public IActionResult RandevuAktif(int id)
        {
            if (!IsAdmin()) return Unauthorized("Yetkisiz");

            var r = _context.Randevular.FirstOrDefault(x => x.Id == id);
            if (r == null) return NotFound("Randevu bulunamadı");

            r.Durum = "Aktif";
            _context.SaveChanges();
            return Ok();
        }

        // ===================== HASTA PASİF =====================
        [HttpPost("hastalar/{id:int}/pasif")]
        public IActionResult HastaPasif(int id)
        {
            if (!IsAdmin()) return Unauthorized("Yetkisiz");

            var h = _context.Hastalar.FirstOrDefault(x => x.Id == id);
            if (h == null) return NotFound("Hasta bulunamadı");

            h.Aktif = false;
            _context.SaveChanges();
            return Ok();
        }

        // ===================== HASTA AKTİF =====================
        [HttpPost("hastalar/{id:int}/aktif")]
        public IActionResult HastaAktif(int id)
        {
            if (!IsAdmin()) return Unauthorized("Yetkisiz");

            var h = _context.Hastalar.FirstOrDefault(x => x.Id == id);
            if (h == null) return NotFound("Hasta bulunamadı");

            h.Aktif = true;
            _context.SaveChanges();
            return Ok();
        }
    }
}
