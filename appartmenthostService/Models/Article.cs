using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Article : EntityData
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }
}
