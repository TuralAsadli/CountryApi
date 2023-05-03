using CountryInfoApi.Dtos.City;
using CountryInfoApi.Utilites.Validator;
using FluentAssertions;
using Xunit;

namespace CountryAPI.Test
{
    public class UnitTestCityDto
    {
        readonly CityValidator validations = new CityValidator();

        [Fact]
        public void Test_Validate_Empty_Cityname()
        {
            // Arrange
            CityDto cityDto = new CityDto()
            {
                CityName = "",
                Area = "4242",
                Description = "wefwfw",
                Population = 3224244,
            };
            // Act
            var res = validations.Validate(cityDto);
            // Assert
            Assert.False(res.IsValid);

        }
        [Fact]
        public void Test_Validate_Empty_CityArea()
        {
            // Arrange
            CityDto cityDto = new CityDto()
            {
                CityName = "Baku",
                Area = "",
                Description = "wefwfw",
                Population = 3224244,
            };
            // Act
            var res = validations.Validate(cityDto);
            // Assert
            Assert.False(res.IsValid);
        }

        [Fact]
        public void Test_Validate_Empty_CityDescription()
        {
            // Arrange
            CityDto cityDto = new CityDto()
            {
                CityName = "Baku",
                Area = "4434",
                Description = "",
                Population = 3224244,
            };
            // Act
            var res = validations.Validate(cityDto);
            // Assert
            
            Assert.False(res.IsValid);
        }

        [Fact]
        public void Test_Validate_Incorrect_Range_CityPopulation()
        {
            // Arrange
            CityDto cityDto = new CityDto()
            {
                CityName = "Baku",
                Area = "4434",
                Description = "f3frwrwfwfr",
                Population = -34,
            };
            // Act
            var res = validations.Validate(cityDto);
            // Assert
            Assert.False(res.IsValid);
        }

        [Fact]
        public void Test_Validate_Incorrect_Lengh_CityName()
        {
            // Arrange
            CityDto cityDto = new CityDto()
            {
                CityName = "B",
                Area = "4434",
                Description = "f3frwrwfwfr",
                Population = 334,
            };
            // Act
            var res = validations.Validate(cityDto);
            // Assert
            Assert.False(res.IsValid);
        }
        [Fact]
        public void Test_Validate_Incorrect_Lengh_CityArea()
        {
            // Arrange
            CityDto cityDto = new CityDto()
            {
                CityName = "Baku",
                Area = "4",
                Description = "f3frwrwfwfr",
                Population = 334,
            };
            // Act
            var res = validations.Validate(cityDto);
            // Assert
            Assert.False(res.IsValid);
        }

        [Fact]
        public void Test_Validate_Incorrect_Lengh_CityDescription()
        {
            // Arrange
            CityDto cityDto = new CityDto()
            {
                CityName = "Baku",
                Area = "4",
                Description = "f",
                Population = 334,
            };
            // Act
            var res = validations.Validate(cityDto);
            // Assert
            Assert.False(res.IsValid);
        }
    }
}