using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;


namespace AnotherJiraRestClient
{
    public class SmtpEmail
    {
        public static void sendMail(string from,string to,string cc,string subject,string html)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.CC.Add(cc);

                mail.IsBodyHtml = true;
                string htmlBody;

                htmlBody = html;

                mail.Body = htmlBody;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("santhoshv456@gmail.com", "9849478273");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                Console.WriteLine("mail Send");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }


    }
}
