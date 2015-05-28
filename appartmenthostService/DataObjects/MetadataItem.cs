﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    public class MetadataItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LangName { get; set; }
        public string Type { get; set; }
        public string DataType { get; set; }
        public string Dictionary { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool Multi { get; set; }
        public MetadataRule GetRule { get; set; }
        public MetadataRule PostRule { get; set; }
        public MetadataRule PutRule { get; set; }
        public MetadataRule DeleteRule { get; set; }
        public string DictionaryId { get; set; }
        public string DictionaryName { get; set; }
        public ICollection<DictionaryItemDTO> DictionaryItems { get; set; }
        public Metadata Metadata { get; set; }
    }
}