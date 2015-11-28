using System;
using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    public class NotificationDTO
    {
        // Уникальный идентификатор
        public string Id { get; set; }

        // Уникальный идентификатор пользователя(User)
        public string UserId { get; set; }

        // Тип оповещения
        public string Type { get; set; }

        // Текст оповещения
        public string Code { get; set; }

        // Дополнительные Данные для подстановки
        public NotificationData Data { get; set; }

        // Признак прочитанного объявления
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool Readed { get; set; }

        // Признак отправленного Email сообщения
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool Emailed { get; set; }

        // Дата и Время создания объекта
        public DateTimeOffset? CreatedAt { get; set; }

        // Дата и Время изменения объекта
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}