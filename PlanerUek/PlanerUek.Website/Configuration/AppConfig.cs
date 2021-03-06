﻿using Microsoft.Extensions.Configuration;

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
        public string GetStudentGroupScheduleTemplate() => _config["PlanerUek:StudentGroupScheduleEndpointTemplate"];
        public string GetGoogleClientId() => _config["PlanerUek:Credentials:Google:ClientId"];
        public string GetGoogleClientSecret() => _config["PlanerUek:Credentials:Google:ClientSecret"];
        public string GetGoogleDataStoreConnectionString() => _config["PlanerUek:TableStorage:GoogleDataStoreConnectionString"];
        public string GetApplicationName() => _config["PlanerUek:ApplicationName"];
        public string GetCorsOrigin() => _config["PlanerUek:CorsOrigin"];
        public string GetRedirectUriForGoogleAuth() => _config["PlanerUek:GoogleAuthRedirectUri"];
    }
}