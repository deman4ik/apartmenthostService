using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apartmenthostService.Attributes
{
   public class MetadataAttribute : Attribute
   {
       public int Visible { get; set; }
       public int Required { get; set; }
   }
}
