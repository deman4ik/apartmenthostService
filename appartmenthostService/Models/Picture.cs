using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Picture : EntityData
    {
        public Picture() 
        {
         this.Profiles = new HashSet<Profile>();
        this.Apartments = new HashSet<Apartment>();
        this.Cards = new HashSet<Card>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        public string CloudinaryPublicId { get; set; }
        public bool Default { get; set; }

        public ICollection<Profile> Profiles { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Apartment> Apartments { get; set; }
        public ICollection<Card> Cards { get; set; }
        public ICollection<Article> Articles { get; set; } 
    }
}
