using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PRM.Infrastructure.Persistence.MySQL.EntityFrameworkCore
{
    public class PrmDbContextDesignTime : IDesignTimeDbContextFactory<PrmDbContext>
    {
        public PrmDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PrmDbContext>();
            optionsBuilder.UseMySql("Server=127.0.0.1; Port=3306; DataBase=prm;Uid=root;Pwd=''");

            return new PrmDbContext(optionsBuilder.Options);
        }
    }
}