using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using Talabat.APIs.Extensions;
using Talabat.APIs.Middleware;
using Talabat.Core.Entities.Identity;
using Talabat.Infrastructure.Identity;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        //Entry point
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            #region Configure Services
            // Add services to the container.
            //Newtons Package User to Stop Nested looping Between 2 Entities
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.SwaggerServices();
            builder.Services.ApplicationServices();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            //builder.Services.AddApplicationServices();

            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvides) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            builder.Services.AddAuthServices(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policyOptions =>
                {
                    policyOptions.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });
            });

            #endregion

            var app = builder.Build();

            #region Update Database Dynamic

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;


            var _dbContext = services.GetRequiredService<StoreContext>(); // ask clr for creating object from DbContext Explicitly
            var _identityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _dbContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(_dbContext);

                await _identityDbContext.Database.MigrateAsync();

                var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                await ApplicationIdentityContextSeed.SeedUsersAsync(_userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been occured during apply the migration");
            }
            #endregion

            #region Kestrel Middlewares
            // Configure the HTTP request pipeline.

            ///1. ByConvention Based

            app.UseMiddleware<ExceptionMiddleware>();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleware();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("MyPolicy");
            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}
