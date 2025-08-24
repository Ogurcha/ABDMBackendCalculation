using Microsoft.EntityFrameworkCore;

namespace Abdm.Calculation.DAL
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
           : base(options)
        {
        }
    }
}
