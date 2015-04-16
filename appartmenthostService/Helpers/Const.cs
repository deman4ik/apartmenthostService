using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appartmenthostService.Helpers
{
    public static class Const
    {
        public const string Custom = "Custom";
        public const string Base = "Base";
    }
    public static class ConstDataType
    {
        public const string Str = "Str";
        public const string Num = "Num";
        public const string Date = "Date";
        public const string Bool = "Bool";
    }
    public static class ConstTable
    {
        public const string ApartmentTable = "Apartment";
        public const string AdvertTable = "Advert";
        public const string ProfileTable = "Profile";
        public const string ReservationTable = "Reservation";
    }

    public static class ConstProp
    {
        public const string ApartmentType = "ApartmentType";
    }

  
    public static class ConstLang
    {
        public const string RU = "RU";
        public const string EN = "EN";
    }

    public static class ConstDicValsRU
    {
        public static List<string> ApartmentTypesList = new List<string>()
        {
            "Дом",
            "Квартира",
            "Комната",
            "Офис",
            "Номер"
        };
    }
}