using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Djm.OGame.Web.Api.Services.Mails
{
    public class DefaultSmtpClient : ISmtpClient
    {
        private readonly SmtpClient _innerSmtpClient;

        private readonly Dictionary<string, string> _additionalHeaders;

        public DefaultSmtpClient(SmtpClient innerSmtpClient)
        {
            _innerSmtpClient = innerSmtpClient;
        }

        public DefaultSmtpClient(SmtpOptions options)
        {

            var deliveryMethod = SmtpDeliveryMethod.Network;
            Enum.TryParse(options.DeliveryMethod, true, out deliveryMethod);

            switch (deliveryMethod)
            {
                case SmtpDeliveryMethod.Network:
                    _innerSmtpClient = new SmtpClient
                    {
                        Host = options.Host,
                        Port = options.Port,
                        EnableSsl = options.EnableSsl,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(options.Username, options.Password),
                        DeliveryMethod = deliveryMethod
                    };
                    break;

                case SmtpDeliveryMethod.SpecifiedPickupDirectory:
                    _innerSmtpClient = new SmtpClient
                    {
                        DeliveryMethod = deliveryMethod,
                        PickupDirectoryLocation = options.PickupDirectoryLocation
                    };
                    break;

                case SmtpDeliveryMethod.PickupDirectoryFromIis:
                    throw new NotImplementedException("PickupDirectoryFromIis not implemented");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _additionalHeaders = new Dictionary<string, string>();

            if (options.AdditionalHeaders != null)
                foreach (var optionsAdditionalHeader in options.AdditionalHeaders)
                {
                    _additionalHeaders.Add(optionsAdditionalHeader.Key, optionsAdditionalHeader.Value);
                }
        }

        private MailMessage addAdditionalHeaders(MailMessage message)
        {
            foreach (var additionalHeader in _additionalHeaders)
                if (!message.Headers.AllKeys.Contains(additionalHeader.Key))
                    message.Headers[(string)additionalHeader.Key] = additionalHeader.Value;

            return message;
        }

        #region Implementation of ISmtpClient

        public X509CertificateCollection ClientCertificates => _innerSmtpClient.ClientCertificates;

        public ICredentialsByHost Credentials
        {
            get => _innerSmtpClient.Credentials;
            set => _innerSmtpClient.Credentials = value;
        }

        public SmtpDeliveryFormat DeliveryFormat
        {
            get => _innerSmtpClient.DeliveryFormat;
            set => _innerSmtpClient.DeliveryFormat = value;
        }

        public SmtpDeliveryMethod DeliveryMethod
        {
            get => _innerSmtpClient.DeliveryMethod;
            set => _innerSmtpClient.DeliveryMethod = value;
        }

        public bool EnableSsl
        {
            get => _innerSmtpClient.EnableSsl;
            set => _innerSmtpClient.EnableSsl = value;
        }

        public string Host
        {
            get => _innerSmtpClient.Host;
            set => _innerSmtpClient.Host = value;
        }

        public string PickupDirectoryLocation
        {
            get => _innerSmtpClient.PickupDirectoryLocation;
            set => _innerSmtpClient.PickupDirectoryLocation = value;
        }

        public int Port
        {
            get => _innerSmtpClient.Port;
            set => _innerSmtpClient.Port = value;
        }

        public ServicePoint ServicePoint => _innerSmtpClient.ServicePoint;

        public string TargetName
        {
            get => _innerSmtpClient.TargetName;
            set => _innerSmtpClient.TargetName = value;
        }

        public int Timeout
        {
            get => _innerSmtpClient.Timeout;
            set => _innerSmtpClient.Timeout = value;
        }

        public bool UseDefaultCredentials
        {
            get => _innerSmtpClient.UseDefaultCredentials;
            set => _innerSmtpClient.UseDefaultCredentials = value;
        }

        public event SendCompletedEventHandler SendCompleted
        {
            add => _innerSmtpClient.SendCompleted += value;
            remove => _innerSmtpClient.SendCompleted -= value;
        }

        public void Dispose()
        {
            _innerSmtpClient.Dispose();
        }

        public void Send(MailMessage message)
        {
            _innerSmtpClient.Send(addAdditionalHeaders(message));
        }

        public void Send(string @from, string recipients, string subject, string body)
        {
            _innerSmtpClient.Send(@from, recipients, subject, body);
        }

        public void SendAsync(MailMessage message, object userToken)
        {
            _innerSmtpClient.SendAsync(addAdditionalHeaders(message), userToken);
        }

        public void SendAsync(string @from, string recipients, string subject, string body, object userToken)
        {
            _innerSmtpClient.SendAsync(@from, recipients, subject, body, userToken);
        }

        public void SendAsyncCancel()
        {
            _innerSmtpClient.SendAsyncCancel();
        }

        public Task SendMailAsync(MailMessage message)
        {
            return _innerSmtpClient.SendMailAsync(addAdditionalHeaders(message));
        }

        public Task SendMailAsync(string @from, string recipients, string subject, string body)
        {
            var message = new MailMessage(from, recipients, subject, body);
            return SendMailAsync(message);
        }

        #endregion
    }
}
