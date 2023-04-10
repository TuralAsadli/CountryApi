using CountryInfoApi.Dtos.City;
using CountryInfoApi.Dtos.RecomendedPlace;
using CountryInfoApi.Models;

namespace CountryInfoApi.Abstractions.Services
{
    public interface IRecomendedPlacesService
    {
        Task<RecomendedPlaceGetDto> GetById(Guid id);
        Task<IEnumerable<RecomendedPlaceGetDto>> GetAll();
        Task CreateAsync(Guid cityId, RecomendedPlaceDto place);
        Task UpdateAsync(string id, RecomendedPlaceDto place);
        Task DeleteAsync(string id);
    }
}
