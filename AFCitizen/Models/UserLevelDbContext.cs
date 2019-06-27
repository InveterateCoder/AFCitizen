using Microsoft.EntityFrameworkCore;

namespace AFCitizen.Models
{
    public class UserLevelDbContext : DbContext, ILevelDbContext
    {
        public UserLevelDbContext(DbContextOptions<UserLevelDbContext> options) : base(options) { }
        public DbSet<Block> Blocks { get; set; }
    }
}
