using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace StackUnderflow.Common.Email
{
    public interface IEmailClientFactory
    {
        Task<SmtpClient> CreateConnectedClientAsync();
    }
}