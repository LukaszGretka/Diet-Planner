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
            try
            {
                using var smptClient = new SmtpClient(_smptClientConfig.SmptHost, _smptClientConfig.SmptPort)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smptClientConfig.SenderEmail, _smptClientConfig.SenderPassword),
                    EnableSsl = _smptClientConfig.EnableSsl
                };

                
                using MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(signUpAccountConfirmationEmail.Email);
                mailMessage.From = new MailAddress("no-replay@diet-planner.com"); //TODO: replace it after development
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Welcome to Diet Planner!";
                mailMessage.Body = EmailHtmlTemplates.GetSignUpVerificationEmailBody(signUpAccountConfirmationEmail.ConfirmationLink);

                smptClient.Send(mailMessage);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
