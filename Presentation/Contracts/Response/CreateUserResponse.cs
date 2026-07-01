namespace Ticketing.Api.Contracts.Response
{
    public class CreateUserResponse
    {
        public bool Success { get; set; }
        public int UserId { get; set; }
        public string? Detail { get; set; }
    }
}
