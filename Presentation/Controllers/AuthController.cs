using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Api.Contracts.Request;
using Ticketing.Api.Contracts.Response;
using Ticketing.Application.UseCases.Ticket.Authentication;


namespace Ticketing.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(ILoginUseCase loginUseCase,
        ILogger<AuthController> logger) : ControllerBase
    {
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status401Unauthorized)]
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
                var response = new LoginUserResponse()
                {
                    Success = output.Success,
                    Token = output.Token,
                    Detail = output.Detail
                };

                if (output.Success == false)
                {
                    return Unauthorized(response);
                }

                return Ok(response);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Failed to process user login for {UserName}", request.UserName);
                return Problem(title: $"Failed to process user login for {request.UserName}", detail: ex.Message);
            }
        }
    }
}
