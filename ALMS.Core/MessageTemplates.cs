using ALMS.Model;

namespace ALMS.Core
{
    public class MessageTemplates
    {
        public static string UserPasswordChangeSubject => "Şifre Sıfırlama Hk.";
        public static string ChangePasswordTemplate(User user, string password)
        {
            var bodyHtmlMessage = $"<p>Sayın <strong>{user.FullName}</strong>,</p><p>Mail hesabınıza tanımlı {GlobalFields.ApiConfig.AppName} uygulamasına ait kullanıcınızın şifre bilgisi sıfırlanmış olup yeni oluşturulan şifre aşağıda paylaşılmıştır. Yeni şifreniz ile oturum a&ccedil;ıktan sonra şifrenizi değiştirmeniz tavsiye edilir.</p><p>&nbsp;</p><table style='border-collapse:collapse; width:350px; border:1px solid #5b9bd5; 'border='1' cellspacing='2' cellpadding='2'><caption style='background-color:#5b9bd5; padding:8px; color:white;'><strong>Oturum Bilgileri</strong></caption><tbody><tr><th style='width:100px; text-align:left; 'scope='row'>Kullanıcı Adı</th><td style='width:478px;' colspan='2'>{user.Email}</td></tr><tr><th style='width:80px; text-align:left; 'scope='row'>Şifre</th><td style='width:478px; 'colspan='2'>{password}</td></tr></tbody></table><p>&nbsp;</p><p>{GlobalFields.ApiConfig.AppName}</p><p>Bilgilerinize,<br/>İyi &ccedil;alışmalar.</p><p>&nbsp;</p><p><span style='color:#999999; font-size:11px;'>Not: Bu e-posta {GlobalFields.ApiConfig.AppName} tarafından g&ouml;nderilmiştir.</span></p><hr/>";
            return bodyHtmlMessage;
        }
    }
}