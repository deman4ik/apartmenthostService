using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class AdvertDate : EntityData
    {
        public string AdvertId { get; set; }
        public string ReservationId { get; set; }
        public DateTimeOffset Date { get; set; }

        public virtual Advert Advert { get; set; }
        public virtual Reservation Reservation { get; set; }

    }
}
