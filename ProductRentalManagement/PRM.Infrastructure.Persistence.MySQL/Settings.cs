using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace PRM.Infrastructure.Persistence.MySQL
{
    public static class Settings
    {
        public static readonly string ConnectionString = "Server=127.0.0.1; Port=3306; DataBase=DATABASE_NAME;Uid=root;Pwd=''";
    }

    public static class MySqlPersistenceExtensions
    {
        public static IServiceCollection UseMySql<TDbContext>(this IServiceCollection services, string databaseName) where TDbContext : DbContext, ICurrentDbContext
        {
            var connectionString = Settings.ConnectionString.Replace("DATABASE_NAME", databaseName); 
            
            services.AddDbContext<TDbContext>(options =>
                options.UseMySql(connectionString));

            services.AddTransient<ICurrentDbContext, TDbContext>();
                
            return services;
        }
    } 
}