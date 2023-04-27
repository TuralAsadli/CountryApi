using CountryInfoApi.Abstractions.Repositories;
using CountryInfoApi.DAL;
using CountryInfoApi.Dtos.User;
using CountryInfoApi.Models;
using CountryInfoApi.Utilites.JwtTokenHelpers;
using CountryInfoApi.Utilites.PasswordHelpers;
using Microsoft.AspNetCore.Mvc;

namespace CountryInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IBaseRepository<User> _context;
        AppDbContext Context;
        IConfiguration _configuration;
        public AccountController(IBaseRepository<User> context, IConfiguration configuration, AppDbContext appcontext)
        {
            _context = context;
            _configuration = configuration;
            Context = appcontext;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromForm] UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.Email = userDto.Email;
                PasswordHelper.HashPassword(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                await _context.Create(user);

                return Ok();
            }
            return BadRequest(ModelState);
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
