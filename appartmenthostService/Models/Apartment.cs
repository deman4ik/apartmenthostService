﻿using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Apartment : EntityData
    {

        public Apartment()
        {
            this.Adverts = new HashSet<Advert>();
            this.Pictures = new HashSet<Picture>();
            this.PropVals = new HashSet<PropVal>();
        }
        public string Name { get; set; }


        public string UserId { get; set; }

        public decimal Price { get; set; }

        public string Adress { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Rating { get; set; }

        public string Lang { get; set; }

        public virtual User User { get; set; }
        public ICollection<Advert> Adverts { get; set; }
        public ICollection<Picture> Pictures { get; set; }
        public ICollection<PropVal> PropVals { get; set; } 
    }
}
