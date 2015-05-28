using System;
using System.Collections.Generic;

namespace apartmenthostService.DataObjects
{
    public class PropDTO
    {
        public PropDTO()
        {
            this.DictionaryItems = new List<DictionaryItemDTO>();
        }
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
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public ICollection<DictionaryItemDTO> DictionaryItems { get; set; }
    }
}
