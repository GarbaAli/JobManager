using System.Net.Mail;
using System.Net;

namespace AnnonceManager.Repositories
{
    public class SendMailRepository : ISendMail
    {
        public void Send(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("demo@tradestake.net");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;

            SmtpClient smtpClient = new SmtpClient("mail.tradestake.net", 587);
            smtpClient.Credentials = new NetworkCredential("demo@tradestake.net", "53@qtJWBO*qx");
            smtpClient.EnableSsl = true;

           // smtpClient.Host = Dns.GetHostName();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpClient.Send(mail);
        }
    }
}
