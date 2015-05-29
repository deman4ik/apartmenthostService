using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Card : EntityData
    {
        public Card()
        {
            this.Pictures = new HashSet<Picture>();
            this.Favorites = new HashSet<Favorite>();
            this.Reservations = new HashSet<Reservation>();
        }
        public string Name { get; set; }

        public string UserId { get; set; }

        public string Description { get; set; }

        public string ApartmentId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public decimal PriceDay { get; set; }

        public string Cohabitation { get; set; }

        public string ResidentGender { get; set; }

        public string Lang { get; set; }

        public virtual User User { get; set; }
        public virtual Apartment Apartment { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Picture> Pictures { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<PropVal> PropVals { get; set; }

    }
}
