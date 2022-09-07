using BloggingApis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BloggingApis.Services.Abstract
{
    public interface ITokenService
    {
        TokenResponse GetToken(IEnumerable<Claim> claim);
        string GetRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
