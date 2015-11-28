using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    public class GendersDTO
    {
        public string Name { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? Price { get; set; }
    }
}