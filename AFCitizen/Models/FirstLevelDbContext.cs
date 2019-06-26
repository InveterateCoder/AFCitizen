using Microsoft.EntityFrameworkCore;

namespace AFCitizen.Models
{
    public class FirstLevelDbContext : DbContext
    {
        public FirstLevelDbContext(DbContextOptions<FirstLevelDbContext> options) : base(options) { }
        public DbSet<Block> Blocks { get; set; }
    }
}
