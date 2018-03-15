using System.ComponentModel.DataAnnotations;

namespace Djm.OGame.Web.Api.BindingModels.Account
{
    public class RegisterBindingModel : LoginBindingModel
    {
        [Required]
        [Display(Name = "Joueur")]
        public int PlayerId { get; set; }
        [Required]
        [Display(Name = "Univers")]
        public int UniverseId { get; set; }

        public string ErrorMsg { get; set; }
    }
}