using System.Collections.Generic;

namespace apartmenthostService.Helpers
{
    public static class ConstLang
    {
        public const string RU = "RU";
        public const string EN = "EN";
    }

    public static class ConstVals
    {
        public const string Any = "DVAL_ANY"; //Любой, не важно
        public const string Pending = "DVAL_PENDING"; //Ожидает подтверждения
        public const string Accepted = "DVAL_ACCEPTED"; //Одобрено
        public const string Declined = "DVAL_DECLINED"; //Отклонено
        public const string PMale = "PG_MALE"; //Мужской
        public const string PFemale = "PG_FEMALE"; //Женский
        public const string Male = "PC_MALE"; //Мужской
        public const string Female = "PC_FEMALE"; //Женский
        public const string Thing = "PC_THING";
        public const string Alien = "PC_ALIEN";
        public const string House = "POT_HOUSE"; //Дом
        public const string Flat = "POT_FLAT"; //Квартира
        public const string Room = "POT_ROOM"; //Комната
        public const string Office = "POT_OFFICE"; //Офис
        public const string HotelRoom = "POT_ROOM"; //Гостинечный номер
        public const string SeperateResidence = "DVAL_SEPARATE_RESIDENCE"; //Раздельное проживание
        public const string Cohabitation = "DVAL_COHABITATION"; //Совместное проживание
        public const string Parking = "Паркова"; //Паркова
        public const string Concierge = "Консьерж"; //Консьерж
        public const string Refrigerator = "Холодильник"; //Холодильник
        public const string WashingMachine = "Стиральная машина"; //Стиральная машина
        public const string AirConditioning = "Кондиционер"; //Кондиционер
        public const string Owner = "DVAL_OWNER"; // Арендодатель
        public const string Renter = "DVAL_RENTER"; // Съемщик
        public const string General = "DVAL_GENERAL"; // Станадартные оповещения
        public const string Global = "DVAL_GLOBAL"; // Глобальные оповещения
        public const string EmailTemp = "EMAILTEMPLATE";
        public const string Article = "ARTICLE";
        public const string Greet = "GREET";
        public const string Reg = "REG";
        public const string Restore = "RESTORE";
        public const string EmailTemplate = "EMAIL_TEMPLATE";
    }
}