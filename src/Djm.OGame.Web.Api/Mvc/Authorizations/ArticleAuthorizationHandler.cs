using System.Linq;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Djm.OGame.Web.Api.Mvc.Authorizations
{
    public class ArticleAuthorizationHandler : AuthorizationHandler<SameAuthorRequirement,Article>
    {
        protected override  Task HandleRequirementAsync(AuthorizationHandlerContext context, SameAuthorRequirement requirement,
            Article resource)
        {
            if (context.User.Claims.First().Value == resource.AuthorEmail)
                context.Succeed(requirement);
            else
                context.Fail();
            return Task.CompletedTask;
        }
    }

    public class SameAuthorRequirement : IAuthorizationRequirement { }
}