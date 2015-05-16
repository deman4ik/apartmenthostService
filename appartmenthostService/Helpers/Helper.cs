using System;
using System.Collections.Generic;
using System.Reflection;
using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace apartmenthostService.Helpers
{
    public class Helper
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
            if (type.PropertyType == typeof (string))
                return ConstType.Str;
            if (type.PropertyType == typeof (decimal?) || type.PropertyType == typeof (decimal) ||
                type.PropertyType == typeof (int?) || type.PropertyType == typeof (float?) ||
                type.PropertyType == typeof (int) || type.PropertyType == typeof (float))
                return ConstType.Num;
            if (type.PropertyType == typeof (DateTime?) || type.PropertyType == typeof (DateTimeOffset?) ||
                type.PropertyType == typeof (DateTime) || type.PropertyType == typeof (DateTimeOffset))
                return ConstType.Date;
            if (type.PropertyType == typeof (bool?) || type.PropertyType == typeof (bool))
                return ConstType.Bool;
            if (type.PropertyType == typeof (ICollection<PropValDTO>))
                return ConstType.PropCollection;
            if (type.PropertyType == typeof(UserDTO))
                return ConstType.User;
            if (type.PropertyType == typeof(ApartmentDTO))
                return ConstType.Apartment;
            if (type.PropertyType == typeof(AdvertDTO))
                return ConstType.Advert;
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

   
}
