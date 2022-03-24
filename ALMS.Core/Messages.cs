namespace ALMS.Core
{
    public struct Messages
    {

        #region License Messages 

        public static string NotExistsLicenseMessage = "Lisans kaydı metcut değil!";
        public static string InvalidLicenseMessage = "Geçersiz lisans!";
        public static string InvalidLicenseUserMessage = "Geçersiz lisans kullanıcısı!";
        public static string InvalidLicenseDateRangeMessage = "Lisans geçerli tarih aralığında değil!";
        public static string ReApprovedLicenseMessage = "Onaylanmış bir lisans yeniden onaylanamaz!";
        public static string ReRegisterLicenseMessage = "Kayıt edilmiş bir lisans yeniden kayıt edilemez!";
        public static string RegisteredLicenseDeleteMessage = "Kayıt edilmiş bir lisans silinemez!";
        public static string UnregisterLicenseMessage = "Lisans kayıt süreci tamamlanmamış!";
        public static string UnapprovedLicenseMessage = "Lisans onay süreci tamamlanmamış!";
        public static string MinLicenseProdutRequiredMessage = "Lisansı aktif etmek için en az 1 tane lisans ürünü tanımlanması gerekmektedir.";
        public static string LimitExcessMessage = "Lisans limit aşımı! Lisans için maksimum oturum sayısına ulaşıldı.";

        #endregion

        #region Session Messages
        public static string InvalidSessionMessage = "Geçersiz oturum!";
        #endregion

        #region Company Messages
        public static string InvalidCompanyMessage = "Geçersiz şirket!";
        #endregion

        #region Organization Messages
        public static string InvalidOrganizationyMessage = "Geçersiz organizasyon!";
        #endregion
    }
}