using System;
using System.Collections.Generic;
using apartmenthostService.Attributes;
using apartmenthostService.Helpers;
using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    // Объявление
    public class AdvertDTO
    {
        
        //public AdvertDTO() 
        //{
        //    this.PropsVals = new List<PropValDTO>();
        //}
        // Уникальный идентификатор(Advert)
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        public string Id { get; set; }

        //  Наименование объявления
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 1, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 1, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 1, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Name { get; set; }

        // Уникальный идентификатор пользователя(User)
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string UserId { get; set; }

        // Описание объявления
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 2, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 2, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Description { get; set; }

        // Уникальный идентификатор жилья(Apartment)
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 1, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 1, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [PutRule(Order = 1, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string ApartmentId { get; set; }

        // Дата с
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 3, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 3, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PutRule(Order = 3, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTime DateFrom { get; set; }

        // Дата по
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PutRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTime DateTo { get; set; }

        // Цена за сутки
        [Metadata(DataType = ConstDataType.Price)]
        [GetRule(Order = 5, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 5, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 5, RequiredForm = true, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal PriceDay { get; set; }

        // Цена за период 
        [Metadata(DataType = ConstDataType.Price)]
        [GetRule(Order = 6, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 6, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 6, RequiredForm = true, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal PricePeriod { get; set; }

        // Сожительство
        [Metadata(DataType = ConstDataType.List, Dictionary = ConstDictionary.Cohabitation)]
        [GetRule(Order = 6, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 6, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 6, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Cohabitation { get; set; }

        // Пол постояльца
        [Metadata(DataType = ConstDataType.List, Dictionary = ConstDictionary.Gender)]
        [GetRule(Order = 6, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 6, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 6, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string ResidentGender { get; set; }

        // Избранное
        [Metadata(DataType = ConstDataType.Favorite)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool IsFavorite { get; set; }

        // Язык
        [Metadata(DataType = ConstDataType.Lang)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Lang { get; set; }

        // Дата и Время создания объекта
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? CreatedAt { get; set; }

        // Дата и Время изменения объекта
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? UpdatedAt { get; set; }

        // Объект пользователь
        [Metadata(DataType = ConstDataType.User)]
        [GetRule(Order = 7, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 7, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 7, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public virtual UserDTO User { get; set; }

        // Объект жилье
        [Metadata(DataType = ConstDataType.Apartment)]
        [GetRule(Order = 8, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 8, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 8, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public virtual ApartmentDTO Apartment { get; set; }

        // Бронирование
        [Metadata(DataType = ConstDataType.ApprovedReservations)]
        [GetRule(Order = 9, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public ICollection<ReservationDTO> ApprovedReservations { get; set; }

        // Отзывы
        [Metadata(DataType = ConstDataType.Reviews)]
        [GetRule(Order = 9, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public ICollection<ReviewDTO> Reviews { get; set; }

        // Похожие Объявления
        [Metadata(DataType = ConstDataType.RelatedAdverts)]
        [GetRule(Order = 9, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public ICollection<RelatedAdvertDTO> RelatedAdverts { get; set; } 
        // [GET][POST][PUT] - {NN} -Список дополнительных колонок(PropVal)
        //[Metadata(DataType = ConstDataType.PropVals)]
        //[GetRule(Order = 9, RequiredForm = false, RequiredTransfer = true, Visible = true)]
        //[PostRule(Order = 9, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        //[PutRule(Order = 9, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        //[DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        //public ICollection<PropValDTO> PropsVals { get; set; }

    }
}