namespace ALMS.Model
{
    public class LicenseUserLimitInfo
    {
        /// <summary>
        /// Toplam oturum limiti
        /// </summary>
        /// <value></value>
        public int UserSessionsLimit { get; set; }

        /// <summary>
        /// Aktif kullanılan oturum sayısı
        /// </summary>
        /// <value></value>
        public int ActiveSessionsCount { get; set; }

        /// <summary>
        /// Boşta olan, kullanılabilir kullanıcı oturum sayısı
        /// </summary>
        public int AvailableSessionsCount => UserSessionsLimit - ActiveSessionsCount;
    }
}