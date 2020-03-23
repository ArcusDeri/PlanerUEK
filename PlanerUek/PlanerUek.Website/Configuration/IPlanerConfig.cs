namespace PlanerUek.Website.Configuration
{
    public interface IPlanerConfig
    {
        string GetEnvironmentName();
        string GetStudentGroupsStorageAccountName();
        string GetStudentGroupsStorageAccountKey();
    }
}