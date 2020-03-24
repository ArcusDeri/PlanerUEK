using System.Threading.Tasks;

namespace PlanerUek.Storage.Interfaces
{
    public interface IStudentGroupsRepository
    {
        Task<string> GetGroupId(string groupName);
    }
}