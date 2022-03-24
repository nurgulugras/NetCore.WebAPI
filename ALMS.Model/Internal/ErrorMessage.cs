namespace ALMS.Model
{
    public class ErrorMessage
    {

        /// <summary>
        /// Sistem mesajı
        /// </summary>
        /// <value></value>
        public string InternalMessage { get; set; }

        /// <summary>
        /// Fırlatılan hatanın tipi
        /// </summary>
        /// <value></value>
        public string ExceptionType { get; set; }
    }
}