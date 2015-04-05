using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.DataObjects
{
    public class Notification : EntityData
    {
        [Required]
        public string UserId { get; set; }
        public string PictureId { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
        public bool Readed { get; set; }
        public bool SendMail { get; set; }
        

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("PictureId")]
        public virtual Picture Picture { get; set; }
    }
}
