using System.Net.Mail;
using System.Net;

using Microsoft.AspNetCore.Identity.UI.Services;

namespace PremFEPost.Services
{
    public class EmailSender : IEmailSender
    {

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient("mail.zb.co.zw")
            {
                Port = 25,
                Credentials = new NetworkCredential("emwale@zb.co.zw", "Syric1234July27#"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("emwale@zb.co.zw"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
