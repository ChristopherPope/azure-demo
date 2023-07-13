namespace AzureDemo.Models
{
    public class PageData
    {
        public PageData(string results)
        {
            Results = results;
        }

        public string Results { get; set; }
    }
}