using System;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // This will create the DB defined in the Persintance/Migrations files.
            using (var scope = host.Services.CreateScope()) 
            {
                var services = scope.ServiceProvider;  
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    context.Database.Migrate();
                    Seed.SeedData(context, userManager).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<Logger<Program>>();
                    logger.LogError(ex, "An error occured during migration");
                }  
            }

            host.Run(); // Run applictaion
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
