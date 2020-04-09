using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using PlanerUek.Storage.Models;
using PlanerUek.Storage.Repositories;

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
        
        public static async Task<object> Retrieve(this CloudTable table, string partitionKey, string rowKey)
        {
            var tableOperation = TableOperation.Retrieve(partitionKey, rowKey);
            var operationResult = await table.ExecuteAsync(tableOperation);
            return operationResult.Result;
        }

        public static async Task<bool> Insert(this CloudTable table, TableEntity entity)
        {
            var tableOperation = TableOperation.Insert(entity);
            try
            {
                await table.ExecuteAsync(tableOperation);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<bool> Delete(this CloudTable table, string partitionKey, string rowKey)
        {
            var entity = new TableEntity(partitionKey, rowKey);
            entity.ETag = "*";
            var tableOperation = TableOperation.Delete(entity);
            try
            {
                await table.ExecuteAsync(tableOperation);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task Clear(this CloudTable table, TableEntity entity)
        {
            var query = new TableQuery();
            var result = await table.ExecuteQuerySegmentedAsync(query, null);
            
            var batchOperation = new TableBatchOperation();
            foreach (var row in result)
            {
                row.ETag = "*";
                batchOperation.Delete(row);
            }

            await table.ExecuteBatchAsync(batchOperation);
        }
    }
}