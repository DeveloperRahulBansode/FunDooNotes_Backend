using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Interface;
using Microsoft.Extensions.Configuration;

namespace DataAcessLayer.Services
{
    public class EmailDataService : IEmailDataService
    {
        private readonly IConfiguration _configuration;

        public EmailDataService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string toEmail, string subject, string body)
        {
            var fromEmail = _configuration["EmailSettings:From"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]); 
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            var client = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
