using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using ALMS.Core;
using ALMS.Model;

namespace ALMS.Service
{
    public class MailService : IMailService
    {
        public MailService()
        {
        }

        public async Task SendMailAsync(MailInfo mailInfo, List<string> attachmentFiles = null)
        {
            try
            {
                var mailProvider = GlobalFields.ApiConfig.MailSettings;

                var client = new SmtpClient(mailProvider.Host);
                if (mailProvider.UseBasicAuthentication)
                {
                    client.Credentials = new NetworkCredential(mailProvider.Username, mailProvider.Password);
                }
                client.Port = mailProvider.Port;
                if (mailProvider.UseDefaultCredentials)
                {
                    client.UseDefaultCredentials = true;
                }
                client.EnableSsl = mailProvider.EnableSSL;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(mailProvider.SenderMail, string.IsNullOrEmpty(mailProvider.SenderDisplayName) ? "Bilgilendirme" : mailProvider.SenderDisplayName);
                mailMessage.IsBodyHtml = true;

                AddToList(mailMessage, mailInfo.To);
                AddAttachments(mailMessage, attachmentFiles);

                mailMessage.Body = mailInfo.Body;
                mailMessage.Subject = mailInfo.Subject;
                await client.SendMailAsync(mailMessage);
            }
            catch (System.Exception exception)
            {
                throw new Exception("There was an error sending mail. Detail: " + exception.TryResolveExceptionMessage());
            }
        }

        private void AddToList(MailMessage mailMessage, string[] toList)
        {
            for (int i = 0; i < toList.Length; i++)
            {
                var to = toList[i].Trim();
                mailMessage.To.Add(to);
            }
        }

        private void AddAttachments(MailMessage mailMessage, List<string> files)
        {
            if (files == null || files.Count == 0)
                return;

            foreach (var file in files)
            {
                // Create  the file attachment for this email message.
                Attachment data = new Attachment(file);
                // Add time stamp information for the file.
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(file);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                // Add the file attachment to this email message.
                mailMessage.Attachments.Add(data);
            }

        }

        public void Dispose()
        {
        }
    }
}