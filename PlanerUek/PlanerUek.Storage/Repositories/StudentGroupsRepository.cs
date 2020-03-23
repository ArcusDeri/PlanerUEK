using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using PlanerUek.Storage.Interfaces;

namespace PlanerUek.Storage.Repositories
{
    public class StudentGroupsRepository : IStudentGroupsRepository
    {
        private readonly string _accountName;
        private readonly string _accountKey;
        private readonly CloudTable _table;
        
        public StudentGroupsRepository(string accountName, string accountKey)
        {
            _accountName = accountName;
            _accountKey = accountKey;
            _table = GetTable();
        }

        private CloudTable GetTable()
        {
            try
            {
                var credentials = new StorageCredentials(_accountName, _accountKey);
                var account = new CloudStorageAccount(credentials, useHttps: true);
                var client = account.CreateCloudTableClient();
                var table = client.GetTableReference("PlanerUekStudentGroups");

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