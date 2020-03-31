using System.Threading.Tasks;
using Google.Apis.Util.Store;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using PlanerUek.Storage.Extensions;
using PlanerUek.Storage.Models;
using PlanerUek.Storage.Providers;

namespace PlanerUek.Storage.Repositories
{
    public class GoogleDataStoreRepository : IDataStore
    {
        private readonly CloudTable _table;

        public GoogleDataStoreRepository(string connectionString)
        {
            _table = CloudTableProvider.GetTable(connectionString, "PlanerUekGoogleDataStore");
        }

        public async Task StoreAsync<T>(string key, T value)
        {
            var serializedData = JsonConvert.SerializeObject(value);
            var entity = new GoogleDataStoreEntity()
            {
                PartitionKey = key,
                RowKey = key,
                Data = serializedData
            };
            await _table.Insert(entity);
        }

        public async Task DeleteAsync<T>(string key)
        {
            await _table.Delete(key, key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var tableResult = await _table.Retrieve<GoogleDataStoreEntity>(key, key);
            var serializedData = tableResult?.Data;
            
            if (string.IsNullOrEmpty(serializedData))
            {
                return default;
            }
            var result = JsonConvert.DeserializeObject<T>(serializedData);

            return result;
        }

        public async Task ClearAsync()
        {
            var entity = new GoogleDataStoreEntity()
            {
                PartitionKey = "user",
                RowKey = "user"
            };
            await _table.Clear(entity);
        }
    }
}