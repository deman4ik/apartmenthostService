using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Notification : EntityData
    {
        public string AdvertId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public bool Readed { get; set; }

        public virtual User User { get; set; }
    }
}
