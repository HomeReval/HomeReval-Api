using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace API
{

    // https://www.jerriepelser.com/blog/using-mariadb-with-aspnet-core/
    // https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.0

    // JWT tokens: https://github.com/spetz/tokenmanager-sample (Needs some improvements but its a good start)
    // https://auth0.com/blog/securing-asp-dot-net-core-2-applications-with-jwts/

    // How to update Database: Open Package Manager Console
    // Execute: Add-Migration InitialCreate
    // Execute: Update-Database

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .Build();

        public static IWebHost BuildWebHost(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = scope.ServiceProvider.GetService<Context>();

                try
                {
                    context.Database.Migrate();
                    SeedData.Initialize(context);
                } catch (Exception e)
                {
                    // something
                }
                

            }

            return host;
        }

    }
}
