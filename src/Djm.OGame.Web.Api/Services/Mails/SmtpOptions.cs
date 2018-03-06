using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Total.AutoCare.Web.Helpers.Mail
{
    public class SmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DeliveryMethod { get; set; }
        public string PickupDirectoryLocation { get; set; }

        public Dictionary<string, string> AdditionalHeaders { get; set; } = new Dictionary<string, string>();
    }
}
