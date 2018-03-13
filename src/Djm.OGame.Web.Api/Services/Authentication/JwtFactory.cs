using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Djm.OGame.Web.Api.Mvc.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Djm.OGame.Web.Api.Services.Authentication
{
    public class JwtFactory : IJwtFactory
    {
        private TokenOptions Opt { get; set; }

        public JwtFactory(IOptions<TokenOptions> opt)
        {
            Opt = opt.Value;
        }

        public string GenerateToken(Claim[] claims)
        {                       
            var jwt = new JwtSecurityToken(
                Opt.Issuer,
                Opt.Audience,
                claims,
                expires:DateTime.Now.AddMinutes(90),
                signingCredentials: new SigningCredentials(Opt.Key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }

        public Claim[] GenerateClaims(int id,string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,ToUnixEpochDate(DateTime.Now).ToString()),
                new Claim(JwtClaimIdentifiers.Role,role)
            };

            return claims;
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);
    }
}