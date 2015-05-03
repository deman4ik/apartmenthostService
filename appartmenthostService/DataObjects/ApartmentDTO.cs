using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apartmenthostService.Attributes;
using apartmenthostService.Models;

namespace apartmenthostService.DataObjects
{
    // Жилье
    public class ApartmentDTO
    {
        public ApartmentDTO()
        {
            this.PropsVals = new List<PropValDTO>();
        }
        // [GET][PUT][DELETE] - {Uniq}{NN} - Уникальный идентификатор(Apartment)
        [Metadata(Visible = 0, Required = 0)]
        public string Id { get; set; }

        // [GET][POST][PUT] - {Uniq}{NN} - Наименование жилья
        [Metadata(Visible = 1,Required = 1)]
        public string Name { get; set; }

        // [GET][POST][PUT] - {NN} - Уникальный идентификатор пользователя(User)
        [Metadata(Visible = 0, Required = 0)]
        public string UserId { get; set; }

        // [GET][POST][PUT] - {NN} - Адрес жилья
        [Metadata(Visible = 1, Required = 1)]
        public string Adress { get; set; }

        // [GET][POST][PUT] - {NN} - Координаты Широта
        [Metadata(Visible = 1, Required = 1)]
        public decimal? Latitude { get; set; }

        // [GET][POST][PUT] - {NN} - Координаты Долгота
        [Metadata(Visible = 1, Required = 1)]
        public decimal? Longitude { get; set; }

        // [GET][POST][PUT] - {NN} - Язык
        [Metadata(Visible = 0, Required = 0)]
        public string Lang { get; set; }

        // [GET] - Дата и Время создания объекта
        [Metadata(Visible = 0, Required = 0)]
        public DateTimeOffset? CreatedAt { get; set; }

        // [GET] - Дата и Время изменения объекта
        [Metadata(Visible = 0, Required = 0)]
        public DateTimeOffset? UpdatedAt { get; set; }

        // [GET][POST][PUT] - {NN} -Список дополнительных колонок(PropVal)
        [Metadata(Visible = 0, Required = 0)]
        public ICollection<PropValDTO> PropsVals { get; set; } 
    }
}
