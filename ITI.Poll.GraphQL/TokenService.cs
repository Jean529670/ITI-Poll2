using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ITI.Poll.GraphQL.Services
{
    public sealed class TokenService
    {
        readonly TokenServiceOptions _options;

        public TokenService(IOptions<TokenServiceOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateToken(string userId)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim( JwtRegisteredClaimNames.Sub, userId )
            };

            var jwt = new JwtSecurityToken(
                claims: claims,
                signingCredentials: _options.SigningCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }

    public sealed class TokenServiceOptions
    {
        public SigningCredentials SigningCredentials { get; set; }
    }
}
