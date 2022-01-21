namespace FinanceAccounting.BusinessLogic.Common.DataTransferObjects.UserDto
{
    public class UserAuthenticationResponse
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
