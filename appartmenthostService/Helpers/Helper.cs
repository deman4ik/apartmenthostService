using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using CloudinaryDotNet;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Helpers
{
    public class MetaHelper
    {
        public static string GetItemLangName(string name)
        {
            return "MD_ITM_" + name.ToUpper();
        }

        public static string GetObjectLangName(string name)
        {
            return "MD_OBJ_" + name.ToUpper();
        }
        public static string GetTypeName(PropertyInfo type)
        {
            if (type.PropertyType == typeof(string))
                return ConstType.Str;
            if (type.PropertyType == typeof(decimal?) || type.PropertyType == typeof(decimal) ||
                type.PropertyType == typeof(int?) || type.PropertyType == typeof(float?) ||
                type.PropertyType == typeof(int) || type.PropertyType == typeof(float))
                return ConstType.Num;
            if (type.PropertyType == typeof(DateTime?) || type.PropertyType == typeof(DateTimeOffset?) ||
                type.PropertyType == typeof(DateTime) || type.PropertyType == typeof(DateTimeOffset))
                return ConstType.Date;
            if (type.PropertyType == typeof(bool?) || type.PropertyType == typeof(bool))
                return ConstType.Bool;
            if (type.PropertyType == typeof(ICollection<PropValDTO>))
                return ConstType.PropCollection;
            if (type.PropertyType == typeof(UserDTO))
                return ConstType.User;
            if (type.PropertyType == typeof(ApartmentDTO))
                return ConstType.Apartment;
            if (type.PropertyType == typeof(CardDTO))
                return ConstType.Card;
            return type.PropertyType.Name;

        }

        public static object GetAttributeValue(Type objectType, string propertyName, Type attributeType, string attributePropertyName)
        {
            var propertyInfo = objectType.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                if (Attribute.IsDefined(propertyInfo, attributeType))
                {
                    var attributeInstance = Attribute.GetCustomAttribute(propertyInfo, attributeType);
                    if (attributeInstance != null)
                    {
                        foreach (PropertyInfo info in attributeType.GetProperties())
                        {
                            if (info.CanRead &&
                                String.Compare(info.Name, attributePropertyName, StringComparison.InvariantCultureIgnoreCase) ==
                                0)
                            {
                                return info.GetValue(attributeInstance, null);
                            }
                        }
                    }
                }
            }

            return null;
        }


    }

    public class CheckHelper
    {
        public static ResponseDTO isNull(string item, string itemName, string errType)
        {
            if (String.IsNullOrWhiteSpace(item))
            {
                var respList = new List<string>();
                respList.Add(itemName);
                return RespH.Create(errType, respList);
            }
            return null;
        }

        public static ResponseDTO isNull(double item, string itemName, string errType)
        {
            if (item <= 0)
            {
                var respList = new List<string>();
                respList.Add(itemName);
                return RespH.Create(errType, respList);
            }
            return null;
        }
        public static ResponseDTO isCardExist(apartmenthostContext context, string userId, string errType)
        {
            var currentAdvertCount = context.Cards.Count(a => a.UserId == userId);
            if (currentAdvertCount > 0)
            {
                var respList = new List<string>();
                respList.Add(userId);
                return RespH.Create(RespH.SRV_CARD_EXISTS, respList);
            }
            return null;
        }

        public static ResponseDTO isCardNameExist(apartmenthostContext context, string name, string errType)
        {
            var currentAdvertCount = context.Cards.Count(a => a.Name == name);
            if (currentAdvertCount > 0)
            {
                var respList = new List<string>();
                respList.Add(name);
                return RespH.Create(RespH.SRV_CARD_EXISTS, respList);
            }
            return null;
        }
        public static ResponseDTO isValidDicItem(apartmenthostContext context, string item, string dicName, string itemName, string errType)
        {
            var dicItem =
                context.DictionaryItems.SingleOrDefault(di => di.Dictionary.Name == dicName && di.StrValue == item);
            if (dicItem == null)
            {
                var respList = new List<string>();
                respList.Add(item);
                respList.Add(itemName);
                respList.Add(dicName);
                return RespH.Create(errType, respList);
            }
            return null;
        }

        public static ResponseDTO isValidDicItem(apartmenthostContext context, decimal item, string dicName, string itemName, string errType)
        {
            var dicItem =
                context.DictionaryItems.SingleOrDefault(di => di.Dictionary.Name == dicName && di.NumValue == item);
            if (dicItem == null)
            {
                var respList = new List<string>();
                respList.Add(itemName);
                respList.Add(dicName);
                return RespH.Create(errType, respList);
            }
            return null;
        }

        public static ResponseDTO isValidDicItem(apartmenthostContext context, DateTime item, string dicName, string itemName, string errType)
        {
            var dicItem =
                context.DictionaryItems.SingleOrDefault(di => di.Dictionary.Name == dicName && di.DateValue == item);
            if (dicItem == null)
            {
                var respList = new List<string>();
                respList.Add(itemName);
                respList.Add(dicName);
                return RespH.Create(errType, respList);
            }
            return null;
        }

        public static ResponseDTO isValidDates(DateTime dateFrom, DateTime dateTo, string errType)
        {
            // Check Dates
            if (String.IsNullOrEmpty(dateFrom.ToString()) && String.IsNullOrEmpty(dateTo.ToString()))
            {
                if (DateTimeOffset.Compare(dateFrom, dateTo) >= 0)
                {
                    var respList = new List<string>();
                    respList.Add(dateFrom.ToString());
                    respList.Add(dateTo.ToString());
                    return RespH.Create(errType, respList);
                }
            }
            return null;
        }

    }

    public static class CloudinaryHelper
    {
        public static Cloudinary Cloudinary { get; set; }

        static CloudinaryHelper()
        {
            CloudinaryDotNet.Account clacc = new CloudinaryDotNet.Account(ConfigurationManager.AppSettings["CLOUDINARY_CLOUD_NAME"],
                                                                           ConfigurationManager.AppSettings["CLOUDINARY_API_KEY"],
                                                                           ConfigurationManager.AppSettings["CLOUDINARY_API_SECRET"]);
            Cloudinary = new Cloudinary(clacc);
        }


    }

}
