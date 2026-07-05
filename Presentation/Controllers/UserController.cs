using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Api.Contracts.Request;
using Ticketing.Api.Contracts.Response;
using Ticketing.Application.UseCases.User.CreateUser;

namespace Ticketing.Api.Controllers
{
    [ApiController]
    [Route("users")]
    [Tags("Users")]
    public class UserController(ICreateUserUseCase createUserUseCase,
        ILogger<UserController> logger) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateUserResponse>> Process(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
        {
            try
            {
                var input = new CreateUserInput()
                {
                   UserName = request.username,
                   Email = request.email,
                   Password = request.password
                };

                var output = await createUserUseCase.Execute(input, cancellationToken);

                var response = new CreateUserResponse()
                {
                    Success = output.Success,
                    UserId = output.UserId,
                    Detail = output.Detail
                };
                if (!output.Success)
                {
                    return BadRequest(response);
                }

                return Created();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process Create Ticket request");
                return Problem(title: "Failed to process Create User request", detail: ex.Message);
            }
        }
    }
}
