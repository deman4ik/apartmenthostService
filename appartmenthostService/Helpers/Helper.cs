using System;
using System.Collections.Generic;
using System.Reflection;
using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Helpers
{
    public class Helper
    {
        public static string GetTypeName(PropertyInfo type)
        {
            if (type.PropertyType == typeof (string))
                return ConstDataType.Str;
            if (type.PropertyType == typeof (decimal?) || type.PropertyType == typeof (decimal) ||
                type.PropertyType == typeof (int?) || type.PropertyType == typeof (float?) ||
                type.PropertyType == typeof (int) || type.PropertyType == typeof (float))
                return ConstDataType.Num;
            if (type.PropertyType == typeof (DateTime?) || type.PropertyType == typeof (DateTimeOffset?) ||
                type.PropertyType == typeof (DateTime) || type.PropertyType == typeof (DateTimeOffset))
                return ConstDataType.Date;
            if (type.PropertyType == typeof (bool?) || type.PropertyType == typeof (bool))
                return ConstDataType.Bool;
            if (type.PropertyType == typeof (ICollection<PropValDTO>))
                return ConstDataType.PropValCollection;
            if (type.PropertyType == typeof (ICollection<UserDTO>))
                return ConstDataType.UserCollection;
            if (type.PropertyType == typeof (ICollection<ApartmentDTO>))
                return ConstDataType.ApartmentCollection;
            if (type.PropertyType == typeof (ICollection<AdvertDTO>))
                return ConstDataType.AdvertCollection;
            return type.PropertyType.Name;

        }
    }
}
