using CountryInfoApi.Models;
using FluentValidation;

namespace CountryInfoApi.Utilites.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(User => User.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .Length(2, 30).WithMessage("Name must be between 2 and 30 characters.")
            .EmailAddress().WithMessage("Write correct Email");
        }
    }
}
