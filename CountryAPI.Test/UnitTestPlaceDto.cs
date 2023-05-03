using CountryInfoApi.Dtos.City;
using CountryInfoApi.Dtos.RecomendedPlace;
using CountryInfoApi.Dtos.User;
using CountryInfoApi.Models;
using CountryInfoApi.Utilites.Validator;
using FluentAssertions;
using Xunit;

namespace CountryAPI.Test
{
    public class UnitTestPlaceDto
    {
        readonly RecomendedPlaceValidator validation = new RecomendedPlaceValidator();

        [Fact]
        public void Test_Validate_Place_Empty_PlaceName()
        {
            // Arrange
            RecomendedPlaceDto user = new RecomendedPlaceDto()
            {
                PlaceName = "",
                Coordinates = "33.44.22.11",
                Description = "fwfwfafa",

            };
            // Act
            var res = validation.Validate(user);
            // Assert
            Assert.False(res.IsValid);
        }

        [Fact]
        public void Test_Validate_Place_Empty_PlaceCoordinates()
        {
            // Arrange
            RecomendedPlaceDto user = new RecomendedPlaceDto()
            {
                PlaceName = "Baku",
                Coordinates = "",
                Description = "fwfwfafa",
            };
            // Act
            var res = validation.Validate(user);
            // Assert
            Assert.False(res.IsValid);
        }

        [Fact]
        public void Test_Validate_Place_Empty_PlaceDescription()
        {
            // Arrange
            RecomendedPlaceDto user = new RecomendedPlaceDto()
            {
                PlaceName = "Baku",
                Coordinates = "33.44.22.11",
                Description = "",
            };
            // Act
            var res = validation.Validate(user);
            // Assert
            Assert.False(res.IsValid);
        }

        [Fact]
        public void Test_Validate_Place_Incorrect_PlaceName()
        {
            // Arrange
            RecomendedPlaceDto user = new RecomendedPlaceDto()
            {
                PlaceName = "B",
                Coordinates = "33.44.22.11",
                Description = "wffwcvrvwrwr",
            };
            // Act
            var res = validation.Validate(user);
            // Assert
            Assert.False(res.IsValid);
        }

        [Fact]
        public void Test_Validate_Place_Incorrect_PlaceCoordinates()
        {
            // Arrange
            RecomendedPlaceDto user = new RecomendedPlaceDto()
            {
                PlaceName = "Baku",
                Coordinates = "3",
                Description = "wffwcvrvwrwr",
            };
            // Act
            var res = validation.Validate(user);
            // Assert
            Assert.False(res.IsValid);
        }

        [Fact]
        public void Test_Validate_Place_Incorrect_PlaceDescription()
        {
            // Arrange
            RecomendedPlaceDto user = new RecomendedPlaceDto()
            {
                PlaceName = "Baku",
                Coordinates = "33.44.33.11",
                Description = "w",
            };
            // Act
            var res = validation.Validate(user);
            // Assert
            Assert.False(res.IsValid);
        }

        
    }
}
