namespace PlanerUek.Website.Configuration
{
    public interface IPlanerConfig
    {
        string GetEnvironmentName();
        string GetStudentGroupsStorageConnectionString();
        string GetStudentGroupScheduleTemplate();
        string GetGoogleClientId();
        string GetGoogleClientSecret();
        string GetGoogleDataStoreConnectionString();
        string GetApplicationName();
        string GetCorsOrigin();
        string GetRedirectUriForGoogleAuth();
    }
}