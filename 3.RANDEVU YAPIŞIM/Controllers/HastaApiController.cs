using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _3.RANDEVU_YAPISIM.Data;
using _3.RANDEVU_YAPISIM.Models;

namespace _3.RANDEVU_YAPISIM.Controllers
{
    /// <summary>
    /// Hasta işlemleri API endpointleri
    /// </summary>
    [ApiController]
    [Route("api/hastalar")]
    public class HastaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public HastaApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Tüm hastaları listeler
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var hastalar = await _db.Hastalar.ToListAsync();
            return Ok(hastalar);
        }

        /// <summary>
        /// Yeni hasta kaydı oluşturur
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] Hasta hasta)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Hastalar.Add(hasta);
            await _db.SaveChangesAsync();

            return Ok(hasta);
        }
    }
}
