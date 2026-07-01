namespace Ticketing.Api.Contracts.Request
{
    public class CreateUserRequest
    {
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;

        public string fullName { get; set; } = string.Empty;

    }
}
