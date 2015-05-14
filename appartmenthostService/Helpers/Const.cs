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
        public const string PropCollection = "PropCollection";

    }
    public static class ConstTable
    {
        public const string ApartmentTable = "Apartment";
        public const string AdvertTable = "Advert";
        public const string ProfileTable = "Profile";
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
        public const string ApartmentType = "MD_PRP_APARTMENTTYPE";
        public const string CohabitationType = "MD_PRP_COHABITATIONTYPE";
        public const string ApartmentOptions = "MD_PRP_APARTMENTOPTIONS";
    }

    public static class ConstMetaDataProp
    {
        public const string Visible = "Visible";
        public const string RequiredForm = "RequiredForm";
        public const string RequiredTransfer = "RequiredTransfer";
        public const string Order = "Order";
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
        public const string Id = "Id";
        public const string Apartment = "Apartment";
        public const string Advert = "Advert";
        public const string User = "User";
        public const string Reservation = "Reservation";
        public const string PropVals = "PropVals";
        public const string Lang = "Lang";
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
            list.Add("DVAL_HOUSE");
            list.Add("DVAL_FLAT");
            list.Add("DVAL_ROOM");
            list.Add("DVAL_OFFICE");
            list.Add("DVAL_HOTEL_ROOM");

            return list;
        }

        public static List<string> CohabitationTypesList()
        {
            List<string> list = new List<string>();
            list.Add("DVAL_SEPARATE_RESIDENCE");
            list.Add("DVAL_COHABITATION");


            return list;
        }

        public static List<string> ApartmentOptionsList()
        {
            List<string> list = new List<string>();
            list.Add("DVAL_PARKING");
            list.Add("DVAL_СONCIERGE");
            list.Add("DVAL_REFRIGERATOR");
            list.Add("DVAL_WASHING_MACHINE");
            list.Add("DVAL_AIR_CONDITIONING");


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