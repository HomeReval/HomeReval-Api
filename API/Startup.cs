using System;
using System.Linq;
using System.Text;
using API.Models;
using API.Models.Tokens;
using API.Services;
using API.Services.Middleware;
using API.Services.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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

            services.AddSingleton<IExercisePlanningService, ExercisePlanningService>();
            services.AddSingleton<IExerciseResultService, ExerciseResultService>();
            services.AddSingleton<IExerciseService, ExerciseService>();
            services.AddSingleton<IUserExerciseService, UserExerciseService>();
            services.AddSingleton<IUserGroupService, UserGroupService>();
            services.AddSingleton<IUserPhysioService, UserPhysioService>();
            services.AddSingleton<IUserService, UserService>();

            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddTransient<TokenManagerMiddleware>();
            services.AddTransient<ITokenManager, TokenManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var url = "http://homereval.ga";

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            //services.AddCors(options =>
            //{
            //    // BEGIN01
            //    options.AddPolicy("AllowSpecificOrigins",
            //    builder =>
            //    {
            //        builder.WithOrigins(url);
            //    });
            //    // END01

            //    // BEGIN02
            //    options.AddPolicy("AllowAllOrigins",
            //        builder =>
            //        {
            //            builder.AllowAnyOrigin();
            //        });
            //    // END02

            //    // BEGIN03
            //    options.AddPolicy("AllowSpecificMethods",
            //        builder =>
            //        {
            //            builder.WithOrigins(url)
            //                   .WithMethods("GET", "POST", "HEAD");
            //        });
            //    // END03

            //    // BEGIN04
            //    options.AddPolicy("AllowAllMethods",
            //        builder =>
            //        {
            //            builder.WithOrigins(url)
            //                   .AllowAnyMethod();
            //        });
            //    // END04

            //    // BEGIN05
            //    options.AddPolicy("AllowHeaders",
            //        builder =>
            //        {
            //            builder.WithOrigins(url)
            //                   .WithHeaders("accept", "content-type", "origin", "x-custom-header");
            //        });
            //    // END05

            //    // BEGIN06
            //    options.AddPolicy("AllowAllHeaders",
            //        builder =>
            //        {
            //            builder.WithOrigins(url)
            //                   .AllowAnyHeader();
            //        });
            //    // END06

            //    // BEGIN07
            //    options.AddPolicy("ExposeResponseHeaders",
            //        builder =>
            //        {
            //            builder.WithOrigins(url)
            //                   .WithExposedHeaders("x-custom-header");
            //        });
            //    // END07

            //    // BEGIN08
            //    options.AddPolicy("AllowCredentials",
            //        builder =>
            //        {
            //            builder.WithOrigins(url)
            //                   .AllowCredentials();
            //        });
            //    // END08

            //    // BEGIN09
            //    options.AddPolicy("SetPreflightExpiration",
            //        builder =>
            //        {
            //            builder.WithOrigins(url)
            //                   .SetPreflightMaxAge(TimeSpan.FromSeconds(2520));
            //        });
            //    // END09
            //});

            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));
            //});

            var jwtSection = Configuration.GetSection("jwt");
            var jwtOptions = new JwtOptions();
            jwtSection.Bind(jwtOptions);
            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                });
            services.Configure<JwtOptions>(jwtSection);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            app.UseCors("MyPolicy");

            loggerFactory.AddDebug().AddConsole();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // VPS settings
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseAuthentication();
            app.UseMiddleware<TokenManagerMiddleware>();

            app.UseMvc();
        }
    }

    // Throw default Data is the database if empty
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
                context.Add(new User {Email = "projecthomereval@gmail.com", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Admin", LastName = "Admin", Gender = 'm', UserGroup = context.UserGroups.Find(API.Models.Type.Administrator) });
                context.Add(new User {Email = "janvanwindmolen@fysio.nl", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Jan", LastName = "van Windmolen", Gender = 'm', UserGroup = context.UserGroups.Find(API.Models.Type.Manager) });
                context.Add(new User { Email = "hermandetas@fysio.nl", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Herman", LastName = "de Tas", Gender = 'm', UserGroup = context.UserGroups.Find(API.Models.Type.Manager) });
                context.Add(new User {Email = "nickwindt@hotmail.nl", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Nick", LastName = "Windt", Gender = 'm', UserGroup = context.UserGroups.Find(API.Models.Type.User) });
                context.Add(new User { Email = "petertorenvalk@hotmail.nl", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Peter", LastName = "Torenvalk", Gender = 'm', UserGroup = context.UserGroups.Find(API.Models.Type.User) });
                context.Add(new User { Email = "liesbakker@hotmail.nl", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Lies", LastName = "Bakker", Gender = 'f', UserGroup = context.UserGroups.Find(API.Models.Type.User) });
                context.Add(new User { Email = "henkjansen@hotmail.nl", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Henk", LastName = "Jansen", Gender = 'm', UserGroup = context.UserGroups.Find(API.Models.Type.User) });
                context.Add(new User { Email = "keesvanberenstein@hotmail.nl", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Kees", LastName = "van Berenstein", Gender = 'm', UserGroup = context.UserGroups.Find(API.Models.Type.User) });
                context.SaveChanges();
            }

            if (!context.UserPhysios.Any())
            {

                User user = context.Users.First(a => a.Email == "nickwindt@hotmail.nl");
                User user2 = context.Users.First(a => a.Email == "petertorenvalk@hotmail.nl");
                User user3 = context.Users.First(a => a.Email == "liesbakker@hotmail.nl");
                User user4 = context.Users.First(a => a.Email == "henkjansen@hotmail.nl");

                User user5 = context.Users.First(a => a.Email == "keesvanberenstein@hotmail.nl");
                


                User fysio = context.Users.First(a => a.Email == "janvanwindmolen@fysio.nl");
                User fysio2 = context.Users.First(a => a.Email == "hermandetas@fysio.nl" );

                context.Add(new UserPhysio { User = user, Physio = fysio });
                context.Add(new UserPhysio { User = user2, Physio = fysio });
                context.Add(new UserPhysio { User = user3, Physio = fysio });
                context.Add(new UserPhysio { User = user4, Physio = fysio });

                context.Add(new UserPhysio { User = user5, Physio = fysio2 });

                context.SaveChanges();
            }

            if (!context.Exercises.Any())
            {
                context.Add(new Exercise { Name = "Nek beweging", Description = "Beweeg uw nek van links naar rechts", Recording = new byte[8] });
                context.Add(new Exercise { Name = "Arm spanning", Description = "Strek uw arm vooruit, span de spieren samen en beweeg de arm 90 graden naarboven", Recording = new byte[8] });
                context.Add(new Exercise { Name = "Been strekking", Description = "Ga staan en strek uw been vooruit", Recording = new byte[8] });
                context.SaveChanges();
            }

            if (!context.UserExercises.Any())
            {

                User user = context.Users.First(a => a.Email == "nickwindt@hotmail.nl");
                User user2 = context.Users.First(a => a.Email == "petertorenvalk@hotmail.nl");
                User user3 = context.Users.First(a => a.Email == "liesbakker@hotmail.nl");
                User user4 = context.Users.First(a => a.Email == "henkjansen@hotmail.nl");

                User user5 = context.Users.First(a => a.Email == "keesvanberenstein@hotmail.nl");

                Exercise exercise = context.Exercises.First(a => a.Name == "Nek beweging");
                Exercise exercise2 = context.Exercises.First(a => a.Name == "Arm spanning");
                Exercise exercise3 = context.Exercises.First(a => a.Name == "Been strekking");
                context.Add(new UserExercise { User = user, Exercise = exercise });
                context.Add(new UserExercise { User = user, Exercise = exercise2 });
                context.Add(new UserExercise { User = user, Exercise = exercise3 });

                context.Add(new UserExercise { User = user2, Exercise = exercise });
                context.Add(new UserExercise { User = user2, Exercise = exercise3 });

                context.Add(new UserExercise { User = user3, Exercise = exercise });
                context.Add(new UserExercise { User = user3, Exercise = exercise2 });

                context.Add(new UserExercise { User = user4, Exercise = exercise });
                context.Add(new UserExercise { User = user4, Exercise = exercise2 });

                context.Add(new UserExercise { User = user5, Exercise = exercise2 });
                context.Add(new UserExercise { User = user5, Exercise = exercise3 });

                context.SaveChanges();
            }

            if (!context.ExercisePlannings.Any())
            {
                UserExercise userExercise = context.UserExercises.First();
                context.Add(new ExercisePlanning { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Description = "Meneer Windt, houd rekening met het feit dat u deze oefening goed op de feedback van het systeem let!", Amount = 100, UserExercise = userExercise});
                context.SaveChanges();
            }
            
            if (!context.ExerciseSessions.Any())
            {
                ExercisePlanning exercisePlanning = context.ExercisePlannings.First();
                context.Add(new ExerciseSession { Date = DateTime.Now, IsComplete = true, ExercisePlanning = exercisePlanning });
                context.Add(new ExerciseSession { Date = DateTime.Now.AddDays(1), IsComplete = true, ExercisePlanning = exercisePlanning });
                context.Add(new ExerciseSession { Date = DateTime.Now.AddDays(2), IsComplete = true, ExercisePlanning = exercisePlanning });
                context.Add(new ExerciseSession { Date = DateTime.Now.AddDays(3), IsComplete = true, ExercisePlanning = exercisePlanning });
                context.Add(new ExerciseSession { Date = DateTime.Now.AddDays(4), IsComplete = false, ExercisePlanning = exercisePlanning });
                context.Add(new ExerciseSession { Date = DateTime.Now.AddDays(5), IsComplete = false, ExercisePlanning = exercisePlanning });
                context.SaveChanges();
            }

            if (!context.ExerciseResults.Any())
            {
                ExerciseSession exerciseSession = context.ExerciseSessions.First();
                context.Add(new ExerciseResult {Duration = 10000, Score = 80, Result = new byte[8], ExerciseSession = exerciseSession });
                context.SaveChanges();
            }
        }
    }
}
