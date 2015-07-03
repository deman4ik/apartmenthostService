using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Apartment : EntityData
    {

        public Apartment()
        {
            this.Cards = new HashSet<Card>();
            this.Pictures = new HashSet<Picture>();
            this.PropVals = new HashSet<PropVal>();
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Options { get; set; }

        public string UserId { get; set; }

        public string Adress { get; set; }

        public string FormattedAdress { get; set; }

        public string AdressTypes { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string PlaceId { get; set; }

        public string Lang { get; set; }

        public virtual User User { get; set; }

        public ICollection<Card> Cards { get; set; }

        public ICollection<Picture> Pictures { get; set; }

        public ICollection<PropVal> PropVals { get; set; }
    }
}
