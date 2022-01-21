namespace FinanceAccounting.BusinessLogic.Common.Models
{
    public class AuthenticationConfig
    {
        public string AccessTokenSecret { get; set; }
        public double AccessTokenExpirationMinutes { get; set; }
        public double RefreshTokenExpirationDays { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SigningAlgorithm { get; set; }
    }
}
