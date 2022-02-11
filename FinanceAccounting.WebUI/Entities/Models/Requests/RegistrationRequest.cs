using System.ComponentModel.DataAnnotations;

namespace FinanceAccounting.WebUI.Entities.Models.Requests
{
    public class RegistrationRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]{3,20}$",
            ErrorMessage = "Username must begin with a letter, can contain letters and numbers and must be 3 to 20 characters long")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address. Valid e-mail can contain only latin letters, numbers, '@' and '.'")]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,30}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter and one number and must be 6 to 30 characters long")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool AddBaseCategories { get; set; }
    }
}
