using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;


namespace appartmenthostService.DataObjects
{
    public class Profile : EntityData
    {
        [Key, ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactKind { get; set; }
        public string Description { get; set; }
        public string PictureId { get; set; }

        public virtual User User { get; set; }
        [ForeignKey("PictureId")]
        public virtual Picture Picture { get; set; }
    }
}
