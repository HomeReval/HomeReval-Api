using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Services.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pomelo.EntityFrameworkCore.MySql;

namespace API
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
            // The connection to the MySQL database
            services.AddDbContext<Context>(options =>
                options.UseMySql(Configuration.GetConnectionString("VPSConnection")));

            // Force the application to only accept Requests from HTTPS: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.0&tabs=visual-studio
            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new RequireHttpsAttribute());
            //});

            services.AddMvc();
            services.AddLogging();
            services.AddSingleton<IEncryptionManager, EncryptionManager>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }

    public static class SeedData
    {

        public static void Initialize(Context context)
        {
            context.Database.EnsureCreated();
            if (!context.UserGroups.Any())
            {
                context.UserGroups.Add(new UserGroup { ID = Models.Type.User, Type = "User", Description = "The end user that will use the application through the Unity application" });
                context.UserGroups.Add(new UserGroup { ID = Models.Type.Manager, Type = "Manager", Description = "To manage and review statistics of its users. The physiotherapist" });
                context.UserGroups.Add(new UserGroup { ID = Models.Type.Administrator, Type = "Administrator", Description = "Highest role to manage Managers" });
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                // m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2 = password
                context.Add(new User { ID = 0, Email = "projecthomereval@gmail.com", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Admin", LastName = "Admin", Gender = 'm', UserGroup = context.UserGroups.Find(API.Models.Type.Administrator) });
                context.Add(new User { ID = 1, Email = "fysio@hotmail.nl", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Admin", LastName = "Admin", Gender = 'm', UserGroup = context.UserGroups.Find(API.Models.Type.Manager) });
                context.Add(new User { ID = 2, Email = "nickwindt@hotmail.nl", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Nick", LastName = "Windt", Gender = 'm', UserGroup = context.UserGroups.Find(API.Models.Type.User) });
                context.SaveChanges();
            }

            if (!context.UserPhysios.Any())
            {
                context.Add(new UserPhysio { User = context.Users.Find(2), Physio = context.Users.Find(1) });
                context.SaveChanges();
            }
            

        }

    }
}
