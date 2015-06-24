using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apartmenthostService.DataObjects
{
    public class CardRequestDTO
    {
        // Уникальный идентификатор Карточки
        public string Id { get; set; }
        // Наименование Карточки
        public string Name { get; set; }
        // Адрес Жилья
        public string Adress { get; set; }
        // Уникальный Идентификатор Владельца
        public string UserId { get; set; }
        // Описание Карточки
        public string Description { get; set; }
        // Уникальный Идентификатор Жилья
        public string ApartmentId { get; set; }
        // Тип Жилья
        public List<string> Type { get; set; }
        // Дополнительные опции Жилья
        public string Options { get; set; }
        // Дата доступности с
        public DateTime? AvailableDateFrom { get; set; }
        // Дата доступности по
        public DateTime? AvailableDateTo { get; set; }
        // Цена за день с
        public decimal? PriceDayFrom { get; set; }
        // Цена за день по
        public decimal? PriceDayTo { get; set; }
        // Цена за период с
        public decimal? PricePeriodFrom { get; set; }
        // Цена за период по
        public decimal? PricePeriodTo { get; set; }
        // Тип проживания
        public List<string> Cohabitation { get; set; }
        // Пол проживающего
        public List<string> ResidentGender { get; set; }
        // Избранное (Уникальный идентификатор пользователя)
        public string IsFavoritedUserId { get; set; }
        // Дата добавления с
        public DateTime? CreatedAtFrom { get; set; }
        // Дата добавления по
        public DateTime? CreatedAtTo { get; set; }
        
    }
}