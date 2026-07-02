using System.Security.Claims;
using Ticketing.Application.Abstractions.Security;
using Ticketing.Domain.Models;

namespace Ticketing.Infrastructure.Services.Security
{
    internal class JwtTokenService : ITokenService
    {
        public string GenerateAccessToken(User user)
        {

            throw new NotImplementedException();
        }

        public string GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
