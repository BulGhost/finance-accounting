using System.ComponentModel.DataAnnotations;

namespace FinanceAccounting.WebUI.Entities.Models
{
    public class AuthenticationRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
