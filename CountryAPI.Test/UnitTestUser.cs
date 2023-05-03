using CountryInfoApi.Dtos.City;
using CountryInfoApi.Dtos.User;
using CountryInfoApi.Models;
using CountryInfoApi.Utilites.Validator;
using FluentAssertions;
using Xunit;

namespace CountryAPI.Test
{
    public class UnitTestUser
    {
        readonly UserValidator validation = new UserValidator();

        [Fact]
        public void Test_Validate_User_Empty_Email()
        {
            // Arrange
            UserDto user = new UserDto()
            {
                Email = "",
                Password = "Tt123456"
                
            };
            // Act
            var res = validation.Validate(user);
            // Assert
            Assert.False(res.IsValid);
        }
        [Fact]
        public void Test_Validate_User_Empty_Password()
        {
            // Arrange
            UserDto user = new UserDto()
            {
                Email = "Name@gmail.com",
                Password = ""

            };
            // Act
            var res = validation.Validate(user);
            // Assert
            Assert.False(res.IsValid);
        }
        [Fact]
        public void Test_Validate_User_Wrong_Email()
        {
            // Arrange
            UserDto user = new UserDto()
            {
                Email = "@gmail.com",
                Password = "Tt123456"

            };
            // Act
            var res = validation.Validate(user);
            // Assert
            Assert.False(res.IsValid);
        }
        [Fact]
        public void Test_Validate_User_Wrong_Password()
        {
            // Arrange
            UserDto user = new UserDto()
            {
                Email = "Name@gmail.com",
                Password = "332"

            };
            // Act
            var res = validation.Validate(user);
            // Assert
            Assert.False(res.IsValid);
        }
        [Fact]
        public void Test_Validate_User_Short_Password()
        {
            // Arrange
            UserDto user = new UserDto()
            {
                Email = "Name@gmail.com",
                Password = "332"

            };
            // Act
            var res = validation.Validate(user);
            // Assert
            Assert.False(res.IsValid);
        }
        [Fact]
        public void Test_Validate_User_Long_Password()
        {
            // Arrange
            UserDto user = new UserDto()
            {
                Email = "Name@gmail.com",
                Password = "Tt1234567844444444444"

            };
            // Act
            var res = validation.Validate(user);
            
            // Assert
            Assert.False(res.IsValid);
        }
    }
}
