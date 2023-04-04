using CountryInfoApi.Abstractions.Repositories;
using CountryInfoApi.Dtos.City;
using CountryInfoApi.Dtos.CurrentInfo;
using CountryInfoApi.Dtos.ForecastInfo;
using CountryInfoApi.Models;
using CountryInfoApi.Utilites.FiIeExtentions;
using CountryInfoApi.Utilites.WeatherInfo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using CountryInfoApi.Abstractions.Services;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Dropbox.Api;
using Dropbox.Api.Files;
using CountryInfoApi.Utilites.CloudStorage;
using CountryInfoApi.Models.Base;
using Microsoft.Extensions.Configuration;

namespace CountryInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        ICityService _db;
        ApiKeys _apiKeys;
        public CityController(ICityService db,IOptions<ApiKeys> apiKeys)
        {
            _db = db;
            _apiKeys = apiKeys.Value;
        }

        [HttpPost("test")]
        public async Task<IActionResult> Test(IFormFile file)
        {
            
            
            var service = new CLoudStorage(_apiKeys.Key);
            var path = await service.UploadImageAsync(file, @"Apps/CountryApi");
            return Ok(new { path });
           
        }

        [HttpGet("GetCurrent")]
        public async Task<IActionResult> GetCurrent(string CityName)
        {

            CurrentInfoDto res = await WeatherApi.GetCurrentInfo(CityName);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpGet("GetForecast")]
        public async Task<IActionResult> GetForecast(string CityName, int days)
        {

            ForecastInfoDto res = await WeatherApi.GetForecastInfo(CityName, days);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpGet("GetCities")]
        public IActionResult GetCities()
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            var res = JsonSerializer.Serialize(_db.GetAll(), options); 
            return Ok(res);
        }

        [HttpPost("CreateCity")]
        public async Task<IActionResult> CreateCity([FromForm] CityDto CityDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }
            
            await _db.CreateAsync(CityDto);
            return Ok();

        }

        [HttpGet("GetCity/{id}")]
        public  IActionResult GetCity(string id)
        {
            if (!Guid.TryParse(id,out Guid guid))
            {
                return BadRequest();
            }

            var city =  _db.GetById(guid);
            if (city == null)
            {
                return NotFound();
            }

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            var res = JsonSerializer.Serialize(city, options);
            return Ok(res);
        }

        [HttpPut("UpdateCity/{id}")]
        public async Task<IActionResult> UpdateCity(string id,[FromForm] CityDto City)
        {
            if (!Guid.TryParse(id,out Guid guid))
            {
                return NotFound();
            }

            var existingObject = _db.GetById(guid);
            if (existingObject == null)
            {
                return NotFound();
            }

            await _db.UpdateAsync(id, City);

            return Ok();

        }

        [HttpDelete("DeleteCity/{id}")]
        public async Task<IActionResult> DeleteCity(string id)
        {
            if (!Guid.TryParse(id,out Guid guid))
            {
                return NotFound();
            }
            var city = _db.GetById(guid);
            if (city != null)
            {
                await _db.DeleteAsync(id);
                return Ok();
            }
           

            return NotFound();
        }
    }
}
