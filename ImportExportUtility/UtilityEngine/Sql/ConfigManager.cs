using System.Configuration;

namespace UtilityEngine.Sql
{
    public class ConfigManager
    {
        private const string providerName = "SqlClient";
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[providerName].ConnectionString;
        }
    }
}
