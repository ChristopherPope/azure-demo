using Azure;
using Azure.Data.Tables;
using System;

namespace AzureDemo.Models
{
    class User : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Email { get; set; }

    }
}