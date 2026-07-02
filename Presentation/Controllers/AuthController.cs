using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Api.Contracts.Request;
using Ticketing.Api.Contracts.Response;
using Ticketing.Application.UseCases.Ticket.Authentication;


namespace Ticketing.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(ILoginUseCase loginUseCase) : ControllerBase
    {
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginUserResponse>> Login(
            [FromBody] LoginUserRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var input = new LoginInput()
                {
                    UserName = request.UserName,
                    Password = request.Password
                };

                var output = await loginUseCase.Execute(input, cancellationToken);

                return Ok(new LoginUserResponse()
                {
                    Success = output.Success,
                    Token = output.Token,
                    Detail = output.Detail
                });
            }
            catch(Exception ex)
            {
                return Problem(title: $"Failed to process user login for {request.UserName}", detail: ex.Message);
            }
        }
    }
}
