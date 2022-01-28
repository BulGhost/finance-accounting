namespace FinanceAccounting.WebUI.Entities.DTO
{
    public class AuthResponseDto
    {
        public bool IsSucceeded { get; set; }
        public string ErrorMessage { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
