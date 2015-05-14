using System;
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
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        public string Id { get; set; }

        // [GET][POST][PUT] - {Uniq}{NN} - Наименование объявления
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 1, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 1, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 1, RequiredForm = true, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Name { get; set; }

        // [GET][POST][PUT] - {NN} - Уникальный идентификатор пользователя(User)
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string UserId { get; set; }

        // [GET][POST][PUT] - Описание объявления
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 2, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 2, RequiredForm = true, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Description { get; set; }

        // [GET][POST][PUT] - {NN} - Уникальный идентификатор жилья(Apartment)
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 1, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 1, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [PutRule(Order = 1, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string ApartmentId { get; set; }

        // [GET][POST][PUT] - {NN} - Дата с
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 3, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 3, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PutRule(Order = 3, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTime DateFrom { get; set; }

        // [GET][POST][PUT] - {NN} - Дата по
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PutRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTime DateTo { get; set; }

        // [GET][POST][PUT] - {NN} - Цена за сутки
        [Metadata(DataType = ConstDataType.Price)]
        [GetRule(Order = 5, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 5, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 5, RequiredForm = true, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public decimal PriceDay { get; set; }

        // [GET][POST][PUT] - {NN} - Цена за период 
        [Metadata(DataType = ConstDataType.Price)]
        [GetRule(Order = 6, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 6, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 6, RequiredForm = true, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public decimal PricePeriod { get; set; }

        // [GET][POST][PUT] - {NN} - Язык
        [Metadata(DataType = ConstDataType.Lang)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Lang { get; set; }

        // [GET] - Дата и Время создания объекта
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? CreatedAt { get; set; }

        // [GET] - Дата и Время изменения объекта
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? UpdatedAt { get; set; }

        // [GET] - Объект пользователь
        [Metadata(DataType = ConstDataType.User)]
        [GetRule(Order = 7, RequiredForm = false, RequiredTransfer = true, Visible = true)]
        [PostRule(Order = 7, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 7, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public virtual UserDTO User { get; set; }

        // [GET] - Объект жилье
        [Metadata(DataType = ConstDataType.Apartment)]
        [GetRule(Order = 8, RequiredForm = false, RequiredTransfer = true, Visible = true)]
        [PostRule(Order = 8, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 8, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public virtual ApartmentDTO Apartment { get; set; }

        // [GET][POST][PUT] - {NN} -Список дополнительных колонок(PropVal)
        [Metadata(DataType = ConstDataType.PropVals)]
        [GetRule(Order = 9, RequiredForm = false, RequiredTransfer = true, Visible = true)]
        [PostRule(Order = 9, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 9, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public ICollection<PropValDTO> PropsVals { get; set; }

    }
}