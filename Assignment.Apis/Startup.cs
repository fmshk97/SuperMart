using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Assignment.Dal;
using Microsoft.EntityFrameworkCore;
using Assignment.Dal.Repositories;

namespace Assignment.Apis
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddDbContext<SuperMartDbContext>(x =>
            { 
                x.UseMySQL("server=localhost;port=3506;database=supermart;user=root;password=!MySQL97;", 
                    o => o.ExecutionStrategy(x => { return new MySqlRetryingExecutionStrategy(x); }));
                x.EnableDetailedErrors();
            });

            services.AddTransient<IStoresRepository, StoresRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SuperMartDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dbContext.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
