using System.Net.Mail;
using System.Net;

namespace Project1
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "guptapanchanand2022@gmail.com";
            var pw = "fbehuqwhunmsqojt";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, pw)
            };

            return client.SendMailAsync(
                new MailMessage(from: "No-reply@hcl.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
