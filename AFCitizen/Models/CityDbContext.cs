using Microsoft.EntityFrameworkCore;

namespace AFCitizen.Models
{
    public class CityDbContext : DbContext
    {
        public CityDbContext(DbContextOptions<CityDbContext> options) : base(options) { }
        public DbSet<Block> Blocks { get; set; }
    }
}
