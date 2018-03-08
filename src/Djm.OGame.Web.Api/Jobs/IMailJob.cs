using System.Threading.Tasks;
using Djm.OGame.Web.Api.Services.Mails.Models;

namespace Djm.OGame.Web.Api.Jobs
{
    public interface IMailJob
    {
        Task SendNotificationAsync(string email, NotificationModel model);
    }
}