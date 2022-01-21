using System.Linq;
using System.Threading;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.UserDto;
using FinanceAccounting.BusinessLogic.Tests.Stubs;
using FinanceAccounting.BusinessLogic.Users.Commands.RegisterUser;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Exceptions;
using FinanceAccounting.Domain.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace FinanceAccounting.BusinessLogic.Tests.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock = new(
            new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);

        private readonly ICategoryRepo _categoryRepo = new CategoryRepoStub();

        private RegisterUserCommand _command =
            new("UserName", "user@mail.com", "password", "password");

        [Fact]
        public void Fail_user_creation_due_to_duplicate_email()
        {
            IdentityError[] errors = { new() {Code = "DuplicateEmail" } };
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors));
            var commandHandler = new RegisterUserCommandHandler(_userManagerMock.Object, _categoryRepo);

            commandHandler.Invoking(h => h.Handle(_command, CancellationToken.None))
                .Should().ThrowAsync<UserCreationException>()
                .WithMessage("A user with this email already exists");
        }

        [Fact]
        public void Fail_user_creation_due_to_duplicate_username()
        {
            IdentityError[] errors = { new() { Code = "DuplicateUserName" } };
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors));
            var commandHandler = new RegisterUserCommandHandler(_userManagerMock.Object, _categoryRepo);

            commandHandler.Invoking(h => h.Handle(_command, CancellationToken.None))
                .Should().ThrowAsync<UserCreationException>()
                .WithMessage("A user with this name already exists");
        }

        [Theory]
        [InlineData("PasswordTooShort", "Password must be at least 6 characters long")]
        [InlineData("PasswordRequiresDigit", "Password must contain at least one numeric value")]
        [InlineData("PasswordRequiresNonAlphanumeric", "Password must contain at least one non-alphanumeric character")]
        [InlineData("PasswordRequiresLower", "Password must contain at least one lower case letter")]
        [InlineData("PasswordRequiresUpper", "Password must contain at least one upper case letter")]
        public void Fail_user_creation_due_to_invalid_password(string errorCode, string errorMessage)
        {
            IdentityError[] errors = { new() { Code = errorCode } };
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors));
            var commandHandler = new RegisterUserCommandHandler(_userManagerMock.Object, _categoryRepo);

            commandHandler.Invoking(h => h.Handle(_command, CancellationToken.None))
                .Should().ThrowAsync<UserCreationException>()
                .WithMessage(errorMessage);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Create_new_user_if_command_is_correct(bool addBaseCategories)
        {
            int expectedUserCategoriesCount = 0;
            if (addBaseCategories)
            {
                _command = new RegisterUserCommand("UserName", "user@mail.com", "password", "password", true);
                expectedUserCategoriesCount = RegisterUserHelper.GetBaseCategories().Count;
            }

            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            var commandHandler = new RegisterUserCommandHandler(_userManagerMock.Object, _categoryRepo);
            var expectedResult = new UserRegistrationResponse {CreatedUserName = "UserName", IsSucceeded = true};

            UserRegistrationResponse actualResult = commandHandler.Handle(_command, CancellationToken.None).Result;
            int actualUserCategoriesCount = _categoryRepo.GetAllAsync().Result.Count(c => c.UserId == 0);

            actualUserCategoriesCount.Should().Be(expectedUserCategoriesCount);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
