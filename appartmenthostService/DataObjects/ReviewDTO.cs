using System;
using apartmenthostService.Attributes;
using apartmenthostService.Helpers;
using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    public class ReviewDTO
    {
        // Уникальный идентификатор
        public string Id { get; set; }

        // Уникальный идентификатор пользователя - Автора отзыва
        public string FromUserId { get; set; }

        // Уникальный идентификатор пользователя - Кому был оставлен отзыв
        public string ToUserId { get; set; }

        // Уникальный идентификатор пользователя - Брониерование
        public string ReservationId { get; set; }

        // Тип отзыва
        public string Type { get; set; }

        // Рейтинг
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Rating { get; set; }

        // Текст отзыва
        public string Text { get; set; }

        // Дата и Время создания объекта
        public DateTimeOffset? CreatedAt { get; set; }

        // Дата и Время изменения объекта
        public DateTimeOffset? UpdatedAt { get; set; }

        // Объект пользователь - автор отзыва
        public virtual BaseUserDTO FromUser { get; set; }

        // Объект пользователь - Кому был оставлен отзыв
        public virtual BaseUserDTO ToUser { get; set; }

        // Объект бронь
        public virtual ReservationDTO Reservation { get; set; }
    }
}