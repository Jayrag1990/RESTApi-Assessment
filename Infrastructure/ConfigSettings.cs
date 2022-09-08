using System.Configuration;

namespace Assessment.Infrastructure
{
    public class ConfigSettings
    {
        public static readonly string SecretKey = ConfigurationManager.AppSettings["SecretKey"];
    }
}