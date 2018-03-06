using System.Collections.Generic;

namespace Djm.OGame.Web.Api.Mvc.Options
{
    public class MailOptions
    {
        public string Address { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Logo { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Directory { get; set; }
        public Template[] Templates { get; set; }
    }

    public class Template
    {
        public string Subject { get; set; }
        public string Path { get; set; }
    }
}