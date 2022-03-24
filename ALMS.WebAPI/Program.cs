using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using ALMS.Core;
namespace ALMS.WebAPI
{

    public class Program
    {
        private static string environmentSettingsFileName = GlobalKeys.AppSettingsFileName;
        public static void Main(string[] args)
        {
            var enviromentValue = GetActiveEnviromentNameFromAppsettings();
            var hostBuilder = CreateHostBuilder(args);
            hostBuilder.UseEnvironment(enviromentValue);
            hostBuilder.Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static string GetActiveEnviromentNameFromAppsettings()
        {
            var currentDirectoryPath = Directory.GetCurrentDirectory();
            var envSettingsPath = Path.Combine(currentDirectoryPath, environmentSettingsFileName);
            var envSettings = JObject.Parse(System.IO.File.ReadAllText(envSettingsPath));

            var enviromentValue = envSettings.GetValue("ASPNETCORE_ENVIRONMENT")?.ToString();
            if (string.IsNullOrWhiteSpace(enviromentValue))
            {
                throw new Exception($"{environmentSettingsFileName} dosyası yada içerisinde [ASPNETCORE_ENVIRONMENT] key bilgisi mevcut değil!");
            }
            return enviromentValue;
        }
    }
}