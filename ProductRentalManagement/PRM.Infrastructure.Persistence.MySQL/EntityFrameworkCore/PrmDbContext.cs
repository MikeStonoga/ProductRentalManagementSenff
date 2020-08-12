using Microsoft.EntityFrameworkCore;
using PRM.Domain.Products;


namespace PRM.Infrastructure.Persistence.MySQL.EntityFrameworkCore
{
    public class PrmDbContext : DbContext
    {
        /*public DbSet<Product> Products { get; set; }*/

        public PrmDbContext(DbContextOptions<PrmDbContext> options) : base(options)
        {
            
        }
    }
}