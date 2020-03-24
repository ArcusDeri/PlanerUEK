using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PlanerUek.Storage.Extensions
{
    public static class CloudTableExtensions
    {
        public static async Task<TResult> Retrieve<TResult>(this CloudTable table, string partitionKey, string rowKey) where TResult : ITableEntity
        {
            var tableOperation = TableOperation.Retrieve<TResult>(partitionKey, rowKey);
            var operationResult = await table.ExecuteAsync(tableOperation);
            var result = (TResult) operationResult.Result;

            return result;
        }
    }
}