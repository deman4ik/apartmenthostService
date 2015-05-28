using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Favorite : EntityData
    {
        public string UserId { get; set; }
        public string CardId { get; set; }

        public virtual User User { get; set; }
        public virtual Card Card { get; set; }
    }
}
