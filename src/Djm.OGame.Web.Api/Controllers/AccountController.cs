using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellation, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(model);

            var result = await SignInManager.PasswordSignInAsync(model.Email,
                model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
                return RedirectToLocal(returnUrl);
            
            if (result.IsLockedOut)
                return View("Lockout");
            
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            
            return RedirectToAction("Index","Articles");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model,CancellationToken cancellation, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser {UserName = model.Username, Email = model.Email};

            var result = await UserManager.CreateAsync(user, model.Password);

            await UserManager.AddToRoleAsync(user, "Membre");

            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Articles");
            }

            AddErrors(result);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
               
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
           
            // If the user does not have an account, then ask the user to create an account.
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LoginProvider"] = info.LoginProvider;
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            
            
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await SignInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Articles");
            }
        }

        private async Task CreateRolesandUsers()
        {
            if (!await RoleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole {Name = "Admin"};
                await RoleManager.CreateAsync(role);

                
                var user = new ApplicationUser
                {
                    UserName = "jean.roger444@gmail.com",
                    Email = "jean.roger444@gmail.com"
                };

                var chkUser = await UserManager.CreateAsync(user,"Azerty99");
 
                if (chkUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user, "Admin");
                }
            }

            if (!await RoleManager.RoleExistsAsync("Moderateur"))
            {
                var role = new IdentityRole {Name = "Moderateur" };
                await RoleManager.CreateAsync(role);

            }

            if (!await RoleManager.RoleExistsAsync("Membre"))
            {
                var role = new IdentityRole {Name = "Membre" };
                await RoleManager.CreateAsync(role);
            }
        }

    }
}