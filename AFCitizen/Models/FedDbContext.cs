using Microsoft.EntityFrameworkCore;

namespace AFCitizen.Models
{
    public class FedDbContext : DbContext
    {
        public FedDbContext(DbContextOptions<FedDbContext> options) : base(options) { }
        public DbSet<Block> Blocks { get; set; }
    }
}
