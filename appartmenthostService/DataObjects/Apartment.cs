using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.DataObjects
{
    public class Apartment : EntityData
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal PriceTotal { get; set; }

        public string Adress { get; set; }

        public decimal X { get; set; }

        public decimal Y { get; set; }

        public decimal Rating { get; set; }

        public string Сohabitation { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }
        public virtual ICollection<Advert> Adverts { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; } 
    }
}
