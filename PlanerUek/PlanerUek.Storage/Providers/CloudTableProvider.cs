using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace PlanerUek.Storage.Providers
{
    public static class CloudTableProvider
    {
        public static CloudTable GetTable(string connectionString, string tableName)
        {
            try
            {
                var account = CloudStorageAccount.Parse(connectionString);
                var client = account.CreateCloudTableClient();
                var table = client.GetTableReference(tableName);

                return table;
            }
            catch
            {
                //TODO: log error
                return null;
            }
        }
    }
}