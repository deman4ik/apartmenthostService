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
        [Required]
        public string Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }


        [ForeignKey("AdvertId")]
        public Advert Advert { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
