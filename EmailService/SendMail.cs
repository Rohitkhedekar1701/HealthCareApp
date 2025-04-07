using PatientService.Models;

using MimeKit;
using Microsoft.Extensions.Configuration;

namespace EmailService
{
    public class   SendMail
    {

        private readonly IConfiguration _configuration;
        private readonly EmailConfigurations _emailConfig;

        public SendMail(  IConfiguration configuration, EmailConfigurations emailConfig)
        {
            _configuration = configuration;
            _emailConfig = emailConfig;
        }
        public void sendEmail(Messages message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Messages message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;

        }


        public void Send(MimeMessage mailMessage)
        {
            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                client.Send(mailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();

            }
        }
    }
}
