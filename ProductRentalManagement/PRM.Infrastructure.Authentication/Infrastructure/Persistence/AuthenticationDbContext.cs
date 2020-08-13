using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PRM.Infrastructure.Authentication.Users;
using PRM.Infrastructure.Persistence.EntityFrameworkCore;

namespace PRM.Infrastructure.Authentication.Infrastructure.Persistence
{
    public class AuthenticationDbContext : BaseDbContext<AuthenticationDbContext>, ICurrentDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbContext Context { get; }

        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
        {
            Context = this;
        }

    }
    
    public class AuthenticationDbContextDesignTime : BaseDbContextDesignTime<AuthenticationDbContext>
    {
        public AuthenticationDbContextDesignTime() : base(PRM.Infrastructure.Persistence.MySQL.Settings.ConnectionString, "authentication")
        {
        }
    }
}