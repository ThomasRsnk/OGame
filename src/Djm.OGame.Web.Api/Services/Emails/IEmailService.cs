using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Mvc.Options;

namespace Djm.OGame.Web.Api.Services.Emails
{
    public interface IEmailService
    {
        Task SendHtmlMailAsync(EmailTemplate type, string address,IMailModel model, CancellationToken ct = default(CancellationToken));

        MailOptions Opt { get; }
    }
}