
namespace appartmenthostService.DataObjects
{
    public class ApartmentDTO
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public decimal Price { get; set; }
        public decimal PriceTotal { get; set; }
        public string Cohabitation { get; set; }
        public string Adress { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Rating { get; set; } 
    }
}