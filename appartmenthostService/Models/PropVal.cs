using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /*
     * Значения дополнительных свойств
     */
    public class PropVal : EntityData
    {
        public string PropId { get; set; }
        public string ApartmentItemId { get; set; }
        public string AdvertItemId { get; set; }
        public string ReservationItemId { get; set; }
        public string StrValue { get; set; }
        public decimal? NumValue { get; set; }
        public DateTime? DateValue { get; set; }
        public bool? BoolValue { get; set; }
        public string DictionaryItemId { get; set; }

        public string Lang { get; set; }

        public virtual Prop Prop { get; set; }
        public virtual DictionaryItem DictionaryItem { get; set; }
        public virtual Apartment Apartment { get; set; }
        public virtual Card Card { get; set; }
        public virtual Reservation Reservation { get; set; }

        // Системные поля: 
        // Id - Уникальный идентификатор записи
        // CreatedAt - Дата и время создания записи
        // UpdatedAt - Дата и время изменения записи
        // Version - Текущая версия записи
        // Deleted - Признак удаленной записи

    }
}
