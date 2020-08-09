using System;
using System.Net;
using System.Threading.Tasks;
using System.Net.Mail;
using MyLog.Models;

namespace E_mail
{
    public static class Mail
    {
        private static readonly string Path = $@"F:\Уроки C#\MikhnoRuslan\it_academy_finalProject\CheckList\MyPdf";

        public static async Task SendEmailAsync((string id, string color, int numberOfSeats, int task) dataProduct)
        {
            try
            {
                Log.Info($"Send E-mail...");
                var from = new MailAddress(Constants.MailFrom, $"Chek list {dataProduct.id}");
                var to = new MailAddress(Constants.MailTo);
                var message = new MailMessage(from, to);
                message.Subject = dataProduct.id;
                message.Body = $"{dataProduct.id}. {Constants.BodyMail}";
                message.Attachments.Add(new Attachment($@"{Path}\PDF\Checklist {dataProduct.id}.pdf"));
                var smtp = new SmtpClient("smtp.mail.ru", 587);
                smtp.Credentials = new NetworkCredential(Constants.MailFrom, Constants.Password);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                Console.WriteLine($"E-mail send.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine($"Error. E-mail not send.");
                Log.Error($"Error. E-mail not send.");
            }
        }
    }
}
