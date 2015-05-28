using System.Collections.Generic;

namespace apartmenthostService.DataObjects
{
    public class Metadata
    {
        public Metadata()
        {
            this.Items = new List<MetadataItem>();
        }
        public string Name { get; set; }
        public string LangName { get; set; }
        public ICollection<MetadataItem> Items { get; set; } 
    }
}
