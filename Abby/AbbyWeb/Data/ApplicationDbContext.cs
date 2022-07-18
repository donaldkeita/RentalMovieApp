using AbbyWeb.Model;
using Microsoft.EntityFrameworkCore;

namespace AbbyWeb.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Category> Category { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        
    }
}
