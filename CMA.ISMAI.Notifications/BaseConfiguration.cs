using Microsoft.Extensions.Configuration;
using System.IO;

namespace CMA.ISMAI.Notifications
{
    public abstract class BaseConfiguration
    {
        private static IConfiguration _configuration = null;
        public static void InitiateConfiguration()
        {
            if (_configuration != null)
                return;
            _configuration = new ConfigurationBuilder()
                                     .SetBasePath(Directory.GetCurrentDirectory()) // Directory where the json files are located
                                     .AddJsonFile("notifications_appsettings.json", optional: false, reloadOnChange: true)
                                     .Build();
        }
        public static string ReturnSettingsValue(string sectionKey, string sectionValue)
        {
            InitiateConfiguration();
            return _configuration.GetSection(sectionKey).GetSection(sectionValue).Value;
        }
    }
}
