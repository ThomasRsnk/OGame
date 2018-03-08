using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Djm.OGame.Web.Api.Services.Mails
{
    public class MailService : IMailService
    {
        public ISmtpClient SmtpClient { get; }
        public RazorViewToStringRenderer ViewRenderer { get; }
        private MailOptions Opt { get; set; }

        public MailService(ISmtpClient smtpClient,RazorViewToStringRenderer viewRenderer, IOptions<MailOptions> opt)
        {
            SmtpClient = smtpClient;
            ViewRenderer = viewRenderer;
            Opt = opt.Value;
        }

        public async Task SendEmailAsync<TModel>(MailTemplate template,string subject, string email,TModel model)
        {
            var html = await ViewRenderer.RenderViewToStringAsync(Opt.Smtp.Templates[(int)template], model);

            var mailMessage = new MailMessage(Opt.From, email)
            {
                Subject = subject,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };

            mailMessage.AddHtmlView(html);

            await SmtpClient.SendMailAsync(mailMessage);
        }
    }

    public enum MailTemplate
    {
        SendNotification
    }
}