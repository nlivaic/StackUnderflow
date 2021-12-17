using System.Threading.Tasks;
using MimeKit;
using MimeKit.Text;

namespace StackUnderflow.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceOptions _options;
        private readonly EmailClientFactory _emailClientFactory;

        public EmailService(EmailServiceOptions options, EmailClientFactory emailClientFactory)
        {
            _options = options;
            _emailClientFactory = emailClientFactory;
        }

        public async Task SendAsync(string fromEmail, string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_options.DoNotReplyEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = $"<span>{body}</span>" };
            using var client = await _emailClientFactory.CreateConnectedClientAsync();
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendFromDoNotReplyAsync(string toEmail, string subject, string body) =>
            await SendAsync(_options.DoNotReplyEmail, toEmail, subject, body);
    }
}
