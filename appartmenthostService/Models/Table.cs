using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Table : EntityData
    {
        public Table()
        {
            this.Props = new HashSet<Prop>();
        }
        public string Name { get; set; }


        public ICollection<Prop> Props { get; set; } 
    }
}
