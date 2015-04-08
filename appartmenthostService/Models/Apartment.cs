using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class Apartment : EntityData
    {

        public string Name { get; set; }


        public string UserId { get; set; }


        public decimal Price { get; set; }

        public decimal PriceTotal { get; set; }

        public string Adress { get; set; }

        public decimal X { get; set; }

        public decimal Y { get; set; }

        public decimal Rating { get; set; }

        public string Сohabitation { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Advert> Adverts { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; } 
    }
}
