using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;

namespace BusinessLayer.Service
{
    public class EmailService : IEmailService
    {
        private readonly IEmailService _emailService;
        public EmailService(IEmailService emailService) 
        { 
            _emailService = emailService;
        }

        public async Task SendEmail(string toEmail, string subject, string body)
        {
             await _emailService.SendEmail(toEmail, subject, body);
        }
    }
}
