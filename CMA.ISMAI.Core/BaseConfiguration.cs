using Microsoft.Extensions.Configuration;
using System.IO;

namespace CMA.ISMAI.Core
{
    public static class BaseConfiguration
    {
        private static IConfiguration _configuration = null;
        private static void InitiateConfiguration()
        {
            if (_configuration != null)
                return;
            _configuration = new ConfigurationBuilder()
                                     .SetBasePath(Directory.GetCurrentDirectory()) // Directory where the json files are located
                                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                     .AddEnvironmentVariables()
                                     .Build();
        }
        public static string ReturnSettingsValue(string sectionKey, string sectionValue)
        {
            InitiateConfiguration();
            return _configuration.GetSection(sectionKey).GetSection(sectionValue).Value;
        }
    }
}
