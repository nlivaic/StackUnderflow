using System;
using MailKit.Security;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace StackUnderflow.Common.Email
{
    public class EmailClientFactory : IEmailClientFactory
    {
        private readonly EmailServiceOptions _options;

        public EmailClientFactory(EmailServiceOptions options)
        {
            _options = options;
        }

        public async Task<SmtpClient> CreateConnectedClientAsync()
        {
            var client = new SmtpClient();
            client.Connect(
                _options.Host,
                _options.Port,
                SecureSocketOptions.None);
            await client.AuthenticateAsync(
                _options.Username,
                _options.Password);
            return client;
        }
    }
}
