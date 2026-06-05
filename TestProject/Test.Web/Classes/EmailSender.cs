using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Isp.Web.Classes
{
    public class EmailSender
    {
        private string _mailBodyTemplate = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'/>
    <style>
        body { font-family: Arial, sans-serif; }
        .header { background: #007bff; padding: 10px; color: white; }
        .content { padding: 15px; }
    </style>
</head>
<body>
    <div class='header'>
        <h2>Welcome to KIBSSL Portal</h2>
    </div>
    <div class='content'>
        <p>Dear {{UserName}},</p>
        <p>Your account has been created successfully.</p>
        <p>Login with your email: <b>{{Email}}</b></p>
        <p>Please reset your password after first login.</p>
    </div>
</body>
</html>";

        public bool SendEmail(string toEmail, string subject, string body)
        {
            string fromEmail = "sharifulcse39@gmail.com";
            string fromPassword = "drvowvyjaseirifs"; // Use Gmail app password here

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
                return false;
            }
        }
    }
}
