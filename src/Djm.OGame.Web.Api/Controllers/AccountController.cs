using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Account;
using Djm.OGame.Web.Api.Services.Authentication;
using Djm.OGame.Web.Api.Services.OGame;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/users/account")]
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
        [HttpPost,Route("login")]
        public async Task<IActionResult> SignIn([FromBody] LoginBindingModel credentials,CancellationToken cancellation)
        {
            if (credentials == null)
                return BadRequest();

            var player = await AccountService.CheckPasswordAsync(credentials, cancellation);

            if (player == null)
            {
                return BadRequest("Échec de l'authentification");
            }

            HttpContext.Session.SetString("login",credentials.Email);

            var token = JwtFactory.GenerateToken(JwtFactory.GenerateClaims(player.EmailAddress, player.Role));

            HttpContext.Session.SetString("token",token);

            return Ok();
            return Ok(new {
                AccessToken = JwtFactory.GenerateToken(JwtFactory.GenerateClaims(player.EmailAddress,player.Role))
            });
        }

        [AllowAnonymous]
        [Authorize]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost, Route("register")]
        public async Task<IActionResult> SignUp([FromBody] RegisterBindingModel bindingModel,CancellationToken cancellation)
        {
            if (bindingModel == null)
                return BadRequest("Body empty");
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await AccountService.RegisterUser(bindingModel, cancellation);
            }
            catch (OGameException e)
            {
               return BadRequest(e.Message);
            }
            

            return Ok();
        }
        
    }
}