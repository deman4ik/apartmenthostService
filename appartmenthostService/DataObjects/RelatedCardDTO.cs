using System;

namespace apartmenthostService.DataObjects
{
    public class RelatedCardDTO
    {
        // Уникальный идентификатор(Advert)
        public string Id { get; set; }

        //  Наименование объявления
        public string Name { get; set; }

        // Уникальный идентификатор пользователя(User)
        public string UserId { get; set; }

        // Описание объявления
        public string Description { get; set; }

        // Уникальный идентификатор жилья(Apartment)
        public string ApartmentId { get; set; }

        // Дата с
        public DateTime DateFrom { get; set; }

        // Дата по
        public DateTime DateTo { get; set; }

        // Цена за сутки
        public decimal PriceDay { get; set; }

        // Цена за период 
        public decimal PricePeriod { get; set; }

        // Сожительство
        public string Cohabitation { get; set; }

        // Пол постояльца
        public string ResidentGender { get; set; }

        // Избранное
        public bool IsFavorite { get; set; }

        // Язык
        public string Lang { get; set; }

        // Дата и Время создания объекта
        public DateTimeOffset? CreatedAt { get; set; }

        // Объект пользователь
        public virtual BaseUserDTO User { get; set; }

        // Объект жилье
        public virtual ApartmentDTO Apartment { get; set; }
    }
}
