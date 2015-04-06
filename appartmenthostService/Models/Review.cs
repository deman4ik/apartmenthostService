using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class Review : EntityData
    {
        public Review()
        {
            this.ReviewComments = new HashSet<ReviewComment>();
        }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string AdvertiserId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        [Required]
        public double Rating { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("AdvertiserId")]
        public virtual User Advertiser { get; set; }

        public ICollection<ReviewComment> ReviewComments { get; set; } 

    }
}
