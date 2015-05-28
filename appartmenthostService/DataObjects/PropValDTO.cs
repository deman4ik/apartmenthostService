using System;

namespace apartmenthostService.DataObjects
{
    public class PropValDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PropId { get; set; }
        public string Type { get; set; }
        public string StrValue { get; set; }
        public decimal? NumValue { get; set; }
        public DateTime? DateValue { get; set; }
        public bool? BoolValue { get; set; }
        public string DictionaryItemId { get; set; }
        public string Lang { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DictionaryItemDTO DictionaryItem { get; set; }
    }
}
