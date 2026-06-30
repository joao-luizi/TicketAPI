namespace Ticketing.Api.Contracts.Request
{
    public class CreateTicketRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
}
