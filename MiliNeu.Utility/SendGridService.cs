using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MiliNeu.Utility
{

    public class SendGridService : ISendGridService
    {
        private readonly IConfiguration _configuration;

        public SendGridService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string username, string toEmail, string subject, string message)
        {
            string apiKey = _configuration["SendGridSettings:ApiKey"];
            string senderEmail = _configuration["SendGridSettings:FromEmail"];
            string fromUsername = _configuration["SendGridSettings:SenderName"];

            var client = new SendGridClient(apiKey);
            EmailAddress fromAddress = new EmailAddress(senderEmail, fromUsername);
            EmailAddress toAddress = new EmailAddress(toEmail, username);
            var plainTextContent = message;
            var htmlContent = "<strong>"+message+"</strong>";
            var msg = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);



        }
    }
}
