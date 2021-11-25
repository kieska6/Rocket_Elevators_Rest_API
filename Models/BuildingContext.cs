using Microsoft.EntityFrameworkCore;

namespace RestfulApi.Models
{
    public class BuildingContext : DbContext
    {
        public BuildingContext(DbContextOptions<BuildingContext> options)
            : base(options)
        {
        }

        public DbSet<Building> buildings { get; set; }
    }
}