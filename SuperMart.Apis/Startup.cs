using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperMart.Dal;
using Microsoft.EntityFrameworkCore;
using SuperMart.Dal.Repositories;
using Microsoft.Extensions.Configuration;

namespace SuperMart.Apis
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen();

            services.AddDbContext<SuperMartDbContext>(x =>
            { 
                x.UseMySQL(_configuration.GetConnectionString("SuperMartDatabase"), 
                    o => o.ExecutionStrategy(x => { return new MySqlRetryingExecutionStrategy(x); }));
                x.EnableDetailedErrors();
            });

            services.AddTransient<IStoresRepository, StoresRepository>();
            services.AddTransient<IProductsRepository, ProductsRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SuperMartDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dbContext.Database.Migrate();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SuperMart APIs");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
