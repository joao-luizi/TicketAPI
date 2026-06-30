using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Domain.Models;

namespace Ticketing.Application.UseCases.CreateTicket
{
    public  class CreateTicketUseCase(
        ITicketRepository ticketRepository,
        IUserRepository userRepository) : ICreateTicketUseCase
    {

        public async Task<CreateTicketOutput> CreateTicketAsync(CreateTicketInput input, 
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

            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
            {
                return new CreateTicketOutput
                {
                    Success = false,
                    Detail = "User is locked out"
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

            var hasDuplicateTicket = await ticketRepository.HasDuplicateTicketAsync(user.Id, input.Title, cancelationToken);
            if (hasDuplicateTicket == true)
            {
                return new CreateTicketOutput
                {
                    Success = false,
                    Detail = "Duplicate ticket title for the user"
                };
            }

            var newTicket = await ticketRepository.CreateTicketAsync(new Ticket
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
