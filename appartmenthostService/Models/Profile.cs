using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;


namespace appartmenthostService.Models
{
    public class Profile : EntityData
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactKind { get; set; }
        public string Description { get; set; }
        public string PictureId { get; set; }

        public virtual User User { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
