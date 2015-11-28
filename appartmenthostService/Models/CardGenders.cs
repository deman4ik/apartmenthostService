using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class CardGenders : EntityData
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string CardId { get; set; }
        public virtual Card Card { get; set; }
    }
}