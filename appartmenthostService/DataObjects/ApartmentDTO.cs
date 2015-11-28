using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    // Жилье
    public class ApartmentDTO
    {
        // Уникальный идентификатор
        public string Id { get; set; }

        // Наименование жилья
        public string Name { get; set; }

        // Тип жилья
        public string Type { get; set; }

        // Дополнительные параметры
        public string Options { get; set; }

        // Уникальный идентификатор пользователя(User)
        public string UserId { get; set; }

        // Краткий адрес жилья
        public string Adress { get; set; }

        // Полный адрес жилья
        public string FormattedAdress { get; set; }

        // Тип адреса
        public string AdressTypes { get; set; }

        // Координаты Широта
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public double? Latitude { get; set; }

        // Координаты Долгота
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public double? Longitude { get; set; }

        // Язык
        public string Lang { get; set; }

        // Уникальный идентификатор Google Places
        public string PlaceId { get; set; }

        // Дата и Время создания объекта
        public DateTimeOffset? CreatedAt { get; set; }

        // Дата и Время изменения объекта
        public DateTimeOffset? UpdatedAt { get; set; }

        // Изображения
        public ICollection<PictureDTO> Pictures { get; set; }

        // Изображение по умолчанию
        public PictureDTO DefaultPicture { get; set; }
    }
}