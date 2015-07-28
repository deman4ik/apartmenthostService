using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    public class MetadataRule
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool Visible { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool RequiredForm { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool RequiredTransfer { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public int Order { get; set; }
    }
}