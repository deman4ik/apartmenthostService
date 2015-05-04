using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apartmenthostService.DataObjects
{
    public class MetadataItem
    {
        
        public string Name { get; set; }
        public string Type { get; set; }
        public string DataType { get; set; }
        public bool Visible { get; set; }
        public bool Required { get; set; }
    }
}