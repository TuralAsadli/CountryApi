using CountryInfoApi.Dtos.City;
using CountryInfoApi.Models;

namespace CountryInfoApi.Abstractions.Services
{
    public interface ICityService
    {
        Task<GetCityDto> GetById(Guid id);
        Task<IEnumerable<GetCityDto>> GetAll();
        Task CreateAsync(CityDto city);
        Task UpdateAsync(string id, CityDto city);
        Task DeleteAsync(string id);
    }
}
