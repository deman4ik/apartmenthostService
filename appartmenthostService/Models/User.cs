using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class User : EntityData
    {
        public User() 
        {
            this.Notifications = new HashSet<Notification>();
            this.Favorites = new HashSet<Favorite>();
            this.Accounts = new HashSet<Account>();
            this.Apartments = new HashSet<Apartment>();
            this.Adverts = new HashSet<Advert>();
            this.Reservations = new HashSet<Reservation>();
            this.OutReviews = new HashSet<Review>();
            this.InReviews = new HashSet<Review>();

        }
        public string Email { get; set; }
        public byte[] Salt { get; set; }
        public byte[] SaltedAndHashedPassword { get; set; }

        public virtual Profile Profile { get; set; }
        public ICollection<Notification> Notifications { get; set; } 
        public ICollection<Favorite> Favorites { get; set; } 
        public ICollection<Account> Accounts { get; set; } 
        public ICollection<Apartment> Apartments { get; set; }
        public ICollection<Advert> Adverts { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Review> OutReviews { get; set; }
        public ICollection<Review> InReviews { get; set; }
    }
}
