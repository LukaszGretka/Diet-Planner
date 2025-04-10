using DietPlanner.Api.Models.Account;
using DietPlanner.Api.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace DietPlanner.Api.UnitTests.Validators
{
    public class SignUpValidatorTests
    {
        private readonly SignUpValidator _validator;

        public SignUpValidatorTests()
        {
            _validator = new SignUpValidator();
        }

        [Fact]
        public void Should_HaveError_When_UsernameIsEmpty()
        {
            // Arrange
            var model = new SignUpRequest { Email = "test@test.com", Username = string.Empty };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username)
                  .WithErrorMessage("'Username' must not be empty.");
        }

        [Fact]
        public void Should_HaveError_When_UsernameIsTooShort()
        {
            // Arrange
            var model = new SignUpRequest { Email = "test@test.com", Username = "abc" };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username)
                  .WithErrorMessage("'Username' must be between 6 and 20 characters. You entered 3 characters.");
        }

        [Fact]
        public void Should_HaveError_When_UsernameIsTooLong()
        {
            // Arrange
            var model = new SignUpRequest { Email = "test@test.com", Username = new string('a', 21) };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username)
                  .WithErrorMessage("'Username' must be between 6 and 20 characters. You entered 21 characters.");
        }

        [Fact]
        public void Should_HaveError_When_EmailIsEmpty()
        {
            // Arrange
            var model = new SignUpRequest { Email = string.Empty };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("'Email' must not be empty.");
        }

        [Fact]
        public void Should_HaveError_When_EmailIsInvalid()
        {
            // Arrange
            var model = new SignUpRequest { Email = "invalid-email" };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("'Email' is not in the correct format.");
        }

        [Fact]
        public void Should_NotHaveError_When_EmailIsValid()
        {
            // Arrange
            var model = new SignUpRequest { Email = "test@example.com" };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_NotHaveError_When_UsernameIsValid()
        {
            // Arrange
            var model = new SignUpRequest { Email = "test@test.com", Username = "ValidUser" };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Username);
        }
    }
}
