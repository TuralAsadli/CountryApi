using System.ComponentModel.DataAnnotations;

namespace CountryInfoApi.Dtos.User
{
    public class UserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
