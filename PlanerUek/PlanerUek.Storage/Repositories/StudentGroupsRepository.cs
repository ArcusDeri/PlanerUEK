using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PlanerUek.Storage.Extensions;
using PlanerUek.Storage.Interfaces;
using PlanerUek.Storage.Models;

namespace PlanerUek.Storage.Repositories
{
    public class StudentGroupsRepository : IStudentGroupsRepository
    {
        private readonly CloudTable _table;

        public StudentGroupsRepository(string connectionString)
        {
            _table = GetTable(connectionString);
        }

        private CloudTable GetTable(string connectionString)
        {
            try
            {
                var account = CloudStorageAccount.Parse(connectionString);
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

        public async Task<string> GetGroupId(string groupName)
        {
            groupName = groupName.ToLower();
            var result = await _table.Retrieve<StudentGroupEntity>(groupName, groupName);

            return result.Id;
        }
    }
}