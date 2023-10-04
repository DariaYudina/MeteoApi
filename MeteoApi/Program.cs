using IMeteoDao;
using IMeteoLogic;
using MeteoApoLogic;
using MeteoDAO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using AutoMapper;
using MeteoApi.Mapping;
using AutoMapper;
using MongoDB.Driver;

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

            // Настройка подключения к MongoDB
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Укажите путь к вашему appsettings.json
                .Build();

            var mongoConnectionString = configuration.GetConnectionString("MongoDB");
            var mongoClient = new MongoClient(mongoConnectionString);
            builder.Services.AddSingleton(mongoClient);

            // Создание WeatherRepository и добавление его в контейнер зависимостей
            var databaseName = "WeatherDB"; // Замените на ваше имя базы данных
            var weatherRepository = new WeatherRepository(mongoClient, databaseName);
            builder.Services.AddSingleton<IWeatherRepository>(weatherRepository);

            // Создание WeatherLogic и добавление его в контейнер зависимостей
            var weatherLogic = new WeatherLogic(weatherRepository);
            builder.Services.AddSingleton<IWeatherLogic>(weatherLogic);
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