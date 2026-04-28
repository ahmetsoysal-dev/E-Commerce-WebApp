using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace E_Commerce_WebApp.Models.Concrete
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string nameSurname)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                _config["Smtp:SenderName"],
                _config["Smtp:From"]
            ));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Hoş Geldiniz! 🎉";

            message.Body = new TextPart("html")
            {
                Text = $"""
                    <h2>Merhaba {nameSurname},</h2>
                    <p>Sitemize kayıt olduğunuz için teşekkür ederiz.</p>
                    <p>Alışverişin tadını çıkarın! 🛒</p>
                """
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(
                _config["Smtp:Host"],
                int.Parse(_config["Smtp:Port"]!),
                SecureSocketOptions.StartTls
            );
            await client.AuthenticateAsync(
                _config["Smtp:User"],
                _config["Smtp:Pass"]
            );
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}