using Microsoft.EntityFrameworkCore;

namespace RestfulApi.Models
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> customers { get; set; }
    }
}