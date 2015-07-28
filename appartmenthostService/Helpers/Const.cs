using System.Collections.Generic;

namespace apartmenthostService.Helpers
{
    public static class ConstType
    {
        public const string Str = "Str";
        public const string Num = "Num";
        public const string Date = "Date";
        public const string Bool = "Bool";
        public const string User = "User";
        public const string Apartment = "Apartment";
        public const string Card = "Card";
        public const string Reservation = "Reservation";
        public const string PropCollection = "PropCollection";
    }

    public static class ConstTable
    {
        public const string ApartmentTable = "Apartment";
        public const string CardTable = "Card";
        public const string ProfileTable = "Profile";
        public const string ReservationTable = "Reservation";

        public static string GetTableByObjectType(string type)
        {
            switch (type)
            {
                case ConstType.Apartment:
                    return ApartmentTable;
                case ConstType.Card:
                    return CardTable;
                case ConstType.User:
                    return ProfileTable;
                default:
                    return null;
            }
        }
    }

    public static class ConstProp
    {
    }

    public static class ConstMetaDataProp
    {
        public const string Visible = "Visible";
        public const string RequiredForm = "RequiredForm";
        public const string RequiredTransfer = "RequiredTransfer";
        public const string Order = "Order";
        public const string DataType = "DataType";
        public const string Dictionary = "Dictionary";
        public const string Multi = "Multi";
    }

    public static class ConstDictionary
    {
        public const string Gender = "D_GENDER";
        public const string ApartmentType = "D_APARTMENTTYPE";
        public const string ApartmentOptions = "D_APARTMENTOPTIONS";
        public const string Cohabitation = "D_COHABITATION";
    }

    public static class ConstDataType
    {
        public const string Bool = "Bool"; // True/False
        public const string Text = "Text"; // Текстовое поле
        public const string Number = "Number"; // Число
        public const string Date = "Date"; // Выбора даты
        public const string DateList = "DateList"; // Список дат DateFrom DateTo
        public const string List = "List"; // Выпадающий список или RadioButton
        public const string Multibox = "Multibox"; // Выбор нескольких значений 
        public const string Email = "Email"; // Поле для ввода email проверка формата 
        public const string Phone = "Phone"; // Поле для ввода телефона проверка формата
        public const string Adress = "Adress"; // Поле для ввода Адреса автоподставнока
        public const string Price = "Price"; // Поле для ввода Цены
        public const string Rating = "Rating"; // 5 звезд рейтинга
        public const string Image = "Image"; // Отображение картинки по URL 
        public const string Id = "Id"; // Уникальный идентификатор
        public const string Apartment = "Apartment"; // Объект типа Жилье
        public const string Card = "Card"; // Объект типа Объявление
        public const string User = "User"; // Объект типа Пользователь
        public const string Picture = "Picture"; // Объект типа Изображение
        public const string PictureList = "PictureList"; // Список Изображений
        public const string Reservation = "Reservation"; // Объект типа Бронь
        public const string Favorite = "Favorite"; // Избранное
        public const string ApprovedReservations = "ApprovedReservations"; //  Список объектов типа Бронь - утвержденные
        public const string Reviews = "Reviews"; // Отзывы
        public const string NotificationData = "NotificationData"; // Дополнительные данные оповещения
        public const string RelatedCards = "RelatedCards"; // Похожие объявления
        public const string PropVals = "PropVals"; // Список объектов типа Значения свойств
        public const string Lang = "Lang"; // Код языка
    }

    public static class ConstLang
    {
        public const string RU = "RU";
        public const string EN = "EN";
    }

    public static class ConstDicVals
    {
        public static List<string> GenderList()
        {
            var list = new List<string>();
            list.Add(ConstVals.Male);
            list.Add(ConstVals.Female);
            list.Add(ConstVals.Any);
            list.Add(ConstVals.Thing);
            list.Add(ConstVals.Alien);
            return list;
        }

        public static List<string> ApartmentTypesList()
        {
            var list = new List<string>();
            list.Add(ConstVals.House);
            list.Add(ConstVals.Flat);
            list.Add(ConstVals.Room);
            list.Add(ConstVals.Office);
            list.Add(ConstVals.HotelRoom);
            return list;
        }

        public static List<string> CohabitationTypesList()
        {
            var list = new List<string>();
            list.Add(ConstVals.SeperateResidence);
            list.Add(ConstVals.Cohabitation);
            list.Add(ConstVals.Any);
            return list;
        }

        public static List<string> ApartmentOptionsList()
        {
            var list = new List<string>();
            list.Add(ConstVals.Parking);
            list.Add(ConstVals.Concierge);
            list.Add(ConstVals.Refrigerator);
            list.Add(ConstVals.WashingMachine);
            list.Add(ConstVals.AirConditioning);
            return list;
        }
    }

    public static class ConstVals
    {
        public const string Any = "DVAL_ANY"; //Любой, не важно
        public const string Pending = "DVAL_PENDING"; //Ожидает подтверждения
        public const string Accepted = "DVAL_ACCEPTED"; //Одобрено
        public const string Declined = "DVAL_DECLINED"; //Отклонено
        public const string Male = "DVAL_MALE"; //Мужской
        public const string Female = "DVAL_FEMALE"; //Женский
        public const string House = "DVAL_HOUSE"; //Дом
        public const string Flat = "DVAL_FLAT"; //Квартира
        public const string Room = "DVAL_ROOM"; //Комната
        public const string Office = "DVAL_OFFICE"; //Офис
        public const string HotelRoom = "DVAL_HOTEL_ROOM"; //Гостинечный номер
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
        public const string Thing = "DVAL_THING";
        public const string Alien = "DVAL_ALIEN";
        public const string Greet = "GREET";
        public const string Reg = "REG";
        public const string Restore = "RESTORE";
        public const string EmailTemplate = "EMAIL_TEMPLATE";
    }
}