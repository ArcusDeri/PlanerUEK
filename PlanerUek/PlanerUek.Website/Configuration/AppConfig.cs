using Microsoft.Extensions.Configuration;

namespace PlanerUek.Website.Configuration
{
    public class AppConfig : IPlanerConfig
    {
        private readonly IConfiguration _config;

        public AppConfig(IConfiguration config)
        {
            _config = config;
        }
        
        public string GetEnvironmentName() => _config["PlanerUek:Env"];
        
        public string GetStudentGroupsStorageAccountName() => _config["PlanerUek:TableStorage:StudentGroups:AccountName"];

        public string GetStudentGroupsStorageAccountKey() => _config["PlanerUek:TableStorage:StudentGroups:AccountKey"];
    }
}