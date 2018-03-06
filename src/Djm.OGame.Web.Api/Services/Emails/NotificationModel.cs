namespace Djm.OGame.Web.Api.Services.Emails
{
    public class NotificationModel : MailModel,IMailModel
    {
        public string FromName { get; set; }
        public int Count { get; set; }
    }

    public abstract class MailModel
    {
        public string ToName { get; set; }
        public string Logo { get; set; }
    }

    public interface IMailModel
    {
        string ToName { get; set; }
        string Logo { get; set; }
    }
}