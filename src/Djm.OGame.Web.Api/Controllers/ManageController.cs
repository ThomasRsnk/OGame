using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.ViewModels.Manage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }

        public ManageController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Roles()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Roles(AlterRoleViewModel viewModel,CancellationToken cancellation)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var user = await UserManager.FindByEmailAsync(viewModel.UserName);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "L'utilisateur <" + viewModel.UserName + "> n'existe pas");
                return View();
            }
                

            await UserManager.AddToRoleAsync(user, viewModel.Role);

            ViewData["msg"] = "Changement effectué !";

            return View();
        }
    }
}