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
        public const string ApartmentOptions = "ApartmentOptions";
    }

    public static class ConstMetaDataProp
    {
        public const string Visible = "Visible";
        public const string Required = "Required";
        public const string DataType = "DataType";
    }
    public static class ConstDataType
    {
        public const string Text = "Text";
        public const string Date = "Date";
        public const string List = "List";
        public const string Multibox = "Multibox";
        public const string Email = "Email";
        public const string Phone = "Phone";
        public const string Adress = "Adress";
        public const string Price = "Price";
        public const string Rating = "Rating";
        public const string Image = "Image";
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

        public static List<string> ApartmentOptionsList()
        {
            List<string> list = new List<string>();
            list.Add("Parking");
            list.Add("Сoncierge");
            list.Add("Refrigerator");
            list.Add("Washing machine");
            list.Add("Air conditioning");


            return list;
        }
    }

    public static class ConstReservStatus
    {
        public const string Pending = "Pending";
        public const string Accepted = "Accepted";
        public const string Declined = "Declined";
        
    }
}