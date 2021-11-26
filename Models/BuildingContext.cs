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
        public DbSet<Battery> batteries{ get; set; }
        public DbSet<Column> columns{ get; set; }
        public DbSet<Elevator> elevators{ get; set; }
    }
}