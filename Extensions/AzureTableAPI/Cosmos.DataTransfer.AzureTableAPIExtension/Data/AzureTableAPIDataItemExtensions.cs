using Azure.Data.Tables;
using Cosmos.DataTransfer.Interfaces;

namespace Cosmos.DataTransfer.AzureTableAPIExtension.Data
{
    public static class AzureTableAPIDataItemExtensions
    {
        public static TableEntity ToTableEntity(this IDataItem item, string? PartitionKeyFieldName, string? RowKeyFieldName)
        {
            var entity = new TableEntity();

            var partitionKeyFieldNameToUse = "PartitionKey";
            if (!string.IsNullOrWhiteSpace(PartitionKeyFieldName))
            {
                partitionKeyFieldNameToUse = PartitionKeyFieldName;
            }

            var rowKeyFieldNameToUse = "RowKey";
            if (!string.IsNullOrWhiteSpace(RowKeyFieldName))
            {
                rowKeyFieldNameToUse = RowKeyFieldName;
            }

            foreach (var key in item.GetFieldNames())
            {
                var value = item.GetValue(key);
                if(value is IEnumerable<object> enumerable)
                {
                    value = string.Join(",", enumerable);
                }
                if (key.Equals(partitionKeyFieldNameToUse, StringComparison.InvariantCultureIgnoreCase))
                {
                    var partitionKey = value?.ToString();
                    entity.PartitionKey = partitionKey;
                }
                if (key.Equals(rowKeyFieldNameToUse, StringComparison.InvariantCultureIgnoreCase))
                {
                    var rowKey = value?.ToString();
                    entity.RowKey = rowKey;
                }
                entity.Add(key, value);
            }

            return entity;
        }
    }
}
