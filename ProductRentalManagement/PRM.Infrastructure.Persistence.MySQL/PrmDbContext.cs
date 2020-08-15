using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PRM.Domain.Products;
using PRM.Domain.Renters;
using PRM.Domain.Rents;
using PRM.Infrastructure.Persistence.EntityFrameworkCore;

namespace PRM.Infrastructure.Persistence.MySQL
{
    public class PrmDbContext : BaseDbContext<PrmDbContext>, ICurrentDbContext
    {
        public DbContext Context { get; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Renter> Renters { get; set; }
        
        public PrmDbContext(DbContextOptions<PrmDbContext> options) : base(options)
        {
            Context = this;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Renter>().HasKey(renter => renter.Id);

            modelBuilder.Entity<RenterRentalHistory>().HasKey(history => history.Id);
            
            modelBuilder.Entity<Rent>().HasKey(rent => rent.Id);

            modelBuilder.Entity<ProductRentalHistory>().HasKey(history => history.Id);
            
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            
            
            
        }
    }
    
    public class PrmDbContextDesignTime : BaseDbContextDesignTime<PrmDbContext>
    {
        public PrmDbContextDesignTime() : base(MySqlSettings.ConnectionString,"prm")
        {
        }
    }
}