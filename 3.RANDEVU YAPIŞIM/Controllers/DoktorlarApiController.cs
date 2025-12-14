using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _3.RANDEVU_YAPISIM.Data;

namespace _3.RANDEVU_YAPISIM.Controllers
{
    /// <summary>
    /// Doktor işlemleri API endpoint'leri
    /// </summary>
    [ApiController]
    [Route("api/doktorlar")]
    public class DoktorlarApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public DoktorlarApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Tüm doktorları listeler
        /// </summary>
        /// <returns>Doktor listesi</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var doktorlar = await _db.Doktorlar.ToListAsync();
            return Ok(doktorlar);
        }
    }
}
