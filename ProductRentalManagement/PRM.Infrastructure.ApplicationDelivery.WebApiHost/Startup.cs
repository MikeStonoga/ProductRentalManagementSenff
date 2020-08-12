using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PRM.Infrastructure.ApplicationDelivery.WebApiHost.Products;
using PRM.Infrastructure.Persistence.MySQL.BaseCore;
using PRM.Infrastructure.Persistence.MySQL.EntityFrameworkCore;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.Products;
using PRM.UseCases.Products.GetProductRentPrice;

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
                .AddTransient<IProductReadOnlyWebController, ProductReadOnlyController>()
                .AddTransient<IProductUseCasesReadOnlyInteractor, ProductUseCasesReadOnlyInteractor>()
                .AddTransient<IGetProductRentPrice, GetProductRentPrice>()
                .AddTransient<IProductUseCasesManipulationInteractor, ProductUseCasesManipulationInteractor>()
                .AddTransient(typeof(IReadOnlyPersistenceGateway<>), typeof(ReadOnlyRepository<>))
                .AddTransient(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>))
                .AddTransient(typeof(IManipulationPersistenceGateway<>), typeof(Repository<>));

            var connection = Configuration["MySqlConnection:MySqlConnectionString"];
            
            services.AddDbContext<PrmDbContext>(options =>
                options.UseMySql(connection));
                
            services.AddControllers();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}