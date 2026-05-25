using DietPlanner.Application.Requests.Account;
using DietPlanner.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace DietPlanner.Api.UnitTests.Validators
{
    public class SignUpValidatorTests(SignUpValidator validator)
    {
        [Fact]
        public void ValidateSignupRequest_WhenUsernameIsEmpty_ShouldReturnError()
        {
            // Arrange
            var model = new SignUpRequest { Email = "test@test.com", Username = string.Empty , Password = "password" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username)
                  .WithErrorMessage("'Username' must not be empty.");
        }

        [Fact]
        public void ValidateSignupRequest_UsernameIsTooShort_ShouldReturnError()
        {
            // Arrange
            var model = new SignUpRequest { Email = "test@test.com", Username = "abc", Password = "password" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username)
                  .WithErrorMessage("'Username' must be between 6 and 20 characters. You entered 3 characters.");
        }

        [Fact]
        public void ValidateSignupRequest_WhenUsernameIsTooLong_ShouldReturnError()
        {
            // Arrange
            var model = new SignUpRequest { Email = "test@test.com", Username = new string('a', 21), Password = "password" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username)
                  .WithErrorMessage("'Username' must be between 6 and 20 characters. You entered 21 characters.");
        }

        [Fact]
        public void ValidateSignupRequest_WhenEmailIsEmpty_ShouldReturnError()
        {
            // Arrange
            var model = new SignUpRequest { Email = string.Empty, Password = "password", Username = "username" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("'Email' must not be empty.");
        }

        [Fact]
        public void ValidateSignupRequest_WhenEmailIsInvalid_ShouldReturnError()
        {
            // Arrange
            var model = new SignUpRequest { Email = "invalid-email", Password = "password", Username = "username" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("'Email' is not in the correct format.");
        }

        [Fact]
        public void ValidateSignupRequest_WhenEmailValid_ShouldReturnSuccess()
        {
            // Arrange
            var model = new SignUpRequest { Email = "test@example.com", Password = "password", Username = "username" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void ValidateSignupRequest_WhenUsernameIsValid_ShouldReturnSuccess()
        {
            // Arrange
            var model = new SignUpRequest { Email = "test@test.com",  Username = "username", Password = "password" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Username);
        }
    }
}
