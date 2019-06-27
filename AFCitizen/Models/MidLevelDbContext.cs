using Microsoft.EntityFrameworkCore;

namespace AFCitizen.Models
{
    public class MidLevelDbContext : DbContext, ILevelDbContext
    {
        public MidLevelDbContext(DbContextOptions<MidLevelDbContext> options) : base(options) { }
        public DbSet<Block> Blocks { get; set; }
    }
}
