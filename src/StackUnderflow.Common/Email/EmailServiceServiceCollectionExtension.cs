using System;
using StackUnderflow.Common.Email;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EmailServiceServiceCollectionExtension
    {
        public static void AddEmailService(this IServiceCollection services, Action<EmailServiceOptions> setupEmailService)
        {
            var emailServiceOptions = new EmailServiceOptions();
            setupEmailService(emailServiceOptions);
            services.AddSingleton<EmailServiceOptions>(emailServiceOptions);
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEmailClientFactory, EmailClientFactory>();
        }
    }
}
