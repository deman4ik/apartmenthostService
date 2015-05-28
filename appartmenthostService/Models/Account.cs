using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Account : EntityData
    {
        public string UserId { get; set; }

        public string AccountId { get; set; }

        public string Provider { get; set; }

        public string ProviderId { get; set; }

        public User User { get; set; }
    }
}
