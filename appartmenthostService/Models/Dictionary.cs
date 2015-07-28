using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /*
     * Словари
     */

    public class Dictionary : EntityData
    {
        public Dictionary()
        {
            DictionaryItems = new HashSet<DictionaryItem>();
            Props = new HashSet<Prop>();
        }

        public string Name { get; set; }
        public ICollection<DictionaryItem> DictionaryItems { get; set; }
        public ICollection<Prop> Props { get; set; }
    }
}