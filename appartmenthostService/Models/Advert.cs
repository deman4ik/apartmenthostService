using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Advert : EntityData
    {
        public Advert()
        {
            this.Pictures = new HashSet<Picture>();
            this.Favorites = new HashSet<Favorite>();
            this.Dates = new HashSet<AdvertDate>();
            this.Reservations = new HashSet<Reservation>();
        }
        public string Name { get; set; }

        public string UserId { get; set; }

        public string Description { get; set; }

        public string ApartmentId { get; set; }

        public DateTimeOffset DateFrom { get; set; }

        public DateTimeOffset DateTo { get; set; }

        public decimal PriceDay { get; set; }

        public decimal PricePeriod { get; set; }
        
        public string Lang { get; set; }

        public virtual User User { get; set; }
        public virtual Apartment Apartment { get; set;}

        public ICollection<AdvertDate> Dates { get; set; }
        public ICollection<Reservation> Reservations { get; set; } 
        public ICollection<Picture> Pictures { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<PropVal> PropVals { get; set; } 

    }
}
