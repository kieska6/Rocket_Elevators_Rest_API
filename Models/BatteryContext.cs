using Microsoft.EntityFrameworkCore;

namespace RestfulApi.Models
{
    public class BatteryContext : DbContext
    {
        public BatteryContext(DbContextOptions<BatteryContext> options)
            : base(options)
        {
        }

        public DbSet<Battery> batteries { get; set; }
    }
}