using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Elsa.NNF.Common.Library;

namespace ALMS.Core
{
    public static class ExceptionResolver
    {
        public static string TryResolveExceptionMessage(this Exception exception)
        {
            var fullExceptionMessage = exception.GetFullErrorMessage();

            // if (fullExceptionMessage.Contains ("duplicate") && fullExceptionMessage.Contains ("IX_Employee_CompanyId_Email"))
            //     return "Girmiş olduğunuz mail adresi daha önceden tanımlı gözüküyor. Tekrar kayıt edilemez";

            if (exception.GetType() == typeof(TaskCanceledException) && fullExceptionMessage.Contains("The operation was canceled"))
                return "İstek zaman aşımı!";

            if (exception.GetType() == typeof(HttpRequestException) && fullExceptionMessage.Contains("Connection refused"))
                return "API servise ulaşılamıyor.";
            if (exception.GetType() == typeof(HttpRequestException) && fullExceptionMessage.Contains("Operation timed out"))
                return "API servise ulaşılamıyor.";

            if (exception.GetType() == typeof(SocketException) && fullExceptionMessage.Contains("Connection refused"))
                return "Veritabanı bağlantısı sağlanamadı.";

            if (exception.GetType().Name == "PostgresException" && fullExceptionMessage.Contains("Connection refused"))
                return "Veritabanı bağlantısı sağlanamadı.";
            if (fullExceptionMessage.Contains("IDX20803: Unable to obtain configuration from"))
            {
                return "Elsa IDM bağlantısı sağlanamadı.";
            }

            if (fullExceptionMessage.Contains("insert or update on table") && fullExceptionMessage.Contains(" violates foreign key constraint "))
                return "Tabloda kayıt bulunamadı!";

            if (fullExceptionMessage.Contains("duplicate key value violates unique constraint"))
                return "Tabloda bu kayıt daha önceden kayıt edilmiş. Tekrardan kayıt edilemez!";

            if (fullExceptionMessage.StartsWith("Column ") && fullExceptionMessage.EndsWith("does not belong to table Sheet1."))
            {
                var cleanText = fullExceptionMessage.Replace("Column ", "").Replace(" does not belong to table Sheet1.", "");
                return $"Yüklenen excel içerisinde {cleanText} isimli kolon bulunamadı.";
            }

            if (fullExceptionMessage.Contains("42703: column") && fullExceptionMessage.Contains("does not exist"))
            {
                var column = fullExceptionMessage.Replace("42703: column ", "").Replace(" does not exist", "");
                return $"Veritabanında {column} isimli kolon bulunamadı.";
            }

            if (fullExceptionMessage.StartsWith("Could not find file"))
            {
                var fileName = fullExceptionMessage.Replace("Could not find file ", "");
                return $"Dosya bulunamadı: {fileName}";
            }

            return fullExceptionMessage;
        }
    }
}