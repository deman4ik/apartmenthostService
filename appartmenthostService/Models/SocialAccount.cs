using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class SocialAccount : EntityData
    {
        public string UserId { get; set; }
        public string Provider { get; set; }
        public string SocialId { get; set; }

        public User User { get; set; }
    }
}
