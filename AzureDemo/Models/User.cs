using Azure;
using Azure.Data.Tables;
using System;

namespace AzureDemo.Models
{
    public class User : ITableEntity
    {
        public User()
        {
        }

        public User(string email)
        {
            Email = email;
            PartitionKey = "SRP";
            RowKey = email;
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Email { get; set; }

    }
}