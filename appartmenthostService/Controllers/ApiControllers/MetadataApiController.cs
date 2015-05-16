using System;
using System.CodeDom;
using System.Linq;
using System.Web.Http;
using apartmenthostService.Attributes;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;


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
            return GetMetadata(ConstType.Apartment, typeof(ApartmentDTO));
        }

        // GET api/Metadata/Advert
        [Route("api/Metadata/Advert")]
        public Metadata GetAdvert()
        {
            return GetMetadata(ConstType.Advert, typeof(AdvertDTO));
        }

        // GET api/Metadata/User
        [Route("api/Metadata/User")]
        public Metadata GetUser()
        {
            return GetMetadata(ConstType.User, typeof(UserDTO));
        }

        private Metadata GetMetadata(string objectType, Type type)
        {
            Metadata metadata = new Metadata
            {
                Name = objectType,
                LangName = Helper.GetObjectLangName(objectType),
                Items = type.GetProperties().Select(prop => new MetadataItem()
                {
                    Name = prop.Name,
                    LangName = Helper.GetItemLangName(prop.Name),
                    Type = Helper.GetTypeName(prop),
                    DataType =
                        (string)
                            Helper.GetAttributeValue(type, prop.Name, typeof(MetadataAttribute),
                                ConstMetaDataProp.DataType),
                    Dictionary = (string)
                Helper.GetAttributeValue(type, prop.Name, typeof(MetadataAttribute),
                    ConstMetaDataProp.Dictionary),
                    Multi = (bool)Helper.GetAttributeValue(type, prop.Name, typeof(MetadataAttribute),
                    ConstMetaDataProp.Multi),
                    GetRule = GetMetadataRule(type,typeof(GetRuleAttribute),prop.Name),
                    PostRule = GetMetadataRule(type, typeof(PostRuleAttribute), prop.Name),
                    PutRule = GetMetadataRule(type, typeof(PutRuleAttribute), prop.Name),
                    DeleteRule = GetMetadataRule(type, typeof(DeleteRuleAttribute), prop.Name),
                    Metadata = GetSubMetadata(Helper.GetTypeName(prop), objectType)
                }).ToList()
            };
            return metadata;
        }

        //private Metadata GetPropsMetadata(string objectType)
        //{
        //    string table = ConstTable.GetTableByObjectType(objectType);
        //    if (table != null)
        //    {
        //        Metadata propMetadata = new Metadata
        //        {
        //            Name = ConstType.PropCollection,
        //            Items = context.Props.AsQueryable()
        //                .Where(p => p.Tables.Any(t => t.Name == table))
        //                .Select(prop => new MetadataItem()
        //                {
        //                    Id = prop.Id,
        //                    Name = prop.Name,
        //                    Type = prop.Type,
        //                    DataType = prop.DataType,
        //                    GetRule = prop.GetRule,
        //                    PostRule = prop.PostRule,
        //                    DeleteRule = prop.DeleteRule,
        //                    DictionaryName = prop.Dictionary.Name,
        //                    DictionaryId = prop.DictionaryId,
        //                    DictionaryItems =
        //                        context.DictionaryItems.Where(di => di.DictionaryId == prop.DictionaryId)
        //                            .Select(dicitem => new DictionaryItemDTO()
        //                            {
        //                                Id = dicitem.Id,
        //                                StrValue = dicitem.StrValue,
        //                                NumValue = dicitem.NumValue,
        //                                DateValue = dicitem.DateValue,
        //                                BoolValue = dicitem.BoolValue,
        //                                Lang = dicitem.Lang
        //                            }).ToList()
        //                }).ToList()
        //        };

        //        return propMetadata;

        //    }
        //    return null;
        //}

        private Metadata GetSubMetadata(string objectType, string metadataType)
        {
            switch (objectType)
            {
                //case ConstType.PropCollection:
                //    return GetPropsMetadata(metadataType);
                case ConstType.Apartment:
                    return GetApartment();
                case ConstType.Advert:
                    return GetAdvert();
                case ConstType.User:
                    return GetUser();

                default:
                    return null;
            }
        }

        private MetadataRule GetMetadataRule(Type objType, Type atrType, string propName)
        {
            return new MetadataRule()
            {
                Order = (int)Helper.GetAttributeValue(objType, propName, atrType,
                                ConstMetaDataProp.Order),
                RequiredForm = (bool)Helper.GetAttributeValue(objType, propName, atrType,
                ConstMetaDataProp.RequiredForm),
                RequiredTransfer = (bool)Helper.GetAttributeValue(objType, propName, atrType,
                ConstMetaDataProp.RequiredTransfer),
                Visible = (bool)Helper.GetAttributeValue(objType, propName, atrType,
                ConstMetaDataProp.Visible)

            };
        }
    }
}
