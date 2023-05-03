using CountryInfoApi.Abstractions.Repositories;
using CountryInfoApi.DAL;
using CountryInfoApi.Dtos;
using CountryInfoApi.Dtos.User;
using CountryInfoApi.Models;
using CountryInfoApi.Utilites.JwtTokenHelpers;
using CountryInfoApi.Utilites.PasswordHelpers;
using CountryInfoApi.Utilites.Validator;
using Microsoft.AspNetCore.Mvc;
using static Dropbox.Api.Sharing.ListFileMembersIndividualResult;

namespace CountryInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IBaseRepository<User> _context;
        AppDbContext Context;
        IConfiguration _configuration;
        UserValidator _validator;
        public AccountController(IBaseRepository<User> context, IConfiguration configuration, AppDbContext appcontext)
        {
            _context = context;
            _configuration = configuration;
            Context = appcontext;
            _validator = new UserValidator();
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromForm] UserDto userDto)
        {
            var res = _validator.Validate(userDto);
            if (res.IsValid)
            {
                if (_context.GetAll().FirstOrDefault(x => x.Email == userDto.Email) == null)
                {
                    User user = new User();
                    user.Email = userDto.Email;
                    PasswordHelper.HashPassword(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    await _context.Create(user);
                    return Ok();
                }
                
                return BadRequest(new ErrorDto() { ErrorMessage = "this email is already taken", PropertyName = "Email" });
                
            }
            List<ErrorDto> errorDtos = new List<ErrorDto>();
            foreach (var error in res.Errors)
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

        [HttpPost("Login")]
        public ActionResult<string> Login([FromBody] UserDto user)
        {

            var User = _context.GetAll().FirstOrDefault(u => u.Email == user.Email);

            if (User == null)
            {
                return Unauthorized();
            }

            if (!PasswordHelper.VerifyPassword(user.Password, User.PasswordHash, User.PasswordSalt))
            {
                return Unauthorized();
            }


            var token = JwtTokenHelper.CreateToken(User, _configuration);
            return Ok(token);
        }
    }
}
