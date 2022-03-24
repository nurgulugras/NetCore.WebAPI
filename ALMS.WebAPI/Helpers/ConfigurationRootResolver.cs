using System;
using ALMS.Core;
using ALMS.Model;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ALMS.WebAPI
{
    internal class ConfigurationRootResolver
    {
        internal static void LoadApiConfig(IConfiguration configuration)
        {
            GlobalFields.ApiConfig = GetApiConfigFromApiSettings(configuration);
            CheckValidations(GlobalFields.ApiConfig);
            DecodeValues(GlobalFields.ApiConfig);
        }
        private static void CheckValidations(ApiConfig apiConfig)
        {
            if (string.IsNullOrWhiteSpace(apiConfig.UIAddress))
                throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'UIAddress' bilgisi alınamadı!");

            if (string.IsNullOrWhiteSpace(apiConfig.AppName))
                throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'AppName' bilgisi alınamadı!");

            if (apiConfig.DatabaseSettings == null)
                throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'DatabaseSettings' ayarları okunamadı!");
            if (apiConfig.DatabaseSettings != null && string.IsNullOrWhiteSpace(apiConfig.DatabaseSettings.Server))
                throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'DatabaseSettings.Server' bilgisi boş olamaz!");
            if (apiConfig.DatabaseSettings != null && string.IsNullOrWhiteSpace(apiConfig.DatabaseSettings.User))
                throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'DatabaseSettings.User' bilgisi boş olamaz!");
            if (apiConfig.DatabaseSettings != null && string.IsNullOrWhiteSpace(apiConfig.DatabaseSettings.Password))
                throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'DatabaseSettings.Password' bilgisi boş olamaz!");


            if (apiConfig.MailSettings == null)
                throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'MailSettings' ayarları okunamadı!");
            if (apiConfig.MailSettings != null)
            {
                if (string.IsNullOrWhiteSpace(apiConfig.MailSettings.Host))
                    throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'MailSettings.Host' bilgisi boş olamaz!");
                if (apiConfig.MailSettings.Port == default(int))
                    throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'MailSettings.Port' bilgisi boş olamaz!");
                if (string.IsNullOrWhiteSpace(apiConfig.MailSettings.SenderMail))
                    throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'MailSettings.SenderMail' bilgisi boş olamaz!");
                if (apiConfig.MailSettings.UseBasicAuthentication)
                {
                    if (string.IsNullOrWhiteSpace(apiConfig.MailSettings.Username))
                        throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'MailSettings.Username' bilgisi boş olamaz!");
                    if (string.IsNullOrWhiteSpace(apiConfig.MailSettings.Password))
                        throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'MailSettings.Password' bilgisi boş olamaz!");
                }
            }

            if (apiConfig.JwtTokenConfig == null)
                throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'JwtTokenConfig' ayarları okunamadı!");
            if (apiConfig.JwtTokenConfig != null)
            {
                if (string.IsNullOrWhiteSpace(apiConfig.JwtTokenConfig.IssuerSigningKey))
                    throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'JwtTokenConfig.IssuerSigningKey' bilgisi boş olamaz!");
                if (apiConfig.JwtTokenConfig.ValidateLifetime && apiConfig.JwtTokenConfig.TokenLifeTime == default(int))
                    throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden 'JwtTokenConfig.TokenLifeTime' bilgisi boş olamaz!");
            }
        }
        private static void DecodeValues(ApiConfig apiConfig)
        {
            apiConfig.DatabaseSettings.Password = CryptographyHelper.SymmetricDecrypt(apiConfig.DatabaseSettings.Password);

            if (apiConfig.JwtTokenConfig != null)
            {
                apiConfig.JwtTokenConfig.IssuerSigningKey = CryptographyHelper.SymmetricDecrypt(apiConfig.JwtTokenConfig.IssuerSigningKey);
            }

            if (apiConfig.MailSettings != null && apiConfig.MailSettings.UseBasicAuthentication)
            {
                apiConfig.MailSettings.Password = CryptographyHelper.SymmetricDecrypt(apiConfig.MailSettings.Password);
            }

            if (apiConfig.ApprovalFlowConfig != null)
            {
                apiConfig.ApprovalFlowConfig.ApiKey = CryptographyHelper.SymmetricDecrypt(apiConfig.ApprovalFlowConfig.ApiKey);
                apiConfig.ApprovalFlowConfig.SecretKey = CryptographyHelper.SymmetricDecrypt(apiConfig.ApprovalFlowConfig.SecretKey);
            }
        }

        private static ApiConfig GetApiConfigFromApiSettings(IConfiguration configuration)
        {
            const string sectionName = "ApiConfig";
            var apiConfigSection = configuration.GetSection(sectionName);
            var apiConfig = apiConfigSection?.Get<ApiConfig>();
            if (apiConfig == null)
                throw new Exception($"{GlobalKeys.AppSettingsFileName} dosyası içerisinden {sectionName} ayarları okunamadı!");
            return apiConfig;
        }
    }
}