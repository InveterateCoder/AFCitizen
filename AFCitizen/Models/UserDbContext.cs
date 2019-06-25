using Microsoft.EntityFrameworkCore;

namespace AFCitizen.Models
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        public DbSet<Block> Blocks { get; set; }
    }
}
