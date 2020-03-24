using Microsoft.WindowsAzure.Storage.Table;

namespace PlanerUek.Storage.Models
{
    public class StudentGroupEntity : TableEntity
    {
        public string Id { get; set; }
    }
}