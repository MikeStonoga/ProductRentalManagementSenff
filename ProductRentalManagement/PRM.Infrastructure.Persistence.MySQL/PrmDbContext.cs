using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PRM.Domain.Products;
using PRM.Domain.Products.Enums;
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
            modelBuilder.Entity<Renter>().Property(r => r.Name).IsRequired();
            modelBuilder.Entity<Renter>().Property(r => r.Email).IsRequired();
            modelBuilder.Entity<Renter>().Property(r => r.BirthDate).IsRequired();
            modelBuilder.Entity<Renter>().Property(r => r.Code).IsRequired();
            modelBuilder.Entity<Renter>().Property(r => r.Phone).IsRequired();
            modelBuilder.Entity<Renter>().Property(r => r.GovernmentRegistrationDocumentCode).IsRequired();

            modelBuilder.Entity<RenterRentalHistory>().HasKey(history => history.Id);
            modelBuilder.Entity<RenterRentalHistory>().HasOne<Rent>().WithMany().HasForeignKey(history => history.RentId);
            modelBuilder.Entity<RenterRentalHistory>().HasOne<Renter>().WithMany().HasForeignKey(history => history.RenterId);
            
            modelBuilder.Entity<Rent>().HasKey(rent => rent.Id);
            modelBuilder.Entity<Rent>().Property(r => r.RenterId).IsRequired();
            modelBuilder.Entity<Rent>().Property(r => r.DailyPrice).IsRequired();
            modelBuilder.Entity<Rent>().Property(r => r.DailyLateFee).IsRequired();
            modelBuilder.Entity<Rent>().Property(r => r.StartDate).IsRequired();
            modelBuilder.Entity<Rent>().Property(r => r.EndDate).IsRequired();


            modelBuilder.Entity<ProductRentalHistory>().HasKey(history => history.Id);
            modelBuilder.Entity<ProductRentalHistory>().HasOne<Rent>().WithMany().HasForeignKey(history => history.RentId);
            modelBuilder.Entity<ProductRentalHistory>().HasOne<Product>().WithMany().HasForeignKey(history => history.ProductId);

            modelBuilder.Entity<Product>().HasKey(product => product.Id);
            modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Code).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Status).HasDefaultValue(ProductStatus.Available);
            modelBuilder.Entity<Product>().Property(p => p.RentDailyPrice).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.RentDailyLateFee).IsRequired();
        }
    }
    
    public class PrmDbContextDesignTime : BaseDbContextDesignTime<PrmDbContext>
    {
        public PrmDbContextDesignTime() : base(MySqlSettings.ConnectionString,"prm")
        {
        }
    }
}