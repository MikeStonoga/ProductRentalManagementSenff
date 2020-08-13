using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PRM.Domain.Products;
using PRM.Domain.Products.Rents;
using PRM.Infrastructure.Persistence.EntityFrameworkCore;

namespace PRM.Infrastructure.Persistence.MySQL
{
    public class PrmDbContext : BaseDbContext<PrmDbContext>, ICurrentDbContext
    {
        public DbContext Context { get; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Rent> Rents { get; set; }
        
        public PrmDbContext(DbContextOptions<PrmDbContext> options) : base(options)
        {
            Context = this;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().HasMany<Rent>();
            modelBuilder.Entity<Product>().Property(p => p.Code).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Rent>()
                .HasOne<Product>()
                .WithMany(r => r.Rents)
                .HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<Rent>().Property(r => r.Code).ValueGeneratedOnAdd();
        }
    }
    
    public class PrmDbContextDesignTime : BaseDbContextDesignTime<PrmDbContext>
    {
        public PrmDbContextDesignTime() : base(Settings.ConnectionString,"prm")
        {
        }
    }
}