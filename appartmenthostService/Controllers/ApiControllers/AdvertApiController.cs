using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class AdvertApiController : ApiController
    {
        public ApiServices Services { get; set; }
        private apartmenthostContext context = new apartmenthostContext();

        /// <summary>
        /// POST api/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        [Route("api/Advert")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage PostAdvert(AdvertDTO advert)
        {
            try
            {
                var respList = new List<string>();

                // Check Advert is not NULL 
                if (advert == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_NULL));

                // Check Advert Name is not NULL
                if (String.IsNullOrWhiteSpace(advert.Name))
                {
                    respList.Add("Name");
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_REQUIRED, respList));
                }

                // Check Advert not Exist
                var currentAdvertCount = context.Apartments.Count(a => a.Name == advert.Name);
                if (currentAdvertCount > 0)
                {
                    respList.Add(advert.Name);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_EXISTS, respList));
                }

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                
                // Check Properties Exists
                foreach (var propVal in advert.PropsVals)
                {
                    var prop = context.Props.AsQueryable().SingleOrDefault(p => p.Id == propVal.PropId);
                    if (prop == null)
                    {
                        respList.Add(propVal.PropId);
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_PROP_NOTFOUND, respList));
                    }
                    if (propVal.DictionaryItemId != null)
                    {
                        var dicItem = context.DictionaryItems.SingleOrDefault(di => di.Id == propVal.DictionaryItemId);
                        if (dicItem == null)
                        {
                            respList.Add(propVal.DictionaryItemId);
                            return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_DICTIONARYITEM_NOTFOUND, respList));
                        }
                    }
                }

                // Check Apartment Exists
                if (String.IsNullOrWhiteSpace(advert.ApartmentId))
                {
                    respList.Add("ApartmentId");
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_REQUIRED, respList));
                }
                var apartment = context.Apartments.SingleOrDefault(a => a.Id == advert.ApartmentId);
                if (apartment == null)
                {
                    respList.Add(advert.ApartmentId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                }
                //Apartment apartment;
                //if (String.IsNullOrWhiteSpace(advert.ApartmentId))
                //{
                //    advert.ApartmentId = Guid.NewGuid().ToString();
                //    apartment = new Apartment()
                //    {
                //        Id = advert.ApartmentId,
                //        Name = advert.Apartment.Name,
                //        UserId = account.UserId,
                //        Price = advert.Apartment.Price,
                //        Adress = advert.Apartment.Adress,
                //        Latitude = advert.Apartment.Latitude,
                //        Longitude = advert.Apartment.Longitude,
                //        Rating = 0,
                //        Lang = advert.Lang,
                //        PropVals = advert.Apartment.PropsVals.Select(pv => new PropVal()
                //        {
                //            Id = Guid.NewGuid().ToString(),
                //            PropId = pv.PropId,
                //            ApartmentItemId = advert.ApartmentId,
                //            StrValue = pv.StrValue,
                //            NumValue = pv.NumValue,
                //            DateValue = pv.DateValue,
                //            BoolValue = pv.BoolValue,
                //            DictionaryItemId = pv.DictionaryItemId,
                //            Lang = advert.Lang
                //        }).ToList()
                //    };
                //}
                //else
                //{
                //    apartment = context.Apartments.SingleOrDefault(a => a.Id == advert.ApartmentId);
                //}
                // Generate 
                string advertGuid = Guid.NewGuid().ToString();
                context.Set<Advert>().Add(new Advert()
                {
                    Id = advertGuid,
                    Name = advert.Name,
                    UserId = account.UserId,
                    Description = advert.Description,
                    ApartmentId = advert.ApartmentId,
                    DateFrom = advert.DateFrom,
                    DateTo = advert.DateTo,
                    PriceDay = advert.PriceDay,
                    PricePeriod = advert.PricePeriod,
                    Lang = advert.Lang
                });

                context.SaveChanges();
                respList.Add(advertGuid);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }

        
        /// <summary>
        /// PUT api/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        /// <param name="id">The ID of the Advert.</param>
        /// <param name="advert">The Advert changed object.</param>
        [Route("api/Advert/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPut]
        public HttpResponseMessage PutAdvert(string id, AdvertDTO advert)
        {
            return null;      
        }

        /// <summary>
        /// DELETE api/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        /// <param name="id">The ID of the Advert.</param>
        [Route("api/Advert/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpDelete]
        public HttpResponseMessage DeleteAdvert(string id)
        {
            return null;
        }
    }
}
