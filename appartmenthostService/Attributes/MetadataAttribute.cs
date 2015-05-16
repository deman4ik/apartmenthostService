using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apartmenthostService.DataObjects;

namespace apartmenthostService.Attributes
{
   [AttributeUsage(AttributeTargets.Property)]
   public class MetadataAttribute : Attribute
   {
       public string DataType { get; set; }
       public string Dictionary { get; set; }
       public bool Multi { get; set; }
   }
}
