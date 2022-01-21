using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.UserDto;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Exceptions;
using FinanceAccounting.Domain.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceAccounting.BusinessLogic.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserRegistrationResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly ICategoryRepo _categoryRepo;

        public RegisterUserCommandHandler(UserManager<User> userManager, ICategoryRepo categoryRepo)
        {
            _userManager = userManager;
            _categoryRepo = categoryRepo;
        }

        public async Task<UserRegistrationResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User {Email = request.Email, UserName = request.UserName};
            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                string errorMessage = GetErrorMessage(result.Errors.FirstOrDefault());
                throw new UserCreationException(errorMessage);
            }

            if (request.AddBaseCategories)
            {
                await AddBaseCategoriesToUser(user, cancellationToken);
            }

            return new UserRegistrationResponse
            {
                CreatedUserName = user.UserName,
                IsSucceeded = true
            };
        }

        private string GetErrorMessage(IdentityError error)
        {
            return error?.Code switch
            {
                nameof(IdentityErrorDescriber.DuplicateEmail) => "A user with this email already exists",
                nameof(IdentityErrorDescriber.DuplicateUserName) => "A user with this name already exists",
                nameof(IdentityErrorDescriber.PasswordTooShort) => "Password must be at least 6 characters long",
                nameof(IdentityErrorDescriber.PasswordRequiresDigit) => "Password must contain at least one numeric value",
                nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric) => "Password must contain at least one non-alphanumeric character",
                nameof(IdentityErrorDescriber.PasswordRequiresLower) => "Password must contain at least one lower case letter",
                nameof(IdentityErrorDescriber.PasswordRequiresUpper) => "Password must contain at least one upper case letter",
                _ => "Something went wrong"
            };
        }

        private Task AddBaseCategoriesToUser(User user, CancellationToken cancellationToken)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<CreateCategoryDto, Category>()
                    .ForMember(c => c.UserId, opt => opt.MapFrom(_ => user.Id))
                    .ForMember(c => c.CategoryName, opt => opt.MapFrom(dto => dto.Name)));

            IMapper mapper = config.CreateMapper();
            var baseCategories = mapper.Map<IEnumerable<Category>>(RegisterUserHelper.GetBaseCategories());
            return _categoryRepo.AddRangeAsync(baseCategories, true, cancellationToken);
        }
    }
}
