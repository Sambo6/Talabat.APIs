using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        //Entry point
        public static async Task Main(string[] args)
        {


            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            webApplicationBuilder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();

            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            }
                );
            //For All [Product- productBrand- productCategory]
            webApplicationBuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            #endregion

            var app = webApplicationBuilder.Build();




            #region Update Database Dynamic
            var Scope = app.Services.CreateScope();
            var services = Scope.ServiceProvider;
            var _dbContext = services.GetRequiredService<StoreContext>();
            // Ask CLR for creating object from DbContext [Explicitly]

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                 await _dbContext.Database.MigrateAsync(); //Update DataBase
                await StoreContextSeeding.SeedAsync(_dbContext);  // Data Seeding

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error has been occurred during apply Migration");
            } 
            #endregion

            #region Kestrel Middlewares
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
