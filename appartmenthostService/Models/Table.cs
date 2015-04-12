using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
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
