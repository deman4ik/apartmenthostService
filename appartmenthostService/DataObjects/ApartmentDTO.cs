using System;
using apartmenthostService.Attributes;
using apartmenthostService.Helpers;
using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    // Жилье
    public class ApartmentDTO
    {
        // Уникальный идентификатор
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0,RequiredForm = false,RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0,RequiredForm = false,RequiredTransfer = true,Visible = false)]
        [DeleteRule(Order = 0,RequiredForm = false, RequiredTransfer = true, Visible = false)]
        public string Id { get; set; }

        // Наименование жилья
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 1, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 1, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 1, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Name { get; set; }

        // Тип жилья
        [Metadata(DataType = ConstDataType.List, Dictionary = ConstDictionary.ApartmentType)]
        [GetRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 2, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 2, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Type { get; set; }

        // Дополнительные параметры
        [Metadata(DataType = ConstDataType.Multibox, Dictionary = ConstDictionary.ApartmentOptions, Multi = true)]
        [GetRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PutRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Options { get; set; }

        // Уникальный идентификатор пользователя(User)
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string UserId { get; set; }

        // Адрес жилья
        [Metadata(DataType = ConstDataType.Adress)]
        [GetRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 2, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 2, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Adress { get; set; }

        // Координаты Широта
        [Metadata(DataType = null)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? Latitude { get; set; }

        // Координаты Долгота
        [Metadata(DataType = null)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? Longitude { get; set; }

        // Язык
        [Metadata(DataType = ConstDataType.Lang)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Lang { get; set; }

        // Дата и Время создания объекта
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 3, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 3, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 3, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? CreatedAt { get; set; }

        // Дата и Время изменения объекта
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? UpdatedAt { get; set; }

        // Список дополнительных колонок(PropVal)
        //[Metadata(DataType = ConstDataType.PropVals)]
        //[GetRule(Order = 4, RequiredForm = false, RequiredTransfer = true, Visible = true)]
        //[PostRule(Order = 4, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        //[PutRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        //[DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        //public ICollection<PropValDTO> PropsVals { get; set; } 
    }
}
