﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using apartmenthostService.Models;
using apartmenthostService.Attributes;
using apartmenthostService.Helpers;

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
        [Metadata(Visible = false, Required = false)]
        public string Id { get; set; }

        // [GET][POST][PUT] - {Uniq}{NN} - Наименование объявления
        [Metadata(Visible = true, Required = true, DataType = ConstDataType.Text)]
        public string Name { get; set; }

        // [GET][POST][PUT] - {NN} - Уникальный идентификатор пользователя(User)
        [Metadata(Visible = false, Required = false)]
        public string UserId { get; set; }

        // [GET][POST][PUT] - Описание объявления
        [Metadata(Visible = true, Required = false, DataType = ConstDataType.Text)]
        public string Description { get; set; }

        // [GET][POST][PUT] - {NN} - Уникальный идентификатор жилья(Apartment)
        [Metadata(Visible = false, Required = false)]
        public string ApartmentId { get; set; }

        // [GET][POST][PUT] - {NN} - Дата с
        [Metadata(Visible = true, Required = true, DataType = ConstDataType.Date)]
        public DateTime DateFrom { get; set; }

        // [GET][POST][PUT] - {NN} - Дата по
        [Metadata(Visible = true, Required = true, DataType = ConstDataType.Date)]
        public DateTime DateTo { get; set; }

        // [GET][POST][PUT] - {NN} - Цена за сутки
        [Metadata(Visible = true, Required = true, DataType = ConstDataType.Price)]
        public decimal PriceDay { get; set; }

        // [GET][POST][PUT] - {NN} - Цена за период дат с по
        [Metadata(Visible = true, Required = true, DataType = ConstDataType.Price)]
        public decimal PricePeriod { get; set; }

        // [GET][POST][PUT] - {NN} - Язык
        [Metadata(Visible = false, Required = false)]
        public string Lang { get; set; }

        // [GET] - Дата и Время создания объекта
        [Metadata(Visible = false, Required = false)]
        public DateTimeOffset? CreatedAt { get; set; }

        // [GET] - Дата и Время изменения объекта
         [Metadata(Visible = false, Required = false)]
        public DateTimeOffset? UpdatedAt { get; set; }

        // [GET] - Объект пользователь
        [Metadata(Visible = false, Required = false)]
        public virtual UserDTO User { get; set; }

        // [GET] - Объект жилье
        [Metadata(Visible = false, Required = false)]
        public virtual ApartmentDTO Apartment { get; set; }

        // [GET][POST][PUT] - {NN} -Список дополнительных колонок(PropVal)
        [Metadata(Visible = false, Required = false)]
        public ICollection<PropValDTO> PropsVals { get; set; } 

    }
}