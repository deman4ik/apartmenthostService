﻿using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class Apartment : EntityData
    {

        public Apartment()
        {
            this.Adverts = new HashSet<Advert>();
            this.Pictures = new HashSet<Picture>();
        }
        public string Name { get; set; }


        public string UserId { get; set; }

        public string Сohabitation { get; set; }

        public decimal Price { get; set; }

        public decimal PriceTotal { get; set; }

        public string Adress { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Rating { get; set; }

        public string Cohabitation { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Advert> Adverts { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; } 
    }
}
