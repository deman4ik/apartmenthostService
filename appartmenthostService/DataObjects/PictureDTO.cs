using System;
using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    public class PictureDTO
    {
        // Уникальный идентификатор
        public string Id { get; set; }

        // Наименование файла изображения с указанием каталога и расширения
        public string Name { get; set; }

        // Описание изображения, используется в tooltip для отображения при mouseover
        public string Description { get; set; }

        // Полные путь к изображению в оригинальном размере
        public string Url { get; set; }

        // Полные путь к изображению в самом маленьком размере
        public string Xsmall { get; set; }

        // Полные путь к изображению в маленьком размере
        public string Small { get; set; }

        // Полные путь к изображению в среднем размере
        public string Mid { get; set; }

        // Полные путь к изображению в большом размере
        public string Large { get; set; }

        // Полные путь к изображению в самом большом размере
        public string Xlarge { get; set; }

        // Признак изображения по умолчанию
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool? Default { get; set; }

        // Уникальный идентификатор Cloudinary
        public string CloudinaryPublicId { get; set; }

        // Дата и Время создания объекта
        public DateTimeOffset? CreatedAt { get; set; }
    }
}