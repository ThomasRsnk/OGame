using System.ComponentModel.DataAnnotations;

namespace Djm.OGame.Web.Api.BindingModels.Account
{
    public class LoginBindingModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Veuillez entrer une adresse e-mail valide")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }
    }
}