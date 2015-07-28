using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /*
     * Жилье пользователя
     */

    public class Apartment : EntityData
    {
        public Apartment()
        {
            Cards = new HashSet<Card>();
            Pictures = new HashSet<Picture>();
            PropVals = new HashSet<PropVal>();
        }

        // Наименование жилья
        public string Name { get; set; }
        /* Тип жилья
        Заполняется из словаря - "D_APARTMENTTYPE"
        Возможные значения:
        "DVAL_HOUSE" - Дом
        "DVAL_FLAT" - Квартира
        "DVAL_ROOM" - Комната
        "DVAL_OFFICE" - Офис
        "DVAL_HOTEL_ROOM" - Гостинечный номер
        */
        public string Type { get; set; }
        // Дополнительные опции жилья (несколько значений через запятую)
        public string Options { get; set; }
        // Уникальный идентификатор пользователя
        public string UserId { get; set; }
        // Адрес жилья
        public string Adress { get; set; }
        // Полный адрес жилья
        public string FormattedAdress { get; set; }
        // Тип адреса жилья
        public string AdressTypes { get; set; }
        // Координаты широта
        public double? Latitude { get; set; }
        // Координаты долгота
        public double? Longitude { get; set; }
        // Уникальный идентификатор адреса из Google Places
        public string PlaceId { get; set; }
        // Язык "RU","EN"
        public string Lang { get; set; }
        // Ссылка на пользователя
        public virtual User User { get; set; }
        // Ссылки на список связанных карточек объявлений
        public ICollection<Card> Cards { get; set; }
        // Ссылки на список связанных изображений
        public ICollection<Picture> Pictures { get; set; }
        // Ссылки на список дополнительных свойств
        public ICollection<PropVal> PropVals { get; set; }
    }
}