using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apartmenthostService.DataObjects
{
    public class MetadataRule
    {
        public bool Visible { get; set; }
        public bool RequiredForm { get; set; }
        public bool RequiredTransfer { get; set; }
        public int Order { get; set; }
    }
}
