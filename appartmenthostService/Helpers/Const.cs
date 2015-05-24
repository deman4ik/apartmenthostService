using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Web;

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
        public const string Advert = "Advert";
        public const string Reservation = "Reservation";
        public const string PropCollection = "PropCollection";

    }
    public static class ConstTable
    {
        public const string ApartmentTable   = "Apartment";
        public const string AdvertTable      = "Advert";
        public const string ProfileTable     = "Profile";
        public const string ReservationTable = "Reservation";


        public static string GetTableByObjectType(string type)
        {
            switch (type)
            {
                case ConstType.Apartment:
                    return ApartmentTable;
                case ConstType.Advert:
                    return AdvertTable;
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
        public const string Text        = "Text";        // Текстовое поле
        public const string Date        = "Date";        // Выбора даты
        public const string List        = "List";        // Выпадающий список или RadioButton
        public const string Multibox    = "Multibox";    // Выбор нескольких значений 
        public const string Email       = "Email";       // Поле для ввода email проверка формата 
        public const string Phone       = "Phone";       // Поле для ввода телефона проверка формата
        public const string Adress      = "Adress";      // Поле для ввода Адреса автоподставнока
        public const string Price       = "Price";       // Поле для ввода Цены
        public const string Rating      = "Rating";      // 5 звезд рейтинга
        public const string Image       = "Image";       // Отображение картинки по URL 
        public const string Id          = "Id";          // Уникальный идентификатор
        public const string Apartment   = "Apartment";   // Объект типа Жилье
        public const string Advert      = "Advert";      // Объект типа Объявление
        public const string User        = "User";        // Объект типа Пользователь
        public const string Reservation = "Reservation"; // Объект типа Бронь
        public const string Favorite    = "Favorite";    // Избранное
        public const string ApprovedReservations = "ApprovedReservations"; //  Список объектов типа Бронь - утвержденные
        public const string Reviews     = "Reviews"; // Отзывы
        public const string RelatedAdverts = "RelatedAdverts"; // Похожие объявления
        public const string PropVals    = "PropVals";    // Список объектов типа Значения свойств
        public const string Lang        = "Lang";        // Код языка
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
            List<string> list = new List<string>();
            list.Add(ConstVals.Male);
            list.Add(ConstVals.Female);
            list.Add(ConstVals.Any);
            return list; 
        }
        public static List<string> ApartmentTypesList()
        {
            List<string> list = new List<string>();
            list.Add(ConstVals.House);
            list.Add(ConstVals.Flat);
            list.Add(ConstVals.Room);
            list.Add(ConstVals.Office);
            list.Add(ConstVals.HotelRoom);
            return list;
        }

        public static List<string> CohabitationTypesList()
        {
            List<string> list = new List<string>();
            list.Add(ConstVals.SeperateResidence);
            list.Add(ConstVals.Cohabitation);
            list.Add(ConstVals.Any);
            return list;
        }

        public static List<string> ApartmentOptionsList()
        {
            List<string> list = new List<string>();
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
        public const string Any               = "DVAL_ANY";                //Любой, не важно
        public const string Pending           = "DVAL_PENDING";            //Ожидает подтверждения
        public const string Accepted          = "DVAL_ACCEPTED";           //Одобрено
        public const string Declined          = "DVAL_DECLINED";           //Отклонено
        public const string Male              = "DVAL_MALE";               //Мужской
        public const string Female            = "DVAL_FEMALE";             //Женский
        public const string House             = "DVAL_HOUSE";              //Дом
        public const string Flat              = "DVAL_FLAT";               //Квартира
        public const string Room              = "DVAL_ROOM";               //Комната
        public const string Office            = "DVAL_OFFICE";             //Офис
        public const string HotelRoom         = "DVAL_HOTEL_ROOM";         //Гостинечный номер
        public const string SeperateResidence = "DVAL_SEPARATE_RESIDENCE"; //Раздельное проживание
        public const string Cohabitation      = "DVAL_COHABITATION";       //Совместное проживание
        public const string Parking           = "DVAL_PARKING";            //Паркова
        public const string Concierge         = "DVAL_СONCIERGE";          //Консьерж
        public const string Refrigerator      = "DVAL_REFRIGERATOR";       //Холодильник
        public const string WashingMachine    = "DVAL_WASHING_MACHINE";    //Стиральная машина
        public const string AirConditioning   = "DVAL_AIR_CONDITIONING";   //Кондиционер



    }
}