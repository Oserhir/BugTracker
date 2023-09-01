using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using TheBugTracker.Models;

namespace TheBugTracker.Services
{
    public class BTEmailService : IEmailSender
    {
        private readonly MailSettings _emailSender;

        public BTEmailService(IOptions<MailSettings> mailSettings)
        {
            _emailSender = mailSettings.Value;
        }


        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
