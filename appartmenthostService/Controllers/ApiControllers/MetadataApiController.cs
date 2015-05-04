using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using apartmenthostService.Attributes;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using AutoMapper.Internal;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace apartmenthostService.Controllers
{
     
     [AuthorizeLevel(AuthorizationLevel.Application)]
    public class MetadataApiController : ApiController
    {
        public ApiServices Services { get; set; }
        apartmenthostContext context = new apartmenthostContext();
        // GET api/Metadata/Apartment
         [Route("api/Metadata/Apartment")]
        public Metadata GetApartment()
        {
             Metadata apartmentMetadata = new Metadata();
             apartmentMetadata.Items = typeof(ApartmentDTO).GetProperties().Select(prop => new MetadataItem()
             {
                 Name = prop.Name,
                 Type = Helper.GetTypeName(prop),
                 DataType = (string)Helper.GetAttributeValue(typeof(ApartmentDTO), prop.Name, typeof(MetadataAttribute), ConstMetaDataProp.DataType),
                 Visible = (bool)Helper.GetAttributeValue(typeof(ApartmentDTO), prop.Name, typeof(MetadataAttribute), ConstMetaDataProp.Visible),
                 Required = (bool)Helper.GetAttributeValue(typeof(ApartmentDTO), prop.Name, typeof(MetadataAttribute), ConstMetaDataProp.Required),
                 

             }).ToList();

             apartmentMetadata.Props = context.Props.AsQueryable().Where(p => p.Tables.Any(t => t.Name == ConstTable.ApartmentTable)).Select(prop => new PropDTO()
             {
                 Id = prop.Id,
                 Name = prop.Name,
                 Type = prop.Type,
                 DataType = prop.DataType,
                 Visbile = prop.Visbile,
                 Required = prop.Required,
                 DictionaryName = prop.Dictionary.Name,
                 DictionaryId = prop.DictionaryId,
                 DictionaryItems = context.DictionaryItems.Where(di => di.DictionaryId == prop.DictionaryId).Select(dicitem => new DictionaryItemDTO()
                 {
                     Id = dicitem.Id,
                     StrValue = dicitem.StrValue,
                     NumValue = dicitem.NumValue,
                     DateValue = dicitem.DateValue,
                     BoolValue = dicitem.BoolValue,
                     Lang = dicitem.Lang
                 }).ToList()
             }).ToList();
             return apartmentMetadata;
        }

         // GET api/Metadata/Advert
         [Route("api/Metadata/Advert")]
         public Metadata GetAdvert()
         {
             Metadata advertMetadata = new Metadata();
             advertMetadata.Items = typeof(AdvertDTO).GetProperties().Select(prop => new MetadataItem()
             {
                 
                 Name = prop.Name,
                 Type = Helper.GetTypeName(prop),
                 DataType = (string)Helper.GetAttributeValue(typeof(ApartmentDTO), prop.Name, typeof(MetadataAttribute), ConstMetaDataProp.DataType),
                 Visible = (bool)Helper.GetAttributeValue(typeof(AdvertDTO), prop.Name, typeof(MetadataAttribute), ConstMetaDataProp.Visible),
                 Required = (bool)Helper.GetAttributeValue(typeof(AdvertDTO), prop.Name, typeof(MetadataAttribute), ConstMetaDataProp.Required)
             }).ToList();
             advertMetadata.Props = context.Props.AsQueryable().Where(p => p.Tables.Any(t => t.Name == ConstTable.AdvertTable)).Select(prop => new PropDTO()
             {
                 Id = prop.Id,
                 Name = prop.Name,
                 Type = prop.Type,
                 DataType = prop.DataType,
                 Visbile = prop.Visbile,
                 Required = prop.Required,
                 DictionaryName = prop.Dictionary.Name,
                 DictionaryId = prop.DictionaryId,
                 DictionaryItems = context.DictionaryItems.Where(di => di.DictionaryId == prop.DictionaryId).Select(dicitem => new DictionaryItemDTO()
                 {
                     Id = dicitem.Id,
                     StrValue = dicitem.StrValue,
                     NumValue = dicitem.NumValue,
                     DateValue = dicitem.DateValue,
                     BoolValue = dicitem.BoolValue,
                     Lang = dicitem.Lang
                 }).ToList()
             }).ToList();
             return advertMetadata;
         }

         // GET api/Metadata/User
         [Route("api/Metadata/User")]
         public Metadata GetUser()
         {
             Metadata userMetadata = new Metadata();
             userMetadata.Items = typeof(UserDTO).GetProperties().Select(prop => new MetadataItem()
             {
                 Name = prop.Name,
                 Type = Helper.GetTypeName(prop),
                 DataType = (string)Helper.GetAttributeValue(typeof(ApartmentDTO), prop.Name, typeof(MetadataAttribute), ConstMetaDataProp.DataType),
                 Visible = (bool)Helper.GetAttributeValue(typeof(UserDTO), prop.Name, typeof(MetadataAttribute), ConstMetaDataProp.Visible),
                 Required = (bool)Helper.GetAttributeValue(typeof(UserDTO), prop.Name, typeof(MetadataAttribute), ConstMetaDataProp.Required)
             }).ToList();
             userMetadata.Props = context.Props.AsQueryable().Where(p => p.Tables.Any(t => t.Name == ConstTable.ProfileTable)).Select(prop => new PropDTO()
             {
                 Id = prop.Id,
                 Name = prop.Name,
                 Type = prop.Type,
                 DataType = prop.DataType,
                 Visbile = prop.Visbile,
                 Required = prop.Required,
                 DictionaryName = prop.Dictionary.Name,
                 DictionaryId = prop.DictionaryId,
                 DictionaryItems = context.DictionaryItems.Where(di => di.DictionaryId == prop.DictionaryId).Select(dicitem => new DictionaryItemDTO()
                 {
                     Id = dicitem.Id,
                     StrValue = dicitem.StrValue,
                     NumValue = dicitem.NumValue,
                     DateValue = dicitem.DateValue,
                     BoolValue = dicitem.BoolValue,
                     Lang = dicitem.Lang
                 }).ToList()
             }).ToList();
             return userMetadata;
         }
    }
}
