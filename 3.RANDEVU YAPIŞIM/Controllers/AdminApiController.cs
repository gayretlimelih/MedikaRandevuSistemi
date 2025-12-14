using Microsoft.AspNetCore.Mvc;
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

        // =========================
        // DOKTOR İŞLEMLERİ
        // =========================

        // ✅ Doktorları listele
        [HttpGet("doktorlar")]
        public IActionResult GetDoktorlar()
        {
            var doktorlar = _context.Doktorlar
                .OrderByDescending(x => x.Id)
                .ToList();

            return Ok(doktorlar);
        }

        // ✅ Doktor ekle
        // Swagger body:
        // { "adSoyad": "Dr. X Y", "brans": "Kardiyoloji" }
        [HttpPost("doktorlar")]
        public IActionResult AddDoktor([FromBody] Doktor doktor)
        {
            if (doktor == null)
                return BadRequest("Doktor verisi boş.");

            if (string.IsNullOrWhiteSpace(doktor.AdSoyad) ||
                string.IsNullOrWhiteSpace(doktor.Brans))
                return BadRequest("Ad Soyad ve Branş zorunludur.");

            doktor.Id = 0;
            doktor.AdSoyad = doktor.AdSoyad.Trim();
            doktor.Brans = doktor.Brans.Trim();

            _context.Doktorlar.Add(doktor);
            _context.SaveChanges();

            return Ok(doktor);
        }

        // ✅ Doktor sil
        [HttpDelete("doktorlar/{id}")]
        public IActionResult DeleteDoktor(int id)
        {
            var doktor = _context.Doktorlar.Find(id);
            if (doktor == null)
                return NotFound("Doktor bulunamadı.");

            _context.Doktorlar.Remove(doktor);
            _context.SaveChanges();

            return Ok("Doktor silindi.");
        }

        // =========================
        // HASTA İŞLEMLERİ
        // =========================

        // ✅ Hastaları listele (Aktif/Pasif dahil)
        [HttpGet("hastalar")]
        public IActionResult GetHastalar()
        {
            var hastalar = _context.Hastalar
                .OrderByDescending(x => x.Id)
                .Select(h => new
                {
                    h.Id,
                    h.AdSoyad,
                    h.Email,
                    h.Role,
                    h.Aktif
                })
                .ToList();

            return Ok(hastalar);
        }

        // ✅ Hasta Aktif / Pasif yap (Swagger’da GÖRÜNÜR)
        // PUT /api/admin/hastalar/5/durum
        // Body: true | false
        [HttpPut("hastalar/{id}/durum")]
        public IActionResult SetHastaDurum(int id, [FromBody] bool aktifMi)
        {
            var hasta = _context.Hastalar.Find(id);
            if (hasta == null)
                return NotFound("Hasta bulunamadı.");

            hasta.Aktif = aktifMi;
            _context.SaveChanges();

            return Ok(new
            {
                message = "Hasta durumu güncellendi.",
                hasta.Id,
                hasta.Aktif
            });
        }

        // =========================
        // RANDEVU İŞLEMLERİ
        // =========================

        // ✅ Randevuları listele
        [HttpGet("randevular")]
        public IActionResult GetRandevular()
        {
            var randevular = _context.Randevular
                .OrderByDescending(x => x.Id)
                .ToList();

            return Ok(randevular);
        }

        // ✅ Randevu iptal
        [HttpPost("randevular/{id}/iptal")]
        public IActionResult IptalRandevu(int id)
        {
            var r = _context.Randevular.Find(id);
            if (r == null)
                return NotFound("Randevu bulunamadı.");

            r.Durum = "İptal";
            _context.SaveChanges();

            return Ok("Randevu iptal edildi.");
        }
    }
}
