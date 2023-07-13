using Azure;
using Azure.Data.Tables;
using AzureDemo.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AzureDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var data = RunDemo().Result;

            return View(data);
        }


        private Task<PageData> RunDemo()
        {
            //var keyValultUrl = "https://fortsa.vault.azure.net";
            //var secretName = "myapp";

            //var credential = new DefaultAzureCredential();
            //var secretClient = new SecretClient(new System.Uri(keyValultUrl), credential);

            //var secret = secretClient.GetDeletedSecret(secretName);

            //var secretValue = secret.Value;

            //var tableEndpoint = "https://fortsab0b6.table.core.windows.net";
            //var tableName = "outTable";

            //var tableClient = new TableClient(new Uri(tableEndpoint), tableName, credential);

            //var entities = tableClient.Query<MyEntity>();

            return Task.FromResult(new PageData { Results = "This is not the data you were looking for." });
        }
    }

    class MyEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Value { get; set; }

    }
}