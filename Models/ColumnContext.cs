using Microsoft.EntityFrameworkCore;

namespace RestfulApi.Models
{
    public class ColumnContext : DbContext
    {
        public ColumnContext(DbContextOptions<ColumnContext> options)
            : base(options)
        {
        }

        public DbSet<Column> columns { get; set; }
    }
}