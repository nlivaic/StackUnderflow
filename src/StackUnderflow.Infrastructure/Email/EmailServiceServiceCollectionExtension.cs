using System;
using Microsoft.Extensions.DependencyInjection;

namespace StackUnderflow.Infrastructure.Email.DependencyInjection
{
    public static class EmailServiceServiceCollectionExtension
    {
        public static void AddEmailService(this IServiceCollection services, Action<EmailServiceOptions> setupEmailService)
        {
            var emailServiceOptions = new EmailServiceOptions();
            setupEmailService(emailServiceOptions);
            services.AddSingleton(emailServiceOptions);
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<EmailClientFactory>();
        }
    }
}
