using System.Threading.Tasks;


namespace Djm.OGame.Web.Api.Services.Mails
{
    public interface IMailService
    {
        Task SendEmailAsync<TModel>(MailTemplate template,string subject, string email,TModel model);
    }

    
}