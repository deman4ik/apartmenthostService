using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apartmenthostService.Attributes;
using apartmenthostService.Helpers;
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
        [Metadata(Visible = false, Required = false)]
        public string Id { get; set; }

        // [GET][POST][PUT] - {NN} - Наименование жилья
        [Metadata(Visible = true, Required = true, DataType = ConstDataType.Text)]
        public string Name { get; set; }

        // [GET][POST][PUT] - {NN} - Уникальный идентификатор пользователя(User)
        [Metadata(Visible = false, Required = false)]
        public string UserId { get; set; }

        // [GET][POST][PUT] - {NN} - Адрес жилья
        [Metadata(Visible = true, Required = true, DataType = ConstDataType.Adress)]
        public string Adress { get; set; }

        // [GET][POST][PUT] - {NN} - Координаты Широта
        [Metadata(Visible = false, Required = true)]
        public decimal? Latitude { get; set; }

        // [GET][POST][PUT] - {NN} - Координаты Долгота
        [Metadata(Visible = false, Required = true)]
        public decimal? Longitude { get; set; }

        // [GET][POST][PUT] - {NN} - Язык
        [Metadata(Visible = false, Required = false)]
        public string Lang { get; set; }

        // [GET] - Дата и Время создания объекта
        [Metadata(Visible = false, Required = false)]
        public DateTimeOffset? CreatedAt { get; set; }

        // [GET] - Дата и Время изменения объекта
        [Metadata(Visible = false, Required = false)]
        public DateTimeOffset? UpdatedAt { get; set; }

        // [GET][POST][PUT] - {NN} -Список дополнительных колонок(PropVal)
        [Metadata(Visible = false, Required = false)]
        public ICollection<PropValDTO> PropsVals { get; set; } 
    }
}
