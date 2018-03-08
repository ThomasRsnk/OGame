using System.Threading.Tasks;
using Djm.OGame.Web.Api.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Account")]
    public class AccountController : Controller
    {
        public IJwtFactory JwtFactory { get; }
        public IAccountService AccountService { get; }

        public AccountController(IJwtFactory jwtFactory, IAccountService accountService)
        {
            JwtFactory = jwtFactory;
            AccountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost,Route("Login")]
        public async Task<IActionResult> RequestToken([FromBody] LoginBindingModel credentials)
        {
            if (credentials == null)
                return BadRequest();

            var player = await AccountService.CheckPasswordAsync(credentials);

            if (player == null)
                return BadRequest("Login failure");

            return Ok(new {
                AccessToken = JwtFactory.GenerateToken(JwtFactory.GenerateClaims(player.Login,player.Role))
            });
        }
        
    }
}