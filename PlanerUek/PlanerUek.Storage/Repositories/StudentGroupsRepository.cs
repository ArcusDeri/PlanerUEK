using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PlanerUek.Storage.Extensions;
using PlanerUek.Storage.Interfaces;
using PlanerUek.Storage.Models;
using PlanerUek.Storage.Providers;

namespace PlanerUek.Storage.Repositories
{
    public class StudentGroupsRepository : IStudentGroupsRepository
    {
        private readonly CloudTable _table;

        public StudentGroupsRepository(string connectionString)
        {
            _table = CloudTableProvider.GetTable(connectionString, "PlanerUekStudentGroups"); //refactor to IPlanerConfig
        }

        public async Task<string> GetGroupId(string groupName)
        {
            groupName = groupName.ToLower();
            var result = await _table.Retrieve<StudentGroupEntity>(groupName, groupName);

            return result is null ? string.Empty: result.Id;
        }
    }
}