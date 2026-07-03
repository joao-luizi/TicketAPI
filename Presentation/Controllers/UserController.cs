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
    public class UserController(ICreateUserUseCase createUserUseCase) : ControllerBase
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

                return new CreateUserResponse
                {
                    Success = output.Success,
                    UserId = output.UserId,
                    Detail = output.Detail
                };
            }
            catch (Exception ex)
            {
                return Problem(title: "Failed to process Create User request", detail: ex.Message);
            }
        }
    }
}
