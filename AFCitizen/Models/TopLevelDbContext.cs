using Microsoft.EntityFrameworkCore;

namespace AFCitizen.Models
{
    public class TopLevelDbContext : DbContext
    {
        public TopLevelDbContext(DbContextOptions<TopLevelDbContext> options) : base(options) { }
        public DbSet<Block> Blocks { get; set; }
    }
}
