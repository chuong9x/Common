using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace KeLi.Common.Tool.Email
{
    /// <summary>
    ///     Email helper.
    /// </summary>
    public static class EmailHelper
    {
        /// <summary>
        ///     Sends mail message.
        /// </summary>
        /// <param name="former"></param>
        /// <param name="mail"></param>
        public static void SendMail(this FromerInfo former, MailInfo mail)
        {
            var msg = new MailMessage
            {
                From = new MailAddress(former.FromAddress, former.DisplayName, Encoding.UTF8),

                Subject = mail.Subject,

                SubjectEncoding = Encoding.UTF8,

                Body = mail.Body,

                BodyEncoding = Encoding.UTF8,

                IsBodyHtml = mail.IsHtml,

                Priority = MailPriority.Normal
            };

            if (mail.Address.IndexOf(',') > -1)
            {
                var addresses = mail.Address.Split(',').Where(w => !string.IsNullOrWhiteSpace(w.Trim()));

                foreach (var address in addresses)
                    msg.To.Add(address);
            }

            else
            {
                msg.To.Add(mail.Address);
            }

            var client = new SmtpClient(former.Host, former.Port)
            {
                EnableSsl = true,

                Credentials = new NetworkCredential(former.FromAddress, former.Password)
            };

            client.Send(msg);
        }
    }
}