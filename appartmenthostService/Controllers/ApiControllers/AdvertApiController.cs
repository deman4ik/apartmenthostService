using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using LinqKit;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class AdvertApiController : ApiController
    {
        public ApiServices Services { get; set; }
        private apartmenthostContext context = new apartmenthostContext();

        //[Route("api/Advert")]
        //[HttpGet]
        //public HttpResponseMessage GetAdvert()
        //{
            
        //    var dic = new Dictionary<string, string>();
        //    var propsvals = context.PropVals.Where(p => p.AdvertItemId == "a1").Select(appdto => new PropValDTO()
        //              {

        //                  Name = appdto.Prop.Name,

        //                  DictionaryItem = new DictionaryItemDTO()
        //                  {
        //                      StrValue = appdto.DictionaryItem.StrValue,
        //                  }
        //              });

  
        //   var multiprops = propsvals.GroupBy(p => p.Name)
        //        .Where(g => g.Count() > 1)
        //        .Select(y => y.Key)
        //        .ToList();

        //   var singleprops = propsvals.GroupBy(p => p.Name)
        //       .Where(g => g.Count() == 1)
        //       .Select(y => y.Key)
        //       .ToList();
        //    var vals = new List<string>();
        //    foreach (var prop in multiprops)
        //    {
        //        var pvs = propsvals.Where(p => p.Name == prop);
        //        foreach (var pv in pvs)
        //        {
        //           vals.Add(pv.DictionaryItem.StrValue); 
        //        }
        //        dic.Add(prop,JsonConvert.SerializeObject(vals));
        //    }

        //    foreach (var prop in singleprops)
        //    {
        //        var pv = propsvals.SingleOrDefault(p => p.Name == prop);
        //        dic.Add(prop, pv.DictionaryItem.StrValue);
        //    }
        //    //foreach (var propval in propsvals)
        //    //{
        //    //   dic.Add(propval.Name, propval.DictionaryItem.StrValue); 
        //    //}
        //    var resp = JsonConvert.SerializeObject(dic, Formatting.Indented);
        //    return this.Request.CreateResponse(HttpStatusCode.OK, resp); 
        //}
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

                // Check Advert not Already Exists
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

                // Check Dates
                if (DateTimeOffset.Compare(advert.DateFrom, advert.DateTo) >= 0)
                {
                    respList.Add(advert.DateFrom.ToLocalTime().ToString());
                    respList.Add(advert.DateTo.ToLocalTime().ToString());
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_WRONG_DATE, respList));
             
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
                //if (String.IsNullOrWhiteSpace(advert.ApartmentId))
                //{
                //    respList.Add("ApartmentId");
                //    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_REQUIRED, respList));
                //}
                //var apartment = context.Apartments.SingleOrDefault(a => a.Id == advert.ApartmentId);
                //if (apartment == null)
                //{
                //    respList.Add(advert.ApartmentId);
                //    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                //}
                
                //Apartment
                if (advert.Apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NULL));

                // Check Apartment Adress is not NULL
                if (String.IsNullOrWhiteSpace(advert.Apartment.Adress))
                {
                    respList.Add("Adress");
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_REQUIRED, respList));
                }

                // Check Properties Exists
                foreach (var propVal in advert.Apartment.PropsVals)
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

                // Generate 
                string advertGuid = Guid.NewGuid().ToString();
                string apartmentGuid = Guid.NewGuid().ToString();
                context.Set<Advert>().Add(new Advert()
                {
                    Id = advertGuid,
                    Name = advert.Name,
                    UserId = account.UserId,
                    Description = advert.Description,
                    ApartmentId = apartmentGuid,
                    DateFrom = advert.DateFrom,
                    DateTo = advert.DateTo,
                    PriceDay = advert.PriceDay,
                    PricePeriod = advert.PricePeriod,
                    Lang = advert.Lang,
                    Apartment = new Apartment()
                    {
                        Id = apartmentGuid,
                        Name = advert.Apartment.Name,
                        UserId = account.UserId,
                        Adress = advert.Apartment.Adress,
                        Latitude = advert.Apartment.Latitude,
                        Longitude = advert.Apartment.Longitude,
                        Lang = advert.Lang,
                        PropVals = advert.Apartment.PropsVals.Select(pv => new PropVal()
                        {
                            Id = Guid.NewGuid().ToString(),
                            PropId = pv.PropId,
                            ApartmentItemId = apartmentGuid,
                            StrValue = pv.StrValue,
                            NumValue = pv.NumValue,
                            DateValue = pv.DateValue,
                            BoolValue = pv.BoolValue,
                            DictionaryItemId = pv.DictionaryItemId,
                            Lang = advert.Lang
                        }).ToList()

                    }
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
            try
            {
                var respList = new List<string>();
                // Check Advert is not NULL 
                if (advert == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_NULL));

                // Check Current Advert is not NULL
                var advertCurrent = context.Adverts.SingleOrDefault(a => a.Id == id);
                if (advertCurrent == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_NOTFOUND, respList));
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

                // Check Advert User
                if (advertCurrent.UserId != account.UserId)
                {
                    respList.Add(advertCurrent.UserId);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_ADVERT_WRONG_USER, respList));
                }

                // Check Advert Name is not NULL
                if (String.IsNullOrWhiteSpace(advert.Name))
                {
                    respList.Add("Name");
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_REQUIRED, respList));
                }

                // Check Dates
                if (DateTimeOffset.Compare(advert.DateFrom, advert.DateTo) >= 0)
                {
                    respList.Add(advert.DateFrom.ToLocalTime().ToString());
                    respList.Add(advert.DateTo.ToLocalTime().ToString());
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_WRONG_DATE, respList));

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

                

                 // Check Advert not already Exists
                 var currentAdvertCount = context.Adverts.Count(a => a.Name == advert.Name && a.Id != id);
                 if (currentAdvertCount > 0)
                 {
                     respList.Add(advert.Name);
                     return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_EXISTS, respList));
                 }

                 //Apartment
                 if (advert.Apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NULL));

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

                 // Check Apartment Adress is not NULL
                 if (String.IsNullOrWhiteSpace(advert.Apartment.Adress))
                 {
                     respList.Add("Adress");
                     return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_REQUIRED, respList));
                 }

                 // Check Properties Exists
                 foreach (var propVal in advert.Apartment.PropsVals)
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
                // Update Advert PropVals
                foreach (var propVal in advert.PropsVals)
                {
                    var propValCurrent = context.PropVals.SingleOrDefault(pv => pv.Id == propVal.Id);
                    if (propValCurrent == null)
                    {
                        respList.Add(propVal.Id);
                        return
                        this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_PROPVAL_NOTFOUND, respList));
                    }
                    propValCurrent.StrValue = propVal.StrValue;
                    propValCurrent.NumValue = propVal.NumValue;
                    propValCurrent.DateValue = propVal.DateValue;
                    propValCurrent.BoolValue = propVal.BoolValue;
                    propValCurrent.DictionaryItemId = propVal.DictionaryItemId;
                    propValCurrent.Lang = advert.Lang;
                    context.SaveChanges();
                }

                // Update Apartment PropVals
                foreach (var propVal in advert.Apartment.PropsVals)
                {
                    var propValCurrent = context.PropVals.SingleOrDefault(pv => pv.Id == propVal.Id);
                    if (propValCurrent == null)
                    {
                        respList.Add(propVal.Id);
                        return
                        this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_PROPVAL_NOTFOUND, respList));
                    }
                    propValCurrent.StrValue = propVal.StrValue;
                    propValCurrent.NumValue = propVal.NumValue;
                    propValCurrent.DateValue = propVal.DateValue;
                    propValCurrent.BoolValue = propVal.BoolValue;
                    propValCurrent.DictionaryItemId = propVal.DictionaryItemId;
                    propValCurrent.Lang = advert.Lang;
                    context.SaveChanges();
                }

                // Update Advert
                advertCurrent.Name = advert.Name;
                advertCurrent.Description = advert.Description;
                advertCurrent.DateFrom = advert.DateFrom;
                advertCurrent.DateTo = advert.DateTo;
                advertCurrent.PriceDay = advertCurrent.PriceDay;
                advertCurrent.PricePeriod = advertCurrent.PricePeriod;

                // Update Apartment
                advertCurrent.Apartment.Name = apartment.Name;
                advertCurrent.Apartment.Adress = apartment.Adress;
                advertCurrent.Apartment.Latitude = apartment.Latitude;
                advertCurrent.Apartment.Longitude = apartment.Longitude;
                advertCurrent.Apartment.Lang = apartment.Lang;

                context.SaveChanges();

                respList.Add(advert.Id);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            } 
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
            try
            {
                var respList = new List<string>();
                var advert = context.Adverts.SingleOrDefault(a => a.Id == id);

                // Check Advert is not NULL
                if (advert == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_NOTFOUND, respList));
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

                // Check Advert User
                if (advert.UserId != account.UserId)
                {
                    respList.Add(advert.UserId);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_ADVERT_WRONG_USER, respList));
                }

                var apartment = context.Apartments.SingleOrDefault(a => a.Id == advert.ApartmentId);

                // Check Apartment is not NULL
                if (apartment == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                }
                // Delete Advert with PropVals
                context.Adverts.Remove(advert);
                context.SaveChanges();
                context.Apartments.Remove(apartment);
                context.SaveChanges();
                respList.Add(advert.Id);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_DELETED, respList));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }
    }
}
