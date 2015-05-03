using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using apartmenthostService.Models;
using apartmenthostService.Attributes;

namespace apartmenthostService.DataObjects
{
    public class AdvertDTO
    {
        // Объявление
        public AdvertDTO() 
        {
            this.PropsVals = new List<PropValDTO>();
        }
        // [GET][PUT][DELETE] - {Uniq}{NN} - Уникальный идентификатор(Advert)
        [Metadata(Visible = 0, Required = 0)]
        public string Id { get; set; }

        // [GET][POST][PUT] - {Uniq}{NN} - Наименование объявления
        [Metadata(Visible = 1, Required = 1)]
        public string Name { get; set; }

        // [GET][POST][PUT] - {NN} - Уникальный идентификатор пользователя(User)
        [Metadata(Visible = 0, Required = 0)]
        public string UserId { get; set; }

        // [GET][POST][PUT] - Описание объявления
        [Metadata(Visible = 1, Required = 0)]
        public string Description { get; set; }

        // [GET][POST][PUT] - {NN} - Уникальный идентификатор жилья(Apartment)
        [Metadata(Visible = 0, Required = 0)]
        public string ApartmentId { get; set; }

        // [GET][POST][PUT] - {NN} - Дата с
        [Metadata(Visible = 1, Required = 1)]
        public DateTime DateFrom { get; set; }

        // [GET][POST][PUT] - {NN} - Дата по
        [Metadata(Visible = 1, Required = 1)]
        public DateTime DateTo { get; set; }

        // [GET][POST][PUT] - {NN} - Цена за сутки
        [Metadata(Visible = 1, Required = 1)]
        public decimal PriceDay { get; set; }

        // [GET][POST][PUT] - {NN} - Цена за период дат с по
        [Metadata(Visible = 1, Required = 1)]
        public decimal PricePeriod { get; set; }

        // [GET][POST][PUT] - {NN} - Язык
        [Metadata(Visible = 0, Required = 0)]
        public string Lang { get; set; }

        // [GET] - Дата и Время создания объекта
        [Metadata(Visible = 0, Required = 0)]
        public DateTimeOffset? CreatedAt { get; set; }

        // [GET] - Дата и Время изменения объекта
         [Metadata(Visible = 0, Required = 0)]
        public DateTimeOffset? UpdatedAt { get; set; }

        // [GET] - Объект пользователь
        [Metadata(Visible = 0, Required = 0)]
        public virtual UserDTO User { get; set; }

        // [GET] - Объект жилье
        [Metadata(Visible = 0, Required = 0)]
        public virtual ApartmentDTO Apartment { get; set; }

        // [GET][POST][PUT] - {NN} -Список дополнительных колонок(PropVal)
        [Metadata(Visible = 0, Required = 0)]
        public ICollection<PropValDTO> PropsVals { get; set; } 

    }
}