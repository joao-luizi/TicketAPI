using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Api.Contracts.Request;
using Ticketing.Api.Contracts.Response;
using Ticketing.Application.UseCases.Ticket.CreateTicket;

namespace Ticketing.Api.Controllers
{
    [ApiController]
    [Route("tickets")]
    [Tags("Tickets")]
    public class TicketController(ICreateTicketUseCase createTicketUseCase) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(CreateTicketResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateTicketResponse>> Process(
        [FromBody] CreateTicketRequest request,
        CancellationToken cancellationToken)
        {
            try
            {
                var input = new CreateTicketInput()
                {
                    Title = request.Title,
                    Description = request.Description,
                    UserEmail = request.UserEmail
                };

                var output = await createTicketUseCase.Execute(input, cancellationToken);
                
                return Ok(new CreateTicketResponse
                {
                    Success = output.Success,
                    TicketId = output.TicketId,
                    Detail = output.Detail
                });
            }
            catch(Exception ex)
            {
                return Problem(title: "Failed to process Create Ticket request", detail: ex.Message);
            }
        }
        

        

    }
}
