using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using TaskForBTC.Services.IServices;

namespace TaskForBTC.Services.Services
{
    //Класс, реализующий интерфейс IEmailService
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Отправляет сообщение пользователю.
        /// </summary>
        /// <param name="email"> Почтовый адрес пользователя. </param>
        /// <param name="subject"> Тема сообщения. </param>
        /// <param name="message"> Тело сообщения. </param>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            //Инициализация сообщения.
            emailMessage.From.Add(new MailboxAddress(_configuration["SmtpSettings:SenderName"], _configuration["SmtpSettings:SenderEmail"])); 
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            //Отправка сообщения.
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_configuration["SmtpSettings:Server"], int.Parse(_configuration["SmtpSettings:Port"]), true);
                await client.AuthenticateAsync(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]); 
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
    
}
