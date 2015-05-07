using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apartmenthostService.DataObjects
{
    public class MetadataItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string DataType { get; set; }
        public bool Visible { get; set; }
        public bool Required { get; set; }
        public bool Get { get; set; }
        public bool Post { get; set; }
        public bool Put { get; set; }
        public bool Delete { get; set; }
        public string DictionaryId { get; set; }
        public string DictionaryName { get; set; }
        public ICollection<DictionaryItemDTO> DictionaryItems { get; set; }
        public Metadata Metadata { get; set; }
    }
}