using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Reservation : EntityData
    {
        public string AdvertId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }

        public virtual Advert Advert { get; set; }
        public virtual User User { get; set; }
        public ICollection<AdvertDate> Dates { get; set; } 

    }
}
