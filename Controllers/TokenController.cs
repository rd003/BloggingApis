using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloggingApis.Models.Domain;
using BloggingApis.Models.DTO;
using BloggingApis.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloggingApis.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly DatabaseContext _context;
        public TokenController(ITokenService tokenService,DatabaseContext ctx)
        {
            this._tokenService = tokenService;
            this._context = ctx;
        }
        [HttpPost]
        public IActionResult Refresh(RefreshTokenRequestModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = _context.TokenInfo.SingleOrDefault(u => u.UserName == username);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");
            var newAccessToken = _tokenService.GetToken(principal.Claims);
            var newRefreshToken = _tokenService.GetRefreshToken();
            user.RefreshToken = newRefreshToken;
            _context.SaveChanges();
            return Ok(new RefreshTokenRequestModel()
            {
                AccessToken = newAccessToken.TokenString,
                RefreshToken = newRefreshToken
            });
        }
        [HttpPost, Authorize]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            var user = _context.TokenInfo.SingleOrDefault(u => u.UserName == username);
            if (user is null)
                return BadRequest();
            user.RefreshToken = null;
            _context.SaveChanges();
            return Ok(true);
        }
    }
}
