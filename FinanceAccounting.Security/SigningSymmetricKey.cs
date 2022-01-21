using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FinanceAccounting.Security
{
    public class SigningSymmetricKey
    {
        public static SecurityKey GetKey(string secret)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
    }
}
