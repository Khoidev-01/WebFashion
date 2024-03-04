using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Blogger_Common
{
    public class EmailService
    {
        public static void SendMail(string name, string subject, string content, string toMail)
        {
            try
            {
                MailMessage message = new MailMessage();

                var smtp = new SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("lequanghaocntt@gmail.com", "sulnllejzzbcvacn");
                    smtp.Timeout = 20000;
                }

                MailAddress fromAddress = new MailAddress("lequanghaocntt@gmail.com", name);

                message.From = fromAddress;
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;

                smtp.Send(message);
            }
            catch(Exception ex)
            {
            }

        }
        

    }
}
