using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appartmenthostService.DataObjects
{
    public class Metadata
    {
        public Metadata()
        {
            this.Items = new List<MetadataItem>();
            this.Props = new List<PropDTO>();
        }
        public ICollection<MetadataItem> Items { get; set; }
        public ICollection<PropDTO> Props { get; set; } 
    }
}
