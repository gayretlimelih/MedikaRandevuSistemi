using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _3.RANDEVU_YAPISIM.Data;
using _3.RANDEVU_YAPISIM.Models;

namespace _3.RANDEVU_YAPISIM.Controllers
{
    /// <summary>
    /// Randevu işlemleri API endpointleri
    /// </summary>
    [ApiController]
    [Route("api/randevular")]
    public class RandevuApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public RandevuApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Tüm randevuları listeler (Doktor ve Hasta bilgileriyle)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var randevular = await _db.Randevular
                .Include(r => r.Doktor)
                .Include(r => r.Hasta)
                .ToListAsync();

            return Ok(randevular);
        }

        /// <summary>
        /// Yeni randevu oluşturur
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] Randevu randevu)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Randevular.Add(randevu);
            await _db.SaveChangesAsync();

            return Ok(randevu);
        }
    }
}
