using System.ComponentModel.DataAnnotations;

namespace Djm.OGame.Web.Api.ViewModels.Account
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}