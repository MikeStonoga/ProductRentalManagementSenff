using Microsoft.EntityFrameworkCore;
using PRM.Domain.Products;
using PRM.Domain.Products.Rents;


namespace PRM.Infrastructure.Persistence.MySQL.EntityFrameworkCore
{
    public class PrmDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Rent> Rents { get; set; }
        
        public PrmDbContext(DbContextOptions<PrmDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasMany<Rent>();
            
            modelBuilder.Entity<Rent>()
                .HasOne<Product>()
                .WithMany(r => r.Rents)
                .HasForeignKey(p => p.ProductId);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}