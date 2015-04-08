using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class Reservation : EntityData
    {
        public string AdvertId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }


        public Advert Advert { get; set; }
        public User User { get; set; }
    }
}
