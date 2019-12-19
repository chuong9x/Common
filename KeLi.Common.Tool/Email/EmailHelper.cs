using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace KeLi.Common.Tool.Email
{
    /// <summary>
    /// Email helper.
    /// </summary>
    public static class EmailHelper
    {
        /// <summary>
        /// Sends mail message.
        /// </summary>
        /// <param name="former"></param>
        /// <param name="mailAddress"></param>
        /// <param name="mailSubject"></param>
        /// <param name="mailBody"></param>
        /// <param name="isHtml"></param>
        public static void SendMail(this FromerInfo former, string mailAddress, string mailSubject, string mailBody, bool isHtml = false)
        {
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(former.FromAddress, former.DisplayName, Encoding.UTF8),
                    Subject = mailSubject,
                    SubjectEncoding = Encoding.UTF8,
                    Body = mailBody,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = isHtml,
                    Priority = MailPriority.Normal
                };

                if (mailAddress.IndexOf(',') > -1)
                {
                    var mailAddresses = mailAddress.Split(',');

                    foreach (var item in mailAddresses)
                    {
                        if (item.Trim() != string.Empty)
                            mail.To.Add(item);
                    }
                }

                else
                    mail.To.Add(mailAddress);

                var client = new SmtpClient(former.Host, former.Port)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(former.FromAddress, former.Password)
                };

                client.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
