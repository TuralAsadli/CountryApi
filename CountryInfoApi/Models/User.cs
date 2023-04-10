using CountryInfoApi.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace CountryInfoApi.Models
{
    public class User :BaseItem
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}
