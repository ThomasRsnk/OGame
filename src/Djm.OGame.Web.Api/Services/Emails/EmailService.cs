using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Mvc.Options;
using Microsoft.Extensions.Options;
using RazorEngine.Templating;


namespace Djm.OGame.Web.Api.Services.Emails
{
    public enum EmailTemplate
    {
        SendNotification=0,
    }

    public class EmailService : IEmailService
    {
        private SmtpClient Smtp { get; }
        public MailOptions Opt { get; }
        private MailAddress FromAddress { get; set; }

        public EmailService(IOptionsSnapshot<MailOptions> opt)
        {
            Opt = opt.Value;

            FromAddress = new MailAddress(Opt.Address, Opt.DisplayName);

            Smtp = new SmtpClient()
            {
                Host = Opt.Host,
                Port = int.Parse(Opt.Port),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(FromAddress.Address, Opt.Password),
                Timeout = 5000
            };
        }
        
        public async Task SendHtmlMailAsync(EmailTemplate type,string address,IMailModel model,CancellationToken ct)
        {
            var toAddress = new MailAddress(address,model.ToName );

            var subject = Opt.Templates[(int) type].Subject;
            var path = Path.Combine(Opt.Directory,Opt.Templates[(int) type].Path);
        
            var templateService = new TemplateService();
            var emailHtmlBody = templateService.Parse(File.ReadAllText(path), model, null, null);

            using (var message = new MailMessage(FromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = emailHtmlBody
            })
            {
                await Smtp.SendMailAsync(message);
            }
        }


    }
}