using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.DataObjects
{
    public class Advert : EntityData
    {
        public Advert()
        {
            this.Pictures = new HashSet<Picture>();
            this.Favorites = new HashSet<Favorite>();
            this.Notifications = new HashSet<Notification>();
        }
        [Required]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }

        public string DefaultPictureId { get; set; }

        [Required]
        public string Type { get; set; }


        public string Description { get; set; }

        public string ApartmentId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        [ForeignKey("UserId")]
        public virtual User Owner { get; set; }
        public virtual Apartment Apartment { get; set;}
    public ICollection<Picture> Pictures { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    }
}
