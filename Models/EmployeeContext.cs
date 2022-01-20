using Microsoft.EntityFrameworkCore;

namespace RestfulApi.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> employees { get; set; }
    }
}