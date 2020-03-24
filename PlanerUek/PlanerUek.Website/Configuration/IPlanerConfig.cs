namespace PlanerUek.Website.Configuration
{
    public interface IPlanerConfig
    {
        string GetEnvironmentName();
        string GetStudentGroupsStorageConnectionString();
    }
}