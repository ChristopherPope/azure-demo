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

        public async Task<ActionResult> ListTables()
        {
            var pageData = await InternalListTables();

            return RedirectToAction("Index", pageData);
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

        public async Task<ActionResult> ReadPrivateCert()
        {
            var pageData = await InternalReadPrivateCert();

            return RedirectToAction("Index", pageData);
        }

        public ActionResult ConnectToUsersTable()
        {
            InternalConnectToUsersTable();

            return RedirectToAction("Index", new PageData("SUCCESS"));
        }

        private async Task<PageData> InternalReadPrivateCert()
        {
            var keyValultUrl = new Uri("https://pope-vault.vault.azure.net/");
            var certificateName = "sci";

            var credential = new DefaultAzureCredential();
            var certClient = new CertificateClient(keyValultUrl, credential);
            var certResponse = await certClient.DownloadCertificateAsync(certificateName);
            var cert = certResponse.Value;

            return new PageData($"Subject: {cert.Subject}, SubjectName: {cert.SubjectName}, Issuer: {cert.Issuer}, FriendlyName: {cert.FriendlyName}, IssuerName: {cert.IssuerName}.");
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
            var users = await usersTable.QueryAsync<User>(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();
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
            var tableEndpoint = new Uri("https://stwuserfiles.table.core.windows.net");
            var tableName = "srpusers";

            return new TableClient(tableEndpoint, tableName, credential);
        }

        private async Task<PageData> InternalListTables()
        {
            var credential = new DefaultAzureCredential();
            var tableEndpoint = new Uri("https://stwuserfiles.table.core.windows.net");
            var tableClient = new TableServiceClient(tableEndpoint, credential);
            var results = await tableClient
                .QueryAsync(ti => ti.Name != "a")
                .ToListAsync();
            var tableNames = results.Select(ti => ti.Name);

            var html = "Table Results: " + string.Join(", ", tableNames);
            return new PageData(html);
        }
    }
}