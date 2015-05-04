using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apartmenthostService.Attributes
{
   public class MetadataAttribute : Attribute
   {
       public string DataType { get; set; }
       public bool Visible { get; set; }
       public bool Required { get; set; }
   }
}
