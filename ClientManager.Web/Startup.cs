using ClientManager.Core.DataInterface.DeskBooker.Core.DataInterface;
using ClientManager.Core.Processor;
using ClientManager.DataAccess;
using ClientManager.DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClientManager.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddScoped<IClientRequestProcessor, ClientRequestProcessor>();


            var connectionString = "DataSource=:memory:";
            var connection = new SqliteConnection(connectionString);
            connection.Open();

            services.AddDbContext<ClientContext>(options =>
                options.UseSqlite(connection)
            );

            EnsureDatabaseExists(connection);


            services.AddTransient<IClientRepository, ClientRepository>();
        }

        private static void EnsureDatabaseExists(SqliteConnection connection)
        {
            var builder = new DbContextOptionsBuilder<ClientContext>();
            builder.UseSqlite(connection);

            using var context = new ClientContext(builder.Options);
            context.Database.EnsureCreated();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}