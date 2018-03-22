using Microsoft.AspNetCore.Identity;

namespace Djm.OGame.Web.Api.Dal.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicturePath { get; set; }
    }
}