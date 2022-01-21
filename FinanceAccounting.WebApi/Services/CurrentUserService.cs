using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinanceAccounting.BusinessLogic.Abstractions;
using Microsoft.AspNetCore.Http;

namespace FinanceAccounting.WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                string id = _httpContextAccessor.HttpContext?.User
                    .FindFirstValue(JwtRegisteredClaimNames.NameId);
                return string.IsNullOrEmpty(id) ? 0 : int.Parse(id);
            }
        }
    }
}
