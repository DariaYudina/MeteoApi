using AutoMapper;
using Entities;
using MeteoApi.Models;

namespace MeteoApi.Mapping
{
    public class WeatherMappingProfile : Profile
    {
        public WeatherMappingProfile()
        {
            CreateMap<CityWeatherData, CityDataModel>()
                        .ForMember(dest => dest.WeatherEntries, opt => opt.MapFrom(src => src.WeatherEntries));

            CreateMap<WeatherEntry, WeatherForecastModel>()
                .ForMember(dest => dest.MaxTemperatureС, opt => opt.MapFrom(src => src.MaxTemperature))
                .ForMember(dest => dest.MinTemperatureС, opt => opt.MapFrom(src => src.MinTemperature));
        }
    }
}
