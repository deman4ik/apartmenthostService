using System;
using apartmenthostService.Attributes;
using apartmenthostService.Helpers;

namespace apartmenthostService.DataObjects
{
    public class UserDTO
    {
        //public UserDTO()
        //{
        //    this.PropsVals = new List<PropValDTO>();
        //}
        // Уникальный идентификатор(User)
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        public string Id { get; set; }

        // Email (Логин)
        [Metadata(DataType = ConstDataType.Email)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Email { get; set; }

        // Имя
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 1, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 1, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string FirstName { get; set; }

        // Фамилия
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 2, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string LastName { get; set; }

        // Пол
        [Metadata(DataType = ConstDataType.List, Dictionary = ConstDictionary.Gender)]
        [GetRule(Order = 3, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 3, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Gender { get; set; }

        // Дата рождения
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTime? Birthday { get; set; }

        // Телефон
        [Metadata(DataType = ConstDataType.Phone)]
        [GetRule(Order = 5, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 5, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Phone { get; set; }

        // Контактный email
        [Metadata(DataType = ConstDataType.Email)]
        [GetRule(Order = 6, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 6, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string ContactEmail { get; set; }

        // Предпочитаемый вид связи (Email/Телефон)
        [Metadata(DataType = ConstDataType.List)]
        [GetRule(Order = 7, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 7, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string ContactKind { get; set; }

        // О себе
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 8, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 8, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Description { get; set; }

        // Уникальный идентификатор картиники(аватар)
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string PictureId { get; set; }

        // Рейтинг
        [Metadata(DataType = ConstDataType.Rating)]
        [GetRule(Order = 9, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public decimal Rating { get; set; }

        // Количество проголосовавших
        [Metadata(DataType = ConstDataType.Rating)]
        [GetRule(Order = 10, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public int RatingCount { get; set; }

        // Общее количество очков
        [Metadata(DataType = ConstDataType.Rating)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public decimal Score { get; set; }

        // Количество карточек
        [Metadata(DataType = ConstDataType.Number)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public int CardCount { get; set; }


        // Дата и Время создания объекта
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? CreatedAt { get; set; }

        // Дата и Время изменения объекта
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? UpdatedAt { get; set; }

        // Изображение
        [Metadata(DataType = ConstDataType.Picture)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public PictureDTO Picture { get; set; }
        // Список дополнительных колонок(PropVal)
        //[Metadata(DataType = ConstDataType.PropVals)]
        //[GetRule(Order = 10, RequiredForm = false, RequiredTransfer = true, Visible = true)]
        //[PostRule(Order = 9, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        //[PutRule(Order = 9, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        //[DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        //public ICollection<PropValDTO> PropsVals { get; set; } 
    }
}