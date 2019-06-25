using Microsoft.EntityFrameworkCore;

namespace AFCitizen.Models
{
    public class SubjectDbContext : DbContext
    {
        public SubjectDbContext(DbContextOptions<SubjectDbContext> options) : base(options) { }
        public DbSet<Block> Blocks { get; set; }
    }
}
