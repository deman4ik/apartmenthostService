using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using CloudinaryDotNet;
using Account = CloudinaryDotNet.Account;

namespace apartmenthostService.Helpers
{
    public class MetaHelper
    {
        public static object GetAttributeValue(Type objectType, string propertyName, Type attributeType,
            string attributePropertyName)
        {
            var propertyInfo = objectType.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                if (Attribute.IsDefined(propertyInfo, attributeType))
                {
                    var attributeInstance = Attribute.GetCustomAttribute(propertyInfo, attributeType);
                    if (attributeInstance != null)
                    {
                        return (from info in attributeType.GetProperties()
                            where
                                info.CanRead &&
                                string.Compare(info.Name, attributePropertyName,
                                    StringComparison.InvariantCultureIgnoreCase) == 0
                            select info.GetValue(attributeInstance, null)).FirstOrDefault();
                    }
                }
            }

            return null;
        }
    }

    public class CheckHelper
    {
        public static ResponseDTO IsNull(string item, string itemName, string errType)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                var respList = new List<string>();
                respList.Add(itemName);
                return RespH.Create(errType, respList);
            }
            return null;
        }

        public static ResponseDTO IsNull(double item, string itemName, string errType)
        {
            if (item <= 0)
            {
                var respList = new List<string>();
                respList.Add(itemName);
                return RespH.Create(errType, respList);
            }
            return null;
        }

        public static ResponseDTO IsCardExist(IApartmenthostContext context, string userId, string errType)
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

        public static ResponseDTO IsCardNameExist(IApartmenthostContext context, string name, string errType)
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

        public static ResponseDTO IsValidDates(DateTime dateFrom, DateTime dateTo, string errType)
        {
            // Check Dates
            if (string.IsNullOrEmpty(dateFrom.ToString()) && string.IsNullOrEmpty(dateTo.ToString()))
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

        public static ResponseDTO IsProfileFill(IApartmenthostContext context, string userId, bool checkPhone = false)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == userId);
            var profile = context.Profile.SingleOrDefault(x => x.Id == userId);
            if (user == null || profile == null) return RespH.Create(RespH.SRV_USER_NOTFOUND);
            if (user.Blocked) return RespH.Create(RespH.SRV_USER_BLOCKED);
            if (!user.EmailConfirmed) return RespH.Create(RespH.SRV_USER_NOT_CONFIRMED);
            if (string.IsNullOrWhiteSpace(profile.FirstName))
                return RespH.Create(RespH.SRV_USER_NO_NAME);
            if (checkPhone)
                if (user.PhoneStatus != ConstVals.PConf) return RespH.Create(RespH.SRV_CARD_PHONE_UNCONF);
            return null;
        }

        public static string CleanPhone(string phone)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(phone, "");
        }
    }

    public static class CloudinaryHelper
    {
        static CloudinaryHelper()
        {
            var clacc = new Account(Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
                Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
                Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET"));
            Cloudinary = new Cloudinary(clacc);
        }

        public static Cloudinary Cloudinary { get; set; }
    }
}