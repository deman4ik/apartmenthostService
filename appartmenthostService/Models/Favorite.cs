using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class Favorite : EntityData
    {
        public string UserId { get; set; }
        public string AdvertId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("AdvertId")]
        public virtual Advert Advert { get; set; }
    }
}
