using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Ticketing.Domain.Models;

namespace Ticketing.Application.Abstractions.Security
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
