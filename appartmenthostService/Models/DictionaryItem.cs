using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class DictionaryItem : EntityData
    {
        public string DictionaryId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual Dictionary Dictionary { get; set; }
    }
}