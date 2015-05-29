using System;
using apartmenthostService.Attributes;
using apartmenthostService.Helpers;

namespace apartmenthostService.DataObjects
{
    public class ReviewDTO
    {
        // Уникальный идентификатор
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Id { get; set; }

        // Уникальный идентификатор пользователя - Автора отзыва
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string FromUserId { get; set; }

        // Уникальный идентификатор пользователя - Кому был оставлен отзыв
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string ToUserId { get; set; }

        // Уникальный идентификатор пользователя - Брониерование
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string ReservationId { get; set; }

        // Рейтинг
        [Metadata(DataType = ConstDataType.Rating)]
        [GetRule(Order = 1, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 1, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 1, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public double Rating { get; set; }

        // Текст отзыва
        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PutRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Text { get; set; }

        // Дата и Время создания объекта
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? CreatedAt { get; set; }

        // Дата и Время изменения объекта
        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? UpdatedAt { get; set; }

        // Объект пользователь - автор отзыва
        [Metadata(DataType = ConstDataType.User)]
        [GetRule(Order = 5, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public virtual BaseUserDTO FromUser { get; set; }

        // Объект пользователь - Кому был оставлен отзыв
        [Metadata(DataType = ConstDataType.User)]
        [GetRule(Order = 5, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public virtual BaseUserDTO ToUser { get; set; }

        // Объект бронь
        [Metadata(DataType = ConstDataType.User)]
        [GetRule(Order = 5, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public virtual ReservationDTO Reservation { get; set; }
    }
}
