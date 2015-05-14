using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class ReservationComment : EntityData
    {
        public string ReservationId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }

        public virtual Reservation Reservation { get; set; }
        public virtual User User { get; set; }
    }
}
