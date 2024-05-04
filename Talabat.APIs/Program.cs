using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
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


			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the container.

			webApplicationBuilder.Services.AddControllers();

			//Newtons Package User to Stop Nested looping Between 2 Entities
				/*.AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});*/

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

			webApplicationBuilder.Services.SwaggerServices();

			webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseLazyLoadingProxies().UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

			webApplicationBuilder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
			});
			webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) =>
			{
				var connection = webApplicationBuilder.Configuration.GetConnectionString("redis");
				return ConnectionMultiplexer.Connect(connection);
			});

			webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>()
										  .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

			webApplicationBuilder.Services.ApplicationServices();

			webApplicationBuilder.Services.AddAuthServices(webApplicationBuilder.Configuration);
			
			#endregion

			var app = webApplicationBuilder.Build();




			#region Update Database Dynamic
			var Scope = app.Services.CreateScope();
			var services = Scope.ServiceProvider;
			var _dbContext = services.GetRequiredService<StoreContext>();
			// Ask CLR for creating object from DbContext [Explicitly]
			var _IdentityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			var logger = loggerFactory.CreateLogger<Program>();
			try
			{
				await _dbContext.Database.MigrateAsync(); //Update DataBase
				await StoreContextSeeding.SeedAsync(_dbContext);  // Data Seeding

				await _IdentityDbContext.Database.MigrateAsync();  //Update DataBase

				var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				await ApplicationIdentityContextSeed.SeedUsersAsync(userManager);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error has been occurred during apply Migration");
			}
			#endregion

			#region Kestrel Middlewares
			// Configure the HTTP request pipeline.

			///1. ByConvention Based
			//app.UseMiddleware<ExceptionMiddleware>();

			///2.Factory Based
			app.Use(async (httpContext, _next) =>
			{
				try
				{
					//take an action with the request
					await _next.Invoke(httpContext); // Go to next middleware
													 //take an action with the response
				}
				catch (Exception ex)
				{
					logger.LogError(ex.Message); // Development env
					httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					httpContext.Response.ContentType = "application/json";
					var response = app.Environment.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) :
					new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
					var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
					var json = JsonSerializer.Serialize(response, options);
					await httpContext.Response.WriteAsync(json);
				}

			});

			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddleware();
			}

			app.UseStatusCodePagesWithReExecute("/errors/{0}");
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.MapControllers();

			#endregion

			app.Run();
		}
	}
}
