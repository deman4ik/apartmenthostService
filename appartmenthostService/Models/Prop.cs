using System.Collections.Generic;
using apartmenthostService.DataObjects;
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
        public MetadataRule GetRule { get; set; }
        public MetadataRule PostRule { get; set; }
        public MetadataRule PutRule { get; set; }
        public MetadataRule DeleteRule { get; set; }
        public string DictionaryId { get; set; }

        public virtual Dictionary Dictionary { get; set; }
        public ICollection<Table> Tables { get; set; }
        public ICollection<PropVal> PropVals { get; set; }
    }
}
