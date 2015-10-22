using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apartmenthostService.Tests.Infrastructure
{
    public class ControllerResult
    {
        public string StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string ResponseCode { get; set; }
        public List<string> ResponseData { get; set; }
    }
}
