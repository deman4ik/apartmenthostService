using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.DataObjects
{
    public class Picture : EntityData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        public ICollection<Profile> Profiles { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Apartment> Apartments { get; set; }
        public ICollection<Advert> Adverts { get; set; } 
    }
}
