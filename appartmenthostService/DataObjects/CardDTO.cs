using System;
using System.Collections.Generic;
using apartmenthostService.Attributes;
using apartmenthostService.Helpers;
using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    // Объявление
    public class CardDTO
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

        // Цена за сутки
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal PriceDay { get; set; }

        // Цена за период 
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal PricePeriod { get; set; }

        // Количество дней в периоде
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public int PeriodDays { get; set; }

        // Сожительство
        public string Cohabitation { get; set; }

        // Пол постояльца
        // TODO: Deprecate
        public string ResidentGender { get; set; }

        // Избранное
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool IsFavorite { get; set; }

        // Язык
        public string Lang { get; set; }

        // Телефон
        public string Phone { get; set; }

        // Дата и Время создания объекта
        public DateTimeOffset? CreatedAt { get; set; }

        // Дата и Время изменения объекта
        public DateTimeOffset? UpdatedAt { get; set; }

        // Объект пользователь
        public virtual UserDTO User { get; set; }

        // Объект жилье
        public virtual ApartmentDTO Apartment { get; set; }

        // Даты недоступности
        public ICollection<DatesDTO> Dates { get; set; }

        // Пол постояльца и цена
        public ICollection<GendersDTO> Genders { get; set; }

        // Бронирование
        public ICollection<ReservationDTO> ApprovedReservations { get; set; }

        // Отзывы
        public ICollection<ReviewDTO> Reviews { get; set; }

        // Похожие Объявления
        public ICollection<RelatedCardDTO> RelatedCards { get; set; }
    }
}