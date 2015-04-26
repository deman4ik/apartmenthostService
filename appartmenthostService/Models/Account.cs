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
    public class Account : EntityData
    {
        public string UserId { get; set; }
        public string AccountId { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }

        public User User { get; set; }
    }
}
