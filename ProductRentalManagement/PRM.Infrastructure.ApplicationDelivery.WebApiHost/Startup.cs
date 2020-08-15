using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PRM.Infrastructure.Authentication;
using PRM.Infrastructure.Persistence.MySQL;
using PRM.Infrastructure.Persistence.MySQL.BaseCore;
using PRM.InterfaceAdapters.Controllers;
using PRM.InterfaceAdapters.Gateways.Persistence;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases;
using PRM.UseCases.Products;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllersTransients()
                .AddUseCasesTransients()
                .AddMySqlPersistenceTransients();
            
            services.UseMySql<PrmDbContext>(databaseName: "prm");
            services.AddControllers();
            AuthenticationSettingsExtensions.AddAuthentication(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}