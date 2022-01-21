using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceAccounting.WebApi.Controllers.Base
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseCrudController : ControllerBase
    {
        private readonly IMediator _mediator;

        protected IMediator Mediator =>
            _mediator ?? HttpContext.RequestServices.GetService<IMediator>();

        protected BaseCrudController(IMediator mediator)
        {
            _mediator = mediator;
        }

        internal int UserId =>
            User?.Identity == null || !User.Identity.IsAuthenticated || User.FindFirst(JwtRegisteredClaimNames.NameId) == null
                ? 0
                : int.Parse(User.FindFirst(JwtRegisteredClaimNames.NameId)!.Value);
    }
}
