﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class DictionaryItem : EntityData
    {
        public DictionaryItem()
        {
            this.PropVals = new HashSet<PropVal>();
        }
        public string Id { get; set; }
        public string DictionaryId { get; set; }
        public string Name { get; set; }
        public string StrValue { get; set; }
        public decimal? NumValue { get; set; }
        public DateTime? DateValue { get; set; }
        public bool? BoolValue { get; set; }

        public string Lang { get; set; }

        public virtual Dictionary Dictionary { get; set; }
        public ICollection<PropVal> PropVals { get; set; } 
    }
}