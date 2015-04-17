using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class PropVal : EntityData
    {
        public string PropId { get; set; }
        public string TableItemId { get; set; }
        public string StrValue { get; set; }
        public decimal? NumValue { get; set; }
        public DateTime? DateValue { get; set; }
        public bool? BoolValue { get; set; }
        public string DictionaryItemId { get; set; }

        public string Lang { get; set; }

        public virtual Prop Prop { get; set; }
        public virtual DictionaryItem DictionaryItem { get; set; }
        public virtual Apartment Apartment { get; set; }
        public virtual Advert Advert { get; set; }

    }
}
