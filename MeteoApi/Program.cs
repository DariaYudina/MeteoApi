using IMeteoDao;
using IMeteoLogic;
using MeteoApoLogic;
using MeteoDAO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using AutoMapper;
using MeteoApi.Mapping;
using AutoMapper;

namespace MeteoApi
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
            // Настройка AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WeatherMappingProfile()); // Указываем ваш профиль маппинга
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper); 

            builder.Services.AddScoped<IWeatherLogic, WeatherLogic>();
            builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseCors();
            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}