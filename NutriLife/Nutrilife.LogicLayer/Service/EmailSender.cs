using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{
    public class EmailSender : IEmailSender
    {
        Task IEmailSender.SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587) //{email provider.., }
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("alaatoma58@gmail.com", "eqtl jskl dhwy frvk")
            };

            return client.SendMailAsync(
                new MailMessage(from: "alaatoma58@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                { IsBodyHtml=true}
                );
        }
    }
}
