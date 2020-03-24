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

        public string GetStudentGroupsStorageConnectionString() => _config["PlanerUek:TableStorage:StudentGroupsConnectionString"];
    }
}