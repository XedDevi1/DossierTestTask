
using MainTestTask.Helpers;
using MainTestTask.Persistence;
using MainTestTask.Services;
using MainTestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MainTestTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DossierDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            builder.Services.AddScoped<IDossierService, DossierService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseDefaultFiles(); // Добавьте эту строку
            app.UseStaticFiles(); // Добавьте эту строку

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }

}