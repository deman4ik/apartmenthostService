using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using appartmenthostService.DataObjects;
using appartmenthostService.Helpers;
using appartmenthostService.Models;
using AutoMapper.Internal;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace appartmenthostService.Controllers
{
     
     [AuthorizeLevel(AuthorizationLevel.Application)]
    public class MetadataApiController : ApiController
    {
        public ApiServices Services { get; set; }
        appartmenthostContext context = new appartmenthostContext();
        // GET api/Metadata/Apartment
         [Route("api/Metadata/Apartment")]
        public string GetApartment()
        {
             Metadata apartmentMetadata = new Metadata();
             apartmentMetadata.Items = typeof(ApartmentDTO).GetProperties().Select(prop => new MetadataItem()
             {
                 Type = Helper.GetTypeName(prop),
                 Name = prop.Name
             }).ToList();
             apartmentMetadata.Props = context.Props.AsQueryable().Where(p => p.Tables.Any(t => t.Name == ConstTable.ApartmentTable)).Select(prop => new PropDTO()
             {
                 Id = prop.Id,
                 CreatedAt = prop.CreatedAt,
                 UpdatedAt = prop.UpdatedAt,
                 Name = prop.Name,
                 DataType = prop.DataType,
                 DictionaryName = prop.Dictionary.Name,
                 DictionaryId = prop.DictionaryId,
                 DictionaryItems = context.DictionaryItems.Where(di => di.DictionaryId == prop.DictionaryId).Select(dicitem => new DictionaryItemDTO()
                 {
                     Id = dicitem.Id,
                     CreatedAt = dicitem.CreatedAt,
                     UpdatedAt = dicitem.UpdatedAt,
                     DictionaryId = dicitem.Id,
                     StrValue = dicitem.StrValue,
                     NumValue = dicitem.NumValue,
                     DateValue = dicitem.DateValue,
                     BoolValue = dicitem.BoolValue,
                     Lang = dicitem.Lang
                 }).ToList()
             }).ToList();
             return JsonConvert.SerializeObject(apartmentMetadata);
        }

         // GET api/Metadata/Advert
         [Route("api/Metadata/Advert")]
         public string GetAdvert()
         {
             Metadata advertMetadata = new Metadata();
             advertMetadata.Items = typeof(AdvertDTO).GetProperties().Select(prop => new MetadataItem()
             {
                 Type = Helper.GetTypeName(prop),
                 Name = prop.Name
             }).ToList();
             advertMetadata.Props = context.Props.AsQueryable().Where(p => p.Tables.Any(t => t.Name == ConstTable.AdvertTable)).Select(prop => new PropDTO()
             {
                 Id = prop.Id,
                 CreatedAt = prop.CreatedAt,
                 UpdatedAt = prop.UpdatedAt,
                 Name = prop.Name,
                 DataType = prop.DataType,
                 DictionaryName = prop.Dictionary.Name,
                 DictionaryId = prop.DictionaryId,
                 DictionaryItems = context.DictionaryItems.Where(di => di.DictionaryId == prop.DictionaryId).Select(dicitem => new DictionaryItemDTO()
                 {
                     Id = dicitem.Id,
                     CreatedAt = dicitem.CreatedAt,
                     UpdatedAt = dicitem.UpdatedAt,
                     DictionaryId = dicitem.Id,
                     StrValue = dicitem.StrValue,
                     NumValue = dicitem.NumValue,
                     DateValue = dicitem.DateValue,
                     BoolValue = dicitem.BoolValue,
                     Lang = dicitem.Lang
                 }).ToList()
             }).ToList();
             return JsonConvert.SerializeObject(advertMetadata);
         }

         // GET api/Metadata/User
         [Route("api/Metadata/User")]
         public string GetUser()
         {
             Metadata userMetadata = new Metadata();
             userMetadata.Items = typeof(UserDTO).GetProperties().Select(prop => new MetadataItem()
             {
                 Type = Helper.GetTypeName(prop),
                 Name = prop.Name
             }).ToList();
             userMetadata.Props = context.Props.AsQueryable().Where(p => p.Tables.Any(t => t.Name == ConstTable.ProfileTable)).Select(prop => new PropDTO()
             {
                 Id = prop.Id,
                 CreatedAt = prop.CreatedAt,
                 UpdatedAt = prop.UpdatedAt,
                 Name = prop.Name,
                 DataType = prop.DataType,
                 DictionaryName = prop.Dictionary.Name,
                 DictionaryId = prop.DictionaryId,
                 DictionaryItems = context.DictionaryItems.Where(di => di.DictionaryId == prop.DictionaryId).Select(dicitem => new DictionaryItemDTO()
                 {
                     Id = dicitem.Id,
                     CreatedAt = dicitem.CreatedAt,
                     UpdatedAt = dicitem.UpdatedAt,
                     DictionaryId = dicitem.Id,
                     StrValue = dicitem.StrValue,
                     NumValue = dicitem.NumValue,
                     DateValue = dicitem.DateValue,
                     BoolValue = dicitem.BoolValue,
                     Lang = dicitem.Lang
                 }).ToList()
             }).ToList();
             return JsonConvert.SerializeObject(userMetadata);
         }
    }
}
