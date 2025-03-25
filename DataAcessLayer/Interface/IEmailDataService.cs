using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Interface
{
   public  interface IEmailDataService
    {
        Task SendEmail(string toEmail, string subject, string body);

    }
}
