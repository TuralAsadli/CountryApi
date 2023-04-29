using AutoMapper;
using CountryInfoApi.Dtos.City;
using CountryInfoApi.Dtos.RecomendedPlace;
using CountryInfoApi.Models;

namespace CountryInfoApi.Utilites.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<City, CityDto>();
            CreateMap<CityDto, City>();
            CreateMap<City, GetCityDto>();
            CreateMap<GetCityDto, City>();

            CreateMap<RecomendedPlace, RecomendedPlaceDto>();
            CreateMap<RecomendedPlaceDto, RecomendedPlace>();
            CreateMap<RecomendedPlace, RecomendedPlaceGetDto>();
            CreateMap<RecomendedPlaceGetDto, RecomendedPlace>();
            
        }
    }
}
