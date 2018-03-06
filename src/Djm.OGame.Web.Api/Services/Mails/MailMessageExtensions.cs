using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Total.AutoCare.Web.Helpers.Mail
{
    public static class MailMessageExtensions
    {
        public static MailMessage AddHtmlView(this MailMessage mailMessage, string html)
        {
            var alternateView = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, "text/html");

            alternateView.TransferEncoding = TransferEncoding.QuotedPrintable;
            mailMessage.AlternateViews.Add(alternateView);

            return mailMessage;
        }
        public static MailMessage AddHtmlView(this MailMessage mailMessage, string html, params Attachment[] attachment)
        {
            var linkedResources = attachment.Select(a =>
            {
                var linkedResource = new LinkedResource(a.Stream)
                {
                    ContentId = a.Name,
                    TransferEncoding = TransferEncoding.Base64,
                    ContentType = { Name = a.Name },
                    ContentLink = new Uri($"cid:{a.Name}")
                };
                return linkedResource;
            });

            mailMessage.AddHtmlView(html, linkedResources.ToArray());

            return mailMessage;
        }

        public static MailMessage AddHtmlView(this MailMessage mailMessage, string html, params LinkedResource[] linkedResources)
        {
            var alternateView = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, "text/html");

            foreach (var linkedResource in linkedResources)
                alternateView.LinkedResources.Add(linkedResource);

            alternateView.TransferEncoding = TransferEncoding.QuotedPrintable;
            mailMessage.AlternateViews.Add(alternateView);

            return mailMessage;
        }
    }

    public class Attachment
    {
        public string Name { get; set; }
        public Stream Stream { get; set; }
    }
}
