using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apartmenthostService.Models;

namespace apartmenthostService.DataObjects
{
    public class ApartmentDTO
    {
        public ApartmentDTO()
        {
            this.PropsVals = new List<PropValDTO>();
        }
        public string Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

        public decimal Price { get; set; }

        public string Adress { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Rating { get; set; }
        
        public string Lang { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public ICollection<PropValDTO> PropsVals { get; set; } 
    }
}
