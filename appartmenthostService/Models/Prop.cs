using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Prop : EntityData
    {
        public Prop()
        {
            this.Tables = new HashSet<Table>();
            this.PropVals = new HashSet<PropVal>();
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string DataType { get; set; }
        public bool Visbile { get; set; }
        public bool Required { get; set; }
        public string DictionaryId { get; set; }

        public virtual Dictionary Dictionary { get; set; }
        public ICollection<Table> Tables { get; set; }
        public ICollection<PropVal> PropVals { get; set; } 
    }
}
