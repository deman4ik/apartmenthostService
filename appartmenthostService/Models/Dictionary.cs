using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class Dictionary : EntityData
    {
        public Dictionary()
        {
            this.DictionaryItems = new HashSet<DictionaryItem>();
            this.Props = new HashSet<Prop>();
        }
        public string Name { get; set; }
        
        public ICollection<DictionaryItem> DictionaryItems { get; set; }
        public ICollection<Prop> Props { get; set; }

    }
}