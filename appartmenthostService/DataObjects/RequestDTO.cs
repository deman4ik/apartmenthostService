using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apartmenthostService.DataObjects
{
    public class RequestDTO
    {
        public RequestDTO()
        {
            this.Params = new List<RequestParamDTO>();
        }
        public string Name { get; set; }
        public ICollection<RequestParamDTO> Params { get; set; } 
    }

    public class RequestParamDTO
    {
        public string Param { get; set; }
        public string Value { get; set; }
    }
}
