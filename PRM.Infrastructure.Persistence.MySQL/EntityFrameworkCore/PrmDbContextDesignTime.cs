using Microsoft.EntityFrameworkCore.Design;

namespace PRM.Infrastructure.Persistence.MySQL.EntityFrameworkCore
{
    public class PrmDbContextDesignTime : IDesignTimeDbContextFactory<PrmDbContext>
    {
        public PrmDbContext CreateDbContext(string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}