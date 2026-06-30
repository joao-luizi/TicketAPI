namespace Ticketing.Api.Contracts.Response
{
    public class CreateTicketResponse
    {
        public bool Success { get; set; }
        public int TicketId { get; set; }
        public string? Detail { get; set; }
    }
}
