using CountryInfoApi.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using CountryInfoApi.Dtos.RecomendedPlace;

namespace CountryInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecomendedPlaceController : ControllerBase
    {
        IRecomendedPlacesService _db;
        ICityService _cityService;

        public RecomendedPlaceController(IRecomendedPlacesService db, ICityService cityService)
        {
            _db = db;
            _cityService = cityService;
        }

        [HttpGet("GetCityPlaces/{cityId}")]
        public async Task<IActionResult> GetCityPlaces(string cityId)
        {
            if (!Guid.TryParse(cityId,out Guid guid))
            {
                return NotFound();
            }

            var places = await _db.GetAll();

            if (places.Where(p => p.City.Id == guid) == null)
            {
                return NotFound();
            }

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            var res = JsonSerializer.Serialize(places.Where(p => p.City.Id == guid), options);
            return Ok(res);
        }

        [HttpGet("GetPlace/{placeId}")]
        public IActionResult GetPlace(string placeId)
        {
            if (!Guid.TryParse(placeId, out Guid guid))
            {
                return NotFound();
            }

            var place = _db.GetById(guid);

            if (place == null)
            {
                return NotFound();
            }

            return Ok(place);
        }

        [HttpPost("CreatePlace")]
        public async Task<IActionResult> CreatePlace(string cityId, [FromForm] RecomendedPlaceDto placeDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }
            if (!Guid.TryParse(cityId,out Guid guid))
            {
                return NotFound(cityId);
            }
            var city = _cityService.GetById(guid);
            if (city == null)
            {
                return NotFound(cityId);
            }

            await _db.CreateAsync(guid, placeDto);
            return Ok();
        }

        [HttpPut("UpdatePlace")]
        public async Task<IActionResult> UpdatePlace(string id, [FromForm] RecomendedPlaceDto placeDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }
            if (!Guid.TryParse(id, out Guid guid))
            {
                return NotFound(id);
            }

            var place = _db.GetById(guid);
            if (place == null)
            {
                return NotFound(id);
            }
            await _db.UpdateAsync(id, placeDto);

            return Ok();
        }

        [HttpDelete("DeletePlace")]
        public async Task<IActionResult> DeletePlace(string id)
        {
            if (!Guid.TryParse(id,out Guid guid))
            {
                return NotFound();
            }
            var place = _db.GetById(guid);

            if (place == null)
            {
                return NotFound();
            }

            await _db.DeleteAsync(id);
            return Ok();
        }

    }
}
