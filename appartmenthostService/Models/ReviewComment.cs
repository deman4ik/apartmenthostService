﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class ReviewComment : EntityData
    {
        public string UserId { get; set; }
        public string ReviewId { get; set; }
        public string Text { get; set; }
        public string Lang { get; set; }
        public virtual User User { get; set; }
        public virtual Review Review { get; set; }
    }
}
