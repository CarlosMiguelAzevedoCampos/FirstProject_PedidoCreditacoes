using Microsoft.Extensions.Configuration;
using System.IO;

namespace CMA.ISMAI.Trello.Settings
{
    public static class SettingsReader
    {
        private static IConfiguration configuration = null;

        private static void InitiateConfiguration()
        {
            if (configuration != null)
                return;
            configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .Build();
        }
        public static string ReturnKey(string sectionKey, string sectionValue)
        {
            InitiateConfiguration();
            return configuration.GetSection(sectionKey).GetSection(sectionValue).Value;            
        }
    }
}
