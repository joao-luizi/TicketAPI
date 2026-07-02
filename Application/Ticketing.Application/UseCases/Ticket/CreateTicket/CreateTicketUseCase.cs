using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Domain.Models;

namespace Ticketing.Application.UseCases.Ticket.CreateTicket
{
    public  class CreateTicketUseCase(
        ITicketRepository ticketRepository,
        IUserRepository userRepository) : ICreateTicketUseCase
    {

        public async Task<CreateTicketOutput> Execute(CreateTicketInput input, 
            CancellationToken cancelationToken)
        {
            var user = await userRepository.GetUserByEmail(input.UserEmail, cancelationToken);
            if(user == null)
            {
                return new CreateTicketOutput
                {
                    Success = false,
                    Detail = "User not found"
                };
            }

            if (user.IsActive == false)
            {
                return new CreateTicketOutput
                {
                    Success = false,
                    Detail = "User is not active"
                };
            }

            var hasDuplicateTicket = await ticketRepository.HasDuplicateTicketAsync(user.UserName, input.Title, cancelationToken);
            if (hasDuplicateTicket == true)
            {
                return new CreateTicketOutput
                {
                    Success = false,
                    Detail = "Duplicate ticket title for the user"
                };
            }

            var newTicket = await ticketRepository.CreateTicketAsync(new Domain.Models.Ticket
            {
                Title = input.Title,
                Description = input.Description,
                UserName = user.UserName,
                UserEmail = user.Email,
                CreatedAt = DateTime.UtcNow
            }, cancelationToken);

            return new CreateTicketOutput
            {
                Success = true,
                TicketId = newTicket.Id,
                Detail = "Ticket created successfully"
            };

        }

    }
}
