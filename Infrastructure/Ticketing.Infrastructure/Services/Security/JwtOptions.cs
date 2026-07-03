using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Infrastructure.Services.Security
{

    public sealed class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Key { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpiryMinutes { get; init; }
    }

}
