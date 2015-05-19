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
    public class Review : EntityData
    {

        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Text { get; set; }
        public double Rating { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }



    }
}
