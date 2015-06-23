using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class CardDates : EntityData
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string CardId { get; set; }

        public virtual Card Card { get; set; }
    }
}