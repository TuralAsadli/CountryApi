using CountryInfoApi.Abstractions.Services;
using CountryInfoApi.Dtos;
using CountryInfoApi.Dtos.City;
using CountryInfoApi.Dtos.CurrentInfo;
using CountryInfoApi.Dtos.ForecastInfo;
using CountryInfoApi.Models.Base;
using CountryInfoApi.Utilites.FiIeExtentions;
using CountryInfoApi.Utilites.Validator;
using CountryInfoApi.Utilites.WeatherInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CountryInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CityController : ControllerBase
    {
        ICityService _db;
        ApiKeys _apiKeys;
        CityValidator _validator;
        public CityController(ICityService db, IOptions<ApiKeys> apiKeys)
        {
            _db = db;
            _apiKeys = apiKeys.Value;
            _validator = new CityValidator();
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
        public async Task<IActionResult> GetCities()
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };


            var res = JsonSerializer.Serialize(await _db.GetAll(), options);
            return Ok(res);
        }

        [HttpPost("CreateCity")]
        public async Task<IActionResult> CreateCity([FromForm] CityDto CityDto)
        {
            var result = _validator.Validate(CityDto);
            if (!result.IsValid)
            {
                List<ErrorDto> errorDtos = new List<ErrorDto>();
                foreach (var error in result.Errors)
                {
                    ErrorDto errorDto = new ErrorDto()
                    {
                        ErrorMessage = error.ErrorMessage,
                        PropertyName = error.PropertyName
                    };
                    errorDtos.Add(errorDto);
                }
                return BadRequest(errorDtos);
            }

            foreach (var formFile in CityDto.CityImgsFormFile)
            {
                
                if (!formFile.CheckImgFileType())
                {
                    return BadRequest(new ErrorDto()
                    {
                        ErrorMessage = "Incorrect Image file type",
                        PropertyName = "CityImgs"
                    });
                }
            }

            await _db.CreateAsync(CityDto);
            return Ok();

        }

        [HttpGet("GetCity/{id}")]
        public IActionResult GetCity(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
            {
                return BadRequest();
            }

            var city = _db.GetById(guid);
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
        public async Task<IActionResult> UpdateCity(string id, [FromForm] CityDto City)
        {
            if (!Guid.TryParse(id, out Guid guid))
            {
                return NotFound();
            }

            var result = _validator.Validate(City);
            if (!result.IsValid)
            {
                List<ErrorDto> errorDtos = new List<ErrorDto>();
                foreach (var error in result.Errors)
                {
                    ErrorDto errorDto = new ErrorDto()
                    {
                        ErrorMessage = error.ErrorMessage,
                        PropertyName = error.PropertyName
                    };
                    errorDtos.Add(errorDto);
                }
                return BadRequest(errorDtos);
            }

            foreach (var formFile in City.CityImgsFormFile)
            {

                if (!formFile.CheckImgFileType())
                {
                    return BadRequest(new ErrorDto()
                    {
                        ErrorMessage = "Incorrect Image file type",
                        PropertyName = "CityImgs"
                    });
                }
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
            if (!Guid.TryParse(id, out Guid guid))
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
