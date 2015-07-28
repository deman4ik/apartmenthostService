using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    public class BaseUserDTO
    {
        // Уникальный идентификатор(User)
        public string Id { get; set; }
        // Email (Логин)
        public string Email { get; set; }
        // Имя
        public string FirstName { get; set; }
        // Фамилия
        public string LastName { get; set; }
        // Рейтинг
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Rating { get; set; }

        // Количество проголосовавших
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal RatingCount { get; set; }

        // Пол
        public string Gender { get; set; }
        // Изображение
        public PictureDTO Picture { get; set; }
    }
}