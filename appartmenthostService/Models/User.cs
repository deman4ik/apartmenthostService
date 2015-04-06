using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class User : EntityData
    {
        public User() 
        {
            this.Notifications = new HashSet<Notification>();
            this.Favorites = new HashSet<Favorite>();
            this.SocialAccounts = new HashSet<SocialAccount>();
            this.Apartments = new HashSet<Apartment>();
            this.Adverts = new HashSet<Advert>();
            this.Reservations = new HashSet<Reservation>();
            this.Reviews = new HashSet<Review>();
            this.AdvertiserReviews = new HashSet<Review>();
            this.ReviewComments = new HashSet<ReviewComment>();

        }
        public string Email { get; set; }
        public byte[] Salt { get; set; }
        public byte[] SaltedAndHashedPassword { get; set; }

     //   public virtual Profile Profile { get; set; }
        public ICollection<Notification> Notifications { get; set; } 
        public ICollection<Favorite> Favorites { get; set; } 
        public ICollection<SocialAccount> SocialAccounts { get; set; } 
        public ICollection<Apartment> Apartments { get; set; }
        public ICollection<Advert> Adverts { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Review> AdvertiserReviews { get; set; }
        public ICollection<ReviewComment> ReviewComments { get; set; } 
    }
}
