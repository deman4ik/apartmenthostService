using System;

namespace apartmenthostService.DataObjects
{
    public class DictionaryItemDTO
    {
        public string Id { get; set; }
        public string DictionaryId { get; set; }
        public string StrValue { get; set; }
        public decimal? NumValue { get; set; }
        public DateTime? DateValue { get; set; }
        public bool? BoolValue { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string Lang { get; set; }
    }
}