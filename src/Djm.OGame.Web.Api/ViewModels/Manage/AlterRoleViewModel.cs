using System.ComponentModel.DataAnnotations;

namespace Djm.OGame.Web.Api.ViewModels.Manage
{
    public class AlterRoleViewModel
    {
        [Required]
        [Display(Name = "Utilisateur")]
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}