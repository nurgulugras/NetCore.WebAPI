using System.Collections.Generic;
using System.Threading.Tasks;
using ALMS.Model;
using ALMS.Service;

public interface IMailService : IServiceBase
{
    /// <summary>
    /// Sistemin mail sağlayıcısından gönderim yapılır
    /// </summary>
    /// <param name="mailInfo"></param>
    /// <param name="attachmentFiles"></param>
    Task SendMailAsync(MailInfo mailInfo, List<string> attachmentFiles = null);
}