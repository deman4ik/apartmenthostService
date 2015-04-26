using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace apartmenthostService.Helpers
{
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
        public static List<string> ApartmentTypesList()
        {
         List<string> list = new List<string>();
         list.Add("House");
         list.Add("Flat");
         list.Add("Room");
         list.Add("Office");
         list.Add("Hotel Room");

         return list;
        }

        public static List<string> CohabitationTypesList()
        {
            List<string> list = new List<string>();
            list.Add("Separate residence");
            list.Add("Cohabitation");


            return list;
        }
 
    }
}