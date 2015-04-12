using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using appartmenthostService.Models;

namespace appartmenthostService.DataObjects
{
    public class ApartmentDTO
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

        public string Сohabitation { get; set; }

        public decimal Price { get; set; }

        public string Adress { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Rating { get; set; }

        public string Cohabitation { get; set; }

        public ICollection<PropVal> PropsVals { get; set; } 
    }
}
