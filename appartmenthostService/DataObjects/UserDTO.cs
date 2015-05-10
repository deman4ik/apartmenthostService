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
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        public string Id { get; set; }

        // [GET] - {Uniq}{NN} - Email (Логин)
        [Metadata(DataType = ConstDataType.Email)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Email { get; set; }

        // [GET][PUT] - {NN} - Имя
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 1, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 1, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 1, RequiredForm = true, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string FirstName { get; set; }

        // [GET][PUT] - {NN} - Фамилия
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 2, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 2, RequiredForm = true, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string LastName { get; set; }

        // [GET][PUT] - {NN} - Пол
        [Metadata(DataType = ConstDataType.List)]
        [GetRule(Order = 3, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 3, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 3, RequiredForm = true, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Gender { get; set; }

        // [GET][PUT] - {NN} - Дата рождения
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PutRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTime Birthday { get; set; }

        // [GET][PUT] - {NN} - Телефон
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 5, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 5, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 5, RequiredForm = true, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Phone { get; set; }

        // [GET][PUT] - Контактный email
        [Metadata(DataType = ConstDataType.Email)]
        [GetRule(Order = 6, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 6, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PutRule(Order = 6, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string ContactEmail { get; set; }

        // [GET][PUT] - Предпочитаемый вид связи (Email/Телефон)
        [Metadata(DataType = ConstDataType.List)]
        [GetRule(Order = 7, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 7, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PutRule(Order = 7, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string ContactKind { get; set; }

        // [GET][PUT] - О себе
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 8, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 8, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PutRule(Order = 8, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Description { get; set; }

        // [GET][PUT] - Уникальный идентификатор картиники(аватар)
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string PictureId { get; set; }

        // [GET] - Рейтинг
        [Metadata(DataType = ConstDataType.Rating)]
        [GetRule(Order = 9, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public decimal Rating { get; set; }

        // [GET] - Дата и Время создания объекта
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? CreatedAt { get; set; }

        // [GET] - Дата и Время изменения объекта
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? UpdatedAt { get; set; }

        // [GET][PUT] - Список дополнительных колонок(PropVal)
        [Metadata(DataType = ConstDataType.PropVals)]
        [GetRule(Order = 10, RequiredForm = false, RequiredTransfer = true, Visible = true)]
        [PostRule(Order = 9, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 9, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public ICollection<PropValDTO> PropsVals { get; set; } 
    }
}