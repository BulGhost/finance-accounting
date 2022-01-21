using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.UserDto;
using FinanceAccounting.BusinessLogic.Users.Commands.RefreshToken;
using FinanceAccounting.BusinessLogic.Users.Commands.RegisterUser;
using FinanceAccounting.BusinessLogic.Users.Queries.AuthenticateUser;
using FinanceAccounting.BusinessLogic.Users.Queries.LogoutUser;
using FinanceAccounting.WebApi.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace FinanceAccounting.WebApi.Controllers
{
    [ApiVersionNeutral]
    [Route("api/v{version:apiVersion}")]
    public class UserController : BaseCrudController
    {
        private readonly TokenValidationParameters _tokenValidationParameters;


        public UserController(IMediator mediator, TokenValidationParameters tokenValidationParameters)
            : base(mediator)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        /// <summary>
        /// Creates new application user and adds base income/expense categories to him if required.
        /// To add base categories set addBaseCategories equals true (default = false)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/register
        ///     {
        ///         "userName": "UserName",
        ///         "email": "my.email@mail.com",
        ///         "password": "Password123",
        ///         "confirmPassword": "Password123",
        ///         "addBaseCategories": true
        ///     }
        /// </remarks>
        /// <param name="command">New user details including username, email, password and addBaseCategories flag</param>
        /// <returns>Created user name and command result</returns>
        /// <response code="200">Success</response>
        /// <response code="400">If the submitted user details are invalid</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            UserRegistrationResponse response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Authenticates user with username and password
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/login
        ///     {
        ///         "userName": "UserName",
        ///         "password": "Password123",
        ///     }
        /// </remarks>
        /// <param name="query">User authorization data</param>
        /// <returns>Username, access and refresh tokens</returns>
        /// <response code="200">Success</response>
        /// <response code="400">If the authentication details are invalid</response>
        /// <response code="401">If the specified password is incorrect</response>
        /// <response code="404">If the user with the specified name is not found</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(AuthenticateUserQuery query)
        {
            UserAuthenticationResponse response = await Mediator.Send(query);
            return Ok(response);
        }

        /// <summary>
        /// Allows get new access and refresh tokens if the old access token is expired
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/refresh-token
        ///     {
        ///         "accessToken": "8lkyB6SJRJfE2Eo2Q6oR4Ugk5I1IZ47Z413Ygadq",
        ///         "refreshToken": "vkoJ8gWG2Z0BLAsmn8cX"
        ///     }
        /// </remarks>
        /// <param name="command">Expired access token and active refresh token</param>
        /// <returns>Username, new access and new refresh tokens</returns>
        /// <response code="200">Success</response>
        /// <response code="400">If one or both of the specified tokens are empty</response>
        /// <response code="401">If tokens validation fails</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            UserAuthenticationResponse response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Deletes all issued refresh tokens so that they cannot be used later
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     DELETE /api/logout
        /// </remarks>
        /// <returns>No content</returns>
        /// <response code="204">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            var query = new LogoutUserQuery(UserId);
            await Mediator.Send(query);
            return NoContent();
        }
    }
}
