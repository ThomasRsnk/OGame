using System.ComponentModel.DataAnnotations;

namespace Djm.OGame.Web.Api.BindingModels.Account
{
    public class RegisterBindingModel : LoginBindingModel
    {
        [Required]
        public int PlayerId { get; set; }
        [Required]
        public int UniverseId { get; set; }
    }
}