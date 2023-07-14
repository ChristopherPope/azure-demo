using Azure.Data.Tables;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
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
        public ActionResult Index(PageData pageData)
        {
            return View(pageData);
        }

        public async Task<ActionResult> ReadUsers()
        {
            var pageData = await InternalReadUsers();

            return RedirectToAction("Index", pageData);
        }

        public async Task<ActionResult> CreateUsers()
        {
            await InternalCreateUsers();

            return RedirectToAction("Index", new PageData("SUCCESS"));
        }

        public async Task<ActionResult> Readuser()
        {
            var pageData = await InternalReadUser("bugs.bunny@associates.tsa.dhs.gov");

            return RedirectToAction("Index", pageData);
        }

        public ActionResult ConnectToUsersTable()
        {
            InternalConnectToUsersTable();

            return RedirectToAction("Index", new PageData("SUCCESS"));
        }


        private void RunDemoAsync()
        {
            //var keyValultUrl = "https://fortsa.vault.azure.net";
            //var secretName = "myapp";

            //var credential = new DefaultAzureCredential();
            //var secretClient = new SecretClient(new System.Uri(keyValultUrl), credential);

            //var secret = secretClient.GetDeletedSecret(secretName);

            //var secretValue = secret.Value;

            //var tableEndpoint = "https://fortsab0b6.table.core.windows.net";
            //var tableName = "outTable";

            //var tableClient = new usersTable(new Uri(tableEndpoint), tableName, credential);

            //var entities = tableClient.Query<MyEntity>();
        }

        private async Task<PageData> ConnectToKeyVaultCertificate()
        {
            var keyValultUrl = "https://pope-vault.vault.azure.net/";
            var certificateName = "sci";

            var credential = new DefaultAzureCredential();
            var secretClient = new CertificateClient(new System.Uri(keyValultUrl), credential);
            var response = await secretClient.GetCertificateAsync(certificateName);
            var cert = response.Value;

            return new PageData($"Certificate name: {cert.Name}.");
        }

        private async Task<PageData> InternalReadUsers()
        {
            var usersTable = InternalConnectToUsersTable();
            var users = await usersTable.QueryAsync<User>().ToListAsync();
            var json = JsonConvert.SerializeObject(users);

            return new PageData(json);
        }

        private async Task<PageData> InternalReadUser(string email)
        {
            var usersTable = InternalConnectToUsersTable();
            var users = await usersTable.QueryAsync<User>(u => u.Email == email).ToListAsync();
            var json = JsonConvert.SerializeObject(users);

            return new PageData(json);
        }

        private async Task InternalCreateUsers()
        {
            var usersTable = InternalConnectToUsersTable();
            await usersTable.AddEntityAsync(new User("christopher.pope@associates.tsa.dhs.gov"));
            await usersTable.AddEntityAsync(new User("bugs.bunny@associates.tsa.dhs.gov"));
        }

        private TableClient InternalConnectToUsersTable()
        {
            var credential = new DefaultAzureCredential();
            var tableEndpoint = "https://stwuserfiles.table.core.windows.net/srpusers";
            var tableName = "srpusers";

            return new TableClient(new Uri(tableEndpoint), tableName, credential);
        }
    }
}