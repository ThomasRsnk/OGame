using Microsoft.IdentityModel.Tokens;

namespace Djm.OGame.Web.Api.Mvc.Options
{
    public class TokenOptions
    {
        public SymmetricSecurityKey Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}