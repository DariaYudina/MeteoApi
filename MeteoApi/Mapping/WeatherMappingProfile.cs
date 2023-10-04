using AutoMapper;
using Entities;
using MeteoApi.Models;

namespace MeteoApi.Mapping
{
    public class WeatherMappingProfile : Profile
    {
        public WeatherMappingProfile()
        {
            CreateMap<WeatherInfo, WeatherForecast>().ReverseMap()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.TemperatureC, opt => opt.MapFrom(src => src.TemperatureC))
            .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));
        }
    }
}
