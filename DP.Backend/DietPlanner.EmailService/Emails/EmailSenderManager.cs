using DietPlanner.Shared.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace DietPlanner.EmailService.Emails
{
    internal class EmailSenderManager
    {
        private readonly SmptClientConfiguration _smptClientConfig;

        public EmailSenderManager(IConfiguration configuration)
        {
            _smptClientConfig = new SmptClientConfiguration();
            configuration.GetSection("SmptClient").Bind(_smptClientConfig);
        }

        public void SendRegistrationEmail(SignUpAccountConfirmationEmail signUpAccountConfirmationEmail)
        {
            using var smptClient = new SmtpClient(_smptClientConfig.SmptHost, _smptClientConfig.SmptPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_smptClientConfig.SenderEmail, _smptClientConfig.SenderPassword)
            };

            //TODO: change localhost to real address.
            var activationLink = "http://localhost:4200?activationToken=" + signUpAccountConfirmationEmail.AccountConfirmationToken;

            using MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(signUpAccountConfirmationEmail.Email);
            mailMessage.From = new MailAddress(_smptClientConfig.SenderEmail);
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "Welcome to Diet Planner!";
            mailMessage.Body = EmailHtmlTemplates.GetSignUpVerificationEmailBody(activationLink);

            smptClient.Send(mailMessage);
        }
    }
}
