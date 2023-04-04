using CountryInfoApi.Dtos.City;
using CountryInfoApi.Models;

namespace CountryInfoApi.Abstractions.Services
{
    public interface ICityService
    {
        City GetById(Guid id);
        IEnumerable<City> GetAll();
        Task CreateAsync(CityDto city);
        Task UpdateAsync(string id, CityDto city);
        Task DeleteAsync(string id);
    }
}
