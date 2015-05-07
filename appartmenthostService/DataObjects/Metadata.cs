using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apartmenthostService.DataObjects
{
    public class Metadata
    {
        public Metadata()
        {
            this.Items = new List<MetadataItem>();
        }
        public string Name { get; set; }
        public ICollection<MetadataItem> Items { get; set; } 
    }
}
