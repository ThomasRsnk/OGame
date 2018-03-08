using System.Threading.Tasks;
using Djm.OGame.Web.Api.Services.Mails;
using Djm.OGame.Web.Api.Services.Mails.Models;
using Hangfire;

namespace Djm.OGame.Web.Api.Jobs
{
    public class MailMailJob : IMailJob
    {
        public IMailService MailService { get; }

        public MailMailJob(IMailService mailService)
        {
            MailService = mailService;
        }

        [Queue(HangfireQueues.Email)]
        public async Task SendNotificationAsync(string email,NotificationModel model)
        {
            const string subject = "Un joueur vous a ajouté à ses favoris";

            await MailService.SendEmailAsync(MailTemplate.SendNotification, subject, email, model);
        }

    }
}