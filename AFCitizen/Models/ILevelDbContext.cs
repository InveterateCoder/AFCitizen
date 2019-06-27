using Microsoft.EntityFrameworkCore;

namespace AFCitizen.Models
{
    public interface ILevelDbContext
    {
        DbSet<Block> Blocks { get; set; }
    }
}
