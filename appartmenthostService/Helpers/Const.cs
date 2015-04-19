using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        public const string UserCollection = "UserCollection";
        public const string PropValCollection = "PropValCollection";
        public const string ApartmentCollection = "ApartmentCollection";
        public const string AdvertCollection = "AdverCollection";
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
        public const string CohabitationType = "CohabitationType";
    }

  
    public static class ConstLang
    {
        public const string RU = "RU";
        public const string EN = "EN";
    }

    public static class ConstDicValsRU
    {
        public static Dictionary<string, string> ApartmentTypesList()
        {
         Dictionary<string, string> ApartmentTypesDic = new Dictionary<string, string>();
         ApartmentTypesDic.Add("House","Дом");
         ApartmentTypesDic.Add("Flat", "Квартира");
         ApartmentTypesDic.Add("Room", "Комната");
         ApartmentTypesDic.Add("Office", "Офис");
         ApartmentTypesDic.Add("Hotel Room", "Номер");

            return ApartmentTypesDic;
        }

        public static Dictionary<string, string> CohabitationTypesList()
        {
            Dictionary<string,string> CohabitationTypesDic = new Dictionary<string, string>();
            CohabitationTypesDic.Add("Separate residence","Раздельное");
            CohabitationTypesDic.Add("Cohabitation", "Совместное");
            return CohabitationTypesDic;
        }
    }
}