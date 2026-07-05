using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Api.Contracts.Request;
using Ticketing.Api.Contracts.Response;
using Ticketing.Application.UseCases.Ticket.CreateTicket;
using Ticketing.Domain.Enums;

namespace Ticketing.Api.Controllers
{
    [ApiController]
    [Route("tickets")]
    [Tags("Tickets")]
    public class TicketController(ICreateTicketUseCase createTicketUseCase,
        ILogger<TicketController> logger) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(CreateTicketResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(CreateTicketResponse), StatusCodes.Status409Conflict)]
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
                var response = new CreateTicketResponse()
                {
                    Success = output.Success,
                    TicketId = output.TicketId,
                    Detail = output.Detail
                };

                if (!output.Success)
                {
                    return output.FailureType switch
                    {
                        CreateTicketFailureType.DuplicateTicket => Conflict(response),
                        _ => BadRequest(response)
                    };
                }

                return Ok(response);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Failed to process Create Ticket request");
                return Problem(title: "Failed to process Create Ticket request", detail: ex.Message);
            }
        }
    }
}
