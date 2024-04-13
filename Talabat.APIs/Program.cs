namespace Talabat.APIs
{
    public class Program
    {
        //Entry point
        public static void Main(string[] args)
        {
            var WebApplicationbuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            WebApplicationbuilder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            WebApplicationbuilder.Services.AddEndpointsApiExplorer();
            WebApplicationbuilder.Services.AddSwaggerGen();

            var app = WebApplicationbuilder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
