using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using _3.RANDEVU_YAPISIM.Models;

namespace _3.RANDEVU_YAPISIM.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Hasta> Hastalar { get; set; }
        public DbSet<Doktor> Doktorlar { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
    }
}
