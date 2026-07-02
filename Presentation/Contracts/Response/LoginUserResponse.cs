namespace Ticketing.Api.Contracts.Response
{
    public class LoginUserResponse
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Detail { get; set; }
    }
}
