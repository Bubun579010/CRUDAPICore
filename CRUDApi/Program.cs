using CRUDApi.Data;
using CRUDApi.Repository;
using CRUDApi.Repository.Services;
using Microsoft.EntityFrameworkCore;

namespace CRUDApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<DeptDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("conStr")));

            builder.Services.AddScoped<IDepartment, DepartmentRepository>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                   policy.AllowAnyOrigin()
                         .AllowAnyHeader()
                         .AllowAnyMethod());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
