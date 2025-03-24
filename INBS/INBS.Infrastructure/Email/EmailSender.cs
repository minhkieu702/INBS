using INBS.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace INBS.Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly string _port = Environment.GetEnvironmentVariable("PORT") ?? throw new Exception("Port not found");
        private readonly string _username = Environment.GetEnvironmentVariable("EMAIL") ?? throw new Exception("Email not found");
        private readonly string _password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? throw new Exception("App password not found");

        public async Task Send(string from, string to, string subject, string messageText)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", from));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = messageText };

            await SendAsync(message);
        }

        private async Task SendAsync(MimeMessage message)
        {
            using var client = new SmtpClient(); 
            await client.ConnectAsync(_smtpServer, int.Parse(_port), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_username, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
