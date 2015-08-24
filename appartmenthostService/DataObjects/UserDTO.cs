using System;
using System.Collections.Generic;
using apartmenthostService.Attributes;
using apartmenthostService.Helpers;
using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    public class UserDTO
    {
        // Уникальный идентификатор(User)
        public string Id { get; set; }

        // Email (Логин)
        public string Email { get; set; }

        // Имя
        public string FirstName { get; set; }

        // Фамилия
        public string LastName { get; set; }

        // Пол
        public string Gender { get; set; }

        // Дата рождения
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public DateTime? Birthday { get; set; }

        // Телефон
        public string Phone { get; set; }

        // Контактный email
        public string ContactEmail { get; set; }

        // Предпочитаемый вид связи (Email/Телефон)
        public string ContactKind { get; set; }

        // О себе
        public string Description { get; set; }

        // Уникальный идентификатор картиники(аватар)
        public string PictureId { get; set; }

        // Рейтинг
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Rating { get; set; }

        // Количество проголосовавших
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public int RatingCount { get; set; }

        // Общее количество очков
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Score { get; set; }

        // Количество карточек
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public int CardCount { get; set; }

        // Признак подтверждения Email
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool EmailConfirmed { get; set; }

        // Дата и Время создания объекта
        public DateTimeOffset? CreatedAt { get; set; }

        // Дата и Время изменения объекта
        public DateTimeOffset? UpdatedAt { get; set; }

        // Изображение
        public PictureDTO Picture { get; set; }

        // Отзывы
        public ICollection<ReviewDTO> Reviews { get; set; }
    }
}