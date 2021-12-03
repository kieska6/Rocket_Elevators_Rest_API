using Microsoft.EntityFrameworkCore;

namespace RestfulApi.Models
{
    public class InterventionContext : DbContext
    {
        public InterventionContext(DbContextOptions<InterventionContext> options)
            : base(options)
        {
        }

        public DbSet<Intervention> interventions { get; set; }
    }
}