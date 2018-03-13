using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Account;
using Djm.OGame.Web.Api.Services.Authentication;
using Djm.OGame.Web.Api.Services.OGame;
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
        public async Task<IActionResult> RequestToken([FromBody] LoginBindingModel credentials,CancellationToken cancellation)
        {
            if (credentials == null)
                return BadRequest();

            var player = await AccountService.CheckPasswordAsync(credentials, cancellation);

            if (player == null)
                return BadRequest("Login failure");

            return Ok(new {
                AccessToken = JwtFactory.GenerateToken(JwtFactory.GenerateClaims(player.Id,player.Role))
            });
        }

        [AllowAnonymous]
        [HttpPost, Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterBindingModel bindingModel,CancellationToken cancellation)
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