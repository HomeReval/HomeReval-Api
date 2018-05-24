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
                context.Add(new User {Email = "fysio@hotmail.nl", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Admin", LastName = "Admin", Gender = 'm', UserGroup = context.UserGroups.Find(API.Models.Type.Manager) });
                context.Add(new User {Email = "nickwindt@hotmail.nl", Password = "m625LrMSdPa9IZlnFAaR4O6X9gXQxC4PRlfuPnZRb9Kb4Ka2", FirstName = "Nick", LastName = "Windt", Gender = 'm', UserGroup = context.UserGroups.Find(API.Models.Type.User) });
                context.SaveChanges();
            }

            if (!context.UserPhysios.Any())
            {

                User user = context.Users.First(a => a.UserGroup.ID == Models.Type.User);
                User fysio = context.Users.First(a => a.UserGroup.ID == Models.Type.Manager);

                context.Add(new UserPhysio { User = user, Physio = fysio });
                context.SaveChanges();
            }

            if (!context.Exercises.Any())
            {
                context.Add(new Exercise { Name = "Zit & Sta", Description = "Ga zitten en sta dan voor 20s", Recording = new byte[8] });
                context.Add(new Exercise { Name = "Slaap & Sta", Description = "Ga slapen en sta dan voor 20s", Recording = new byte[8] });
                context.Add(new Exercise { Name = "Ren & Vecht", Description = "Ga rennen en vecht dan voor 20s", Recording = new byte[8] });
                context.SaveChanges();
            }

            if (!context.UserExercises.Any())
            {

                User user = context.Users.First(a => a.UserGroup.ID == Models.Type.User);
                Exercise exercise = context.Exercises.First(a => a.ID == 1);
                context.Add(new UserExercise { User = user, Exercise = exercise });
                context.SaveChanges();
            }

            if (!context.ExercisePlannings.Any())
            {
                UserExercise userExercise = context.UserExercises.First();
                context.Add(new ExercisePlanning { Date = DateTime.Now, Description = "Mevrouw bakkertjes, houd rekening met het feit dat u deze oefening GOED moet doen!", Amount = 100, IsComplete = false, UserExercise = userExercise});
                context.SaveChanges();
            }

            if (!context.ExerciseResults.Any())
            {
                UserExercise userExercise = context.UserExercises.First();
                context.Add(new ExerciseResult { Date = DateTime.Now, Duration = 10000, Score = 80, Result = "Geweldige opname met resultaten!", UserExercise = userExercise });
                context.SaveChanges();
            }
        }
    }
}
