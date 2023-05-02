using CountryInfoApi.Dtos.City;
using CountryInfoApi.Dtos.User;
using CountryInfoApi.Models;
using FluentValidation;

namespace CountryInfoApi.Utilites.Validator
{
    public class CityValidator : AbstractValidator<CityDto>
    {
        public CityValidator()
        {
            RuleFor(City => City.CityName)
            .NotEmpty().WithMessage("City Name cannot be empty.")
            .Length(2, 30).WithMessage("City Name must be between 2 and 30 characters.");

            RuleFor(City => City.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .Length(2, 30).WithMessage("Description must be between 2 and 30 characters.");

            RuleFor(City => City.Area)
            .NotEmpty().WithMessage("Area cannot be empty.")
            .Length(2, 30).WithMessage("Area must be between 2 and 30 characters.");

            RuleFor(City => City.Population)
                .NotNull().WithMessage("Population cannot be empty.")
                .GreaterThan(0).WithMessage("Minimum Population to create City is 0");
        }
    }
}
