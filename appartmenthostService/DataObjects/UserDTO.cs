using System;
using System.Collections.Generic;
using apartmenthostService.Attributes;
using apartmenthostService.Helpers;

namespace apartmenthostService.DataObjects
{
    public class UserDTO
    {
        public UserDTO()
        {
            this.PropsVals = new List<PropValDTO>();
        }
        // [GET][PUT] - {Uniq}{NN} - Уникальный идентификатор(User)
        [Metadata(Visible = false, Required = false)]
        public string Id { get; set; }

        // [GET] - {Uniq}{NN} - Email (Логин)
        [Metadata(Visible = false, Required = false, DataType = ConstDataType.Email)]
        public string Email { get; set; }

        // [GET][PUT] - {NN} - Имя
        [Metadata(Visible = true, Required = true, DataType = ConstDataType.Text)]
        public string FirstName { get; set; }

        // [GET][PUT] - {NN} - Фамилия
        [Metadata(Visible = true, Required = true, DataType = ConstDataType.Text)]
        public string LastName { get; set; }

        // [GET][PUT] - {NN} - Пол
        [Metadata(Visible = true, Required = true, DataType = ConstDataType.List)]
        public string Gender { get; set; }

        // [GET][PUT] - {NN} - Дата рождения
        [Metadata(Visible = true, Required = false, DataType = ConstDataType.Text)]
        public DateTime Birthday { get; set; }

        // [GET][PUT] - {NN} - Телефон
        [Metadata(Visible = true, Required = false, DataType = ConstDataType.Text)]
        public string Phone { get; set; }

        // [GET][PUT] - Контактный email
        [Metadata(Visible = true, Required = false, DataType = ConstDataType.Email)]
        public string ContactEmail { get; set; }

        // [GET][PUT] - Предпочитаемый вид связи (Email/Телефон)
        [Metadata(Visible = true, Required = false, DataType = ConstDataType.List)]
        public string ContactKind { get; set; }

        // [GET][PUT] - О себе
        [Metadata(Visible = true, Required = false, DataType = ConstDataType.Text)]
        public string Description { get; set; }

        // [GET][PUT] - Уникальный идентификатор картиники(аватар)
        [Metadata(Visible = false, Required = false)]
        public string PictureId { get; set; }

        // [GET] - Рейтинг
        [Metadata(Visible = true, Required = false, DataType = ConstDataType.Rating)]
        public decimal Rating { get; set; }

        // [GET] - Дата и Время создания объекта
        [Metadata(Visible = false, Required = false)]
        public DateTimeOffset? CreatedAt { get; set; }
        // [GET] - Дата и Время изменения объекта
        [Metadata(Visible = false, Required = false)]
        public DateTimeOffset? UpdatedAt { get; set; }

        // [GET][PUT] - Список дополнительных колонок(PropVal)
        [Metadata(Visible = false, Required = false)]
        public ICollection<PropValDTO> PropsVals { get; set; } 
    }
}