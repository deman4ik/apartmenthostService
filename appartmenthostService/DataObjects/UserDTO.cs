using System;
using System.Collections.Generic;
using apartmenthostService.Attributes;

namespace apartmenthostService.DataObjects
{
    public class UserDTO
    {
        public UserDTO()
        {
            this.PropsVals = new List<PropValDTO>();
        }
        // [GET][PUT] - {Uniq}{NN} - Уникальный идентификатор(User)
        [Metadata(Visible = 0, Required = 0)]
        public string Id { get; set; }

        // [GET] - {Uniq}{NN} - Email (Логин)
        [Metadata(Visible = 0, Required = 0)]
        public string Email { get; set; }

        // [GET][PUT] - {NN} - Имя
        [Metadata(Visible = 1, Required = 1)]
        public string FirstName { get; set; }

        // [GET][PUT] - {NN} - Фамилия
        [Metadata(Visible = 1, Required = 1)]
        public string LastName { get; set; }

        // [GET][PUT] - {NN} - Пол
        [Metadata(Visible = 1, Required = 1)]
        public string Gender { get; set; }

        // [GET][PUT] - {NN} - Дата рождения
        [Metadata(Visible = 1, Required = 0)]
        public DateTime Birthday { get; set; }

        // [GET][PUT] - {NN} - Телефон
        [Metadata(Visible = 1, Required = 0)]
        public string Phone { get; set; }

        // [GET][PUT] - Контактный email
        [Metadata(Visible = 1, Required = 0)]
        public string ContactEmail { get; set; }

        // [GET][PUT] - Предпочитаемый вид связи (Email/Телефон)
        [Metadata(Visible = 1, Required = 0)]
        public string ContactKind { get; set; }

        // [GET][PUT] - О себе
        [Metadata(Visible = 1, Required = 0)]
        public string Description { get; set; }

        // [GET][PUT] - Уникальный идентификатор картиники(аватар)
        [Metadata(Visible = 0, Required = 0)]
        public string PictureId { get; set; }

        // [GET] - Рейтин
        [Metadata(Visible = 1, Required = 0)]
        public decimal Rating { get; set; }

        // [GET] - Дата и Время создания объекта
        [Metadata(Visible = 0, Required = 0)]
        public DateTimeOffset? CreatedAt { get; set; }
        // [GET] - Дата и Время изменения объекта
        [Metadata(Visible = 0, Required = 0)]
        public DateTimeOffset? UpdatedAt { get; set; }

        // [GET][PUT] - Список дополнительных колонок(PropVal)
        [Metadata(Visible = 0, Required = 0)]
        public ICollection<PropValDTO> PropsVals { get; set; } 
    }
}