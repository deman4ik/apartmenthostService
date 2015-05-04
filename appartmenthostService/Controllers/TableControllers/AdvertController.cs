using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using apartmenthostService.DataObjects;
using Microsoft.WindowsAzure.Mobile.Service;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class AdvertController : TableController<Advert>
    {
         apartmenthostContext context = new apartmenthostContext();
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            
            DomainManager = new EntityDomainManager<Advert>(context, Request, Services);
        }

        // GET tables/Advert
       // [QueryableExpand("Apartments")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public IQueryable<AdvertDTO> GetAllAdverts()
        {
            return Query().Select(x => new AdvertDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                Description = x.Description,
                ApartmentId = x.ApartmentId,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                PriceDay = x.PriceDay,
                PricePeriod = x.PricePeriod,
                Lang = x.Lang,
                User = new UserDTO()
                {
                    Id = x.User.Profile.Id,
                    FirstName = x.User.Profile.FirstName,
                    LastName = x.User.Profile.LastName,
                    Gender = x.User.Profile.Gender,
                    Phone = x.User.Profile.Phone
                },
                Apartment = new ApartmentDTO()
                {
                    Id = x.Apartment.Id,
                    Name = x.Apartment.Name,
                    UserId = x.Apartment.UserId,
                    Adress = x.Apartment.Adress,
                    Latitude = x.Apartment.Latitude,
                    Longitude = x.Apartment.Longitude,
                    PropsVals = x.Apartment.PropVals
                      .Select(appdto => new PropValDTO()
                      {
                          Id = appdto.Id,
                          PropId = appdto.PropId,
                          Name = appdto.Prop.Name,
                          Type = appdto.Prop.Type,
                          StrValue = appdto.StrValue,
                          NumValue = appdto.NumValue,
                          DateValue = appdto.DateValue,
                          BoolValue = appdto.BoolValue,
                          DictionaryItemId = appdto.DictionaryItemId,
                          DictionaryItem = new DictionaryItemDTO()
                          {
                              StrValue = appdto.DictionaryItem.StrValue,
                              NumValue = appdto.DictionaryItem.NumValue,
                              DateValue = appdto.DictionaryItem.DateValue,
                              BoolValue = appdto.DictionaryItem.BoolValue
                          }
                      }).ToList()
                },
                PropsVals = x.PropVals
                      .Select(pdto => new PropValDTO()
                      {
                          Id = pdto.Id,
                          PropId = pdto.PropId,
                          Name = pdto.Prop.Name,
                          Type = pdto.Prop.Type,
                          StrValue = pdto.StrValue,
                          NumValue = pdto.NumValue,
                          DateValue = pdto.DateValue,
                          BoolValue = pdto.BoolValue,
                          DictionaryItemId = pdto.DictionaryItemId,
                          DictionaryItem = new DictionaryItemDTO()
                          {
                              StrValue = pdto.DictionaryItem.StrValue,
                              NumValue = pdto.DictionaryItem.NumValue,
                              DateValue = pdto.DictionaryItem.DateValue,
                              BoolValue = pdto.DictionaryItem.BoolValue
                          }
                      }).ToList()

            });
        }

        // GET tables/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
       // [QueryableExpand("Apartments")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public SingleResult<AdvertDTO> GetAdvert(string id)
        {
            var result = Lookup(id).Queryable.Select(x => new AdvertDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                Description = x.Description,
                ApartmentId = x.ApartmentId,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                PriceDay = x.PriceDay,
                PricePeriod = x.PricePeriod,
                Lang = x.Lang,
                User = new UserDTO()
                {
                    Id = x.User.Profile.Id,
                    FirstName = x.User.Profile.FirstName,
                    LastName = x.User.Profile.LastName,
                    Gender = x.User.Profile.Gender,
                    Phone = x.User.Profile.Phone
                },
                Apartment = new ApartmentDTO()
                {
                    Id = x.Apartment.Id,
                    Name = x.Apartment.Name,
                    UserId = x.Apartment.UserId,
                    Adress = x.Apartment.Adress,
                    Latitude = x.Apartment.Latitude,
                    Longitude = x.Apartment.Longitude,
                    PropsVals = x.Apartment.PropVals
                      .Select(appdto => new PropValDTO()
                      {
                          Id = appdto.Id,
                          PropId = appdto.PropId,
                          Name = appdto.Prop.Name,
                          Type = appdto.Prop.Type,
                          StrValue = appdto.StrValue,
                          NumValue = appdto.NumValue,
                          DateValue = appdto.DateValue,
                          BoolValue = appdto.BoolValue,
                          DictionaryItemId = appdto.DictionaryItemId,
                          DictionaryItem = new DictionaryItemDTO()
                          {
                              StrValue = appdto.DictionaryItem.StrValue,
                              NumValue = appdto.DictionaryItem.NumValue,
                              DateValue = appdto.DictionaryItem.DateValue,
                              BoolValue = appdto.DictionaryItem.BoolValue
                          }
                      }).ToList()
                },
                PropsVals = x.PropVals
                      .Select(pdto => new PropValDTO()
                      {
                          Id = pdto.Id,
                          PropId = pdto.PropId,
                          Name = pdto.Prop.Name,
                          Type = pdto.Prop.Type,
                          StrValue = pdto.StrValue,
                          NumValue = pdto.NumValue,
                          DateValue = pdto.DateValue,
                          BoolValue = pdto.BoolValue,
                          DictionaryItemId = pdto.DictionaryItemId,
                          DictionaryItem = new DictionaryItemDTO()
                          {
                              StrValue = pdto.DictionaryItem.StrValue,
                              NumValue = pdto.DictionaryItem.NumValue,
                              DateValue = pdto.DictionaryItem.DateValue,
                              BoolValue = pdto.DictionaryItem.BoolValue
                          }
                      }).ToList()

            });
            return SingleResult.Create(result);
        }

    }
}