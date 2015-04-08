using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class Favorite : EntityData
    {
        public string UserId { get; set; }
        public string AdvertId { get; set; }

        public virtual User User { get; set; }
        public virtual Advert Advert { get; set; }
    }
}
