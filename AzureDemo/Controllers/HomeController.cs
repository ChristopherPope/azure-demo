using Azure.Data.Tables;
using Azure.Identity;
using AzureDemo.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AzureDemo.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var data = await RunDemoAsync();

            return View(data);
        }


        private async Task<PageData> RunDemoAsync()
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

            return await ConnectToUserTableAsync();
        }

        private async Task<PageData> ConnectToUserTableAsync()
        {
            var credential = new DefaultAzureCredential();
            var tableEndpoint = "https://stw-storage.table.cosmos.azure.com:443/";
            var tableName = "users";

            var tableClient = new TableClient(new Uri(tableEndpoint), tableName, credential);
            var users = await tableClient.QueryAsync<User>().ToListAsync();
            var json = JsonConvert.SerializeObject(users);

            return new PageData(json);
        }
    }
}