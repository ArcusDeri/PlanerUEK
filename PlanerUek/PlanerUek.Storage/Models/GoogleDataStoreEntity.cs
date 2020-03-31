using Microsoft.WindowsAzure.Storage.Table;

namespace PlanerUek.Storage.Models
{
    public class GoogleDataStoreEntity : TableEntity
    {
        public string Data { get; set; }
    }
}