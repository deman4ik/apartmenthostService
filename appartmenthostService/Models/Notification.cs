using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Notification : EntityData
    {
        public string CardId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public bool Readed { get; set; }

        public virtual User User { get; set; }
    }
}
