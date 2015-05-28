using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Reservation : EntityData
    {
        public string CardId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public virtual Card Card { get; set; }
        public virtual User User { get; set; }
        public ICollection<PropVal> PropVals { get; set; }
    }
}
