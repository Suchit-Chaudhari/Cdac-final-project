using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

namespace CareerConnect
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowEverything",
                    builder =>
                    {
                        builder.AllowAnyOrigin()    // Allow requests from any origin
                               .AllowAnyHeader()    // Allow any header
                               .AllowAnyMethod();   // Allow any HTTP method
                    });
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<JobPortalContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 37))));
            

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CareerConnect API", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseRouting();

            app.UseCors("AllowEverything");

            app.UseHttpsRedirection();
            //app.UseStaticFiles(); // Serves files from wwwroot by default
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "uploads")),
                RequestPath = "/uploads"
            });
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
