using FinanceAccounting.BusinessLogic.Users.Commands.RegisterUser;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Users.Commands.RegisterUser
{
    public class RegisterUserCommandValidatorTests
    {
        private readonly RegisterUserCommandValidator _validator = new();

        [Theory]
        [InlineData("7User")]
        [InlineData("user$user&")]
        [InlineData("ur")]
        [InlineData("userName123UserName123")]
        public void Should_have_error_when_specified_invalid_username(string userName)
        {
            var command = new RegisterUserCommand(userName, "myemail@mail.com",
                "PassWord123", "PassWord123");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.UserName);
        }

        [Theory]
        [InlineData("myemail.mail.com")]
        [InlineData("@mail.com")]
        public void Should_have_error_when_specified_invalid_email(string email)
        {
            var command = new RegisterUserCommand("UserName", email,
                "PassWord123", "PassWord123");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Theory]
        [InlineData("short")]
        [InlineData("TooLongPassword0TooLongPassword")]
        public void Should_have_error_when_specified_password_has_invalid_length(string password)
        {
            var command = new RegisterUserCommand("UserName", "myemail@mail.com",
                password, password);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.Password);
        }

        [Fact]
        public void Should_have_error_when_password_mismatch()
        {
            var command = new RegisterUserCommand("UserName", "myemail@mail.com",
                "MyPassword1", "MyPassword2");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.ConfirmPassword);
        }

        [Fact]
        public void Should_not_have_error_when_command_is_valid()
        {
            var command = new RegisterUserCommand("UserName", "myemail@mail.com",
                "MyPassword", "MyPassword");

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
