using System.ComponentModel.DataAnnotations;

namespace Djm.OGame.Web.Api.BindingModels.Account
{
    public class LoginBindingModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}