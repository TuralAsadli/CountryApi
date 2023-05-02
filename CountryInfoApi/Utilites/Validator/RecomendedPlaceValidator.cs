using CountryInfoApi.Dtos.City;
using CountryInfoApi.Dtos.RecomendedPlace;
using FluentValidation;

namespace CountryInfoApi.Utilites.Validator
{
    public class RecomendedPlaceValidator : AbstractValidator<RecomendedPlaceDto>
    {
        public RecomendedPlaceValidator()
        {
            RuleFor(place => place.PlaceName)
            .NotEmpty().WithMessage("City Name cannot be empty.")
            .Length(2, 30).WithMessage("City Name must be between 2 and 30 characters.");

            RuleFor(place => place.Coordinates)
            .NotEmpty().WithMessage("Coordinates cannot be empty.")
            .Length(2, 30).WithMessage("Coordinates must be between 2 and 30 characters.");

            RuleFor(place => place.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .Length(2, 30).WithMessage("Description must be between 2 and 30 characters.");

        }
    }
}
