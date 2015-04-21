using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appartmenthostService.DataObjects
{
    public class PropDTO
    {
        public PropDTO()
        {
            this.DictionaryItems = new List<DictionaryItemDTO>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string DictionaryId { get; set; }
        public string DictionaryName { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public ICollection<DictionaryItemDTO> DictionaryItems { get; set; }
    }
}
